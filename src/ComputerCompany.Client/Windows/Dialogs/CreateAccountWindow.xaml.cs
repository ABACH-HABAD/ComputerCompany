using System.Windows;
using System.Windows.Media.Imaging;

namespace ComputerCompany.Presentation.Windows.Dialogs;

/// <summary>
/// Логика взаимодействия для WelcomeWindow.xaml
/// </summary>
public partial class CreateAccountWindow : Window
{
    public CreateAccountWindow() { InitializeComponent(); }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Random random = new();

        Uri imageUri = new($"/ComputerCompany.Presentation;component/Resources/Images/WelcomePC{random.Next(1, 5)}.png", UriKind.Relative);

        BitmapImage bitmap = new();
        bitmap.BeginInit();
        bitmap.UriSource = imageUri;
        bitmap.EndInit();

        WelcomeImage.Source = bitmap;
    }
}