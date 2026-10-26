using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Controls = System.Windows.Controls;
using Effects = System.Windows.Media.Effects;
using Media = System.Windows.Media;
using WPF = System.Windows;

namespace InfiniteMouse.Model
{
    public class RoundButton : Controls.Button
    {
        #region Constantes

        #endregion

        #region Champs

        // Aucun pour l'instant

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty NeonProperty =
            DependencyProperty.Register(
                nameof(Neon),
                typeof(bool),
                typeof(RoundButton),
                new PropertyMetadata(false, OnNeonChanged));

        public static readonly DependencyProperty NeonRadiusProperty =
            DependencyProperty.Register(
                nameof(NeonRadius),
                typeof(double),
                typeof(RoundButton),
                new PropertyMetadata(20.0, OnNeonRadiusChanged));

        public static readonly DependencyProperty NeonColorProperty =
            DependencyProperty.Register(
                nameof(NeonColor),
                typeof(Media.Color),
                typeof(RoundButton),
                new PropertyMetadata(
                    Media.Colors.Cyan,
                    OnNeonColorChanged));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(RoundButton),
                new PropertyMetadata(new CornerRadius(15)));

        #endregion

        #region Propriétés

        public bool Neon
        {
            get => (bool)GetValue(NeonProperty);
            set => SetValue(NeonProperty, value);
        }

        public Media.Color NeonColor
        {
            get => (Media.Color)GetValue(NeonColorProperty);
            set => SetValue(NeonColorProperty, value);
        }

