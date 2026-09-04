########################################################################
# Импортируем важные библиотеки                                        #
########################################################################
import time
import wx
import wx.adv
import sys
sys.coinit_flags = 0
import os
import wx.lib.platebtn as platebtn
import keyboard
import threading
import mss
from win32api import GetSystemMetrics
from PIL import Image
from PIL import ImageGrab
from colormath.color_objects import LabColor, sRGBColor
from colormath.color_conversions import convert_color

########################################################################
# Импортируем библиотеки программы                                     #
########################################################################
import led_config as LC
import anim as AN

################################################################################################################################################
# Псевдо функция для треда (знаю, что можно и без неё обойтись, но как по мне, этот метод сильно упрощает жизнь. Так-что, пусть будет          #
################################################################################################################################################
def pse():
    pass
##########################################################################
# Фэйковая версия основной библиотеки EasyLED                            #
# ---                                                                    #
# Так, как подключение будет не сразу, то предотвращаем ряд тупых ошибок #
##########################################################################
class LD_FAKE:
    isConnected = False
    def count(self):
        return 0
    def is_fake(self):
        return True
    def setAllLED(self,e):
        return False
    def setLED(self, e, d):
        return False
    def updateLED(self):
        return False

LD = LD_FAKE() #Инитим фэйковую версию библиотеки
########################################################################
def resource_path(relative_path):
    #Получаем абсолютный файл ресурса. Работает для тестовой и скомпилированной версии
    try:
        #компилятор PyInstaller создаёт временную папку и сохраняет путь в _MEIPASS
        base_path = sys._MEIPASS
    except Exception:
        #в случае ошибки указываем стандартный абсолютный путь
        base_path = os.path.abspath(".")

    return os.path.join(base_path, relative_path) #возвращаем полный путь
########################################################################


anim_id = LC.loadLEDLastAnim() #Получаем из кеша номер последней анимации и сохраняем в переменную
if anim_id == False: #Если кеш недоступный
    anim_id = 0 #Ставим 0

#Start a test thread to fill the void and prevent errors
#Стартуем тестовый тред чтобы заполнить void и предотвратить ошибки
t1 = threading.Thread(target=pse)
t1.setDaemon(True)
t1.start()

#Размеры фрейма
width = 1000
height = 600

########################################################################
########################################################################
########################################################################
########################################################################
#####################           Config          ########################
########################################################################

enable_led = True #Переменная активности ленты (True - активна, False - неактивна)
dev_mode = True #Режим разработчика (виртуальная лента обновляется только при старте)

########################################################################
########################################################################
########################################################################
########################################################################

LC.LD.enable_led = enable_led #Переводим значение активности ленты в основную библиотеку

#Детали приложения
app_name = 'EasyLED' #Название
app_ver = 1.0 #Версия
app_icon = resource_path('data\icon.ico') #Значок приложения
app_logo = resource_path('data\logo.png') #Логотип приложения
app_mn = resource_path('data\\uk-soft-evo.png') #Логотип Uk. Soft - Evolution

#Переменная перезагрузки приложения
reload = False

#Переменная, которая означает показывалось ли сообщение о том, что приложение свернулось в трэй
hintShowed = False

#Блокируем лишние ошибки
block = True

#Блокировка сочитаний клавиш
kblock = False

#Получаем размеры экрана
screen_width = GetSystemMetrics(0)
screen_height = GetSystemMetrics(1)

#Считаем размеры секторов экрана
sector_width = int(screen_width / LC.led_num)
sector_height = int(screen_height / LC.led_num)

#Основные цвета приложения
accent_color = [104,0,255]
bg_color = [0,0,0]
inactive_color = [14,14,14]
text_inactive_color = [150, 150, 150]
bg_opacity = 100

#Переводим основные цвета в WX
accent_wx_color = wx.Colour(accent_color[0], accent_color[1], accent_color[2])
bg_wx_color = wx.Colour(bg_color[0], bg_color[1], bg_color[2])
inactive_wx_color = wx.Colour(inactive_color[0], inactive_color[1], inactive_color[2])
text_inactive_wx_color = wx.Colour(text_inactive_color[0], text_inactive_color[1], text_inactive_color[2])

#Функция получения изображения из байтов
def pil_frombytes(im):
    return Image.frombytes('RGB', im.size, im.bgra, 'raw', 'BGRX')

#Переменные для захвата экрана
MAP = {}
TRANS = []

#Заполняем переменные для захвата экрана
for i in range(0, LC.led_num, 1):
    TRANS.append(i)
    MAP[i] = [0,0,0]

