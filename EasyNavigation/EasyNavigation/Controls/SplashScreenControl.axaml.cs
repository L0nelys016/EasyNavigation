using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Styling;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasyNavigation.Controls;

public partial class SplashScreenControl : UserControl
{
    public static readonly StyledProperty<string?> SourceProperty =
        AvaloniaProperty.Register<SplashScreenControl, string?>(nameof(Source));

    public string? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly StyledProperty<Bitmap?> ImageSourceProperty =
        AvaloniaProperty.Register<SplashScreenControl, Bitmap?>(nameof(ImageSource));

    public Bitmap? ImageSource
    {
        get => GetValue(ImageSourceProperty);
        private set => SetValue(ImageSourceProperty, value);
    }

    public static readonly StyledProperty<ICommand?> SplashScreenCommandProperty =
        AvaloniaProperty.Register<SplashScreenControl, ICommand?>(nameof(SplashScreenCommand));

    public ICommand? SplashScreenCommand
    {
        get => GetValue(SplashScreenCommandProperty);
        set => SetValue(SplashScreenCommandProperty, value);
    }

    public SplashScreenControl()
    {
        InitializeComponent();

        this.GetObservable(SourceProperty).Subscribe(LoadImage);

        this.AttachedToVisualTree += async (_, __) =>
        {
            await AnimateAsync();
        };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void LoadImage(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            ImageSource = null;
            return;
        }

        try
        {
            if (path.StartsWith("avares://"))
            {
                var assets = AssetLoader.Open(new Uri(path));
                ImageSource = new Bitmap(assets);
            }
            else if (File.Exists(path))
            {
                using var stream = File.OpenRead(path);
                ImageSource = new Bitmap(stream);
            }
        }
        catch
        {
            ImageSource = null;
        }
    }

    private async Task AnimateAsync()
    {
        var logo = this.FindControl<Image>("AnimatedImage");
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

        await Task.Delay(3000);

        if (SplashScreenCommand?.CanExecute(null) == true)
            SplashScreenCommand.Execute(null);
    }
}