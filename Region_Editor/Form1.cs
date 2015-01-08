using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Region_Editor
{

    public partial class Form1 : Form
    {
        List<Region> listRegions = new List<Region>();
        Point click = new Point(-1, -1);
        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            comboBoxShape.DataSource = Enum.GetValues(typeof(RegionShape));
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = buttonColor.BackColor;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                buttonColor.BackColor = dlg.Color;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Region r = new Region();
            r.X = (int)numericUpDownX.Value;
            r.Y = (int)numericUpDownY.Value;
            r.Width = (int)numericUpDownWidth.Value;
            r.Height = (int)numericUpDownHeight.Value;
            r.Color = buttonColor.BackColor;
            r.Label = textBoxLabel.Text;
            r.Shape = (RegionShape)comboBoxShape.SelectedValue;
            listRegions.Add(r);
            textBoxRegion.Text = listRegions.Count.ToString();
            int area = 0;
            for (int i = 0; i < listRegions.Count; i++)
            {
                area += listRegions[i].Width * listRegions[i].Height;
            }
            textBoxArea.Text = area.ToString();
            graphicPanel1.Invalidate();
        }

        private void graphicPanel1_Paint(object sender, PaintEventArgs e)
        {

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            Font font = new Font("Arial", 8.0f);
            Pen pen = new Pen(Color.Red, 5);
            foreach (Region r in listRegions)
            {
                Brush brush = new SolidBrush(r.Color);

                if (r.Shape == RegionShape.Ellipse)
                {
                    e.Graphics.FillEllipse(brush, r.Rectangle);
                    e.Graphics.DrawString(r.Label, font, Brushes.Black, r.Rectangle, format);
                    if (r.Rectangle.Contains(click))
                    {
                        e.Graphics.DrawEllipse(pen, r.Rectangle);
                    }
                    else
                    {
                        e.Graphics.DrawEllipse(Pens.Transparent, r.Rectangle);
                    }
                }
                else
                {
                    e.Graphics.FillRectangle(brush, r.Rectangle);
                    e.Graphics.DrawString(r.Label, font, Brushes.Black, r.Rectangle, format);
                    if (r.Rectangle.Contains(click))
                    {
                        e.Graphics.DrawRectangle(pen, r.Rectangle);
                    }
                    else
                    {
                        e.Graphics.DrawRectangle(Pens.Transparent, r.Rectangle);
                    }
                }
            }

        }

        private void graphicPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                click = e.Location;
                graphicPanel1.Invalidate();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listRegions.Clear();
            graphicPanel1.Invalidate();
        }
    }
}
