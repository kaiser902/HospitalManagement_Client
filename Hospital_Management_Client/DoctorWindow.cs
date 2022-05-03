using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalManagement_Client
{
    public partial class DoctorWindow : Form
    {
        PatientDataRef.PatientDataServiceClient _patientProxy = new PatientDataRef.PatientDataServiceClient("BasicHttpBinding_IPatientDataService");
        string selectedPatientId;
        int flag = 0;
        string selectedXrayId;
        string selectedMriId;
        string selectedEcgId;
        string DocId;

        public DoctorWindow(DoctorDataRef.Doctor u)
        {
            InitializeComponent();
            DocId = u.Id;
            label1.Text = "Welcome " + u.Name;
            label_id.Text = "ID : " + u.Id;

        }

        private void DoctorWindow_Load(object sender, EventArgs e)
        {
            DataSet s = _patientProxy.GetPatientListByDocId(DocId);
            bindingPatientData.DataSource = s.Tables[0];
            dataGridView1.DataSource = bindingPatientData;
            tabControl1.TabPages.Remove(tabPage2);
        }

        private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            selectedPatientId = dataGridView1.SelectedRows[0].Cells[0].Value + string.Empty;
            tabControl1.TabPages.Add(tabPage2);
            this.tabPage2.Show();
            backButton.Show();
            button1.Show();
            button2.Show();
            button3.Show();
            backButton.Text = "Patient ID:" + selectedPatientId;
            backButton.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            backButton.Enabled = false;
            tabControl1.SelectedIndex = tabControl1.SelectedIndex + 1;
            pictureBox1.Hide();
        }

        private void dataGridView2_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.pictureBox1.Show();
            if (flag == 1)
            {
                selectedXrayId = dataGridView2.SelectedRows[0].Cells[1].Value + string.Empty;
                string loc = _patientProxy.getXrayImgLocation(selectedPatientId);
                pictureBox1.ImageLocation = loc.Trim() + "\\" + selectedXrayId + ".jpg";

            }
            else if (flag == 2)
            {
                selectedMriId = dataGridView2.SelectedRows[0].Cells[1].Value + string.Empty;
                string loc = _patientProxy.getMriImgLocation(selectedPatientId);
                pictureBox1.ImageLocation = loc.Trim() + "\\" + selectedMriId + ".jpg";

            }
            else if (flag == 3)
            {
                selectedEcgId = dataGridView2.SelectedRows[0].Cells[1].Value + string.Empty;
                string loc = _patientProxy.getEcgImgLocation(selectedPatientId);
                pictureBox1.ImageLocation = loc.Trim() + "\\" + selectedEcgId + ".jpg";
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Hide();
            button2.Hide();
            button3.Hide();
            backButton.Text = "Back to Reports";
            backButton.ForeColor = Color.Black;
            backButton.Enabled = true;
            backButton.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            flag = 1;
            DataSet s = _patientProxy.GetXrayData(selectedPatientId);
            bindingReportsData.DataSource = s.Tables[0];
            dataGridView2.DataSource = bindingReportsData;
            this.dataGridView2.Columns[0].Visible = false;
            this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Hide();
            button2.Hide();
            button3.Hide();
            backButton.Text = "Back to Reports";
            backButton.ForeColor = Color.Black;
            backButton.Enabled = true;
            backButton.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            flag = 2;
            DataSet s = _patientProxy.GetMriData(selectedPatientId);
            bindingReportsData.DataSource = s.Tables[0];
            dataGridView2.DataSource = bindingReportsData;
            this.dataGridView2.Columns[0].Visible = false;
            this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button1.Hide();
            button2.Hide();
            button3.Hide();
            backButton.Text = "Back to Reports";
            backButton.ForeColor = Color.Black;
            backButton.Enabled = true;
            backButton.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            flag = 3;
            DataSet s = _patientProxy.GetEcgData(selectedPatientId);
            bindingReportsData.DataSource = s.Tables[0];
            dataGridView2.DataSource = bindingReportsData;
            this.dataGridView2.Columns[0].Visible = false;
            this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private void tabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages[0])
            {
                this.tabPage2.Hide();
                tabControl1.TabPages.Remove(tabPage2);
                dataGridView2.DataSource = null;
                DataSet s = _patientProxy.GetPatientList();
                bindingPatientData.DataSource = s.Tables[0];
                dataGridView1.DataSource = bindingPatientData;
            }
        }

        private void DoctorWindow_ClickOnClose(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            Application.Exit();
            System.Environment.Exit(0);
            for (int intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
                App.Current.Windows[intCounter].Close();

        }

        private void Button_BackToReport_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Hide();
            dataGridView2.DataSource = null;
            backButton.Text = "Patient ID:" + selectedPatientId;
            backButton.ForeColor = Color.Blue;
            backButton.Enabled = false;
            backButton.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            button1.Show();
            button2.Show();
            button3.Show();

        }

        //*****Zoom function//
        protected override void OnMouseWheel(MouseEventArgs mea)
        {
            // Override OnMouseWheel event, for zooming in/out with the scroll wheel
            if (pictureBox1.Image != null)
            {
                // If the mouse wheel is moved forward (Zoom in)
                if (mea.Delta > 0)
                {
                    // Check if the pictureBox dimensions are in range (15 is the minimum and maximum zoom level)
                    if ((pictureBox1.Width < (15 * this.Width)) && (pictureBox1.Height < (15 * this.Height)))
                    {
                        // Change the size of the picturebox, multiply it by the ZOOM FACTOR
                        pictureBox1.Width = (int)(pictureBox1.Width * 1.25);
                        pictureBox1.Height = (int)(pictureBox1.Height * 1.25);
                    }
                }
                //Zoom out
                else
                {
                    // Check if the pictureBox dimensions are in range (15 is the minimum and maximum zoom level)
                    if ((pictureBox1.Width > (this.Width / 15)) && (pictureBox1.Height > (this.Height / 15)))
                    {
                        // Change the size of the picturebox, divide it by the ZOOM FACTOR
                        pictureBox1.Width = (int)(pictureBox1.Width / 1.25);
                        pictureBox1.Height = (int)(pictureBox1.Height / 1.25);
                    }
                }
            }
        }

        // End--Zoom Function//

        //Pen
        private Point? _Previous = null;
        private Point? start = null;
        private Pen _Pen = new Pen(Color.IndianRed);
        private bool dragging; //End--Pen
        private Point mouseDown;

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //PAN
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                Point mousePosNow = mouse.Location;

                int deltaX = mousePosNow.X - mouseDown.X;
                int deltaY = mousePosNow.Y - mouseDown.Y;

                int newX = pictureBox1.Location.X + deltaX;
                int newY = pictureBox1.Location.Y + deltaY;

                pictureBox1.Location = new Point(newX, newY);
            }
            //PAN
            if (_Previous != null)
            {
                if (pictureBox1.Image == null)
                {
                    Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.White);
                    }
                    pictureBox1.Image = bmp;
                }
                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {

                    g.DrawLine(_Pen, _Previous.Value.X, _Previous.Value.Y, e.X, e.Y);
                }
                pictureBox1.Invalidate();
                _Previous = new Point(e.X, e.Y);

            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        { //PAN
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                mouseDown = mouse.Location;
            }

            else if (mouse.Button == MouseButtons.Right)
            {
                // Do something else, not important in this example
            }
            //PAN
            _Previous = e.Location;
            pictureBox1_MouseMove(sender, e);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            _Previous = null;
        }

        private void LogOut_button_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginWindow logWindow = new LoginWindow();
            logWindow.Show();
            //this.Close();
        }
    }
}