        public double NeonRadius
        {
            get => (double)GetValue(NeonRadiusProperty);
            set => SetValue(NeonRadiusProperty, value);

        }
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }


        #endregion

        #region Constructeur

        public RoundButton()
        {
            Background = Media.Brushes.LightGray;
            Foreground = Media.Brushes.Black;

            BorderThickness = new Thickness(1);

            FontWeight = FontWeights.SemiBold;

            UpdateNeon();

            Template = BuildTemplate();

            MouseEnter += RoundButton_MouseEnter;
            MouseLeave += RoundButton_MouseLeave;

            PreviewMouseLeftButtonDown += RoundButton_Down;
            PreviewMouseLeftButtonUp += RoundButton_Up;
        }

        #endregion

        #region Callbacks

        private static void OnNeonRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RoundButton button)
            {
                button.UpdateNeon();
            }
        }

        private static void OnNeonChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is RoundButton button)
            {
                button.UpdateNeon();
            }
        }

        private static void OnNeonColorChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is RoundButton button)
            {
                button.UpdateNeon();
            }
        }

        #endregion

        #region Événements Souris

        private void RoundButton_MouseEnter(
            object sender,
            System.Windows.Input.MouseEventArgs e)
        {
            Opacity = 0.85;
        }

        private void RoundButton_MouseLeave(
            object sender,
            System.Windows.Input.MouseEventArgs e)
        {
            Opacity = 1;
        }

        private void RoundButton_Down(
            object sender,
            System.Windows.Input.MouseButtonEventArgs e)
        {
            RenderTransform =
                new Media.ScaleTransform(0.97, 0.97);
        }

        private void RoundButton_Up(
            object sender,
            System.Windows.Input.MouseButtonEventArgs e)
        {
            RenderTransform =
                new Media.ScaleTransform(1, 1);
        }

        #endregion

        #region Méthodes Privées

        private void UpdateNeon()
        {
            if (!Neon)
            {
                Effect = null;
                return;
            }

            Effect = new Effects.DropShadowEffect()
            {
                BlurRadius = NeonRadius,
                ShadowDepth = 0,
                Opacity = 1,
                Color = NeonColor
            };
        }

        private static Controls.ControlTemplate BuildTemplate()
        {
            FrameworkElementFactory border =
                new(typeof(Controls.Border));

            border.SetBinding(
                Controls.Border.BackgroundProperty,
                new WPF.Data.Binding(nameof(Background))
                {
                    RelativeSource =
                        new WPF.Data.RelativeSource(
                            WPF.Data.RelativeSourceMode.TemplatedParent)
                });

            border.SetBinding(
                Controls.Border.CornerRadiusProperty,
                new WPF.Data.Binding(nameof(CornerRadius))
                {
                    RelativeSource =
                        new WPF.Data.RelativeSource(
                            WPF.Data.RelativeSourceMode.TemplatedParent)
                });

            border.SetBinding(
                Controls.Border.BorderBrushProperty,
                new WPF.Data.Binding(nameof(BorderBrush))
                {
                    RelativeSource =
                        new WPF.Data.RelativeSource(
                            WPF.Data.RelativeSourceMode.TemplatedParent)
                });

            border.SetBinding(
                Controls.Border.BorderThicknessProperty,
                new WPF.Data.Binding(nameof(BorderThickness))
                {
                    RelativeSource =
                        new WPF.Data.RelativeSource(
                            WPF.Data.RelativeSourceMode.TemplatedParent)
                });

            FrameworkElementFactory presenter =
                new(typeof(Controls.ContentPresenter));

            presenter.SetBinding(
                Controls.ContentPresenter.ContentProperty,
                new WPF.Data.Binding(nameof(Content))
                {
                    RelativeSource =
                        new WPF.Data.RelativeSource(
                            WPF.Data.RelativeSourceMode.TemplatedParent)
                });

            presenter.SetValue(
                Controls.ContentPresenter.HorizontalAlignmentProperty,
                WPF.HorizontalAlignment.Center);

            presenter.SetValue(
                Controls.ContentPresenter.VerticalAlignmentProperty,
                VerticalAlignment.Center);

            border.AppendChild(presenter);

            Controls.ControlTemplate template =
                new(typeof(RoundButton));

            template.VisualTree = border;

            return template;
        }

        #endregion
    }

    public class CheckBoxCustom : Controls.CheckBox
    {
        public static readonly DependencyProperty CornerRadiusProperty =
     DependencyProperty.Register(
         nameof(CornerRadius),
         typeof(CornerRadius),
         typeof(CheckBoxCustom),
         new PropertyMetadata(new CornerRadius(0)));



        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }


        public static readonly DependencyProperty BorderHoverProperty =
    DependencyProperty.Register(
        nameof(BorderHover),
        typeof(Media.Brush),
        typeof(CheckBoxCustom),
        new PropertyMetadata(new Media.SolidColorBrush(
        (Media.Color)Media.ColorConverter.ConvertFromString("#FF2997DE")), OnBorderHoverChanged));

        private static void OnBorderHoverChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CheckBoxCustom cb)
            {
                cb.Template = cb.BuildTemplate();
            }
        }

        public Media.Brush BorderHover
        {
            get => (Media.Brush)GetValue(BorderHoverProperty);
            set => SetValue(BorderHoverProperty, value);
        }


        public static readonly DependencyProperty CheckColorProperty =
    DependencyProperty.Register(
        nameof(CheckColor),
        typeof(Media.Brush),
        typeof(CheckBoxCustom),
        new PropertyMetadata(Media.Brushes.Black, OnCheckColorChanged));

        private static void OnCheckColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CheckBoxCustom cb)
            {
                cb.Template = cb.BuildTemplate();
            }
        }

        public Media.Brush CheckColor
        {
            get => (Media.Brush)GetValue(CheckColorProperty);
            set => SetValue(CheckColorProperty, value);
        }


        public bool Neon
        {
            get => (bool)GetValue(NeonProperty);
            set => SetValue(NeonProperty, value);
        }

        public Media.Color NeonColor
        {
            get => (Media.Color)GetValue(NeonColorProperty);
            set => SetValue(NeonColorProperty, value);
        }

        public double NeonRadius
        {
            get => (double)GetValue(NeonRadiusProperty);
            set => SetValue(NeonRadiusProperty, value);

        }

        public static readonly DependencyProperty NeonProperty =
           DependencyProperty.Register(
               nameof(Neon),
               typeof(bool),
               typeof(CheckBoxCustom),
               new PropertyMetadata(false, OnNeonChanged));

        public static readonly DependencyProperty NeonRadiusProperty =
            DependencyProperty.Register(
                nameof(NeonRadius),
                typeof(double),
                typeof(CheckBoxCustom),
                new PropertyMetadata(20.0, OnNeonRadiusChanged));

        public static readonly DependencyProperty NeonColorProperty =
            DependencyProperty.Register(
                nameof(NeonColor),
                typeof(Media.Color),
                typeof(CheckBoxCustom),
                new PropertyMetadata(
                    Media.Colors.Cyan,
                    OnNeonColorChanged));


        private void UpdateNeon()
        {
            if (!Neon)
            {
                Effect = null;
                return;
            }

            Effect = new Effects.DropShadowEffect()
            {
                BlurRadius = NeonRadius,
                ShadowDepth = 0,
                Opacity = 1,
                Color = NeonColor
            };
        }
        private static void OnNeonRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CheckBoxCustom chk)
            {
                chk.UpdateNeon();
            }
        }

        private static void OnNeonChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is CheckBoxCustom chk)
            {
                chk.UpdateNeon();
            }
        }

        private static void OnNeonColorChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is CheckBoxCustom chk)
            {
                chk.UpdateNeon();
            }
        }

        public CheckBoxCustom()
        {
            Template = BuildTemplate();
            UpdateNeon();
        }

        public Controls.ControlTemplate BuildTemplate()
        {
            FrameworkElementFactory panel =
                new(typeof(Controls.StackPanel));

            panel.SetValue(
                Controls.StackPanel.OrientationProperty,
                Controls.Orientation.Horizontal);

            FrameworkElementFactory border =
                new(typeof(Controls.Border));

            border.Name = "checkBoxBorder";

            border.SetValue(
                Controls.Border.WidthProperty,
                14.0);

            border.SetValue(
                Controls.Border.HeightProperty,
                14.0);

            border.SetBinding(
       Controls.Border.BackgroundProperty,
       new WPF.Data.Binding(nameof(Background))
       {
           RelativeSource =
               new WPF.Data.RelativeSource(
                   WPF.Data.RelativeSourceMode.TemplatedParent)
       });

            border.SetBinding(
                Controls.Border.BorderBrushProperty,
                new WPF.Data.Binding(nameof(BorderBrush))
                {
                    RelativeSource =
                        new WPF.Data.RelativeSource(
                            WPF.Data.RelativeSourceMode.TemplatedParent)
                });

            border.SetBinding(
                Controls.Border.BorderThicknessProperty,
                new WPF.Data.Binding(nameof(BorderThickness))
                {
                    RelativeSource =
                        new WPF.Data.RelativeSource(
                            WPF.Data.RelativeSourceMode.TemplatedParent)
                });
            border.SetBinding(
                Controls.Border.CornerRadiusProperty,
                new WPF.Data.Binding(nameof(CornerRadius))
                {
                    RelativeSource =
                        new WPF.Data.RelativeSource(
                            WPF.Data.RelativeSourceMode.TemplatedParent)
                });

            FrameworkElementFactory path =
                new(typeof(WPF.Shapes.Path));

            path.Name = "optionMark";

            path.SetValue(
                WPF.Shapes.Path.DataProperty,
                Geometry.Parse(
                    "F1 M 9.97498,1.22334L 4.6983,9.09834L 4.52164,9.09834L 0,5.19331L 1.27664,3.52165L 4.255,6.08833L 8.33331,1.52588e-005L 9.97498,1.22334 Z"));

            path.SetValue(
                WPF.Shapes.Path.FillProperty,
                CheckColor);

            path.SetValue(
                FrameworkElement.MarginProperty,
                new Thickness(2));

            path.SetValue(
            FrameworkElement.WidthProperty,
            8.0);

            path.SetValue(
                FrameworkElement.HeightProperty,
                8.0);

            path.SetValue(
                FrameworkElement.HorizontalAlignmentProperty,
                WPF.HorizontalAlignment.Center);

            path.SetValue(
                FrameworkElement.VerticalAlignmentProperty,
                VerticalAlignment.Center);

            path.SetValue(
             WPF.Shapes.Shape.StretchProperty,
             Stretch.Uniform);

            path.SetValue(
                UIElement.OpacityProperty,
                0.0);

            border.AppendChild(path);

            FrameworkElementFactory presenter =
                new(typeof(Controls.ContentPresenter));

            presenter.SetBinding(
                Controls.ContentPresenter.ContentProperty,
                new WPF.Data.Binding(nameof(Content))
                {
                    RelativeSource =
                        new WPF.Data.RelativeSource(
                            WPF.Data.RelativeSourceMode.TemplatedParent)
                });

            presenter.SetValue(
                FrameworkElement.MarginProperty,
                new Thickness(5, 0, 0, 0));

            presenter.SetValue(
                Controls.ContentPresenter.VerticalAlignmentProperty,
                VerticalAlignment.Center);

            panel.AppendChild(border);
            panel.AppendChild(presenter);

            Controls.ControlTemplate template =
                new(typeof(CheckBoxCustom));

            Trigger checkedTrigger = new()
            {
                Property = Controls.Primitives.ToggleButton.IsCheckedProperty,
                Value = true
            };

            Setter checkedSetter = new();
            checkedSetter.Property = UIElement.OpacityProperty;
            checkedSetter.Value = 1.0;
            checkedSetter.TargetName = "optionMark";

            checkedTrigger.Setters.Add(checkedSetter);

            template.Triggers.Add(checkedTrigger);

            Trigger mouseTrigger = new()
            {
                Property = UIElement.IsMouseOverProperty,
                Value = true
            };

            Setter mouseSetter = new();
            mouseSetter.Property = Controls.Border.BorderBrushProperty;
            mouseSetter.Value = BorderHover;
            mouseSetter.TargetName = "checkBoxBorder";

            mouseTrigger.Setters.Add(mouseSetter);

            template.Triggers.Add(mouseTrigger);

            template.VisualTree = panel;

            return template;
        }

    }

}

