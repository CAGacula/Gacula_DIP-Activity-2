using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing.Imaging;
using System.Windows.Forms;


namespace WebCam
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private Bitmap img1, img2, imgRes;
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;
        }

        private void webCamModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitializeWebcam();
        }

        private void InitializeWebcam()
        {
            // Get available video devices
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            // If there is at least one video device, use it
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
                videoSource.Start();
            }
            else
            {
                MessageBox.Show("No video devices found.");
            }
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap resizedFrame = new Bitmap(eventArgs.Frame, pictureBox1.Size);
            img1 = (Bitmap)eventArgs.Frame.Clone();
            // Display the live feed in PictureBox
            pictureBox1.Image = resizedFrame;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop the webcam when closing the form
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (imgRes != null)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Bitmap Image|*.bmp|JPEG Image|*.jpg|PNG Image|*.png";
                saveDialog.Title = "Save Image";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveDialog.FileName;

                    ImageFormat imageFormat = ImageFormat.Bmp;
                    string extension = Path.GetExtension(filePath);
                    switch (extension.ToLower())
                    {
                        case ".jpg":
                            imageFormat = ImageFormat.Jpeg;
                            break;
                        case ".png":
                            imageFormat = ImageFormat.Png;
                            break;
                        default:
                            break;
                    }

                    imgRes.Save(filePath, imageFormat);
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void btnLoad_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            string fileName = openFileDialog1.FileName;
            string extension = Path.GetExtension(fileName).ToLower();

            //checks if the file has a valid extension name
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                img1 = new Bitmap(fileName);

                pictureBox1.Image = img1;
            }
            else
            {
                MessageBox.Show("Please select a valid file with JPG or PNG extension.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private void openFileDialog2_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            string fileName = openFileDialog2.FileName;
            string extension = Path.GetExtension(fileName).ToLower();

            //checks if the file has a valid extension name
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                img2 = new Bitmap(fileName);

                pictureBox3.Image = img2;
            }
            else
            {
                MessageBox.Show("Please select a valid file with JPG or PNG extension.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox3.Image = null;

            imgRes = new Bitmap(img1.Width, img1.Height);

            for (int x = 0; x < img1.Width; x++)
            {
                for (int y = 0; y < img1.Height; y++)
                {
                    Color originalColor = img1.GetPixel(x, y);
                    imgRes.SetPixel(x, y, originalColor);
                }
            }

            pictureBox2.Image = imgRes;

        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox3.Image = null;
            imgRes = new Bitmap(img1.Width, img1.Height);
            int greyScale;
            for (int x = 0; x < img1.Width; x++)
            {
                for (int y = 0; y < img1.Height; y++)
                {
                    Color img1Color = img1.GetPixel(x, y);
                    greyScale = ((img1Color.R + img1Color.G + img1Color.B) / 3);
                    Color greyColor = Color.FromArgb(greyScale, greyScale, greyScale);
                    imgRes.SetPixel(x, y, greyColor);
                }
            }

            pictureBox2.Image = imgRes;
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox3.Image = null;
            imgRes = new Bitmap(img1.Width, img1.Height);
            int r, g, b;
            for (int x = 0; x < img1.Width; x++)
            {
                for (int y = 0; y < img1.Height; y++)
                {
                    Color img1Color = img1.GetPixel(x, y);
                    r = 255 - img1Color.R;
                    g = 255 - img1Color.G;
                    b = 255 - img1Color.B;
                    Color invertColor = Color.FromArgb(r, g, b);
                    imgRes.SetPixel(x, y, invertColor);
                }
            }
            pictureBox2.Image = imgRes;

        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            imgRes = new Bitmap(img1.Width, img1.Height);
            int greyScale;
            int[] histogram = new int[256];
            Color result;
            for (int x = 0; x < img1.Width; x++)
            {
                for (int y = 0; y < img1.Height; y++)
                {
                    Color img1Color = img1.GetPixel(x, y);
                    greyScale = ((img1Color.R + img1Color.G + img1Color.B) / 3);
                    Color greyColor = Color.FromArgb(greyScale, greyScale, greyScale);
                    imgRes.SetPixel(x, y, greyColor);
                }
            }

            for (int x = 0; x < img1.Width; x++)
            {
                for (int y = 0; y < img1.Height; y++)
                {
                    result = imgRes.GetPixel(x, y);
                    histogram[result.R] += 1;
                }
            }
            Bitmap visGraph = new Bitmap(256, 1000);
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    visGraph.SetPixel(x, y, Color.White);
                }
            }

            //plotting histogram 
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < Math.Min(histogram[x] / 5, 1000); y++)
                {
                    visGraph.SetPixel(x, 999 - y, Color.Black);
                }
            }
            pictureBox2.Image = visGraph;

        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox3.Image = null;
            imgRes = new Bitmap(img1.Width, img1.Height);
            int r, g, b;
            for (int x = 0; x < img1.Width; x++)
            {
                for (int y = 0; y < img1.Height; y++)
                {
                    Color img1Color = img1.GetPixel(x, y);
                    r = (int)(0.393 * img1Color.R) + (int)(0.769 * img1Color.G) + (int)(0.272 * img1Color.B);
                    g = (int)(0.349 * img1Color.R) + (int)(0.686 * img1Color.G) + (int)(0.168 * img1Color.B);
                    b = (int)(0.272 * img1Color.R) + (int)(0.534 * img1Color.G) + (int)(0.131 * img1Color.B);
                    if (r > 255)
                    {
                        r = 255;
                    }
                    if (g > 255)
                    {
                        g = 255;
                    }
                    if (b > 255)
                    {
                        b = 255;
                    }
                    Color sepiaColor = Color.FromArgb(r, g, b);
                    imgRes.SetPixel(x, y, sepiaColor);
                }
            }
            pictureBox2.Image = imgRes;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            callBrightness();
        }

        private void callBrightness()
        {
            pictureBox3.Image = null;
            int value = trackBar1.Value;
            imgRes = new Bitmap(img1.Width, img1.Height);
            for (int x = 0; x < img1.Width; x++)
            {
                for (int y = 0; y < img1.Height; y++)
                {

                    Color img1Color = img1.GetPixel(x, y);
                    if (value >= 0)
                    {
                        imgRes.SetPixel(x, y, Color.FromArgb(Math.Min(img1Color.R + value, 255), Math.Min(img1Color.G + value, 255), Math.Min(img1Color.B + value, 255)));

                    }
                    else
                    {
                        imgRes.SetPixel(x, y, Color.FromArgb(Math.Max(img1Color.R + value, 0), Math.Max(img1Color.G + value, 0), Math.Max(img1Color.B + value, 0)));
                    }

                }
            }
            pictureBox2.Image = imgRes;
        }

        private void subtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img1 == null || img2 == null)
            {
                MessageBox.Show("No loaded image.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                if (img1.Width != img2.Width || img1.Height != img2.Height)
                {
                    MessageBox.Show("Image sizes are different. Cannot perform subtraction.", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    imgRes = new Bitmap(img1.Width, img1.Height);
                    Color mygreen = Color.FromArgb(0, 0, 255);
                    int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
                    int threshold = 5;

                    for (int x = 0; x < img1.Width; x++)
                    {
                        for (int y = 0; y < img1.Height; y++)
                        {
                            Color pixel = img1.GetPixel(x, y);
                            Color backpixel = img2.GetPixel(x, y);
                            int grey = (pixel.R + pixel.G + pixel.B) / 3;
                            int subtractval = Math.Abs(grey - greygreen);
                            if (subtractval < threshold)
                            {
                                imgRes.SetPixel(x, y, backpixel);
                            }
                            else
                            {
                                imgRes.SetPixel(x, y, pixel);
                            }
                        }
                        pictureBox2.Image = imgRes;
                    }
                }
            }
        }

       
    }
}