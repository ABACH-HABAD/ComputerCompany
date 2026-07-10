using ComputerCompany;
using ComputerCompanyClasses;
using Microsoft.Data.Sqlite;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;
using static ComputerCompanyClasses.Profile;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ComputerCompanyClasses
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        private ChangeDisplayName changeDisplayName = null!;
        private readonly Account currentAccount;
        private readonly MainWindow main;

        public Account CurrentAccount { get => currentAccount; }
        public MainWindow Main { get => main; }

        public Profile(Account currentAccount, MainWindow main)
        {
            InitializeComponent();

            this.main = main;
            this.currentAccount = currentAccount;

            AccountName.Text = currentAccount.DisplayName;
            Status.Text = currentAccount.DisplayAccountType;
            if (currentAccount.AccountType != "Client") Status.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 125));
            UpdateImage();

            ReviewText.Text = Review.FindReviewByLogin(currentAccount.Login);

        }

        private void UpdateImage()
        {
            byte[] imageData = currentAccount.Bitmap;

            if (currentAccount.HasImage)
            {
                using var stream = new MemoryStream(imageData);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                ProfileImage.Source = bitmap;
            }
            else
            {
                Uri imageUri = new("pack://application:,,,/ComputerCompany;component/Resources/Images/NoImage.png", UriKind.Absolute);

                BitmapImage bitmap = new();
                bitmap.BeginInit();
                bitmap.UriSource = imageUri;
                bitmap.EndInit();

                ProfileImage.Source = bitmap;
            }

            main.UpdateAccountInfo();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Stars.Text = Review.SelectStars((short)StarSlider.Value);
        }

        private void ChangeDisplayName_Click(object sender, RoutedEventArgs e)
        {
            changeDisplayName ??= new(this);
            changeDisplayName.ShowDialog();
        }

        private void ChangeImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Картинки (*.png)|*.png"
            };
            dialog.ShowDialog();
            string filename = dialog.FileName;

            if (filename == string.Empty) return;

            byte[] imageData;
            using (FileStream stream = new(filename, FileMode.Open))
            {
                imageData = new byte[stream.Length];
                stream.Read(imageData, 0, imageData.Length);
            }

            currentAccount.HasImage = true;

            string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Image", "AccountImage.png");
            if (File.Exists(imagePath)) File.Delete(imagePath);
            File.Copy(filename, imagePath);
            ProfileImage.Source = new BitmapImage(new(imagePath, UriKind.Relative));

            currentAccount.UpdateData(Image: imageData);
            UpdateImage();
            Main.UpdateAccountInfo();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            main.Logout();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            Review.SendReview(
                new Review(
                    currentAccount.Login,
                    currentAccount.DisplayName,
                    ReviewText.Text,
                    (long)StarSlider.Value
                    ));

            MessageBox.Show("Отзыв отправлен!");
        }

        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            DeleteAccount delete = new(CurrentAccount);
            if (delete.ShowDialog() == true)
            {
                main.Logout();
            }
        }
    }
}
