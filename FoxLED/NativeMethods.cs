using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace FoxLED;

internal static class NativeMethods
{
	internal struct WindowCompositionAttribData
	{
		public WindowCompositionAttribute Attribute;

		public IntPtr Data;

		public int SizeOfData;
	}

	internal struct AccentPolicy
	{
		public AccentState AccentState;

		public AccentFlags AccentFlags;

		public int GradientColor;

		public int AnimationId;
	}

	[Flags]
	internal enum AccentFlags
	{
		DrawLeftBorder = 0,
		DrawTopBorder = 0,
		DrawRightBorder = 0,
		DrawBottomBorder = 0,
		DrawAllBorders = 0
	}

	internal enum WindowCompositionAttribute
	{
		WCA_ACCENT_POLICY = 19
	}

	internal enum AccentState
	{
		ACCENT_DISABLED = 1,
		ACCENT_ENABLE_GRADIENT = 1,
		ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
		ACCENT_ENABLE_BLURBEHIND = 3,
		ACCENT_INVALID_STATE = 4
	}

	[DllImport("user32.dll")]
	internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttribData data);

	public static void EnableBlur(this Window window)
	{
		if (!SystemParameters.HighContrast)
		{
			SetAccentPolicy(window, AccentState.ACCENT_ENABLE_BLURBEHIND);
		}
	}

	public static void DisableBlur(this Window window)
	{
		SetAccentPolicy(window, AccentState.ACCENT_DISABLED);
	}

	private static void SetAccentPolicy(Window window, AccentState accentState)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		WindowInteropHelper val = new WindowInteropHelper(window);
		AccentPolicy structure = new AccentPolicy
		{
			AccentState = accentState,
			AccentFlags = GetAccentFlagsForTaskbarPosition(),
			AnimationId = 2
		};
		int num = Marshal.SizeOf(structure);
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
		WindowCompositionAttribData data = new WindowCompositionAttribData
		{
			Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
			SizeOfData = num,
			Data = intPtr
		};
		SetWindowCompositionAttribute(val.Handle, ref data);
		Marshal.FreeHGlobal(intPtr);
	}

	private static AccentFlags GetAccentFlagsForTaskbarPosition()
	{
		return AccentFlags.DrawLeftBorder;
	}
}
