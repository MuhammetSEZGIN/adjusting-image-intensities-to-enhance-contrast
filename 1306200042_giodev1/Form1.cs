using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;

namespace _1306200042_giodev1
{
    public partial class Form1 : Form
    {
        // to save operation we made, store result to op.change
        Operations op = new Operations();
        public Form1()
        {
            InitializeComponent();
              
        }

        // choose button
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
           
            ofd.Filter = "Bitmap Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
            
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(ofd.FileName);
            }

        }

        // save button
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.DefaultExt = "sonuc.png";
            sf.Filter = "Bitmap Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                op.Hchanged.Save(sf.FileName);
            }
        }

        // histogram strecth operations
        private void button2_Click(object sender, EventArgs e)
        {
            Color  ColorT;
            int temp = 0;
            Bitmap originalPicture, changedPicture;
            originalPicture = new Bitmap(pictureBox1.Image);

            int Width = originalPicture.Width;
            int Height = originalPicture.Height;
            changedPicture = new Bitmap(Width, Height);

            int oldMin = findMin(originalPicture);
            int oldMax = findMax(originalPicture);
            int newMin = Convert.ToInt16(textBox2.Text);
            int newMax = Convert.ToInt16(textBox1.Text);
            
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    temp = originalPicture.GetPixel(x, y).R;
                     
                    int GreyValue = temp;

                    int current = GreyValue;
                    int C = (((current - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;

                    if (C > 255)
                        C = 255;
                    if (C < 0)
                        C = 0;
                    ColorT = Color.FromArgb(C, C, C); // all values are equel R=G=B because its greyscale picture
                    changedPicture.SetPixel(x, y, ColorT);
                }
            }
           pictureBox2.Image= changedPicture;
            op.Hchanged = changedPicture;
        }


        
        private int findMax(Bitmap originalPicture)
        {

            int maxElement = originalPicture.GetPixel(0,0).G;
            for (int x = 0; x < originalPicture.Width; x++)
            {
                for (int y = 0; y < originalPicture.Height; y++)
                {
                    if (originalPicture.GetPixel(x, y).G > maxElement)
                        maxElement = originalPicture.GetPixel(x, y).G;
                }
            }
            return maxElement;
        }
        private int findMin(Bitmap originalPicture)
        {

            int minElement = originalPicture.GetPixel(0, 0).G;
            for (int x = 0; x < originalPicture.Width; x++)
            {
                for (int y = 0; y < originalPicture.Height; y++)
                {
                    if (originalPicture.GetPixel(x, y).G < minElement)
                        minElement = originalPicture.GetPixel(x, y).G;
                }
            }
            return minElement;
        }
        // end of strecth operation
      

        // histogram equalization operation

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap originalPicture, changedPicture;
            originalPicture = new Bitmap(pictureBox1.Image);


            int Width = originalPicture.Width;
            int Height = originalPicture.Height;
            changedPicture = new Bitmap(Width, Height);

            int[] histogram= new int[256] ;
            Array.Clear(histogram, 0, histogram.Length);
            int[] cumulativeHistogram = new int[256];
            histogram.CopyTo(cumulativeHistogram, 0);


            
            for(int j=0; j< Height; j++)
            {
                for(int i=0; i<Width; i++)
                {
                    histogram[originalPicture.GetPixel(i,j ).R]++;
                }
            }

            int sum = 0;
            for (int i = 0; i < histogram.Length; i++)
            {
                sum+=histogram[i];
                cumulativeHistogram[i] = sum;
            }

            int[] map = new int[256];
            for (int i = 0; i < 256; i++)
            {
                map[i] = (int)(255.0 * cumulativeHistogram[i] / (Width * Height));
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color pixel = originalPicture.GetPixel(x, y);
                    int r = map[pixel.R];

                    changedPicture.SetPixel(x, y, Color.FromArgb(r, r, r));
                }
            }


            pictureBox2.Image = changedPicture;
            op.Hchanged = changedPicture;
        }
        // end of equalization operation
    }// end of form1
}