#Тред захвата экрана
def screen_thread():
    global TRANS, MAP #Получаем необходимые глобальные переменные

    try:
        with mss.mss() as sct:
            im = ImageGrab.grab() #Захватываем экран

            for w in range(0, LC.led_num, 1): #Проходимся по циклу из всех светодиодов
                sector_hor_start = w * sector_width #Получаем начало горизонтального сектора
                sector_hor_end = (w+1) * sector_width #Получаем конец горизонтального сектора

                #Переменные для суммы цветов LAB
                summ_l = 0
                summ_a = 0
                summ_b = 0

                #Дополнительный цикл для вертикальных светодиодов
                for h in range(0, LC.led_num, 1):
                    sector_ver_start = h * sector_height #Начало вертикального сектора
                    sector_ver_end = (h+1) * sector_height #Конец вертикального сектора

                    rgb = im.getpixel((sector_hor_start, sector_ver_start)) #Получаем пиксель по началу горизонтального и вертикального сектора
                    rgb = sRGBColor(rgb[0], rgb[1], rgb[2]) #Переводим в sRGB
                    lab = convert_color(rgb, LabColor) #Конвертим в LAB

                    #Добавляем к общей сумме LAB
                    summ_l += lab.lab_l
                    summ_a += lab.lab_a
                    summ_b += lab.lab_b

                #Получаем средний LAB цвет
                mid_l = summ_l / LC.led_num
                mid_a = summ_a / LC.led_num
                mid_b = summ_b / LC.led_num

                mid_lab = LabColor(mid_l, mid_a, mid_b) #Переводим в нужный вид
                mid_rgb = convert_color(mid_lab, sRGBColor) #Конвертируем цвет в sRGB
                mid_rgb = [mid_rgb.rgb_r, mid_rgb.rgb_g, mid_rgb.rgb_b] #Переводим в цветной массив

                #На всякий случай
                try:
                    TRANS[w] = LD.transition(MAP[w], mid_rgb, TRANS[w], 1) #Добавляем новый транзишн в массив транзишнов
                except Exception:
                    pass

                #Добавляем полученный цвет в массив цветов захвата
                MAP[w] = mid_rgb
    except Exception:
        pass

#Трэд транзишна захвата экрана
def transition_thread():
    global TRANS, MAP #Получаем глобальные переменные

    for i in range(0, LC.led_num, 1): #Проходимся по всем светодиодам
        if LD.is_fake() != True: #Если есть в кармане пачка.. т.е. если есть подключение с лентой
            currentColor = LD.getTransitionColor(TRANS[i]) #Закрашиваем текущий светодиод в текущий цвет транзишна
        else:
            currentColor = [0,0,0] #Иначе закрашиваем в чёрный

        if currentColor == False: #Если транзишн недоступен
            currentColor = MAP[i] #Закрашиваем в цвет из карты цветов захвата
        if LD.is_fake() != True: #Опять же, если есть коннект
            LD.setLED(i+1, MAP[i]) #Закрашиваем светодиод
    if LD.is_fake() != True: #Если есть коннект
        return LD.updateLED() #Обновляем ленту

#Функция, которая включает в себя все основные анимации
def anim(id):
    if block != True: #Если не включена блокировка
        result = '' #Создаём переменную для результата (упеха анимации или не успеха)
        #Так, как в Python нет switch/case, то юзаем if
        if id == 0:
            LD.setAllLED([0,0,0])
            result = LD.updateLED()
            LD.time.sleep(1)
        if id == 1:
            result = AN.animation.rainbow(LD)
        if id == 2:
            result = AN.animation.red(LD)
        if id == 3:
            result = AN.animation.green(LD)
        if id == 4:
            result = AN.animation.blue(LD)
        if id == 5:
            result = AN.animation.white(LD)
        if id == 6:
            result = AN.animation.violet(LD)
        if id == 7:
            result = AN.animation.colors(LD)
        if id == 8:
            result = AN.animation.sm_theme_color(LD)
        if id == 9:
            result = AN.animation.sm_wall_color(LD)
        if id == 10:
            result = AN.animation.sm_cpu1(LD)
        if id == 11:
            result = AN.animation.sm_cpu2(LD)
        if id == 12:
            result = AN.animation.sm_syst(LD)
        if id == 14:
            result = AN.animation.sm_colormusic_1(LD)
        if id == 15:
            result = AN.animation.sm_colormusic_2(LD)
        if id == 16:
            result = AN.animation.sm_colormusic_3(LD)
        if id == 17:
            result = AN.animation.sm_colormusic_4(LD)
        if id == 18:
            result = AN.animation.sm_colormusic_5(LD)
        if id == 19:
            result = AN.animation.static_white(LD)
        if id == 20:
            result = AN.animation.static_red(LD)
        if id == 21:
            result = AN.animation.static_green(LD)
        if id == 22:
            result = AN.animation.static_blue(LD)
        if id == 23:
            result = AN.animation.static_yellow(LD)
        if id == 24:
            result = AN.animation.static_magenta(LD)
        if id == 25:
            result = AN.animation.static_cyan(LD)
        if id == 26:
            result = AN.animation.static_orange(LD)
        if id == 27:
            result = AN.animation.static_violet(LD)
        if id == 28:
            result = AN.animation.static_grass(LD)
        if id == 29:
            result = AN.animation.static_peach(LD)
        if id == 30:
            result = AN.animation.static_tomato(LD)
        if id == 31:
            result = AN.animation.static_light_sea_green(LD)

        return result
