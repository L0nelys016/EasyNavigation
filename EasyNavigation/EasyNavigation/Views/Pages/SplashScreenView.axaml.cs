using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace EasyNavigation.Views.Pages
{
    public partial class SplashScreen : UserControl
    {
        public SplashScreen()
        {
            InitializeComponent();

            this.AttachedToVisualTree += async (_, __) =>
            {
                await AnimateLogoAsync();
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task AnimateLogoAsync()
        {
            Image? logo = this.FindControl<Image>("Logo");
            if (logo == null)
                return;

            logo.Opacity = 0;

            await Task.Delay(500);

            Animation fadeInAnimation = new Animation
            {
                Duration = TimeSpan.FromSeconds(1.5),
                FillMode = FillMode.Forward,
                Children =
                {
                    new KeyFrame
                    {
                        Cue = new Cue(0),
                        Setters = { new Setter(Control.OpacityProperty, 0d) },
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1),
                        Setters = { new Setter(Control.OpacityProperty, 1d) },
                    },
                },
            };

            await fadeInAnimation.RunAsync(logo, CancellationToken.None);
        }
    }
}
