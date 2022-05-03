using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace HospitalManagement_Client
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        UserDataRef.UserDataServiceClient _userProxy = new UserDataRef.UserDataServiceClient("BasicHttpBinding_IUserDataService");
        PatientDataRef.PatientDataServiceClient _patientProxy = new PatientDataRef.PatientDataServiceClient("BasicHttpBinding_IPatientDataService");
        DoctorDataRef.DoctorDataServiceClient _doctorProxy = new DoctorDataRef.DoctorDataServiceClient("BasicHttpBinding_IDoctorDataService");
        AdminDataRef.AdminDataServiceClient _adminProxy = new AdminDataRef.AdminDataServiceClient("BasicHttpBinding_IAdminDataService");
        int _currentAdminPriviledge;
        BitmapImage img;
        string patientId;
        public AdminWindow(AdminDataRef.Admin _admin)
        {
            InitializeComponent();
            Rect workArea = System.Windows.SystemParameters.WorkArea;
            this.Left = (workArea.Width - this.Width) / 2 + workArea.Left;
            this.Top = (workArea.Height - this.Height) / 2 + workArea.Top;
            _currentAdminPriviledge = _admin.Privileges;
            Adminlabel.Content = "Welcome " + _admin.Name.ToUpper();
            label_Id.Content = "ID : " + _admin.Id;
        }

        public void refresh()
        {
            DocTab.Visibility = Visibility.Hidden;
            AdminTab.Visibility = Visibility.Hidden;
            PatientTab.Visibility = Visibility.Hidden;
            ReportsTab.Visibility = Visibility.Hidden;
            dataGrid.ItemsSource = null;
            tabControl.SelectedIndex = 0;
            DocRoleText.Text = "";
            DocIdText.Text = "";
            AdminIdText.Text = "";
            PatientIdText.Text = "";
            ReportType.Text = "";
            DataSet ds = new DataSet();
            if (SelectTab.Header.Equals("Doctor Table"))
            {
                ds = _doctorProxy.GetDoctorList();
                DataTable dt = ds.Tables[0];
                dataGrid.ItemsSource = dt.DefaultView;
                dataGrid.Columns[4].Visibility = Visibility.Hidden;
            }
            else if (SelectTab.Header.Equals("Admin Table"))
            {
                ds = _adminProxy.GetAdminDisplayed();
                DataTable dt = ds.Tables[0];
                dataGrid.ItemsSource = dt.DefaultView;
                dataGrid.Columns[4].Visibility = Visibility.Hidden;
            }
            else if (SelectTab.Header.Equals("Patient Table"))
            {
                ds = _patientProxy.GetPatientList();
                DataTable dt = ds.Tables[0];
                dataGrid.ItemsSource = dt.DefaultView;
            }
            //SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable("Patient");
            //sda.Fill(dt);
        }

        public void DocTabControls_ifUpdate(DataRowView row)
        {
            DocTab.Visibility = Visibility.Visible;
            AdminTab.Visibility = Visibility.Hidden;
            PatientTab.Visibility = Visibility.Hidden;
            ReportsTab.Visibility = Visibility.Hidden;
            tabControl.SelectedIndex = 1;
            DocTab.Header = "Update Doctor";
            DocIdText.IsReadOnly = false;
            DocIdText.Text = row["Id"].ToString();
            DocIdText.IsReadOnly = true;
            DocRoleText.Text = row["role"].ToString();
            DocRegionText.Text = row["region"].ToString();
            DocUsernameText.Text = row["name"].ToString();
            DocPasswordText.Text = row["password"].ToString();
            DocAddButton.Visibility = Visibility.Hidden;
            DocUpdateButton.Visibility = Visibility.Visible;
        }

        public void DocTabControls_ifAdd()
        {
            DocTab.Visibility = Visibility.Visible;
            AdminTab.Visibility = Visibility.Hidden;
            PatientTab.Visibility = Visibility.Hidden;
            ReportsTab.Visibility = Visibility.Hidden;
            tabControl.SelectedIndex = 1;
            DocTab.Header = "Add Doctor";
            tabControl.SelectedIndex = 1;
            DocIdText.IsReadOnly = false;
            DocIdText.Text = "";
            DocRoleText.Text = "doctor";
            DocRegionText.Text = "";
            DocUsernameText.Text = "";
            DocPasswordText.Text = "";
            DocUpdateButton.Visibility = Visibility.Hidden;
            DocAddButton.Visibility = Visibility.Visible;
        }

        public void AdminTabControls_ifUpdate(DataRowView row)
        {
            DocTab.Visibility = Visibility.Hidden;
            AdminTab.Visibility = Visibility.Visible;
            PatientTab.Visibility = Visibility.Hidden;
            ReportsTab.Visibility = Visibility.Hidden;
            tabControl.SelectedIndex = 2;
            AdminIdText.IsReadOnly = false;
            AdminIdText.Text = row["Id"].ToString();
            AdminIdText.IsReadOnly = true;
            AdminRoleText.Text = row["role"].ToString();
            PriviledgeText.IsReadOnly = false;
            PriviledgeText.Text = row["priviledge"].ToString();


            if (Convert.ToInt32(row["priviledge"].ToString()) == _currentAdminPriviledge || _currentAdminPriviledge==1)
            {
                AdminUsernameLabel.Visibility = Visibility.Visible;
                AdminPasswordLabel.Visibility = Visibility.Visible;
                AdminUsernameText.Visibility = Visibility.Visible;
                AdminPasswordText.Visibility = Visibility.Visible;

                
                AdminUsernameText.Text = row["name"].ToString();
               // AdminPasswordText.Text = row["password"].ToString();
                if(Convert.ToInt32(row["priviledge"].ToString())==1)
                    PriviledgeText.IsReadOnly = true;
            }
            else
            {
                AdminUsernameLabel.Visibility = Visibility.Hidden;
                AdminPasswordLabel.Visibility = Visibility.Hidden;
                AdminUsernameText.Visibility = Visibility.Hidden;
                AdminPasswordText.Visibility = Visibility.Hidden;
                AdminUsernameText.Text = row["name"].ToString();
               // AdminPasswordText.Text = row["password"].ToString();
            }
            AdminTab.Visibility = Visibility.Visible;
            AdminTab.Header = "Update Admin";
            AdminAddButton.Visibility = Visibility.Hidden;
            AdminUpdateButton.Visibility = Visibility.Visible;

        }

        public void AdminTabControls_ifAdd()
        {
            DocTab.Visibility = Visibility.Hidden;
            AdminTab.Visibility = Visibility.Visible;
            PatientTab.Visibility = Visibility.Hidden;
            ReportsTab.Visibility = Visibility.Hidden;
            tabControl.SelectedIndex = 2;
            AdminIdText.IsReadOnly = false;
            AdminIdText.Text = "";
            AdminRoleText.Text = "admin";
            AdminTab.Header = "Add Admin";
            AdminUsernameText.Text = "";
            AdminPasswordText.Text = "";
            PriviledgeText.Text = "3";

            AdminUpdateButton.Visibility = Visibility.Hidden;
            AdminAddButton.Visibility = Visibility.Visible;

        }

        public void PatientTabControls_ifUpdate(DataRowView row)
        {
            DocTab.Visibility = Visibility.Hidden;
            AdminTab.Visibility = Visibility.Hidden;
            PatientTab.Visibility = Visibility.Visible;
            ReportsTab.Visibility = Visibility.Hidden;
            tabControl.SelectedIndex = 3;
            PatientIdText.IsReadOnly = false;
            PatientIdValidation.Visibility = Visibility.Hidden;
            PatientIdText.Text = row["id"].ToString();
            button_Report.Visibility = Visibility.Visible;
            PatientIdText.IsReadOnly = true;
            PatientDocText.Text = row["docId"].ToString();

            PatientNameText.Text = row["name"].ToString();
            PatientAgeText.Text = row["age"].ToString();

            PatientTab.Header = "Update Patient";
            PatientAddButton.Visibility = Visibility.Hidden;
            PatientUpdateButton.Visibility = Visibility.Visible;
        }

        public void PatientTabControls_ifAdd()
        {
            DocTab.Visibility = Visibility.Hidden;
            AdminTab.Visibility = Visibility.Hidden;
            PatientTab.Visibility = Visibility.Visible;
            ReportsTab.Visibility = Visibility.Hidden;
            tabControl.SelectedIndex = 3;
            button_Report.Visibility = Visibility.Hidden;
            PatientIdValidation.Visibility = Visibility.Visible;
            PatientIdText.IsReadOnly = false;
            tabControl.SelectedIndex = 3;
            PatientIdText.Text = "";
            PatientDocText.Text = "";
            PatientNameText.Text = "";
            PatientAgeText.Text = "";
            PatientTab.Header = "Add Patient";
            PatientUpdateButton.Visibility = Visibility.Hidden;
            PatientAddButton.Visibility = Visibility.Visible;
        }

        public void Delete_doctor(DataRowView row)
        {
            string docID = row["Id"].ToString();
            
            int IfPatientExists = _patientProxy.CheckPatientForDoctor(docID);
            string sMessageBoxText;
            string sCaption;
            MessageBoxButton btnMessageBox;
            MessageBoxImage icnMessageBox;
            MessageBoxResult rsltMessageBox;
            if (IfPatientExists == 1)
            {
                sMessageBoxText = "Cannot delete Doctor! \nPatient for doctor exists.";
                sCaption = "Error";
                btnMessageBox = MessageBoxButton.OK;
                icnMessageBox = MessageBoxImage.Warning;

                rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                return;
            }
            else
            {
                sMessageBoxText = "Are you sure you want to delete doctor " + row["Id"] + " ?";
                sCaption = "Confirmation message";
                btnMessageBox = MessageBoxButton.YesNoCancel;
            }
            icnMessageBox = MessageBoxImage.Warning;
            rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            
            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    string docIdToDelete = row["Id"].ToString();
                    _doctorProxy.Delete_Doctor(docIdToDelete);
                    break;

                case MessageBoxResult.No:
                    break;

                case MessageBoxResult.Cancel:
                    /* ... */
                    break;
            }
            refresh();
        }
        public void Delete_admin(DataRowView row)
        {

            string sMessageBoxText = "Are you sure you want to delete admin " + row["Id"] + " ?";
            string sCaption = "Confirmation message";
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            if (Convert.ToInt32(row["priviledge"]) == 1)
            {
                sMessageBoxText = "Cannot delete superadmin!";
                sCaption = "Error";
                btnMessageBox = MessageBoxButton.OK;
            }
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    string adminIdToDelete = row["Id"].ToString();
                    _adminProxy.Delete_Admin(adminIdToDelete);
                    break;

                case MessageBoxResult.No:
                    break;

                case MessageBoxResult.Cancel:
                    /* ... */
                    break;
                case MessageBoxResult.OK:
                    break;
            }
            refresh();

        }
        public void Delete_patient(DataRowView row)
        {
            string sMessageBoxText = "Are you sure you want to delete patient " + row["id"] + " ?";
            string sCaption = "Confirmation message";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);


            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    string patientIdToDelete = row["id"].ToString();
                    _patientProxy.Delete_Patient(patientIdToDelete);
                    break;

                case MessageBoxResult.No:
                    break;

                case MessageBoxResult.Cancel:
                    /* ... */
                    break;
            }
            refresh();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = _adminProxy.GetAdminList();
                dataGrid.ItemsSource = ds.Tables[0].AsEnumerable();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DocTab.Visibility = Visibility.Hidden;
            AdminTab.Visibility = Visibility.Hidden;
            PatientTab.Visibility = Visibility.Hidden;
            ReportsTab.Visibility = Visibility.Hidden;
            if (_currentAdminPriviledge == 1)
            {
                ViewAdmin.Visibility = Visibility.Visible;
                ViewDoctor.Visibility = Visibility.Visible;
                ViewPatient.Visibility = Visibility.Visible;
            }
            else if (_currentAdminPriviledge == 2)
            {
                ViewAdmin.Visibility = Visibility.Collapsed;
                ViewDoctor.Visibility = Visibility.Visible;
                ViewPatient.Visibility = Visibility.Collapsed;
            }
            else if (_currentAdminPriviledge == 3)
            {
                ViewAdmin.Visibility = Visibility.Collapsed;
                ViewDoctor.Visibility = Visibility.Collapsed;
                ViewPatient.Visibility = Visibility.Visible;
            }
            else if (_currentAdminPriviledge == 23 || _currentAdminPriviledge == 32)
            {
                ViewAdmin.Visibility = Visibility.Collapsed;
                ViewPatient.Visibility = Visibility.Visible;
                ViewDoctor.Visibility = Visibility.Visible;
            }
            
        }


        //Show patient Code
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 0;
            SelectTab.Header = "Patient Table";
            DataSet ds = new DataSet();
            try
            {
                ds = _patientProxy.GetPatientList();
                DataTable dt = ds.Tables[0];
                dataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        //Show Doctor Code
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 0;
            SelectTab.Header = "Doctor Table";
            DataSet ds = new DataSet();
            try
            {
                ds = _doctorProxy.GetDoctorList();
                DataTable dt = ds.Tables[0];
                dataGrid.ItemsSource = dt.DefaultView;
                dataGrid.Columns[4].Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        //Show Admin Code
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 0;
            SelectTab.Header = "Admin Table";
            DataSet ds = new DataSet();
            try
            {
                ds = _adminProxy.GetAdminDisplayed();
                DataTable dt = ds.Tables[0];
                dataGrid.ItemsSource = dt.DefaultView;
                dataGrid.Columns[4].Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }



        //For context menu options add and update
        private void MenuItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //DataRowView row = (DataRowView)dataGrid.SelectedItems[0];

            MenuItem m = (MenuItem)sender;

            if (SelectTab.Header.Equals("Doctor Table"))
            {
                
                if (m.Header.Equals("Add"))
                {
                    DocTabControls_ifAdd();
                }
                else if (m.Header.Equals("Update"))
                {
                    DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                    DocTabControls_ifUpdate(row);
                }
            }


            else if (SelectTab.Header.Equals("Admin Table"))
            {
                if (m.Header.Equals("Add"))
                {
                    AdminTabControls_ifAdd();
                }
                else if (m.Header.Equals("Update"))
                {
                    DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                    if (Convert.ToInt32(row["priviledge"].ToString()) == 1 && _currentAdminPriviledge != 1 )
                    {
                        string sMessageBoxText = "Cannot update superadmin!";
                        string sCaption = "Error";
                        MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                        MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
                        MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                        return;
                    }
                    
                    AdminTabControls_ifUpdate(row);
                }

            }
            else if (SelectTab.Header.Equals("Patient Table"))
            {
               
                if (m.Header.Equals("Add"))
                {
                    PatientTabControls_ifAdd();

                }
                else if (m.Header.Equals("Update"))
                {
                    DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                    PatientTabControls_ifUpdate(row);
                }
            }
        }

        //to add admin ;
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string _adminid = AdminIdText.Text;
            string role = AdminRoleText.Text;
            string username = AdminUsernameText.Text;
            string password = AdminPasswordText.Text;
            int priviledge = Convert.ToInt32(PriviledgeText.Text);

            if (priviledge == 1)
            {
                string sMessageBoxText = "Cannot add superadmin!";
                string sCaption = "Error";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                return;
            }
            

            string _commandText = "proc_select_userTable";
            int status = _userProxy.CheckId_replication(_adminid,_commandText); 
            if (status == 0)
            {
                string sMessageBoxText = "ID already exists!";
                string sCaption = "Error";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                return;
            }
            AdminDataRef.Admin adminUser = new AdminDataRef.Admin();
            adminUser.Id = _adminid;
            adminUser.Name = username;
            adminUser.Password = password;
            adminUser.Role = role;
            adminUser.Privileges = priviledge;
            _adminProxy.Add_Admin(adminUser);
            tabControl.SelectedIndex = 0;
            AdminTab.Visibility = Visibility.Hidden;
            refresh();
        }

        


        //to add doctor
        private void button_Click(object sender, RoutedEventArgs e)
        {
            string _docId = DocIdText.Text;
            string role = DocRoleText.Text;
            string region = DocRegionText.Text;
            string username = DocUsernameText.Text;
            string password = DocPasswordText.Text;

            string _commandText = "proc_select_UserId";
            int status = _userProxy.CheckId_replication(_docId, _commandText);
            if (status == 0)
            {
                string sMessageBoxText = "ID already exists!";
                string sCaption = "Error";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                return;
            }
            DoctorDataRef.Doctor _docUser= new DoctorDataRef.Doctor();
            _docUser.Id = _docId;
            _docUser.Name = username;
            _docUser.Region = region;
            _docUser.Password = password;
            _docUser.Role = role;
            status = _doctorProxy.Add_Doctor(_docUser);
            tabControl.SelectedIndex = 0;   
            DocTab.Visibility = Visibility.Hidden;
            refresh();
        }

        //to add patients
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            string _patientId = PatientIdText.Text;
            string docId = PatientDocText.Text;
            string age = PatientAgeText.Text;
            string name = PatientNameText.Text;
            
            string _commandText = "proc_select_Patient";
            int status1 = _userProxy.CheckId_replication(_patientId, _commandText);

            _commandText = "proc_select_DeletedPatient";
            int status2 = _userProxy.CheckId_replication(_patientId, _commandText);
            if (status1 == 0 || status2 == 0)
            {
                string sMessageBoxText = "Patient ID already exists!";
                string sCaption = "Error";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                return;
            }

            int flag = _doctorProxy.VerifyDoctor(docId);

            if (flag == 0)
            {
                string sMessageBoxText = "Doctor Provided does not exist\n Please provide valid doctor!";
                string sCaption = "Error";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                return;
            }
            PatientDataRef.Patient _patient = new PatientDataRef.Patient();
            _patient.Name = name;
            _patient.DocId = docId;
            _patient.Age = Int32.Parse(age);
            _patient.Id = _patientId;
            int status = _patientProxy.Add_Patient(_patient);
            tabControl.SelectedIndex = 0;
            PatientTab.Visibility = Visibility.Hidden;
            refresh();
        }

        //to update doctor
        private void DocUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string id = DocIdText.Text;
            string region = DocRegionText.Text;
            string role = DocRoleText.Text;
            string username = DocUsernameText.Text;
            string password = DocPasswordText.Text;
            DoctorDataRef.Doctor _doctor = new DoctorDataRef.Doctor();
            _doctor.Id = id;
            _doctor.Name = username;
            _doctor.Region = region;
            _doctor.Role = role;
            _doctor.Password = password;
            int status = _doctorProxy.Update_Doctor(_doctor);
            DocTab.Visibility = Visibility.Hidden;
            tabControl.SelectedIndex = 0;
            refresh();
        }

        //to display doctor details corresponding to id
        private void DocIdText_TextChanged(object sender, TextChangedEventArgs e)
        {
            string id = DocIdText.Text;

            if (DocTab.Header.Equals("Add Doctor"))
            {
                string _commandText = "proc_select_DocId";
                int status = _userProxy.CheckId_replication(id, _commandText);
                if (status == 0)
                    DocIdValidation.Content = "Id already exists";
                else
                    DocIdValidation.Content = "";
            }
            else if (DocTab.Header.Equals("Update Doctor"))
            {
                DocIdValidation.Content = "";
                DataSet ds = new DataSet();
                ds = _doctorProxy.GetDoctorList();
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if ((r["Id"].ToString()).Equals(id))
                    {
                        DocRegionText.Text = r["region"].ToString();
                        DocRoleText.Text = r["role"].ToString();
                    }
                }            
            }
        }

        private void PatientCancelButton_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 0;
            PatientTab.Visibility = Visibility.Hidden;
            refresh();
        }

        private void AdminCancelButton_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 0;
            AdminTab.Visibility = Visibility.Hidden;
            refresh();
        }

        private void DoccancelButton_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 0;
            DocTab.Visibility = Visibility.Hidden;
            refresh();
        }

        private void AdminIdText_TextChanged(object sender, TextChangedEventArgs e)
        {
            string id = AdminIdText.Text;
            if (AdminTab.Header.Equals("Add Admin"))
            {
                string _commandText = "proc_select_AdminTable";
                int status = _userProxy.CheckId_replication(id, _commandText);
                if (status == 0)
                    AdminIdValidation.Content = "id already exists";
                else
                    AdminIdValidation.Content = "";
            }
            else if (AdminTab.Header.Equals("Update Admin"))
            {
                AdminRoleText.Text = "admin";
                AdminIdValidation.Content = "";
                DataSet ds = new DataSet();
                ds = _adminProxy.GetAdminList();
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if ((r["Id"].ToString()).Equals(id))
                    {
                        AdminRoleText.Text = r["role"].ToString();
                    }
                }

            }
        }

        private void PatientIdText_TextChanged(object sender, TextChangedEventArgs e)
        {
            string id = PatientIdText.Text;

            if (PatientTab.Header.Equals("Add Patient"))
            {
                string _commandText = "proc_select_Patient";
                int status = _userProxy.CheckId_replication(id, _commandText);
                if (status == 0)
                {
                    PatientIdValidation.Content = "id already exists";
                }

                else
                    PatientIdValidation.Content = "";
            }
            else if (PatientTab.Header.Equals("Update Patient"))
            {
                PatientIdValidation.Content = "";
                DataSet ds = new DataSet();
                ds = _patientProxy.GetPatientList();
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if ((r["Id"].ToString()).Equals(id))
                    {
                        PatientDocText.Text = r["docId"].ToString();
                        PatientNameText.Text = r["name"].ToString();
                        PatientAgeText.Text = r["age"].ToString();
                    }
                }
            }
        }

     
        //to update admin
        private void AdminUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string id = AdminIdText.Text;
            string role = AdminRoleText.Text;
            string username = AdminUsernameText.Text;
            string password = AdminPasswordText.Text;
            int priviledge = Convert.ToInt32(PriviledgeText.Text);

            if (PriviledgeText.IsReadOnly==false)
            {
                if (priviledge==1)
                {
                    MessageBox.Show("There can only be one SuperAdmin");
                    return;
                }
            }
            AdminDataRef.Admin _adminUser = new AdminDataRef.Admin();
            _adminUser.Id = id;
            _adminUser.Role = role;
            _adminUser.Name = username;
            _adminUser.Password = password;
            _adminUser.Privileges = priviledge;
            int status = _adminProxy.Update_Admin(_adminUser);
            AdminTab.Visibility = Visibility.Hidden;
            tabControl.SelectedIndex = 0;
            refresh();
        }

        //to update patient
        private void PatientUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string id = PatientIdText.Text;
            string docId = PatientDocText.Text;
            string name = PatientNameText.Text;
            int age = Convert.ToInt32(PatientAgeText.Text);
            int flag = _doctorProxy.VerifyDoctor(docId);
            if (flag == 0)
            {
                string sMessageBoxText = "Doctor Provided does not exist\n Please provide valid doctor!";
                string sCaption = "Error";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                return;
            }
            PatientDataRef.Patient _patient = new PatientDataRef.Patient();
            _patient.Id = id;
            _patient.DocId = docId;
            _patient.Name = name;
            _patient.Age = age;
            int status = _patientProxy.Update_Patient(_patient);
    
            PatientTab.Visibility = Visibility.Hidden;
            tabControl.SelectedIndex = 0;
            refresh();
        }

        //for context menu option delete
        private void MenuItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
            string _deletedId = row[0].ToString();
            //string _deletedId = dataGrid.SelectedCells[0].ToString(); 
            if (SelectTab.Header.Equals("Doctor Table"))
                Delete_doctor(row);
            else if (SelectTab.Header.Equals("Admin Table"))
                Delete_admin(row);
            else if (SelectTab.Header.Equals("Patient Table"))
                Delete_patient(row);
        }

      

        private void Upload_Button_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an image !!";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            op.FilterIndex = 1;
            if (op.ShowDialog() == true)
            {
                patientId = PatientIdText.Text;
                string ext = System.IO.Path.GetExtension(op.FileName);
                Uploaded_FileLocation.Content = op.FileName;
                img = new BitmapImage(new Uri(op.FileName));
            }
            else
            {
                Uploaded_FileLocation.Content = "You didn't select any image file....";
            }
        }

        private void button_AddReport_Click(object sender, RoutedEventArgs e)
        {
            string reportType = ReportType.Text;
            string reportId = ReportId.Text;
            string date = dateTime.Text;
            string commandText = "";
            string location = Uploaded_FileLocation.Content.ToString();
            if (reportType.Equals("Xray"))
            {
                reportId = "xray" + reportId;
                string xrayLoc = _patientProxy.getXrayImgLocation(patientId);
                commandText = "proc_insert_XrayReport";
                System.IO.File.Copy(location, xrayLoc + "\\" + reportId+ System.IO.Path.GetExtension(location));
            }
            else if (reportType.Equals("Ecg"))
            {
                reportId = "ecg" + reportId;
                string ecgLoc = _patientProxy.getEcgImgLocation(patientId);
                commandText = "proc_insert_EcgReport";
                System.IO.File.Copy(location, ecgLoc + "\\" + reportId+System.IO.Path.GetExtension(location));
            }
            else
            {
                reportId = "mri" + reportId;
                string mriLoc = _patientProxy.getMriImgLocation(patientId);
                commandText = "proc_insert_MriReport";
                System.IO.File.Copy(location, mriLoc + "\\" + reportId+ System.IO.Path.GetExtension(location));
            }

            PatientDataRef.Report _report = new PatientDataRef.Report();
            _report.ReportId = PatientIdText.Text;
            _report.Id = reportId;
            _report.DateTime = date;
            int status = _patientProxy.Add_Report(_report, commandText);
            ReportsTab.Visibility = Visibility.Hidden;
            tabControl.SelectedIndex = 0;
            refresh();

        }

        private void button_Report_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 4;
            ReportsTab.Header = "Add Reports";
            dateTime.Text = DateTime.Now.ToString();
        }

        private void ReportCancelButton_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 0;
            ReportsTab.Visibility = Visibility.Hidden;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow logWindow = new LoginWindow();
            logWindow.Show();
            this.Close();
        }
    }
}
