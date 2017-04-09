using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyPaint.P;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace MyPaint
{
    public partial class Form1 : Form
    {

        public int NumberOfTool = 1; // 1  - pencil, 2  - rectangle, 3 - circle;
        Point a, b;
        Graphics g_bitmap;
        Bitmap bitmap;
        Graphics g;
        Bitmap ClearBitmap;
        Graphics ClearG;
        TextBox txt;
        bool Move = false;
        GraphicsPath path;


        Color PickedColor = Color.Black;
        int PenSize = 2;


        bool IsPressed;

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g_bitmap = Graphics.FromImage(bitmap);

            ClearBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            ClearG = Graphics.FromImage(ClearBitmap);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }


       

        

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (Move)
            {

                int x = Math.Min(a.X, b.X);
                int y = Math.Min(a.Y, b.Y);
                int w = Math.Abs(a.X - b.X);
                int h = Math.Abs(a.Y - b.Y);

                if (NumberOfTool == 2)
                {
                    g_bitmap.DrawPath(new Pen(PickedColor, PenSize), path);
                }
                if (NumberOfTool == 3)
                {
                    Pen p = new Pen(PickedColor, 4);
                    g_bitmap.DrawPath(p, path);
                }
                if (NumberOfTool == 4)
                {
                    Pen p = new Pen(PickedColor, 4);
                    p.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    g_bitmap.DrawLine(p, b, a);
                }

                pictureBox1.Image = bitmap;
            }
            IsPressed = false;
            Move = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            a = e.Location;
            IsPressed = true;
            timer1.Enabled = false;
            if (NumberOfTool == 5)
            {
                txt = new TextBox();
                txt.Location = e.Location;
                txt.Width = 120;
                txt.DoubleClick += new EventHandler(txt_DoubleClick);
                pictureBox1.Controls.Add(txt);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
             if (IsPressed == true)
            {

                Move = true;
                b = e.Location;

                Graphics g = pictureBox1.CreateGraphics();
                
                int x = Math.Min(a.X, b.X);
                int y = Math.Min(a.Y, b.Y);
                int w = Math.Abs(a.X - b.X);
                int h = Math.Abs(a.Y - b.Y);

                if (NumberOfTool != 1)
                {
                    g.Clear(Color.White);
                    timer1.Enabled = true; 
                    g.Clear(Color.White);     
                }
                if (NumberOfTool == 2)
                {
                    path = new GraphicsPath();
                    path.AddRectangle(new Rectangle(x, y, w, h));
                }

                if (NumberOfTool == 3)
                {
                    if (radioButton1.Checked && !radioButton2.Checked)
                    {
                        g.FillEllipse(new SolidBrush(PickedColor), x, y, w, h);
                    }
                    if (radioButton2.Checked && !radioButton1.Checked)
                    {
                        g.DrawEllipse(new Pen(PickedColor, PenSize), x, y, w, h);
                    }
                }
                if (NumberOfTool == 4)
                {
                    Pen p = new Pen(PickedColor, 4);
                    p.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    g.DrawLine(p, b,a);
                }
                if (NumberOfTool == 1)
                {
                    g.DrawLine(new Pen(PickedColor, PenSize), a, b);
                    g.FillEllipse(new SolidBrush(PickedColor), a.X, a.Y, PenSize/2, PenSize/2);
                    g_bitmap.DrawLine(new Pen(PickedColor, PenSize), a, b);
                    g_bitmap.FillEllipse(new SolidBrush(PickedColor), a.X, a.Y, PenSize/2, PenSize/2);
                    pictureBox1.Image = bitmap;
                    a = b;
                }
                if (NumberOfTool == 6)
                {
                    g.DrawLine(new Pen(Color.White, PenSize), a, b);

                    g_bitmap.DrawLine(new Pen(Color.White, PenSize), a, b);
                    pictureBox1.Image = bitmap;
                    a = b;
                }

            }
        }
        void txt_DoubleClick(object sender, EventArgs e)
        {
            g_bitmap.DrawString(((TextBox)sender).Text, ((TextBox)sender).Font, Brushes.Black, ((TextBox)sender).Location);
            ((TextBox)sender).DoubleClick -= new EventHandler(txt_DoubleClick);
            pictureBox1.Controls.Remove((TextBox)sender);
            ((TextBox)sender).Dispose();
            pictureBox1.Invalidate();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        //TOOOOOLS***************************************************
        //***********************************************************
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            NumberOfTool = 1;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            NumberOfTool = 2;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            NumberOfTool = 4;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            NumberOfTool = 5;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            NumberOfTool = 6;
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                PickedColor = colorDialog1.Color;
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            PenSize = trackBar1.Value;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                FileStream fs =(FileStream)saveFileDialog1.OpenFile();
                pictureBox1.Image.Save(fs, ImageFormat.Jpeg);

            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Open Image";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image i = Image.FromFile(openFileDialog1.FileName); // This is 300x300
                g_bitmap.DrawImage(i, 0, 0,i.Width, i.Height);
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (path != null && NumberOfTool==2)
            {
                e.Graphics.DrawPath(new Pen(PickedColor, PenSize), path);
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            NumberOfTool = 3;
        }
        //********************************************************************

    }
}
