using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FoxLED;

public class BrushAnimation : AnimationTimeline
{
	public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(Brush), typeof(BrushAnimation));

	public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(Brush), typeof(BrushAnimation));

	public override Type TargetPropertyType => typeof(Brush);

	public Brush From
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			return (Brush)((DependencyObject)this).GetValue(FromProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(FromProperty, (object)value);
		}
	}

	public Brush To
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			return (Brush)((DependencyObject)this).GetValue(ToProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(ToProperty, (object)value);
		}
	}

	public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
	{
		return GetCurrentValue((Brush)((defaultOriginValue is Brush) ? defaultOriginValue : null), (Brush)((defaultDestinationValue is Brush) ? defaultDestinationValue : null), animationClock);
	}

	public object GetCurrentValue(Brush defaultOriginValue, Brush defaultDestinationValue, AnimationClock animationClock)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00bb: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		if (!((Clock)animationClock).CurrentProgress.HasValue)
		{
			return Brushes.Transparent;
		}
		defaultOriginValue = From ?? defaultOriginValue;
		defaultDestinationValue = To ?? defaultDestinationValue;
		if (((Clock)animationClock).CurrentProgress.Value == 0.0)
		{
			return defaultOriginValue;
		}
		if (((Clock)animationClock).CurrentProgress.Value == 1.0)
		{
			return defaultDestinationValue;
		}
		return (object)new VisualBrush((Visual)new Border
		{
			Width = 1.0,
			Height = 1.0,
			Background = defaultOriginValue,
			Child = (UIElement)new Border
			{
				Background = defaultDestinationValue,
				Opacity = ((Clock)animationClock).CurrentProgress.Value
			}
		});
	}

	protected override Freezable CreateInstanceCore()
	{
		return (Freezable)(object)new BrushAnimation();
	}
}
