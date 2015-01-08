using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Region_Editor
{
    public class Region
    {
        Color color;

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        int x;

        public int X
        {
            get { return x; }
            set
            {
                x = value;
                location.X = x;
                rectangle.X = x;
            }
        }

        int y;

        RegionShape shape;

        public RegionShape Shape
        {
            get { return shape; }
            set { shape = value; }
        }

        public int Y
        {
            get { return y; }
            set
            {
                y = value;
                location.Y = y;
                rectangle.Y = y;
            }
        }

        Point location;

        public Point Location
        {
            get { return location; }
            set
            {
                location = value;
                rectangle.Location = location;
            }
        }


        int height;

        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                size.Height = height;
                rectangle.Height = height;
            }
        }

        int width;

        public int Width
        {
            get { return width; }
            set
            {
                width = value;
                size.Width = width;
                rectangle.Width = width;
            }
        }

        Size size;

        public Size Size
        {
            get { return size; }
            set { size = value;
            rectangle.Size = size;
            }
        }

        string label;

        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        Rectangle rectangle;

        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }


        public Region()
        {

        }


    }
}
