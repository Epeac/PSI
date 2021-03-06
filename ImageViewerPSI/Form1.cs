using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageViewerPSI
{
    public partial class Form1 : Form
    {
        OpenFileDialog dlg;
        string nomImage = "";
        MyImage image = new MyImage("./ImagesLecture/idle.bmp");

        decimal rotation = 0;

        public Form1()
        {
            InitializeComponent();
            dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.bmp)|*.bmp";
            pictureBox1.Image = Image.FromFile("./ImagesLecture/idle.bmp");
            toolTip1.SetToolTip(button7, "Fractale de Mandelbrot");
            toolTip2.SetToolTip(comboBox2, "L'image doit être de la même taille ou plus petite que l'image ouverte");
            toolTip3.SetToolTip(textBox1, "Le texte doit faire 45 caractères MAX");
        }

        private void cocoToolStripMenuItem_Click(object sender, EventArgs e) //Ouvrir coco
        {
            nomImage = "coco";
            image = new MyImage("./ImagesLecture/" + nomImage + ".bmp");
            Image imageAff = Image.FromFile("./ImagesLecture/" + nomImage + ".bmp");
            pictureBox1.Image = imageAff;
        }
        private void lenaToolStripMenuItem_Click(object sender, EventArgs e) //Ouvrir lena
        {
            nomImage = "lena";
            image = new MyImage("./ImagesLecture/" + nomImage + ".bmp");
            Image imageAff = Image.FromFile("./ImagesLecture/" + nomImage + ".bmp");
            pictureBox1.Image = imageAff;
        }
        private void testaToolStripMenuItem_Click(object sender, EventArgs e) //Ouvrir grille
        {
            nomImage = "grille";
            image = new MyImage("./ImagesLecture/" + nomImage + ".bmp");
            Image imageAff = Image.FromFile("./ImagesLecture/" + nomImage + ".bmp");
            pictureBox1.Image = imageAff;
        }
        private void lacToolStripMenuItem_Click(object sender, EventArgs e) //Ouvrir aigle
        {
            nomImage = "aigle";
            image = new MyImage("./ImagesLecture/" + nomImage + ".bmp");
            Image imageAff = Image.FromFile("./ImagesLecture/" + nomImage + ".bmp");
            pictureBox1.Image = imageAff;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) //Save
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "bmp (*.bmp)|*.bmp";

            if (sfd.ShowDialog() == DialogResult.OK && sfd.FileName.Length > 0)
            {
                pictureBox1.Image.Save(sfd.FileName);
                
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) //Exit
        {
            Application.Exit();
        }

        private void button10_Click(object sender, EventArgs e) //Noir et Blanc
        {
            Program.NoirEtBlanc(image);
            nomImage += "NetB";
            image.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
            pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
        }
        private void button1_Click(object sender, EventArgs e) //Miroir
        {
            Program.Miroir(image);
            nomImage += "Miroir";
            image.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
            pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
        }
        private void button2_Click(object sender, EventArgs e) //Contour
        {
            Program.Contour(image, Convert.ToInt32(numericUpDown1.Value));
            nomImage += "Contour";
            image.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
            pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
        }
        private void button3_Click(object sender, EventArgs e) //RenforceBord
        {
            Program.Renforcebord(image);
            nomImage += "Renfbord";
            image.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
            pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
        }
        private void button5_Click(object sender, EventArgs e) //Flou
        {
            Program.Flou(image);
            nomImage += "Flou";
            image.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
            pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
        }
        private void button11_Click(object sender, EventArgs e) //Repoussage
        {
            Program.Repoussage(image);
            nomImage += "Repoussage";
            image.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
            pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
        }
        private void button6_Click(object sender, EventArgs e) //Historigramme
        {
            Program.HistogrammeRGB(image);
            nomImage += "Histo";
            image.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
            pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
        }
        private void button7_Click(object sender, EventArgs e) //Fractale
        {
            Program.FractaleMandelbrot(image);
            nomImage += "Fractale";
            image.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
            pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
        }
        private void button8_Click(object sender, EventArgs e) //Reveler Image
        {
            MyImage temp = Program.RevelerImage(image);
            nomImage += "Revele";
            temp.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
            image = new MyImage("./ImagesSortie/" + nomImage + ".bmp");
            pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) //Cacher Image
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    MyImage coco = new MyImage("./ImagesLecture/coco.bmp");
                    MyImage temp = Program.CacherImage(image, coco);
                    if(temp != null)
                    {
                        nomImage += "Cachecoco";
                        temp.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
                        image = new MyImage("./ImagesSortie/" + nomImage + ".bmp");
                        pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
                    }
                    break;
                case 1:
                    MyImage lena = new MyImage("./ImagesLecture/lena.bmp");
                    temp = Program.CacherImage(image, lena);
                    if (temp != null)
                    {
                        nomImage += "Cachelena";
                        temp.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
                        image = new MyImage("./ImagesSortie/" + nomImage + ".bmp");
                        pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
                    }
                    break;
                case 2:
                    MyImage aigle = new MyImage("./ImagesLecture/aigle.bmp");
                    temp = Program.CacherImage(image, aigle);
                    if (temp != null)
                    {
                        nomImage += "Cacheaigle";
                        temp.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
                        image = new MyImage("./ImagesSortie/" + nomImage + ".bmp");
                        pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
                    }
                    break;
                case 3:
                    MyImage grille = new MyImage("./ImagesLecture/grille.bmp");
                    temp = Program.CacherImage(image, grille);
                    if (temp != null)
                    {
                        nomImage += "Cachegrille";
                        temp.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
                        image = new MyImage("./ImagesSortie/" + nomImage + ".bmp");
                        pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
                    }
                    break;
                case 4:
                    MyImage test = new MyImage("./ImagesLecture/test.bmp");
                    temp = Program.CacherImage(image, test);
                    if (temp != null)
                    {
                        nomImage += "Cachetest";
                        temp.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
                        image = new MyImage("./ImagesSortie/" + nomImage + ".bmp");
                        pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
                    }
                    break;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //Selection
        {
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    cocoToolStripMenuItem_Click(sender, e);
                    break;
                case 1:
                    lenaToolStripMenuItem_Click(sender, e);
                    break;
                case 2:
                    lacToolStripMenuItem_Click(sender, e);
                    break;
                case 3:
                    testaToolStripMenuItem_Click(sender, e);
                    break;
                case 4:
                    testaToolStripMenuItem_Click(sender, e);
                    break;
            }
        }
        private void button4_Click(object sender, EventArgs e) //RESET
        {
            switch(nomImage[0])
            {
                case 'c':
                    cocoToolStripMenuItem_Click(sender, e);
                    break;
                case 'l':
                    lenaToolStripMenuItem_Click(sender, e);
                    break;
                case 'a':
                    lacToolStripMenuItem_Click(sender, e);
                    break;
                case 'g':
                    testaToolStripMenuItem_Click(sender, e);
                    break;
                case 't':
                    testaToolStripMenuItem_Click(sender, e);
                    break;
            }
            nomImage += "R";
        }

        private void button9_Click(object sender, EventArgs e) //Generer QR code
        {
            if(textBox1.Text != null && textBox1.Text != "")
            {
                string phrase = textBox1.Text;
                phrase = phrase.ToUpper();
                List<string> donnée = Program.Encode(phrase);
                int version;
                int eccCount;
                if (phrase.Length <= 25)
                {
                    version = 19;
                    eccCount = 7;
                }
                else
                {
                    version = 34;
                    eccCount = 10;
                }
                string chaine = Program.Correction(donnée, version);

                List<string> don = new List<string>();
                string passage = "";
                for (int i = 0; i < chaine.Length; i++)
                {
                    passage += chaine[i];
                    int j = (i + 1) / 8;
                    if (i + 1 - j * 8 == 0)
                    {
                        don.Add(passage);
                        passage = "";
                    }
                }
                byte[] reed = new byte[don.Count];
                for (int i = 0; i < don.Count; i++)
                {
                    reed[i] = Convert.ToByte(don[i], 2);
                }
                byte[] result = ReedSolomonAlgorithm.Encode(reed, eccCount, ErrorCorrectionCodeType.QRCode);
                don = new List<string>();
                foreach (byte val in result)
                {
                    don.Add(Convert.ToString(val, 2));
                    while (don[don.Count - 1].Length < 8)
                    {
                        don[don.Count - 1] = "0" + don[don.Count - 1];
                    }
                }
                foreach (string val in don) chaine += val;
                if (eccCount != 7) chaine += "0000000";
                MyImage test = new MyImage("./ImagesLecture/coco.bmp");
                test.Im = Program.QRcodeV1(version);
                Program.RemplirQrcod(test.Im, chaine);
                test.Im = Program.Contour(test.Im);
                test.ChangerTailleImage(true);
                Program.Agrandir(test, 40);
                Program.Retrecir(test, 2);
                nomImage += "QRCode"; 
                test.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
                image = new MyImage("./ImagesSortie/" + nomImage + ".bmp");
                pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }
               
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) //Taille
        {
            int t = image.Largeur / image.TailleOriginale;
            if (t < 1)
            {
                t = image.TailleOriginale / image.Largeur;
                Program.Agrandir(image, t);
            }
            else { Program.Retrecir(image, t); }
            nomImage += "Normal"; 
            
            if (treeView1.SelectedNode.Text == "25%")
            {
                Program.Retrecir(image, 4);
                nomImage += "Retreci"; 
            }
            if (treeView1.SelectedNode.Text == "50%")
            {
                Program.Retrecir(image, 2);
                nomImage += "Retreci"; 
            }
            if (treeView1.SelectedNode.Text == "200%")
            {
                Program.Agrandir(image, 2);
                nomImage += "Agrandi"; 
            }
            if (treeView1.SelectedNode.Text == "300%")
            {
                Program.Agrandir(image, 3);
                nomImage += "Agrandi"; 
            }
            image.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
            pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
        }
        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e) //Rotation (non fonctionnel)
        {
            //if (0 > rotation)
            //{
            //    image.Rotate45();
            //    nomImage += "Rotation";
            //    image.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
            //    pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
            //}
            //else
            //{
            //    image.Rotate180();
            //    image.Rotate90();
            //    image.Rotate45();
            //    nomImage += "Rotation";
            //    image.FromImagetoFile("./ImagesSortie/" + nomImage + ".bmp");
            //    pictureBox1.Image = Image.FromFile("./ImagesSortie/" + nomImage + ".bmp");
            //}
        }

    }
}
