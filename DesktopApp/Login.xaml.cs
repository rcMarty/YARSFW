using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using DataLayer;
using DataLayer.DatabaseEntites;

namespace DesktopApp
{
    /// <summary>
    /// Interakční logika pro Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }
       

        private void BLogin_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> loginData = new Dictionary<string, string>();
            loginData.Add("username", TBUsername.Text);
            loginData.Add("password", TBPassword.Password);
            List<User> aa = DBConnection.Select<User>(loginData);
            if(aa.Count == 1)
            {   
                MessageBox.Show("Login successful");
               
                
                MainWindow mainWindow = new MainWindow();

                var thiswindow = (Window)System.Windows.Application.Current.MainWindow;
                MainWindow mw = new MainWindow();
                mw.Show();
                thiswindow.Close();
            }
            else
            {
                LError.Content = "Wrong username or password";
            }
        }
    }
}