########################################################################
class AniThread(threading.Thread):
    #Тред анимации
    #----------------------------------------------------------------------
    def __init__(self, frame):
        global t1 #Получаем глобальную переменную, в которую поместим тред

        #Инитим тред
        threading.Thread.__init__(self)
        self.start() #Запускаем
        t1 = self #Переносим тред в глобальную переменную
        t1.shutdown = False #Указываем параметр shutdown, чтобы потом его изменять
        self.frame = frame #Переносим в параметр frame основной фрейм проги

    #----------------------------------------------------------------------
    def run(self):
        global t1, anim_id, appSelf, LD, block #Получаем необходимые глобальные переменные
        #Для предотвращения бага спим 1 секунду
        time.sleep(1)
        #Запускаем основной тред
        main_t = threading.Thread(target=self.main_thread)
        main_t.setDaemon(True)
        main_t.start()
        #Запускаем дополнительный тред (для захвата экрана)
        addt_t = threading.Thread(target=self.addt_thread)
        addt_t.setDaemon(True)
        addt_t.start()
        #Очищаем ленту
        LD.setAllLED([0,0,0])
        LD.updateLED()
    #Основной тред
    def main_thread(self):
        while getattr(t1, "shutdown") == False: #Цикл работает пока параметр shutdown не будет True
            #Если номер анимации 13 (захват экрана)
            if anim_id == 13:
                result = transition_thread() #Запускаем тред транзишна
            else: #Иначе
                result = anim(anim_id) #Выполняем обычный образом анимации
            if block != True: #Если ошибки не заблокированны
                #Если вышла ошибочка и юзер что-то выбрал
                if result == 'EXIT': #Если юзер выбрал "Отмена", то выходим из проги
                    t1.shutdown = True #Завершаем основной тред
                    self.frame.Destroy() #Уничтожаем нафиг фрейм
                    try: #Пробуем удалить значок из трея
                        self.frame.tbIcon.RemoveIcon() #Убираем значок
                        self.frame.tbIcon.Destroy() #Уничтожаем его
                    except Exception:
                        pass
                    raise SystemExit #Завершаем скрипт
                    break #Останавливаем цикл
                if result == 'RETRY': #Если юзер выбрал "Повторить"
                    LD = LC.init() #Реконнектимся
    #Дополнительный тред
    def addt_thread(self):
        if anim_id == 13: #Если выбран 13 режим (захват экрана)
            while getattr(t1, "shutdown") == False: #Цикл работает пока параметр shutdown не будет True
                screen_thread() #выполняем тред захвата экрана

########################################################################
# Переменная для кнопок
btns = {}
# Переменная для внешнего доступа к фрейму
appSelf = ''
########################################################################

