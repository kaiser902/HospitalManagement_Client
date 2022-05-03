using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace HospitalManagement_Client
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        string region;
        static string Path = "C:\\Users\\310246678\\Documents\\Visual Studio 2015\\Projects\\HospitalManagement_Client\\";
        string imagesFolder = Path + "HospitalManagement_Client\\images\\";
        string inputName;
        // System.IO.Directory.GetCurrentDirectory();
        UserDataRef.UserDataServiceClient  _userProxy = new UserDataRef.UserDataServiceClient("BasicHttpBinding_IUserDataService");
        DoctorDataRef.DoctorDataServiceClient _doctorProxy = new DoctorDataRef.DoctorDataServiceClient("BasicHttpBinding_IDoctorDataService");
        AdminDataRef.AdminDataServiceClient _adminProxy = new AdminDataRef.AdminDataServiceClient("BasicHttpBinding_IAdminDataService");
        public LoginWindow()
        {
            InitializeComponent();
            //this.StartupLocation.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Rect workArea = System.Windows.SystemParameters.WorkArea;
            this.Left = (workArea.Width - this.Width) / 2 + workArea.Left;
            this.Top = (workArea.Height - this.Height) / 2 + workArea.Top;
            username.Focus();
            regionBox.IsEnabled = false;
            ImgShowHide.Source = new BitmapImage(new Uri(imagesFolder + "Show.jpg"));
            password.Visibility = Visibility.Hidden;
            txtPasswordbox.Visibility = Visibility.Visible;
        }

        private void Username_GotFocus(object sender, MouseButtonEventArgs e)
        {
            userErrorBlock.Text = "";
            loginErrorBlock.Text = "";
            passwordErrorBlock.Text = "";

        }

        private void Password_GotFocus(object sender, MouseButtonEventArgs e)
        {
            passwordErrorBlock.Text = "";
        }


        private void OnLoginButton_Click(object sender, RoutedEventArgs e)
        {
            string role = "";
            if (username.Text.Length > 1 && password.Text.Length > 1)
            {
                if (!Regex.IsMatch(username.Text, @"^[A-Za-z]*$"))
                {
                    userErrorBlock.Text = "Only use alphabets !";
                    username.Text = "";
                    return;
                }
                UserDataRef.User temp = new UserDataRef.User();
                temp.Name = username.Text;
                inputName = username.Text;
                temp.Password = password.Text;
                role = _userProxy.AuthenticateUser(temp).ToString();
                if (role != "")
                 {
                    role = role.ToString();
                    if (role.Equals("doctor"))
                    {
                        username.IsEnabled = false;
                        password.IsEnabled = false;
                        clearButton.IsEnabled = false;
                        loginButton.Visibility = System.Windows.Visibility.Hidden;
                        cancelButton.Visibility = System.Windows.Visibility.Visible;
                        List<string> regions = new List<string>();
                        regions.Add("Select a region--");
                        List<string> regionList;
                        try
                        {
                            regionList = _doctorProxy.GetRegions().ToList<string>();
                            foreach (string s in regionList)
                            {
                                regions.Add(s);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                        }

                        regionBox.ItemsSource = regions;
                        regionBox.SelectedIndex = 0;
                        regionBox.IsEnabled = true;
                        regionBox.Visibility = System.Windows.Visibility.Visible;

                    }
                    else
                    {
                        AdminDataRef.Admin _admin = new AdminDataRef.Admin();
                        _admin.Name = temp.Name;
                        _admin.Password = password.Text;
                        _admin.Role = role;
                        _admin.Id = _adminProxy.Get_Id(_admin);
                        _admin.Privileges = _adminProxy.Get_Priviledge(_admin.Id);
                         AdminWindow _adminwindow = new AdminWindow(_admin);
                         _adminwindow.Show();
                         this.Close();
                    }
                }
                 else
                  {
                    loginErrorBlock.Text = "Username or Password is Invalid !";
                    username.Text = "";
                    password.Text = "";
                    txtPasswordbox.Clear();
                    }
                }
                else
                {
                    if (username.Text.Length < 1)
                    {
                        userErrorBlock.Text = "Username can't be blank";
                    }
                    if (password.Text.Length < 1)
                    {
                        passwordErrorBlock.Text = "Password can't be blank";
                    }
                }
        }
         
        private void OnClearButton_Click(object sender, RoutedEventArgs e)
        {
            username.Text = " ";
            password.Clear();
            txtPasswordbox.Clear();
            userErrorBlock.Text = "";
            passwordErrorBlock.Text = "";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            regionBox = sender as ComboBox;
            string value = regionBox.SelectedItem as string;
            if (!(regionBox.SelectedItem as string).Equals("Select a region--"))
            {
                region = regionBox.SelectedItem as string;
                string data = username.Text + '%' + region;
                string docIdByRegion = _doctorProxy.GetDoctorIdByRegion(data);
                DoctorDataRef.Doctor tempUser = new DoctorDataRef.Doctor();
                tempUser.Id = docIdByRegion;
                tempUser.Name = inputName;
                string roleByDocId = _doctorProxy.AuthenticateDoctorByRegion(tempUser);
                if(roleByDocId.Equals("doctor"))
                {
                    DoctorWindow _doctorWindow = new DoctorWindow(tempUser);
                    _doctorWindow.Show();
                    this.Hide();
                }
                else
                {
                    loginErrorBlock.Text = "Selct your own region!";

                }
           }
        }

        private void OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            username.IsEnabled = true;
            password.IsEnabled = true;
            username.Text = "";
            password.Text = "";
            loginErrorBlock.Text = "";
            txtPasswordbox.Clear();
            regionBox.IsEnabled = false;
            regionBox.Visibility = System.Windows.Visibility.Hidden;
            cancelButton.Visibility = System.Windows.Visibility.Hidden;
            loginButton.Visibility = System.Windows.Visibility.Visible;
            clearButton.IsEnabled = true;
        }

        private void ImgShowHide_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePassword();
        }
        private void ImgShowHide_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPassword();
        }
        private void ImgShowHide_MouseLeave(object sender, MouseEventArgs e)
        {
            HidePassword();
        }
        private void txtPasswordbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPasswordbox.Password.Length > 0)
                ImgShowHide.Visibility = Visibility.Visible;
            else
                ImgShowHide.Visibility = Visibility.Hidden;
            password.Text = txtPasswordbox.Password;

        }
        void ShowPassword()
        {
            ImgShowHide.Source = new BitmapImage(new Uri(imagesFolder + "\\Hide.jpg"));
            password.Visibility = Visibility.Visible;
            txtPasswordbox.Visibility = Visibility.Hidden;
            password.Text = txtPasswordbox.Password;
        }
        void HidePassword()
        {
            ImgShowHide.Source = new BitmapImage(new Uri(imagesFolder + "\\Show.jpg"));
            password.Visibility = Visibility.Hidden;
            txtPasswordbox.Visibility = Visibility.Visible;
            txtPasswordbox.Focus();
        }

    }
}
