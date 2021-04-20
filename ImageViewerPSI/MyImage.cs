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

        public void Rotate90()
        {
            Pixel[,] rot = new Pixel[largeur, hauteur];

            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    rot[largeur - 1 - j, i] = new Pixel(im[i, j].R, im[i, j].G, im[i, j].B);
                }
            }
            im = rot;
            hauteur = im.GetLength(0);
            largeur = im.GetLength(1);
        }
        public void Rotate180()
        {
            Pixel[,] rot = new Pixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    rot[hauteur - 1 - i, largeur - 1 - j] = new Pixel(im[i, j].R, im[i, j].G, im[i, j].B);
                }
            }
            im = rot;
        }

        public void Espoire()
        {
            double angle = 90 * Math.PI / 180;
            int midlar = (largeur - 1) / 2;
            int midhau = (hauteur - 1) / 2;
            int x = Convert.ToInt32(largeur * Math.Cos(angle) + hauteur * Math.Sin(angle));
            int y = Convert.ToInt32(largeur * Math.Sin(angle) + hauteur * Math.Cos(angle));
            Pixel[,] rot = new Pixel[y, x];
            int n = 0;
            int k = 0;
            for (int i = 0; i < hauteur; i++)
            {
                k++;
                for (int j = 0; j < largeur; j++)
                {
                    n++;
                    if (i < (hauteur - 1) / 2)
                    {
                        if (j < (largeur - 1) / 2)
                        {
                            x = -Convert.ToInt32((midlar - j) * Math.Cos(angle) + (midhau - i) * Math.Sin(angle)) - (rot.GetLength(1) - 1) / 2;
                            y = -Convert.ToInt32(-(midlar - j) * Math.Sin(angle) + (midhau - i) * Math.Cos(angle)) - (rot.GetLength(0) - 1) / 2;
                        }
                        else
                        {
                            x = Convert.ToInt32(-(j - midlar) * Math.Sin(angle) + (midhau - i) * Math.Cos(angle)) - (rot.GetLength(1) - 1) / 2;
                            y = Convert.ToInt32((j - midlar) * Math.Cos(angle) + (midhau - i) * Math.Sin(angle)) - (rot.GetLength(0) - 1) / 2;
                        }
                    }
                    else
                    {
                        if (j < midlar)
                        {
                            x = -Convert.ToInt32(-(midlar - j) * Math.Sin(angle) + (i - midhau) * Math.Cos(angle)) - (rot.GetLength(1) - 1) / 2;
                            y = Convert.ToInt32((midlar - j) * Math.Cos(angle) + (i - midhau) * Math.Sin(angle)) - (rot.GetLength(0) - 1) / 2;
                        }
                        else
                        {
                            x = Convert.ToInt32((j - midlar) * Math.Cos(angle) + (i - midhau) * Math.Sin(angle)) - (rot.GetLength(1) - 1) / 2;
                            y = Convert.ToInt32(-(j - midlar) * Math.Sin(angle) + (i - midhau) * Math.Cos(angle)) - (rot.GetLength(0) - 1) / 2;
                        }
                    }

                    if (x < 0)
                    {
                        x = -x;

                    }
                    if (y < 0)
                    {
                        y = -y;
                    }

                    rot[y, x] = new Pixel(im[i, j].R, im[i, j].G, im[i, j].B);

                    if (n == rot.GetLength(1))
                    {
                        n = 0;
                    }
                    if (k == rot.GetLength(0))
                    {
                        k = 0;
                    }

                    if (rot[k, n] == null)
                    {
                        rot[k, n] = new Pixel(255, 255, 255);
                    }
                }
            }
            im = rot;
            hauteur = im.GetLength(0);
            largeur = im.GetLength(1);
        }
        public void Rotate45()
        {
            Pixel[,] rot = new Pixel[im.GetLength(0) + im.GetLength(1) - 1, im.GetLength(0) + im.GetLength(1) - 1];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    rot[largeur - 1 - i + j, i + j] = new Pixel(im[i, j].R, im[i, j].G, im[i, j].B);
                    for (int k = 0; k <= (largeur - 1 - i + j); k++)
                    {
                        for (int n = 0; n <= (i + j); i++)
                        {
                            if (rot[k, n] == null)
                            {
                                rot[k, n] = new Pixel(255, 255, 255);
                            }
                        }
                    }
                }
            }
            im = rot;
            ChangerTailleImage();
        }
        public void Autre()
        {
            Pixel[,] rot = new Pixel[im.GetLength(0) + im.GetLength(1) - 1, im.GetLength(0) + im.GetLength(1) - 1];
            int k = 0;
            int n = 0;
            for (int i = 0; i < rot.GetLength(0); i++)
            {

                for (int j = 0; j < rot.GetLength(1); j++)
                {

                    if (n == largeur)
                    {
                        n = 0;
                    }
                    if (k == hauteur)
                    {
                        k = 0;
                    }

                    rot[largeur - 1 - n + k, n + k] = new Pixel(im[k, n].R, im[k, n].G, im[k, n].B);
                    if ((k + 1) < hauteur && (n + 1) < largeur)
                    {
                        int r = (im[k, n].R + im[k + 1, n + 1].R) / 2;
                        int g = (im[k, n].G + im[k + 1, n + 1].G) / 2;
                        int b = (im[k + 1, n].B + im[k + 1, n + 1].B) / 2;
                        rot[largeur - 1 - n + k, n + k + 1] = new Pixel(Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
                    }
                    if (rot[i, j] == null)
                    {
                        rot[i, j] = new Pixel(255, 255, 255);
                    }
                    n++;
                }
                k++;
            }
            hauteur = rot.GetLength(0);
            largeur = rot.GetLength(1);
            im = rot;
        }
        public void Fractale()
        {
            typeImage = "BM";
            tailleFichier = 40054;
            tailleOffset = 54;
            largeur = 200;
            hauteur = 200;
            nbbitforcolor = 24;
            tailleImage = 0;
            im = new Pixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if (i == (hauteur - 1))
                    {
                        im[i, j] = new Pixel(0, 0, 0);
                    }
                    else
                    {
                        im[i, j] = new Pixel(255, 255, 255);
                    }
                }
            }
            int n = 6;
            while (n > 0)
            {
                n--;
                for (int i = 0; i < hauteur; i++)
                {
                    for (int j = 0; j < largeur; j++)
                    {

                    }
                }
            }


        }
        public SortedList<byte, int[]> HistogrammeRGB()
        {
            SortedList<byte, int[]> histo = new SortedList<byte, int[]>();
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    byte keyR = im[i, j].R;
                    byte keyG = im[i, j].G;
                    byte keyB = im[i, j].B;
                    if (histo.ContainsKey(keyR) == true)
                    {
                        histo.Values[histo.IndexOfKey(keyR)][0]++;
                    }
                    else
                    {
                        int[] val = new int[3] { 1, 0, 0 };
                        histo.Add(keyR, val);
                    }
                    if (histo.ContainsKey(keyG) == true)
                    {
                        histo.Values[histo.IndexOfKey(keyG)][1]++;
                    }
                    else
                    {
                        int[] val = new int[3] { 0, 1, 0 };
                        histo.Add(keyG, val);
                    }
                    if (histo.ContainsKey(keyB) == true)
                    {
                        histo.Values[histo.IndexOfKey(keyB)][2]++;
                    }
                    else
                    {
                        int[] val = new int[3] { 0, 0, 1 };
                        histo.Add(keyB, val);
                    }

                }

            }
            return histo;
        }
        public SortedList<int, int[]> HistogrammeRGBb()
        {
            SortedList<int, int[]> histo = new SortedList<int, int[]>();
            int vmax = 1;
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    int keyR = Convert.ToInt32(im[i, j].R);
                    int keyG = Convert.ToInt32(im[i, j].G);
                    int keyB = Convert.ToInt32(im[i, j].B);
                    if (histo.ContainsKey(keyR) == true)
                    {
                        histo.Values[histo.IndexOfKey(keyR)][0]++;

                        if (histo.Values[histo.IndexOfKey(keyR)][0] > vmax)
                        {
                            vmax = histo.Values[histo.IndexOfKey(keyR)][0];
                        }
                    }
                    else
                    {
                        int[] val = new int[3] { 1, 0, 0 };
                        histo.Add(keyR, val);
                    }
                    if (histo.ContainsKey(keyG) == true)
                    {
                        histo.Values[histo.IndexOfKey(keyG)][1]++;

                        if (histo.Values[histo.IndexOfKey(keyG)][1] > vmax)
                        {
                            vmax = histo.Values[histo.IndexOfKey(keyG)][1];
                        }
                    }
                    else
                    {
                        int[] val = new int[3] { 0, 1, 0 };
                        histo.Add(keyG, val);
                    }
                    if (histo.ContainsKey(keyB) == true)
                    {
                        histo.Values[histo.IndexOfKey(keyB)][2]++;

                        if (histo.Values[histo.IndexOfKey(keyB)][2] > vmax)
                        {
                            vmax = histo.Values[histo.IndexOfKey(keyB)][2];
                        }
                    }
                    else
                    {
                        int[] val = new int[3] { 0, 0, 1 };
                        histo.Add(keyB, val);
                    }

                }

            }
            int[] valu = new int[1] { vmax };
            histo.Add(256, valu);
            return histo;
        }
        public void Historigramme()
        {
            SortedList<int, int[]> histo = HistogrammeRGBb();
            Pixel[,] graph = new Pixel[histo.Values[histo.Count - 1][0] + 2, histo.Count];

            for (int i = 0; i < graph.GetLength(0); i++)
            {
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    if (i == 0 || j == 0)
                    {
                        graph[i, j] = new Pixel(0, 0, 0);
                    }
                    else
                    {
                        int[] val = histo.Values[j - 1];
                        graph[val[0], j] = new Pixel(255, 0, 0);
                        graph[val[1], j] = new Pixel(0, 255, 0);
                        graph[val[2], j] = new Pixel(0, 0, 255);
                        if (val[0] == val[1] && val[1] == val[2])
                        {
                            graph[val[0], j] = new Pixel(0, 0, 0);
                        }
                    }
                    if (graph[i, j] == null)
                    {
                        graph[i, j] = new Pixel(255, 255, 255);
                    }
                }
            }
            CreatImage(graph, "historigramme");
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
        public void Verif()
        {
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    Console.Write(im[i, j].R + "" + im[i, j].G + "" + im[i, j].B + "");
                }
                Console.WriteLine();
            }
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