#Класс значка в таскбаре
class TaskBarIcon(wx.adv.TaskBarIcon):
    def __init__(self, frame):
        global app_name, hintShowed #Получаем глобальные переменные названия приложения и о том, показывалась ли подсказка
        wx.adv.TaskBarIcon.__init__(self) #Инитим класс
        self.frame = frame #Добавляем параметр с основным фреймом

        icon = wx.Icon(app_icon, wx.BITMAP_TYPE_ICO) #Переводим значок в нужный вид

        self.SetIcon(icon, app_name) #Устанавливаем значок
        if hintShowed == False: #Если подсказка не выводилась
            self.ShowBalloon(app_name, "App still running here") #Показываем её
            hintShowed = True #Изменяем значение переменной

        #Делаем меню
        self.menu = wx.Menu()
        self.menu.Append(wx.ID_FILE, "&Restore", "Restore app window")
        self.menu.AppendSeparator()
        self.menu.Append(wx.ID_EXIT, "&Exit", "Exit the app")

        #Обрабатываем события в меню
        self.Bind(wx.EVT_MENU, self.menuHandler)

        #Обрабатываем события клика на значок таскбара
        self.Bind(wx.adv.EVT_TASKBAR_LEFT_UP, self.OnTaskBarClick)
        self.Bind(wx.adv.EVT_TASKBAR_RIGHT_UP, self.OnTaskBarRClick)
        self.Bind(wx.adv.EVT_TASKBAR_LEFT_DCLICK, self.OnTaskBarClick)

    #Обработчик событий меню
    def menuHandler(self, evt):
        id = evt.GetId() #Получаем ID события

        if id == wx.ID_FILE: #Если юзер выбрал "восстановить окно"
            self.OnTaskBarClick(evt) #Вызываем нужную функцию
        if id == wx.ID_EXIT: #Если юзер выбрал "Выйти"
            self.OnTaskBarClose(evt) #Вызываем функцию завершения
    def OnTaskBarClose(self, evt): #Функция завершения приложения из таскбара
        global LD
        self.RemoveIcon()
        self.Destroy()
        #Убиваем фрейм
        t1.shutdown = True
        connect_status.shutdown = True
        sim_led.shutdown = True
        self.frame.Destroy()
        if LD.is_fake() == False:
            for i in LD.erange(0, LD.count() * 2 - 1, 1):
                LD.setAllLED([0,0,0])
                LD.updateLED()
                LD.time.sleep(0.02)
        LD = LD_FAKE()
        os.system("taskkill /f /im python.exe")
        os.system("taskkill /f /im gui.exe")
        os.system("taskkill /f /im pythonw.exe")
        sys.exit()


    def OnTaskBarClick(self, evt): #Функция восстановления окна из таскбара
        #Убираем значок
        self.RemoveIcon()
        self.Destroy()

        #Показываем фрейм
        self.frame.Show()
        self.frame.Restore()
    def OnTaskBarRClick(self, evt): #Действие при клике на правую клавишу
        self.PopupMenu(self.menu) #Вызываем меню
########################################################################
def colorBrightness(r, g, b): #Получаем яркость цвета
    return (r * 299 + g * 587 + b * 114) / 1000 #Творя магию
def blackOrWhite(color): #Определяем контрастный цвет
    brightness = colorBrightness(color[0], color[1], color[2]) #Получаем яркость

    if brightness < 123: #Если яркость меньше 123
        return [255,255,255] #Возвращаем белый
    else: #Иначе
        return [0,0,0] #Возвращаем чёрный
def GUIbuildButtons(self, buttons, panel): #Функция создания кнопок режимов (чтобы не писать кучу кода)
    global btns, width, height #Получаем необходимые глобальные переменные

    for i in range(0, len(buttons), 1): #Цикл стройки кнопок
        #Создаём кнопку
        self.btns[i] = btns[i] = platebtn.PlateButton(panel, 5, buttons[i][0], size=(width / 4, height / 8 -17.2), style=platebtn.PB_STYLE_SQUARE)
        #Указываем цвет наведения
        btns[i].SetPressColor(inactive_wx_color)
        #Определяем контрастный цвет
        rev_color = blackOrWhite(inactive_color)
        #Закрашиваем цвет текста в нужный
        btns[i].SetLabelColor(normal=text_inactive_wx_color, hlight=wx.Colour(rev_color[0], rev_color[1], rev_color[2]))
        #Привязываем события кнопки к тому, что указано в переменной buttons вторым параметром каждого элемента
        btns[i].Bind(wx.EVT_BUTTON, buttons[i][1])
        #Заменяем шрифт кнопки на Segoe UI
        font = wx.Font(10, wx.DEFAULT, wx.NORMAL, wx.NORMAL, False, 'Segoe UI')
        btns[i].SetFont(font)

    #Дополнительно переводим кнопки в параметр основного класса
    self.buttons = buttons



########################################################################
########################################################################
########################################################################
########################################################################
########################################################################
########################################################################
########################################################################
########################################################################
########################################################################

connect_status = ''
sim_led = ''

