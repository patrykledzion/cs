using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace tasklistcs
{

    class List
    {
        private Form1 sf = new();
        public List<Element> elements = new List<Element>();
        public List() { }
        public bool AddElement(Element element)
        {
            if (element.id < 1 || string.IsNullOrEmpty(element.name) || string.IsNullOrEmpty(element.place) ||
                element.date.Day < 1 || element.date.Month < 1 || element.date.Year < 1) return false;
            this.elements.Add(element);

            return true;
        }

        public bool SaveToFile(string filename, bool append = true)
        {

            if (!File.Exists(filename))
            {
                using (StreamWriter sw = File.CreateText(filename))
                {
                    foreach (Element element in this.elements)
                    {
                        if (element.id < 1 || string.IsNullOrEmpty(element.name) || string.IsNullOrEmpty(element.place) ||
                        element.date.Day < 1 || element.date.Month < 1 || element.date.Year < 1) continue; ;
                        sw.WriteLine(Convert.ToString(element.id));
                        sw.WriteLine(Convert.ToString(element.date.ToString()));
                        sw.WriteLine(Convert.ToString(element.name));
                        sw.WriteLine(Convert.ToString(element.place));
                    }

                }
            } else
            {
                using (StreamWriter sw = File.AppendText(filename))
                {
                    foreach (Element element in this.elements)
                    {
                        if (element.id < 1 || string.IsNullOrEmpty(element.name) || string.IsNullOrEmpty(element.place) ||
                        element.date.Day < 1 || element.date.Month < 1 || element.date.Year < 1) continue; ;
                        sw.WriteLine(Convert.ToString(element.id));
                        sw.WriteLine(Convert.ToString(element.date.ToString()));
                        sw.WriteLine(Convert.ToString(element.name));
                        sw.WriteLine(Convert.ToString(element.place));
                    }

                }
            }
            return true;
        }

        private bool GetDateFromString(string date, ref int day, ref int month, ref int year)
        {
            string[] temp = new string[3];
            int index = 0;
            foreach (char c in date)
            {
                if (c == ' ')
                {
                    day = Convert.ToInt32(temp[0]);
                    month = Convert.ToInt32(temp[1]);
                    year = Convert.ToInt32(temp[2]);
                    return true;
                }
                if (c == '.')
                {
                    index++;
                    if (index > 2) break;
                    continue;
                }else
                {
                    try
                    {
                        Convert.ToInt32(c.ToString());
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    temp[index] += c;
                }
                

            }

            day = Convert.ToInt32(temp[0]);
            month = Convert.ToInt32(temp[1]);
            year = Convert.ToInt32(temp[2]);

            return true;
        }

        public int LoadFromFile(string filename)
        {
            this.elements.Clear();
            if (!File.Exists(filename)) return -1;
            int id = 0;
            using (StreamReader sr = File.OpenText(filename))
            {
                while (!sr.EndOfStream)
                {
                    int d = 0, m =0, y = 0;
                    string name;
                    string place;
                    string date_str;
                    DateTime date;
                    try
                    {
                        id = Convert.ToInt32(sr.ReadLine());
                    }
                    catch (Exception)
                    {
                        return -3;
                    }

                    date_str = sr.ReadLine();
                    if (!GetDateFromString(date_str, ref d, ref m, ref y)) return -4;

                    try
                    {
                        date = new DateTime(y, m, d);
                    }
                    catch (Exception)
                    {
                        return -2;
                    }

                    name = sr.ReadLine();
                    place = sr.ReadLine();

                    Element el = new(id, name, place, date);

                    this.elements.Add(el);
                }
            }
            return id;

            
        } 

        public void Show()
        {
            
            
            foreach(Element el in this.elements)
            {
                 
                Font bigFont = new Font("Nirmala", 17);
                Font smallFont = new Font("Nirmala", 11);
                System.Windows.Forms.Panel _list_element = new System.Windows.Forms.Panel();
                System.Windows.Forms.Label _lbl_name = new System.Windows.Forms.Label();
                System.Windows.Forms.Label _lbl_placedate = new System.Windows.Forms.Label(); ;

                _lbl_name.Text = el.name;
                _lbl_placedate.Text = el.place + " " +
                    Convert.ToString(el.date.Day) + "/" +
                    Convert.ToString(el.date.Month) + "/" +
                    Convert.ToString(el.date.Year);

                _lbl_name.Dock = System.Windows.Forms.DockStyle.Top;
                _lbl_name.Font = bigFont;

                _lbl_placedate.Dock = System.Windows.Forms.DockStyle.Top;
                _lbl_placedate.Font = smallFont;

                _list_element.Controls.Add(_lbl_name);
                _list_element.Controls.Add(_lbl_placedate);

                _list_element.BackColor = Color.FromArgb(24, 30, 54);
                _list_element.Size = new Size(155, 265);
                sf.Controls.Add(_list_element); 
            }
        }

        public void RemoveElement(int id)
        {
            for(int i=0;i<this.elements.Count();i++)
            {
                if(this.elements[i].id==id)
                {
                    this.elements.RemoveAt(i);
                }
            }

        }

    }
}

