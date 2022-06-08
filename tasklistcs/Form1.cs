using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tasklistcs
{
    public partial class Form1 : Form
    {

        int currentWindow = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            for(int i=DateTime.Now.Year; i<DateTime.Now.Year+50;i++)
            {
                Year_box.Items.Add(i);
            }

            
            for(int i=1;i<=12;i++)
            {
                DateTime temp_date = new DateTime(2022, i, 1);
                Month_box.Items.Add(temp_date.ToString("MMMM"));
            }

            Year_box.SelectedIndex = 0;
            //Month_box.SelectedIndex = 0;
            //Day_box.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List l = new List();
            int id = 2;
            string name = "Task 2";
            string place = "Place 2";
            DateTime date = new DateTime(2023, 11, 27);

            Element el = new Element(id, name, place, date);
            l.AddElement(el);
            l.AddElement(el);
            l.AddElement(el);
            l.SaveToFile("tasklist.txt");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Month_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            Day_box.Items.Clear();
            for (int i=0;i<DateTime.DaysInMonth(Convert.ToInt32(Year_box.SelectedItem),Month_box.SelectedIndex+1);i++)
            {
                Day_box.Items.Add(i+1);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            List l = new List();
            DateTime d = new DateTime(
                Convert.ToInt32(Year_box.SelectedItem), 
                Convert.ToInt32(Month_box.SelectedIndex+1),
                Convert.ToInt32(Day_box.SelectedItem));
            int id = 1;
            if (l.elements.Count() > 0) id = l.elements[l.elements.Count - 1].id + 1;
            Element el = new Element(id, Name_txtBox.Text, Place_txtBox.Text, d);
            l.AddElement(el);
            l.SaveToFile("tasklist.txt");
            Name_txtBox.Clear();
            Place_txtBox.Clear();
            Day_box.SelectedIndex = 0;
            Month_box.SelectedIndex = 0;
            Year_box.SelectedIndex = 0;
        }

        private void Year_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Month_box.SelectedIndex == -1) return;
            Day_box.Items.Clear();
            for(int i=0;i<DateTime.DaysInMonth(Convert.ToInt32(Year_box.SelectedItem),Month_box.SelectedIndex+1);i++)
            {
                Day_box.Items.Add(i + 1);
            }
        }

        void SwitchWindows()
        {

            switch(this.currentWindow)
            {
                case 0:
                    show_form1.Visible = false;
                    //Show_Panel1.Visible = false;
                    break;
                case 1:
                    show_form1.Visible = true;
                    
                    //Show_Panel1.Visible = true;
                    break;
            }
        }

        private void ShowTasks_btn_Click(object sender, EventArgs e)
        {
            this.currentWindow = 1;
            this.SwitchWindows();
            this.ShowTasks();

        }

        private void ShowTasks()
        {
            List l = new();
            show_form1.Controls.Clear();
            l.LoadFromFile("tasklist.txt");
            foreach (Element el in l.elements)
            {

                Font bigFont = new("Nirmala", 21);
                Font smallFont = new("Nirmala", 14);

                Panel _container = new();
                Panel _list_element = new();
                Label _lbl_name = new();
                Label _lbl_placedate = new();
                Button _button_delete = new();

                _button_delete.Text = "x";
                _button_delete.Dock = DockStyle.Left;
                _button_delete.Padding = new(0);
                _button_delete.FlatStyle = FlatStyle.Flat;
                _button_delete.ForeColor = Color.FromArgb(255, 0, 0);
                _button_delete.FlatAppearance.BorderSize = 0;
                _button_delete.Font = bigFont;
                _button_delete.Click += RemoveTask(el.id);

                _lbl_name.Text = el.name;
                _lbl_placedate.Text = el.place + " " +
                    Convert.ToString(el.date.Day) + "/" +
                    Convert.ToString(el.date.Month) + "/" +
                    Convert.ToString(el.date.Year);

                _lbl_name.Dock = DockStyle.Top;
                _lbl_name.Font = bigFont;
                _lbl_name.AutoSize = true;

                _lbl_placedate.Dock = DockStyle.Bottom;
                _lbl_placedate.Font = smallFont;

                _list_element.Controls.Add(_lbl_placedate);
                _list_element.Controls.Add(_lbl_name);
                _list_element.Controls.Add(_button_delete);

                _list_element.Dock = DockStyle.Fill;
                _list_element.BackColor = Color.FromArgb(24, 30, 54);
                _list_element.Size = new Size(250, 50);
                _list_element.Padding = new Padding(15);


                _container.Dock = DockStyle.Top;
                _container.Padding = new Padding(5);
                _container.Controls.Add(_list_element);
                show_form1.Controls.Add(_container);
            }
        }

        private EventHandler RemoveTask(int id)
        {
            List l = new();
            l.LoadFromFile("tasklist.txt");
            l.RemoveElement(id);
            l.SaveToFile("tasklist.txt", false);
            
            return new EventHandler(RemoveTask);
        }

        private void RemoveTask(object sender, EventArgs e)
        {
            this.ShowTasks();
        }

        private void AddTask_btn_Click(object sender, EventArgs e)
        {
            this.currentWindow = 0;
            this.SwitchWindows();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

         
    }
}