#Основной класс приложения
class AppGUI(wx.Frame):
    def ledInit(self): #Функция инициализации подключения к ленте
        global LD, block, width, height, connect_status, sim_led #Получаем необходимые глобальные переменные
        LD = LC.init() #Устанавливаем подключение
        block = False #Отменяем блокировку ошибок
        if enable_led != False: #Если лента не отключена вручную
           self.cb.SetValue(str(LD.count())) #Устанавливаем значения списка на текущее количество светодиодов
        LD.block_errors = False #Отменяем блокировку ошибок в самой библиотеки

        #Запускаем тред, который будет определять подключена ли лента
        connect_status = threading.Thread(target=self.checkConnect)
        connect_status.setDaemon(True)
        connect_status.start()

        #Если включен режим разработчика
        if dev_mode == True:
            #Запускаем тред, который будет закрашивать визуальную ленту
            sim_led = threading.Thread(target=self.simLED)
            sim_led.setDaemon(True)
            sim_led.start()
    def hotkeys(self, id, ww = False): #Функция для обработки горячих клавиш
        global anim_id #Получаем глобальную переменную ID анимации
        hotkey = keyboard.get_hotkey_name() #Получаем дополнительно горячую клавишу

        #Чекаем нажаты ли клавиши стрелок
        isHasUP = 'up' in hotkey
        isHasDOWN = 'down' in hotkey
        isHasLEFT = 'left' in hotkey
        isHasRIGHT = 'right' in hotkey


        #Если стрелки не нажаты
        if isHasUP == False and isHasDOWN == False and isHasLEFT == False and isHasRIGHT == False and self.kblock == False:
            id = int(id) #Переводим ID в int
            anim_id = id #Переводим id в переменную id анимации
            LC.storeLEDParams(int(LC.led_num), id) #Сохраняем номер анимации в кеш
            t1.shutdown = True #Останавливаем треды анимаций

            btn = self.btns[id] #Получаем кнопку, привязанную к режиму
            self.textbar.SetLabel("Current: "+self.buttons[id][0]) #Выводим название текущей анимации в статус

            for i in range(0, len(btns), 1): #Запускаем цикл, который уберёт статус активности со всех кнопок режимов
                if self.btns[i] != btn: #Если текущая кнопка цикла не равна текущей кнопке
                    self.btns[i].SetBackgroundColour(bg_wx_color) #Закрашиваем кнопку в якобы прозрачный цвет (цвет фона приложения)
                    self.btns[i].SetPressColor(inactive_wx_color) #Указываем цвет при наведении
                    rev_color = blackOrWhite(inactive_color) #Определяем контрастный цвет
                    self.btns[i].SetLabelColor(normal=text_inactive_wx_color, hlight=wx.Colour(rev_color[0], rev_color[1], rev_color[2])) #Указываем цвета текста

            #Указываем стили кнопки текущего режима
            btn.SetBackgroundColour(accent_wx_color)
            btn.SetPressColor(accent_wx_color)
            rev_color = blackOrWhite(accent_color)
            btn.SetLabelColor(normal=wx.Colour(rev_color[0], rev_color[1], rev_color[2]), hlight=wx.Colour(rev_color[0], rev_color[1], rev_color[2]))

            #Показываем в статусбаре, что идёт загрузка
            self.textbar.SetLabel("Loading...")
            for i in LD.erange(0, LD.count() - 1, 1): #Очищаем ленту
                LD.setAllLED([0,0,0])
                LD.updateLED()
                LD.time.sleep(0.02)

            #Спим одну секунду, чтобы предотвратить баги
            LD.time.sleep(1)

            #Проходимся по всем кнопкам режимов
            for i in range(0, len(btns), 1):
                if self.btns[i] == btn: #Если кнопка текущая
                    self.textbar.SetLabel("Current: "+self.buttons[i][0]) #Указываем в статусбаре текущий режим
            #Запускаем тред анимации
            AniThread(self)
    #Функция для назначения горячих клавиш
    def keyboardLogic(self):
        for i in range(0, 10, 1):
            keyboard.add_hotkey('Num /+'+str(i), self.hotkeys, args=(i, True))
        for i in range(10, 20, 1):
            keyboard.add_hotkey('Num *+'+str(i - 10), self.hotkeys, args=(i, True))
        for i in range(20, 28, 1):
            keyboard.add_hotkey('Num -+'+str(i - 20), self.hotkeys, args=(i, True))

    #Функция, которая будет выполнятся при выборе кол-ва светодиодов
    def OnNumSelect(self, e):
        global LD, LC, reload, anim_id, block, MAP, TRANS, sector_width, sector_height #Получаем необходимые глобальные переменные
        block = True #Блокируем ошибки
        t1.shutdown = True #Останавливаем тред анимации
        i = e.GetString() #Получаем выбранное кол-во
        LC.led_num = int(i) #Заменяем кол-во в конфиге
        LD.rebuildMap(LC.led_num) #Перестраиваем карту светодиодов в библиотеке

        #Обновляем переменные захвата экрана
        MAP = {}
        TRANS = []

        #Заполняем переменные захвата экрана
        for i in range(0, LC.led_num, 1):
            TRANS.append(i)
            MAP[i] = [0,0,0]

        #Пересчитываем сектора захвата экрана
        sector_width = int(screen_width / LC.led_num)
        sector_height = int(screen_height / LC.led_num)

        #Спим одну секунду
        time.sleep(1)

        #Вырубаем блокировку ошибок
        block = False

        #Сохраняем кол-во в кеш
        LC.storeLEDParams(int(i), anim_id)

        #Запускаем тред анимации
        AniThread(self)
    #----------------------------------------------------------------------
    #Тред проверки коннекта
    def checkConnect(self):
        self.shutdown = False
        while getattr(self, "shutdown") == False: #Запускаем бесконечный цикл
            if enable_led != False: #Если лента не отключена вручную
                try: #Пробуем проверить
                    if LD.isConnected:
                        if LD.comLED == True: #Если подключенно через COM
                            self.connect.SetLabel("Connected to "+LD.UsedCOM)
                        else: #Если через AmbiBox
                            self.connect.SetLabel("Connected to AmbiBox")
                    else: #Если нет коннекта
                        self.connect.SetLabel("Disconnected")
                except Exception: #Если ошибка - стопаррим скрипт
                    raise SystemExit
                    break
                    pass
            else: #Если отключена вручную, то выводим статус оффлайна
                try:
                    self.connect.SetLabel("Offline Mode")
                except Exception:
                    pass
            time.sleep(1) #Спим 1 секунду

    #Тред виртуальной ленты
    def simLED(self):
        self.shutdown = False
        while getattr(self, "shutdown") == False: #Запускаем бесконечный цикл
            try:
                for i in LD.allLED(): #Проходимся по всем светодиодам
                    self.lbt[i-1].SetBackgroundColour(LD.LED[i-1]) #Закрашиваем текущий в цвет ленты

                self.panel.Update() #Обновляем панель
                time.sleep(0.01) #Спим 10 мс., чтобы не багало
            except Exception:
                pass
    def CloseApp(self, frame): #Функция для закрытия приложения
        global connect_status, sim_led

        t1.shutdown = True
        connect_status.shutdown = True
        sim_led.shutdown = True
        #frame.Destroy() #Уничтожаем фрейм

        #Убираем значок приложения из трея
        try:
            frame.tbIcon.RemoveIcon()
            frame.tbIcon.Destroy()
        except Exception:
            pass
    #Обработчик события закрытия программы
    def onClose(self, evt):
        self.CloseApp()
    #Обработчик события сворачивания приложения
    def onMinimize(self, event):
        if self.IsIconized():
            #Вызываем класс значка таскбара
            self.tbIcon = TaskBarIcon(self)
            self.Hide()
    #----------------------------------------------------------------------
    #Функция инициализации основного класса приложения
    def __init__(self):
        global width, height, appSelf, app_name, app_ver, app_icon, app_logo, app_mn, kblock #Получаем важные глобальные переменные

        #Запускаем тред инициализации ленты
        ledinit = threading.Thread(target=self.ledInit)
        ledinit.setDaemon(True)
        ledinit.start()

        self.kblock = kblock

        #Заполняем глобальную переменную доступа к классу
        appSelf = self
        self.btns = {}

        #Создаём фрейм
        no_resize = wx.DEFAULT_FRAME_STYLE & ~ (wx.RESIZE_BORDER | wx.MAXIMIZE_BOX)
        wx.Frame.__init__(self, None, wx.ID_ANY, app_name, style=no_resize)

        #Привязываем события сворачивания и закрытия приложения
        self.Bind(wx.EVT_ICONIZE, self.onMinimize)
        #self.Bind(wx.EVT_CLOSE, self.onClose)

        #Указываем значок фрейма
        icon_path = app_icon
        self.SetIcon(wx.Icon(icon_path))

        #Создаём панель и закрашиваем её в главный фон
        panel = wx.Panel(self, wx.ID_ANY)
        panel.SetBackgroundColour(bg_wx_color)

        #Привязываем панель к классу
        self.panel = panel

        #Строим кнопки режимов
        GUIbuildButtons(self, [
            ['None', self.animBtn],
            ['Rainbow animation', self.animBtn],
            ['Red animation', self.animBtn],
            ['Green animation', self.animBtn],
            ['Blue animation', self.animBtn],
            ['White animation', self.animBtn],
            ['Violet animation', self.animBtn],
            ['Colors animation', self.animBtn],
            ['Windows Theme Color', self.animBtn],
            ['Wallpaper color', self.animBtn],
            ['CPU Load', self.animBtn],
            ['CPU Load 2', self.animBtn],
            ['System temperature', self.animBtn],
            ['Screen capture (beta)', self.animBtn],
            ['ColorMusic 1', self.animBtn],
            ['ColorMusic 2', self.animBtn],
            ['ColorMusic 3', self.animBtn],
            ['ColorMusic 4', self.animBtn],
            ['ColorMusic 5', self.animBtn],
            ['Static white', self.animBtn],
            ['Static red', self.animBtn],
            ['Static green', self.animBtn],
            ['Static blue', self.animBtn],
            ['Static yellow', self.animBtn],
            ['Static magenta', self.animBtn],
            ['Static cyan', self.animBtn],
            ['Static orange', self.animBtn],
            ['Static violet', self.animBtn],
            ['Static grass', self.animBtn],
            ['Static PeachPuff', self.animBtn],
            ['Static Tomato', self.animBtn],
            ['Static LightSeaGreen', self.animBtn]
        ], panel)

        #Выделяем кнопку текущего режима
        btns[anim_id].SetBackgroundColour(accent_wx_color)
        btns[anim_id].SetPressColor(accent_wx_color)
        rev_color = blackOrWhite(accent_color)
        self.btns[anim_id].SetLabelColor(normal=wx.Colour(rev_color[0], rev_color[1], rev_color[2]), hlight=wx.Colour(rev_color[0], rev_color[1], rev_color[2]))

        if dev_mode == True: #Если включён режим разработчика
            self.lbt = {} #Создаём параметр в котором будем хранить виртуальную ленту
            for i in range(0, LC.led_num, 1): #Строим виртуальные светодиоды
                self.lbt[i] = wx.Button(self.panel, 5, '', pos=((width / LC.led_num) * i - 5, height-83), size=(width / LC.led_num + 1, 20))
                self.lbt[i].SetBackgroundColour(bg_wx_color)

        #Запускаем последнюю анимацию
        AniThread(self)

        #Делаем основную разметку
        vbox = wx.BoxSizer(wx.VERTICAL) #Основной сайзер
        self.spacer = vbox.AddSpacer(55) #Отступ сверху
        sizer = {} #Массив для сайзеров

        #Переменные для осуществления переноса кнопок в новый сайзер (перенос на новую строку)
        m = 0
        t = 0

        #Проходимся по всем кнопкам
        for i in range(0, len(btns), 1):
            m += 1 #Добавляем к переменной, которая обозначает номер кнопки в ряду
            if m > 4: #Если номер больше за 4
                m = 1 #Номер снова единица
            if m == 1: #Если номер - единица, то добавляем новый сайзер
                sizer[t] = cur = wx.BoxSizer(wx.HORIZONTAL)
                #Добавляем +1 к ряду
                t += 1

            #Добавляем к текущему ряду кнопку
            cur.Add(btns[i],  10, wx.LEFT, 0)

        #Проходимся по всем сайзерам
        for i in range(0, len(sizer), 1):
            vbox.Add(sizer[i], flag=wx.LEFT | wx.TOP, border=0) #Добавляем к основному сайзеру текущий сайзер

        panel.SetSizer(vbox) #Указываем основной сайзер для панели

        #Добавляем варианты кол-ва светодиодов
        ch = []
        for i in range(1, 301, 1):
            ch.append(str(i))

        #Строим основные компоненты
        app_logo_wx = wx.Image(app_logo, wx.BITMAP_TYPE_ANY).ConvertToBitmap()
        app_mn_wx = wx.Image(app_mn, wx.BITMAP_TYPE_ANY).ConvertToBitmap()
        self.logo = wx.BitmapButton(panel, id=-1, bitmap=app_logo_wx, pos=(0, 0), size=(48, 48))
        self.mn = platebtn.PlateButton(panel, id=-1, bmp=app_mn_wx, pos=(width - 75, 2), size=(48, 48))
        self.mn.SetBackgroundColour(bg_wx_color)
        self.mn.SetPressColor(bg_wx_color)
        self.titleTXT = wx.StaticText(panel, -1, app_name, pos=(55, 6))
        self.titleTXT.SetForegroundColour(wx.Colour(255,255,255))
        font = wx.Font(18, wx.DEFAULT, wx.NORMAL, wx.NORMAL, False, 'Segoe UI Light')
        self.titleTXT.SetFont(font)
        self.textbar = wx.StaticText(panel, label="Welcome to "+app_name, pos=(5, height - 59))
        self.textbar.SetForegroundColour(wx.Colour(255,255,255))
        font = wx.Font(10, wx.DEFAULT, wx.NORMAL, wx.NORMAL, False, 'Segoe UI')
        self.textbar.SetFont(font)
        self.cb = wx.ComboBox(panel, pos=(width - 60, height - 62), choices=ch, style=wx.CB_READONLY)
        self.cb.Bind(wx.EVT_COMBOBOX, self.OnNumSelect)
        self.cb.SetValue(str(LD.count()))
        self.cb.SetFont(font)
        self.tx = wx.StaticText(panel, label="Num LED's", pos=(width - 130, height - 59))
        self.tx.SetFont(font)
        self.tx.SetForegroundColour(wx.Colour(255,255,255))
        self.connect = wx.StaticText(panel, label="Connection...", pos=(width / 2 - 100, height - 59), size=(200, 30), style=wx.ALIGN_CENTER)
        self.connect.SetForegroundColour(wx.Colour(255,255,255))
        self.connect.SetFont(font)
        self.SetTransparent(255 * bg_opacity / 100)

        #Запускаем тред горячих клавиш
        t2 = threading.Thread(target=self.keyboardLogic)
        t2.setDaemon(True)
        t2.start()


    #----------------------------------------------------------------------
    #Тред запуска анимации
    def animBtnStart(self, event):
        global anim_id #Получаем глобальную переменную номера анимации

        t1.shutdown = True #Завершаем тред анимации

        btn = event.GetEventObject() #Получаем текущую кнопку

        for i in range(0, len(btns), 1): #Делаем все кнопки, которые не равны текущей неактивными
            if self.btns[i] != btn:
                self.btns[i].SetBackgroundColour(bg_wx_color)
                self.btns[i].SetPressColor(inactive_wx_color)
                rev_color = blackOrWhite(inactive_color)
                self.btns[i].SetLabelColor(normal=text_inactive_wx_color, hlight=wx.Colour(rev_color[0], rev_color[1], rev_color[2]))

        #Анимируем выделение активной кнопки
        for i in AN.ease.generateMap(AN.ease.easeInOutCubic, 0, 100, 100):
            btn.SetBackgroundColour(wx.Colour(accent_color[0] * i / 100,accent_color[1] * i / 100,accent_color[2] * i / 100))
            btn.SetPressColor(wx.Colour(accent_color[0] * i / 100,accent_color[1] * i / 100,accent_color[2] * i / 100))
            time.sleep(0.002)

        #Получаем контрастный цвет и закрашиваем в него текст кнопки
        rev_color = blackOrWhite(accent_color)
        btn.SetLabelColor(normal=wx.Colour(rev_color[0], rev_color[1], rev_color[2]), hlight=wx.Colour(rev_color[0], rev_color[1], rev_color[2]))
        self.textbar.SetLabel("Loading...") #Отображаем в статусбаре, что идёт загрузка

        #Очищаем ленту
        for i in LD.erange(0, LD.count() - 1, 1):
            LD.setAllLED([0,0,0])
            LD.updateLED()
            LD.time.sleep(0.02)

        #Спим 1 секунду
        LD.time.sleep(1)

        #Проходимся по всем кнопкам
        for i in range(0, len(btns), 1):
            if self.btns[i] == btn: #Если кнопка текущая
                anim_id = i #Переводим в id анимации переменной текущую анимацию
                LC.storeLEDParams(LC.led_num, i) #Сохраняем id анимации в кеш
                self.textbar.SetLabel("Current: "+self.buttons[i][0]) #Отображаем текущую анимацию в статусбаре

        #Запускаем тред анимации
        AniThread(self)
    def animBtn(self, event):
        #Обработчик клика по кнопке режима

        #Запускаем тред
        t1 = threading.Thread(target=self.animBtnStart, args=(event,))
        t1.setDaemon(True)
        t1.start()

    #----------------------------------------------------------------------


#Функция инициализации приложения
def AppInit():
    global reload, connect_status, sim_led, LD, LC

    app = wx.App(0)
    frame = AppGUI()
    frame.SetSize(100,100,width,height)
    frame.Show(True)
    LC.frame = frame
    app.MainLoop()
    if reload != True:
        print('------')
        print('Closing app...')
        try:
            t1.shutdown = True
            connect_status.shutdown = True
            sim_led.shutdown = True
        except Exception:
            pass

        for thread in threading.enumerate():
            if(thread.isAlive()):
                try:
                    thread._Thread_stop()
                    thread._Thread_delete()
                except Exception:
                    pass
        if LD.is_fake() == False:
            for i in LD.erange(0, LD.count() * 2 - 1, 1):
                LD.setAllLED([0,0,0])
                LD.updateLED()
                LD.time.sleep(0.02)
        LD = LD_FAKE()
        sys.exit()
        os.system("taskkill /f /im python.exe")
        os.system("taskkill /f /im gui.exe")
        os.system("taskkill /f /im pythonw.exe")
    else:
        reload = False
        AppInit()
#App init
if __name__ == "__main__":
    AppInit()
