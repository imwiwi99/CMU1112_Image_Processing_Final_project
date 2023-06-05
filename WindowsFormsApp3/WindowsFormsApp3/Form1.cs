using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Load picture
        private void button1_Click(object sender, EventArgs e)
        {
            // textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Loading picture... ");
            openFileDialog1.Filter= "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap openImg = new Bitmap(openFileDialog1.FileName);

                pictureBox1.Image = openImg;
                pictureBox2.Image = openImg;

                textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Picture LOADED: {openFileDialog1.FileName}\r\n");
                long length = new System.IO.FileInfo(openFileDialog1.FileName).Length/1000;
                textBox1.AppendText($"(Loaded picture info) {pictureBox1.Image.Size} , File Size: {length} KB\r\n");
            }
            else
            {
                // textBox1.AppendText($"Cancel \r\n");
            }
        }

        // Save picture
        private void button2_Click(object sender, EventArgs e)
        {
            // textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Saving picture... ");

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save Result Image";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String sfd = saveFileDialog1.FileName;
                pictureBox2.Image.Save(sfd);
                
                textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Picture SAVED: {saveFileDialog1.FileName}\r\n");
                long length = new System.IO.FileInfo(saveFileDialog1.FileName).Length / 1000;
                textBox1.AppendText($"(Saved picture info) {pictureBox2.Image.Size} , File Size: {length} KB\r\n");
            }
        }

        // GrayScale
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Grayscale -> processing... \r\n");

            Bitmap openImg = new Bitmap(pictureBox2.Image);
            Bitmap grayLvImg = new Bitmap (openImg.Width, openImg.Height);

            for (int i =0;i<openImg.Height;i++) 
                for (int j = 0; j < openImg.Width; j++)
                {
                    Color RGB = openImg.GetPixel(j, i);

                    int grayLv = (int)((0.299 * RGB.R) + (0.587 * RGB.G) + (0.114 * RGB.B));

                    grayLvImg.SetPixel(j,i,Color.FromArgb(grayLv,grayLv,grayLv));
                }
            pictureBox2.Image = grayLvImg;

            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Grayscale -> Done! \r\n");
        }

        // Binary
        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Binarization (threshold: {trackBar1.Value} , ForeColor: {button12.BackColor.Name} , BackColor: {button13.BackColor.Name}) -> processing... \r\n");

            Bitmap openImg = new Bitmap(pictureBox2.Image);
            Bitmap resultImg = new Bitmap(openImg.Width, openImg.Height);

            for (int i = 0; i < openImg.Height; i++) 
                for (int j = 0; j < openImg.Width; j++)
                {
                    Color RGB = openImg.GetPixel(j, i);
                    int grayLv = (int)((0.299 * RGB.R) + (0.587 * RGB.G) + (0.114 * RGB.B));

                    int threshold = trackBar1.Value;

                    if (grayLv>=threshold)
                    {
                        // resultImg.SetPixel(j,i,Color.FromArgb(255,255,255));
                        resultImg.SetPixel(j,i, button12.BackColor);
                    }
                    else
                    {
                        // resultImg.SetPixel(j,i,Color.FromArgb(0,0,0));
                        resultImg.SetPixel(j, i, button13.BackColor);
                    }
                }
            pictureBox2.Image = resultImg;
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Binarization -> Done! \r\n");
        }

        // Robert - Edge Detection
        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Robert - Edge Detection (threshold: {trackBar2.Value} , HighLightColor: {button14.BackColor.Name}) -> processing... \r\n");

            Bitmap openImg = new Bitmap(pictureBox2.Image);
            Bitmap robertImg = new Bitmap(openImg.Width,openImg.Height);

            // Robert mask
            int[,] mask1 =
            {
                {-1,0 },
                {0,1 },
            };

            int[,] mask2 =
            {
                {0,-1 },
                {1,0 },
            };

            for (int i = 0; i < openImg.Height-1;i++)
                for (int j = 0; j < openImg.Width-1; j++)
                {
                    Color RGB1 = openImg.GetPixel(j, i);
                    Color RGB2 = openImg.GetPixel(j+1, i);
                    Color RGB3 = openImg.GetPixel(j, i+1);
                    Color RGB4 = openImg.GetPixel(j+1, i+1);

                    int grayLv1 = (int)((0.299 * RGB1.R) + (0.587 * RGB1.G) + (0.114 * RGB1.B));
                    int grayLv2 = (int)((0.299 * RGB2.R) + (0.587 * RGB2.G) + (0.114 * RGB2.B));
                    int grayLv3 = (int)((0.299 * RGB3.R) + (0.587 * RGB3.G) + (0.114 * RGB3.B));
                    int grayLv4 = (int)((0.299 * RGB4.R) + (0.587 * RGB4.G) + (0.114 * RGB4.B));

                    int[,] maskPixel =
                    {
                        {grayLv1,grayLv2 },
                        {grayLv3,grayLv4 },
                    };

                    int value1 = 0;
                    int value2 = 0;

                    for (int y = 0; y<2;y++)
                        for (int x = 0; x < 2; x++)
                        {
                            value1 += maskPixel[x, y] * mask1[x, y];
                            value2 += maskPixel[x, y] * mask2[x, y];
                        }

                    int threshold = trackBar2.Value;

                    // Mask 1 and 2
                    if (Math.Abs(value1)>= threshold || Math.Abs(value2)>=threshold)
                    {
                        // robertImg.SetPixel(j, i, Color.FromArgb(0, 255, 0));
                        robertImg.SetPixel(j, i, button14.BackColor);
                    }
                    else
                    {
                        robertImg.SetPixel(j,i,Color.FromArgb(RGB1.R,RGB1.G, RGB1.B));
                    }
                }
            pictureBox2.Image= robertImg;
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Robert - Edge Detection -> Done! \r\n");
        }

        // Reset button
        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Reset image \r\n");
            pictureBox2.Image = pictureBox1.Image;
        }

        // Binarization - Scroll and Change Threshold value dynamically
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = $"Threshold: {trackBar1.Value}";
        }

        // Robert(Edge detection) - Scroll and Change Threshold value dynamically
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label4.Text = $"Threshold: {trackBar2.Value}";
        }

        // Flip Vertically
        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Flip Vertically -> processing... \r\n");

            Bitmap openImg = new Bitmap(pictureBox2.Image);
            Bitmap resultImg = new Bitmap(openImg.Width, openImg.Height);

            for (int i = 0; i < resultImg.Height; i++) 
                for (int j = 0; j< resultImg.Width; j++)
                {
                    Color RGB = openImg.GetPixel(j, i);
                    resultImg.SetPixel(j, openImg.Height - i - 1, RGB);
                }
            pictureBox2.Image= resultImg;
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Flip Vertically -> Done!\r\n");
        }

        // Flop Horizontally
        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Flop Horizontally -> processing... \r\n");

            Bitmap openImg = new Bitmap(pictureBox2.Image);
            Bitmap resultImg = new Bitmap(openImg.Width, openImg.Height);

            for (int i = 0; i < resultImg.Height; i++)
                for (int j = 0; j < resultImg.Width; j++)
                {
                    Color RGB = openImg.GetPixel(j, i);
                    resultImg.SetPixel(openImg.Width - j - 1, i, RGB);
                }
            pictureBox2.Image = resultImg;

            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Flop Horizontally -> Done! \r\n");
        }

        // 90 degrees Clockwise rotation
        private void button9_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Rotating 90 degrees ClockWise -> processing... \r\n");

            Bitmap openImg = new Bitmap(pictureBox2.Image);
            Bitmap resultImg = new Bitmap(openImg.Height, openImg.Width); // Width, Height exchange (because 90 degrees)

            for (int i = 0; i<openImg.Height; i++)
                for (int j = 0; j < openImg.Width; j++)
                {
                    Color RGB = openImg.GetPixel(j, i);
                    resultImg.SetPixel(openImg.Height-i-1,j, RGB);
                }
            pictureBox2.Image = resultImg;

            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Rotating 90 degrees ClockWise -> Done! \r\n");
        }

        // 90 degrees counter-Clockwise rotation
        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Rotating 90 degrees Counter-ClockWise -> processing... \r\n");

            Bitmap openImg = new Bitmap(pictureBox2.Image);
            Bitmap resultImg = new Bitmap(openImg.Height, openImg.Width); // Width, Height exchange (because 90 degrees)

            for (int i = 0; i < openImg.Height; i++)
                for (int j = 0; j < openImg.Width; j++)
                {
                    Color RGB = openImg.GetPixel(j, i);
                    resultImg.SetPixel(i, openImg.Width - j - 1, RGB);
                }
            pictureBox2.Image = resultImg;

            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Rotating 90 degrees Counter-ClockWise -> Done! \r\n");
        }

        // 180 degrees rotation
        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] 180 degrees rotation -> processing...\r\n");

            Bitmap openImg = new Bitmap(pictureBox2.Image);
            Bitmap resultImg = new Bitmap(openImg.Width, openImg.Height);

            for (int i = 0;i<openImg.Height;i++)
                for (int j = 0;j<openImg.Width;j++)
                {
                    Color RGB = openImg.GetPixel(j, i);
                    resultImg.SetPixel(openImg.Width-j-1,openImg.Height-i-1, RGB);
                }
            pictureBox2.Image = resultImg;

            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] 180 degrees rotation -> Done!\r\n");
        }

        // Select Binarization ForeColor
        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Select Binarization ForeColor... ");
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button12.BackColor = colorDialog1.Color;
                button12.ForeColor = Color.FromArgb(255 - colorDialog1.Color.R, 255 - colorDialog1.Color.G, 255 - colorDialog1.Color.B);
                textBox1.AppendText($"{colorDialog1.Color.Name}\r\n");
            }
            else
            {
                textBox1.AppendText($"Cancel (current: {button12.BackColor.Name})\r\n");
            }
        }
        
        // Select Binarization BackColor
        private void button13_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Select Binarization BackColor... ");
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button13.BackColor = colorDialog1.Color;
                button13.ForeColor = Color.FromArgb(255 - colorDialog1.Color.R, 255 - colorDialog1.Color.G, 255 - colorDialog1.Color.B);
                textBox1.AppendText($"{colorDialog1.Color.Name}\r\n");
            }
            else
            {
                textBox1.AppendText($"Cancel (current: {button13.BackColor.Name})\r\n");
            }
        }

        // Robert - Select HighLight Color
        private void button14_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Select Edge Detection HighLight Color... ");
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                button14.BackColor = colorDialog1.Color;
                button14.ForeColor = Color.FromArgb(255 - colorDialog1.Color.R, 255 - colorDialog1.Color.G, 255 - colorDialog1.Color.B);
                textBox1.AppendText($"{colorDialog1.Color.Name}\r\n");
            }
            else
            {
                textBox1.AppendText($"Cancel (current: {button14.BackColor.Name})\r\n");
            }
        }

        // Clear history
        private void button15_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        // Reset all settings
        private void button16_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Reset all settings\r\n");

            trackBar1.Value = 50;

            label3.Text = "Threshold: 50";

            button12.ForeColor = Color.Black;
            button12.BackColor= Color.White;

            button13.ForeColor = Color.White;
            button13.BackColor= Color.Black;

            trackBar2.Value = 30;

            label4.Text = "Threshold: 30";

            button14.ForeColor = Color.Black;
            button14.BackColor= Color.Lime;

            // Properties.Settings.Default.Reset();
        }

        // Export history
        private void button17_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog2 = new SaveFileDialog();
            saveFileDialog2.Filter = "Normal text file|*.txt|All types|*.*";
            saveFileDialog2.Title = "Export history as";
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog2.FileName, textBox1.Text);
                textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] History exported to: {saveFileDialog2.FileName}\r\n");
            }
            
        }

        // Inverse color
        private void button18_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Inverse color -> processing... \r\n");

            Bitmap openImg = new Bitmap(pictureBox2.Image);
            Bitmap resultImg = new Bitmap(openImg.Width, openImg.Height);

            for (int i = 0; i < openImg.Height; i++)
                for (int j = 0; j < openImg.Width; j++)
                {
                    Color RGB = openImg.GetPixel(j, i);
                    resultImg.SetPixel(j, i, Color.FromArgb(255 - RGB.R, 255 - RGB.G, 255 - RGB.B));
                }
            pictureBox2.Image = resultImg;

            textBox1.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Inverse color -> Done! \r\n");
        }
    }
}
