using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;

namespace ImageViewerPSI
{
    class MyImage
    {
        string typeImage;
        int tailleFichier;
        int tailleOffset;
        int largeur;
        int hauteur;
        int nbbitforcolor;
        int tailleImage;
        int tailleOriginale;
        Pixel[,] im;
        public MyImage(string myfile)
        {
            byte[] head = File.ReadAllBytes(myfile);
            typeImage = Convert.ToString(Convert.ToChar(head[0]));
            typeImage += Convert.ToString(Convert.ToChar(head[1]));

            byte[] tab = new byte[4] { head[2], head[3], head[4], head[5] };
            tailleFichier = Convertir_Endian_To_Int(tab);

            tab = new byte[4] { head[10], head[11], head[12], head[13] };
            tailleOffset = Convertir_Endian_To_Int(tab);

            tab = new byte[4] { head[18], head[19], head[20], head[21] };
            largeur = Convertir_Endian_To_Int(tab);
            tailleOriginale = largeur;

            tab = new byte[4] { head[22], head[23], head[24], head[25] };
            hauteur = Convertir_Endian_To_Int(tab);

            tab = new byte[2] { head[28], head[29] };
            nbbitforcolor = Convertir_Endian_To_Int(tab);

            tab = new byte[4] { head[34], head[35], head[36], head[37] };
            tailleImage = Convertir_Endian_To_Int(tab);

            im = new Pixel[hauteur, largeur];
            int n = 54;
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    im[i, j] = new Pixel(head[n], head[n + 1], head[n + 2]);
                    n += 3;
                }
            }
            //Console.ReadKey();
        }

        public MyImage(MyImage image)
        {
            typeImage = image.TypeImage;

            tailleFichier = image.TailleFichier;

            tailleOffset = image.TailleOffset;

            largeur = image.Largeur;

            hauteur = image.Hauteur;

            nbbitforcolor = image.NbBitForColor;

            tailleImage = image.TailleImage;

            im = new Pixel[hauteur, largeur];
        }

        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int rep = 0;
            for (int i = 0; i < tab.Length - 1; i++)
            {
                rep += Convert.ToInt32(tab[i] * Math.Pow(256, i));
            }
            return rep;
        }
        static byte[] Convertir_Int_To_Endian(int val)
        {
            byte[] rep = null;
            if (val >= 0)
            {
                rep = new byte[4];

                if (val < 256)
                {
                    rep[0] = Convert.ToByte(val);
                    rep[1] = 0;
                    rep[2] = 0;
                    rep[3] = 0;
                }
                else
                {
                    rep[3] = Convert.ToByte(val / (256 * 256 * 256));
                    if (val >= 256 * 256 * 256)
                    {
                        int j = val / (256 * 256 * 256);
                        val -= j * 256 * 256 * 256;
                    }
                    rep[2] = Convert.ToByte(val / (256 * 256));
                    if (val >= 256 * 256)
                    {
                        int j = val / (256 * 256);
                        val -= j * 256 * 256;
                    }
                    rep[1] = Convert.ToByte(val / 256);
                    if (val >= 256)
                    {
                        int j = val / (256);
                        val -= j * 256;
                    }
                    rep[0] = Convert.ToByte(val);

                }
            }
            return rep;
        }
        public void FromImagetoFile(string file)
        {
            BinaryWriter nouvfile = new BinaryWriter(File.Create(file));

            nouvfile.Write(Convert.ToByte(typeImage[0]));
            nouvfile.Write(Convert.ToByte(typeImage[1]));

            byte[] head = Convertir_Int_To_Endian(tailleFichier);
            for (int i = 0; i < head.Length; i++)
            {
                nouvfile.Write(head[i]);
            }
            nouvfile.Write(0);

            head = Convertir_Int_To_Endian(tailleOffset);
            for (int i = 0; i < head.Length; i++)
            {
                nouvfile.Write(head[i]);
            }


            head = Convertir_Int_To_Endian(40);
            for (int i = 0; i < head.Length; i++)
            {
                nouvfile.Write(head[i]);
            }

            head = Convertir_Int_To_Endian(largeur);
            for (int i = 0; i < head.Length; i++)
            {
                nouvfile.Write(head[i]);
            }

            head = Convertir_Int_To_Endian(hauteur);
            for (int i = 0; i < head.Length; i++)
            {
                nouvfile.Write(head[i]);
            }

            nouvfile.Write(true); nouvfile.Write(false);

            head = Convertir_Int_To_Endian(nbbitforcolor);
            for (int i = 0; i < 2; i++)
            {
                nouvfile.Write(head[i]);
            }
            nouvfile.Write(0);
            head = Convertir_Int_To_Endian(tailleImage);
            nouvfile.Write(head);
            for (int i = 0; i < 4; i++) { nouvfile.Write(0); }

            //n = 2;
            //while (n > 0)
            //{
            //    for (int i = 0; i < head.Length; i++)
            //    {
            //        nouvfile.Write(head[i]);
            //    }
            //    n--;
            //}

            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    nouvfile.Write(im[i, j].R);
                    nouvfile.Write(im[i, j].G);
                    nouvfile.Write(im[i, j].B);

                }
            }

            nouvfile.Close();
        }

        public void CreatImage(Pixel[,] picture, string nom)
        {
            BinaryWriter nouvfile = new BinaryWriter(File.Create(nom));

            nouvfile.Write(Convert.ToByte(typeImage[0]));
            nouvfile.Write(Convert.ToByte(typeImage[1]));

            byte[] head = Convertir_Int_To_Endian(picture.Length * 3 + 54);
            for (int i = 0; i < head.Length; i++)
            {
                nouvfile.Write(head[i]);
            }
            nouvfile.Write(0);

            head = Convertir_Int_To_Endian(54);
            for (int i = 0; i < head.Length; i++)
            {
                nouvfile.Write(head[i]);
            }


            head = Convertir_Int_To_Endian(40);
            for (int i = 0; i < head.Length; i++)
            {
                nouvfile.Write(head[i]);
            }

            head = Convertir_Int_To_Endian(picture.GetLength(0));
            for (int i = 0; i < head.Length; i++)
            {
                nouvfile.Write(head[i]);
            }

            head = Convertir_Int_To_Endian(picture.GetLength(1));
            for (int i = 0; i < head.Length; i++)
            {
                nouvfile.Write(head[i]);
            }
            nouvfile.Write(true); nouvfile.Write(false);

            head = Convertir_Int_To_Endian(nbbitforcolor);
            for (int i = 0; i < 2; i++)
            {
                nouvfile.Write(head[i]);
            }
            nouvfile.Write(0);
            head = Convertir_Int_To_Endian(tailleImage);
            nouvfile.Write(head);
            for (int i = 0; i < 4; i++) { nouvfile.Write(0); }

            for (int i = 0; i < picture.GetLength(0); i++)
            {
                for (int j = 0; j < picture.GetLength(1); j++)
                {
                    nouvfile.Write(picture[i, j].R);
                    nouvfile.Write(picture[i, j].G);
                    nouvfile.Write(picture[i, j].B);

                }
            }

            nouvfile.Close();
        }
        
        public void ChangerTailleImage(bool ChangerTailleOriginale = false)
        {
            tailleImage = Im.Length * 3;
            hauteur = Im.GetLength(0);
            largeur = Im.GetLength(1);
            if (ChangerTailleOriginale) { tailleOriginale = largeur; }
        }

        public string TypeImage
        {
            get { return typeImage; }
        }

        public int TailleFichier
        {
            get { return tailleFichier; }
        }

        public int TailleOffset
        {
            get { return tailleOffset; }
        }

        public int Largeur
        {
            get { return largeur; }
        }

        public int Hauteur
        {
            get { return hauteur; }
        }

        public int NbBitForColor
        {
            get { return nbbitforcolor; }
        }

        public int TailleImage
        {
            get { return tailleImage; }
        }

        public int TailleOriginale
        {
            get { return tailleOriginale; }
        }

        public Pixel[,] Im
        {
            get { return im; }
            set { im = value; }
        }
    }
}
