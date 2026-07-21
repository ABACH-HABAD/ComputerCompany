using System.Windows;
using System.Windows.Controls;

namespace ComputerCompany.Presentation.Services.Helpers;

public static class PasswordBoxHelper
{
    public static readonly DependencyProperty BoundPasswordProperty =
    DependencyProperty.RegisterAttached
    (
        "BoundPassword",
        typeof(string),
        typeof(PasswordBoxHelper),
        new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged)
        {
            BindsTwoWayByDefault = true,
            DefaultUpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged
        }
    );

    public static readonly DependencyProperty BindPasswordProperty =
    DependencyProperty.RegisterAttached
    (
        "BindPassword",
        typeof(bool),
        typeof(PasswordBoxHelper),
        new PropertyMetadata(false, OnBindPasswordChanged)
    );

    public static void SetBoundPassword(DependencyObject dp, string value)
    {
        dp.SetValue(BoundPasswordProperty, value);
    }

    public static string GetBoundPassword(DependencyObject dp)
    {
        return (string)dp.GetValue(BoundPasswordProperty);
    }

    public static void SetBindPassword(DependencyObject dp, bool value)
    {
        dp.SetValue(BindPasswordProperty, value);
    }

    public static bool GetBindPassword(DependencyObject dp)
    {
        return (bool)dp.GetValue(BindPasswordProperty);
    }

    private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PasswordBox passwordBox)
        {
            //Отключаем обработчик, чтобы избежать бесконечного цикла
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            var newPassword = (string)e.NewValue ?? string.Empty;

            //Обновляем пароль только если он изменился
            if (passwordBox.Password != newPassword)
            {
                passwordBox.Password = newPassword;
            }

            //Включаем обработчик обратно
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }
    }

    private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
    {
        if (dp is PasswordBox passwordBox)
        {
            var wasBound = (bool)e.OldValue;
            var isBound = (bool)e.NewValue;

            if (wasBound)
            {
                // Отписываемся от событий
                passwordBox.Loaded -= PasswordBox_Loaded;
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
            }

            if (isBound)
            {
                // Подписываемся на события
                passwordBox.Loaded += PasswordBox_Loaded;
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }
    }

    private static void PasswordBox_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox passwordBox)
        {
            var password = GetBoundPassword(passwordBox);
            if (passwordBox.Password != password)
            {
                passwordBox.Password = password;
            }
        }
    }

    private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox passwordBox)
        {
            var newPassword = passwordBox.Password;
            var currentBoundPassword = GetBoundPassword(passwordBox);

            if (currentBoundPassword != newPassword)
            {
                SetBoundPassword(passwordBox, newPassword);
            }
        }
    }
}