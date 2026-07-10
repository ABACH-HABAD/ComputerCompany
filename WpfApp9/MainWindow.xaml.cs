using ComputerCompany;
using ComputerCompanyClasses;
using Microsoft.Data.Sqlite;
using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ComputerCompanyClasses
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Random rnd = new();
        private readonly Profile profile;

        private readonly ClientComponentPage clientComponentPage = new();
        private readonly ClientMainPage clientMainPage;
        private readonly ClientShoppingCartPage clientShoppingCartPage = new();
        private readonly ClientBuildPage clientBuildPage = new();
        private readonly ClientUpgradePage clientUpgradePage = new();

        private readonly ManagerDataBasePage managerDataBasePage = null!;
        private readonly AdminDataBasePage adminDataBasePage = null!;

        private InformationWindow informationWindow = null!;
        private MusicWindow musicWindow = null!;

        private readonly Account NullableAccount = new(null);
        private Account activeAccount;

        public Account ActiveAccount { get => activeAccount; }

        public MainWindow(Account account)
        {
            activeAccount = account;

            InitializeComponent();

            if (account.AccountType == "Client")
            {
                JobTitle.Text = "";
                BottomPad.Height = new(1, GridUnitType.Star);
            }
            else
            {
                JobTitle.Text = account.DisplayAccountType;
                BottomPad.Height = new(0, GridUnitType.Star);
                if (account.AccountType == "Admin" || account.AccountType == "Manager")
                {
                    DataBaseSlot.Width = new GridLength(1, GridUnitType.Star);
                    managerDataBasePage = new();
                }
                if (account.AccountType == "Admin")
                {
                    EmployesSlot.Width = new GridLength(1, GridUnitType.Star);
                    adminDataBasePage = new(activeAccount);
                }

            }

            UpdateAccountInfo();

            profile = new(activeAccount, this);

            clientMainPage = new(toBuildingPage: Order_Click, toComponentPage: Components_Click, toUpgradePage: Upgrade_Click);
            MainFrame.Navigate(clientMainPage);

            Review.LoadReviews();
            UpdateReview();
            System.Windows.Threading.DispatcherTimer timer = new();

            timer.Tick += new EventHandler(CyclicUpdateReview);
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();

            //Крутой звук
            mediaPlayer = new MediaElement
            {
                LoadedBehavior = MediaState.Manual,
                UnloadedBehavior = MediaState.Stop,
                Volume = 0.5
            };
            ContentGrid.Children.Add(mediaPlayer);
            string tempAudioPath = System.IO.Path.GetTempFileName() + ".wav";
            var resourceUri = new Uri("pack://application:,,,/ComputerCompany;component/Resources/Sounds/YouHaveSuccessfullyLoggedIn.mp3", UriKind.Absolute);
            var streamInfo = Application.GetResourceStream(resourceUri);
            if (streamInfo == null)
            {
                MessageBox.Show($"Аудио-ресурс ({resourceUri}) не найден!");
                return;
            }
            using (var resourceStream = streamInfo.Stream)
            using (var fileStream = File.Create(tempAudioPath))
            {
                resourceStream.CopyTo(fileStream);
            }
            mediaPlayer.Source = new Uri(tempAudioPath);
            mediaPlayer.Play();

        }

        private void CyclicUpdateReview(object? sender, EventArgs e)
        {
            UpdateReview();
        }

        private void UpdateReview()
        {
        SelectReview:
            Review review = Review.Reviews[rnd.Next(0, Review.Reviews.Count)];
            if (ReviewMessage.Text == review.Message) goto SelectReview; //отзывы должны меняться

            imageBorder_review.Background = new ImageBrush(review.Bitmap);
            ReviewMessage.Text = review.Message;
            ReviewStars.Content = review.Stars;
            ReviewNickname.Content = review.Sender;
        }

        public void UpdateAccountInfo()
        {
            AccountName.Content = ActiveAccount.DisplayName;

            MyAccount.ApplyTemplate();
            if (MyAccount.Template?.FindName("imageBorder", MyAccount) is not Border imageBorder)
            {
                MessageBox.Show("Элемент imageBorder не найден в шаблоне!");
                return;
            }

            if (imageBorder.Background is not ImageBrush imageBrush) imageBrush = new ImageBrush();

            if (activeAccount.HasImage)
            {
                imageBrush.ImageSource = activeAccount.Image();
                imageBorder.Background = imageBrush;
            }
            else NoImage(imageBorder, imageBrush);
        }

        private static void NoImage(Border imageBorder, ImageBrush imageBrush)
        {
            Uri imageUri = new("pack://application:,,,/ComputerCompany;component/Resources/Images/NoImage.png", UriKind.Absolute);

            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.UriSource = imageUri;
            bitmap.EndInit();

            imageBrush.ImageSource = bitmap;
            imageBorder.Background = imageBrush;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            activeAccount.LogOut();

            string BackupDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBaseBackups");
            if (!Directory.Exists(BackupDirectory)) Directory.CreateDirectory(BackupDirectory);
            if (Directory.GetFiles(BackupDirectory).Length > 10)
            {
                foreach (var file in Directory.GetFiles(BackupDirectory).Skip(10))
                {
                    File.Delete(file);
                }
            }
            File.Copy(
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "DataBase", "ComputerCompany.db"),
                System.IO.Path.Combine(BackupDirectory, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.db"));

            if (activeAccount != NullableAccount) Application.Current.Shutdown();
        }

        public void Logout()
        {
            activeAccount.LogOut();
            activeAccount = NullableAccount;
            MessageBox.Show("Вы вышли из аккаунта");
            new WelcomeWindow().Show();
            Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e) => Logout();

        private void Player_Click(object sender, RoutedEventArgs e)
        {
            if (musicWindow != null)
            {
                try
                {
                    musicWindow.Show();
                    musicWindow.WindowState = WindowState.Normal;
                }
                catch
                {
                    musicWindow = new();
                    musicWindow.Show();
                }
            }
            else 
            {
                musicWindow = new();
                musicWindow.Show();
            }
        }

        private void MainFrame_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Title = "Информационная система «Компьютерная компания»";
        }

        private void MyAccount_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(profile);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(clientMainPage);
        }

        private void Basket_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(clientShoppingCartPage);
        }

        private void Upgrade_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(clientUpgradePage);
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(clientBuildPage);
        }

        private void Components_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(clientComponentPage);
        }

        private void DataBase_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(managerDataBasePage);
        }

        private void Employers_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(adminDataBasePage);
        }

        private void WeAreInSocialNetworks_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            informationWindow = new(
                "Наши ресурсы: \n" +
                "VK: https://vk.com/feed \n" +
                "Telegram: https://web.telegram.org/ \n"
                , "Мы в соц сетях");
            informationWindow.Show();
        }

        private void StoreLicense_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            informationWindow = new(
    "ЛИЦЕНЗИЯ \n" +

"А 608786\n" +
"Регистрационный номер 981256 от «12» Мая 2025\n" +
"\n" +
"Федеральная служба по регулированию алкогольного рынка\n" +
"разрешает осуществление следующего вида деятельности: розлив, хранение и поставка алкогольной продукции.\n" +
"\n" +
"Лицензия выдана\n" +
"\n" +
"ГУ МВД России по Самарской области" +
"\n" +
"Условия осуществления данного вида деятельности:\n" +
"Средства организации\n" +
"Полное управление\n" +
"М.П.\n" +
"Подпись\n" +
"О.Б. Галанина\n" +
"\n" +
"Сведения о регистрации лицензии на территориях субъектов Российской Федерации\n" +
"\n" +
"М.П.\n" +
"Подпись\n" +
"Г.И. Синих"
    , "Лицензия");
            informationWindow.Show();
        }

        private void AboutTheCompany_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            informationWindow = new(
                "Компания по сборке ПК предлагает услуги по разработке и продаже персональных компьютеров, а также сопутствующие услуги, такие как установка программного обеспечения, настройка оборудования и консультации по выбору комплектующих.\r\nСборка ПК включает выбор и закупку качественных компонентов, таких как материнская плата, процессор, видеокарта, оперативная память, жёсткий диск или SSD, блок питания и корпус. Затем специалисты компании собирают компьютер, устанавливают операционную систему и драйверы, а также проводят тестирование системы на стабильность и производительность.\r\nКак работает компания:\r\n1.\tСобираем компьютеры по индивидуальным заказам\r\n–\tПомогаем подобрать подходящие детали для компьютера\r\n–\tСобираем всё в единую систему\r\n–\tПроверяем, чтобы все части компьютера работали вместе без проблем\r\n–\tУстанавливаем необходимые программы\r\n2.\tНастраиваем программное обеспечение\r\n–\tУстанавливаем Windows или Linux\r\n–\tНастраиваем систему для игр или работы\r\n–\tОптимизируем производительность\r\nКомпания также может предложить услуги по кастомизации ПК, то есть созданию индивидуальных конфигураций компьютера с учётом потребностей клиента. Это может включать установку дополнительных компонентов, таких как звуковые карты, сетевые адаптеры, оптические приводы и другие периферийные устройства.\r\n"
                , "О компании");
            informationWindow.Show();
        }
    }
}