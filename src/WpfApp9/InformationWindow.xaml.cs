using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerCompany
{
    /// <summary>
    /// Логика взаимодействия для InformationWindow.xaml
    /// </summary>
    public partial class InformationWindow : Window
    {
        public InformationWindow(string Text, string Title)
        {
            InitializeComponent();

            this.Title = Title;
            this.Text.Text = Text;
        }
    }
}
