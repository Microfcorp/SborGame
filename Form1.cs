using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyGame
{
    public partial class Form1 : Form
    {
        bool isDown;

        List<PictureBox> pb = new List<PictureBox>();

        public Form1(string[] args)
        {
            InitializeComponent();
            if(args.Length > 0)
            {
                load(args[0]);
            }            
        }

        public void load(string path)
        {
            //123.bmp; 100; 100
            //1234.bmp; 100; 100

            string game = File.ReadAllText(path).Replace("\r", "");
            foreach (var item in game.Split('\n'))
            {
                PictureBox tmp = new PictureBox();
                tmp.ImageLocation = item.Split(';')[0];
                tmp.Name = item.Split(';')[3];
                if (item.Split(';')[3] == "0005")
                {
                    pictureBox17.ImageLocation = item.Split(';')[0];
                    pictureBox17.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox17.BringToFront();
                }
                else
                {
                    tmp.BackColor = Color.Empty;
                    label2.Text = (Convert.ToInt32(label2.Text.Substring(0, label2.Text.Length - 1)) + Convert.ToInt32(item.Split(';')[3])).ToString() + "%";
                    tmp.Size = new Size(Convert.ToInt32(item.Split(';')[1]), Convert.ToInt32(item.Split(';')[2]));
                    if (pb.Count > 0)
                    {
                        //tmp.Location = pb.Max().Location + new Size(50, 50);
                        tmp.Location = new Point(80, 74);
                    }
                    else
                    {
                        tmp.Location = new Point(80, 74);
                    }
                    // tmp.Location = new Point(80, 74);
                    panel1.Controls.Add(tmp);
                    pb.Add(tmp);
                }         
            }
            podp(all());
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isDown = true;
            //Console.WriteLine(e.Location);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            if (isDown)
            {
                //Console.WriteLine(e.Location);
                Point p = Control.MousePosition;               
                c.Location = this.PointToClient(p);
                if(trig(sender as PictureBox, del))
                {
                    c.MouseMove -= pictureBox1_MouseMove;
                    c.MouseDown -= pictureBox1_MouseDown;
                    isDown = false;

                    label2.Text = (Convert.ToInt32(label2.Text.Substring(0, label2.Text.Length - 1)) - Convert.ToInt32(c.Name)).ToString() + "%";
                    panel1.Controls.Remove(c);
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDown = false;
        }

        private void podp(Control[] obj)
        {
            foreach (var item in obj)
            {
                if (item.Name != "del")
                {
                    item.MouseDown += pictureBox1_MouseDown;
                    item.MouseMove += pictureBox1_MouseMove;
                    item.MouseUp += pictureBox1_MouseUp;
                }
            }
        }
        private Control[] all()
        {
            List<Control> tmp = new List<Control>();

            foreach (var item in this.Controls)
            {
                if (item is Panel)
                {
                    foreach (var item1 in (item as Panel).Controls)
                    {
                        if (item1 is PictureBox)
                            tmp.Add((Control)item1);
                    }
                }
            }
            return tmp.ToArray();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            podp(all());
            //MessageBox.Show(Convert.ToInt32(label2.Text.Substring(0, label2.Text.Length - 1)).ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;        
            //Bitmap myBitmap = new Bitmap(980, 421, panel1.CreateGraphics());
            SaveFileDialog sf = new SaveFileDialog();
            if(sf.ShowDialog() == DialogResult.OK)
            {
                int width = panel1.Size.Width;
                int height = panel1.Size.Height;

                Bitmap bm = new Bitmap(width, height);
                panel1.DrawToBitmap(bm, new Rectangle(0, 0, width, height));

                bm.Save(sf.FileName, ImageFormat.Bmp);
            }
            button1.Visible = true;
        }
        public bool trig(PictureBox object1, PictureBox object2)
        {
            bool result = false;
            bool result1 = false;

            Size obj1 = object1.Size;
            Size obj2 = object2.Size;

            Point loc1 = object1.Location;
            Point loc2 = object2.Location;

            //Console.WriteLine("Size ob1 {0} Size obj2 {1} Point loc1 {2} Point loc2 {3}", obj1, obj2, loc1, loc2);

            for (int i = 0; i < obj2.Height; i++)
            {
                for (int ia = 0; ia < obj1.Height; ia++)
                {
                    if (loc2.Y + i + -ia == loc1.Y)
                    {
                        result1 = true;
                    }
                }
            }
            if (result1)
            {
                for (int i = 0; i < obj2.Width; i++)
                {
                    for (int ia = 0; ia < obj1.Width; ia++)
                    {
                        if (loc2.X + i + -ia == loc1.X)
                        {
                            result = true;
                            //Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        }
                    }
                }
            }
            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!pictureBox17.Visible)
            {
                pictureBox17.Visible = true;
               // panel1.Visible = false;
            }
            else
            {
                pictureBox17.Visible = false;
                //panel1.Visible = true;
            }
        }
    }
}
