using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Windows.Input;

namespace EasyNavigation.Controls;

public partial class AuthControls : UserControl
{
    private bool _isPasswordVisible = false;
    private bool _regIsPasswordVisible = false;

    public static readonly StyledProperty<ICommand?> LoginCommandProperty =
        AvaloniaProperty.Register<AuthControls, ICommand?>(nameof(LoginCommand));

    public ICommand? LoginCommand
    {
        get => GetValue(LoginCommandProperty);
        set => SetValue(LoginCommandProperty, value);
    }

    public static readonly StyledProperty<ICommand?> RegistrationCommandProperty =
        AvaloniaProperty.Register<AuthControls, ICommand?>(nameof(RegistrationCommand));

    public ICommand? RegistrationCommand
    {
        get => GetValue(RegistrationCommandProperty);
        set => SetValue(RegistrationCommandProperty, value);
    }

    public AuthControls()
    {
        InitializeComponent();
    }

    private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;

        if (PasswordTextBox != null)
            PasswordTextBox.PasswordChar = _isPasswordVisible ? '\0' : '*';

        if (ShowPasswordButton != null && HidePasswordButton != null)
        {
            ShowPasswordButton.IsVisible = _isPasswordVisible;
            HidePasswordButton.IsVisible = !_isPasswordVisible;
        }
    }

    private void RegTogglePasswordVisibility(object sender, RoutedEventArgs e)
    {
        _regIsPasswordVisible = !_regIsPasswordVisible;

        if (RegPasswordTextBox != null)
            RegPasswordTextBox.PasswordChar = _regIsPasswordVisible ? '\0' : '*';

        if (RegShowPasswordButton != null && RegHidePasswordButton != null)
        {
            RegShowPasswordButton.IsVisible = _regIsPasswordVisible;
            RegHidePasswordButton.IsVisible = !_regIsPasswordVisible;
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (ShowPasswordButton != null && HidePasswordButton != null)
        {
            ShowPasswordButton.IsVisible = false;
            HidePasswordButton.IsVisible = true;
        }

        if (RegShowPasswordButton != null && RegHidePasswordButton != null)
        {
            RegShowPasswordButton.IsVisible = false;
            RegHidePasswordButton.IsVisible = true;
        }
    }
}
