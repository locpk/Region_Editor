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
        Region selected;
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
            RefreshTotal();
            graphicPanel1.Invalidate();
        }

        private void graphicPanel1_Paint(object sender, PaintEventArgs e)
        {

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            Font font = new Font("Arial", 8.0f);
            Pen pen = new Pen(Color.Red, 3);
            Pen p = new Pen(Color.Black, 2);


            foreach (Region r in listRegions)
            {
                Brush brush = new SolidBrush(r.Color);

                if (r.Shape == RegionShape.Ellipse)
                {
                    if (r.Color == SystemColors.Window)
                    {
                        e.Graphics.DrawEllipse(p, r.Rectangle);
                    }

                    e.Graphics.FillEllipse(brush, r.Rectangle);
                    e.Graphics.DrawString(r.Label, font, Brushes.Black, r.Rectangle, format);
                    if (r.Rectangle.Contains(click))
                    {
                        e.Graphics.DrawEllipse(pen, r.Rectangle);
                        selected = r;
                        RefreshSelected();
                        buttonUpdate.Enabled = true;
                        buttonDeselect.Enabled = true;
                        buttonRemove.Enabled = true;
                    }
                    else
                    {
                        e.Graphics.DrawEllipse(Pens.Transparent, r.Rectangle);
                        selected = null;
                        Deselected();
                        buttonUpdate.Enabled = false;
                        buttonDeselect.Enabled = false;
                        buttonRemove.Enabled = false;
                    }
                }
                else
                {
                    if (r.Color == SystemColors.Window)
                    {
                        e.Graphics.DrawRectangle(p, r.Rectangle);
                    }

                    e.Graphics.FillRectangle(brush, r.Rectangle);
                    e.Graphics.DrawString(r.Label, font, Brushes.Black, r.Rectangle, format);
                    if (r.Rectangle.Contains(click))
                    {
                        e.Graphics.DrawRectangle(pen, r.Rectangle);
                        selected = r;
                        RefreshSelected();
                        buttonUpdate.Enabled = true;
                        buttonDeselect.Enabled = true;
                        buttonRemove.Enabled = true;
                    }
                    else
                    {
                        e.Graphics.DrawRectangle(Pens.Transparent, r.Rectangle);
                        selected = null;
                        Deselected();
                        buttonUpdate.Enabled = false;
                        buttonDeselect.Enabled = false;
                        buttonRemove.Enabled = false;
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
            Deselected();
            graphicPanel1.Invalidate();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            selected.X = (int)numericUpDownX.Value;
            selected.Y = (int)numericUpDownY.Value;
            selected.Width = (int)numericUpDownWidth.Value;
            selected.Height = (int)numericUpDownHeight.Value;
            selected.Color = buttonColor.BackColor;
            selected.Label = textBoxLabel.Text;
            selected.Shape = (RegionShape)comboBoxShape.SelectedValue;
            RefreshTotal();
            RefreshSelected();
            graphicPanel1.Invalidate();
        }

        private void buttonDeselect_Click(object sender, EventArgs e)
        {
            click.X = -1;
            click.Y = -1;
            graphicPanel1.Invalidate();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            listRegions.Remove(selected);
            RefreshTotal();
            Deselected();
            graphicPanel1.Invalidate();
        }
        private void RefreshTotal()
        {
            textBoxRegion.Text = listRegions.Count.ToString();
            int area = 0;
            for (int i = 0; i < listRegions.Count; i++)
            {
                area += listRegions[i].Width * listRegions[i].Height;
            }
            textBoxArea.Text = area.ToString();
        }
        private void RefreshSelected()
        {
            textBoxLabel.Text = selected.Label;
            numericUpDownX.Value = selected.X;
            numericUpDownY.Value = selected.Y;
            numericUpDownHeight.Value = selected.Height;
            numericUpDownWidth.Value = selected.Width;
            buttonColor.BackColor = selected.Color;
            comboBoxShape.SelectedItem = selected.Shape;
        }

        private void Deselected()
        {
            textBoxLabel.Text = "Untitled";
            numericUpDownX.Value = 0;
            numericUpDownY.Value = 0;
            numericUpDownHeight.Value = 100;
            numericUpDownWidth.Value = 100;
            buttonColor.BackColor = Color.White;
            comboBoxShape.SelectedItem = comboBoxShape.Items[0];
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an open file dialog
            OpenFileDialog dlg = new OpenFileDialog();
            // Set the filter strings
            dlg.Filter = "All Files(*.*)|*.*|csv(*.csv)|*.csv";
            // Display the dialog to the user
            if (DialogResult.OK == dlg.ShowDialog())
            {
                listRegions.Clear();
                // Open a stream for reading
                System.IO.StreamReader reader = new System.IO.StreamReader(dlg.FileName);
                // Write a line to the stream
                while (reader.Peek() > -1)
                {
                    string[] subs = reader.ReadLine().Split(new Char[] { ',' });
                    if (subs.Length == 7)
                    {
                        Region r = new Region();
                        r.Label = subs[0];
                        r.X = int.Parse(subs[1]);
                        r.Y = int.Parse(subs[2]);
                        r.Width = int.Parse(subs[3]);
                        r.Height = int.Parse(subs[4]);
                        r.Shape = (RegionShape)Enum.Parse(typeof(RegionShape), subs[5]);
                        r.Color = Color.FromArgb(int.Parse(subs[6]));
                        listRegions.Add(r);
                    }

                }

                // Close the stream
                reader.Close();
            }
            graphicPanel1.Invalidate();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a size file dialog
            SaveFileDialog dlg = new SaveFileDialog();
            // Set the filter strings
            dlg.Filter = "All Files(*.*)|*.*|csv(*.csv)|*.csv";
            // Set the default extension
            dlg.DefaultExt = "csv";
            // Display the dialog to the user
            if (DialogResult.OK == dlg.ShowDialog())
            {
                // Open a stream for writing
                System.IO.StreamWriter writer = new System.IO.StreamWriter(dlg.FileName);
                // Write the string
                foreach (Region value in listRegions)
                {
                    writer.WriteLine(value.ToFile());
                }
                // Close the stream
                writer.Close();
            }
        }
    }
}
