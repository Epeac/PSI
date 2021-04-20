using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using System.ComponentModel;
using System.Diagnostics;

namespace ImageViewerPSI
{
    static class Program
    {
        static void Bitmap()
        {
            //Bitmap a = new Bitmap(200, 100);
            Bitmap b = new Bitmap("./Images/lena.bmp");
            //b.RotateFlip(RotateFlipType.Rotate180FlipX);
            //b.Save("./Images2/lenasortie1.bmp");

            ColorPalette pal = b.Palette;
            for (int i = 0; i < pal.Entries.Length; i++)
                Console.WriteLine(pal.Entries[i] + "0");

            RectangleF rec = new RectangleF(10.0f, 10.0f, 100.0f, 100.0f);
            Bitmap c = b.Clone(rec, PixelFormat.DontCare);
            c.Save("./Images2/lenasortie2.bmp");

            for (int i = 0; i < b.Height; i++)
                for (int j = 0; j < b.Width; j++)
                {
                    Color mycolor = b.GetPixel(j, i);
                    b.SetPixel(j, i, Color.FromArgb(255 - mycolor.R, 255 - mycolor.R, 255 - mycolor.R));

                    //c.SetPixel(i, j, Color.Coral);
                }

            c.MakeTransparent(Color.Gray);
            c.MakeTransparent(Color.White);
            b.Save("./Images2/lenasortie3.bmp");

            c.Save("./Images2/lenasortie4.bmp"); ;
            //Marche si l'image est sous le même répertoire que l'exécutable
            //Process.Start("lenasortie1.bmp");
            Console.ReadKey();
        }

        static void LectureImage(string fichier)
        {
            byte[] myfile = File.ReadAllBytes(fichier);
            //myfile est un vecteur composé d'octets représentant les métadonnées et les données de l'image

            //Métadonnées du fichier
            Console.WriteLine("\n Header \n");
            for (int i = 0; i < 14; i++)
                Console.Write(myfile[i] + " ");
            //Métadonnées de l'image
            Console.WriteLine("\n HEADER INFO \n");
            for (int i = 14; i < 54; i++)
                Console.Write(myfile[i] + " ");
            //L'image elle-même
            Console.WriteLine("\n IMAGE \n");
            for (int i = 54; i < myfile.Length; i++ /*= i + 60*/)
            {
                //for (int j = i; j < i + 60; j++)
                //{
                //    Console.Write(myfile[j] + " ");

                //}
                //Console.WriteLine();
                Console.Write(myfile[i] + " ");
            }

            //File.WriteAllBytes("./ImagesLecture/Sortie.bmp", myfile);

            Console.ReadLine();
        }

        static void NoirEtBlanc(byte[] image)
        {
            int a = 0;
            for (int i = 54; i < image.Length - 3; i += 3)
            {
                a = (image[i] + image[i + 1] + image[i + 2]) / 3;
                image[i] = Convert.ToByte(a); image[i + 1] = Convert.ToByte(a); image[i + 2] = Convert.ToByte(a);
            }
        }

        //TD2

        public static void Miroir(MyImage image2)
        {
            Pixel temp = new Pixel(0, 0, 0);
            for (int i = 0; i < image2.Im.GetLength(0); i++)
            {
                for (int j = 0; j < image2.Im.GetLength(1) / 2; j++)
                {
                    temp = image2.Im[i, j];
                    image2.Im[i, j] = image2.Im[i, image2.Im.GetLength(1) - j - 1];
                    image2.Im[i, image2.Im.GetLength(1) - j - 1] = temp;
                }
            }
        }

        public static void Retrecir(MyImage image, int taille)
        {
            Pixel[,] res = new Pixel[image.Hauteur / taille, image.Largeur / taille];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    res[i, j] = image.Im[i * taille, j * taille];
                }
            }
            image.Im = res;
            image.ChangerTailleImage(); //changer taille image
        }

        public static void Agrandir(MyImage image, int taille)
        {
            Pixel[,] res = new Pixel[image.Hauteur * taille, image.Largeur * taille];
            for (int i = 0; i < image.Hauteur; i++)
            {
                for (int j = 0; j < image.Largeur; j++)
                {
                    for (int a = 0; a < taille; a++)
                    {
                        for (int b = 0; b < taille; b++)
                        {
                            res[i * taille + a, j * taille + b] = image.Im[i, j];
                        }
                    }
                }
            }
            image.Im = res;
            image.ChangerTailleImage(); //changer taille image
        }

        public static void Rotation(MyImage image, int angle)
        {

        }

        //TD4

        static Pixel Somme(int[,] noyau, MyImage image, int ligne, int colonne, int n)
        {
            int r = 0; int g = 0; int b = 0;
            int ligneNoyau = noyau.GetLength(0);
            int colonneNoyau = noyau.GetLength(1);

            for (int i = 0; i < ligneNoyau; i++)
            {
                for (int j = 0; j < colonneNoyau; j++)
                {

                    int x = i + (ligne - ligneNoyau / 2);
                    if (x < 0) x = image.Im.GetLength(0) + x;
                    if (x >= image.Im.GetLength(0)) x = x - image.Im.GetLength(0);

                    int y = j + (colonne - colonneNoyau / 2);
                    if (y < 0) y = image.Im.GetLength(1) + y;
                    if (y >= image.Im.GetLength(1)) y = y - image.Im.GetLength(1);
                    r += noyau[i, j] * Convert.ToInt32(image.Im[x, y].R) * 1 / n;
                    g += noyau[i, j] * Convert.ToInt32(image.Im[x, y].G) * 1 / n;
                    b += noyau[i, j] * Convert.ToInt32(image.Im[x, y].B) * 1 / n;
                }
            }
            if (r > 255) { r = 255; }
            if (r < 0) { r = 0; }

            if (g > 255) { g = 255; }
            if (g < 0) { g = 0; }

            if (b > 255) { b = 255; }
            if (b < 0) { b = 0; }
            Pixel somme = new Pixel(Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
            return somme;
        }

        static Pixel[,] Convolution(int[,] noyau, MyImage image, int n)
        {
            Pixel[,] matriceinter = new Pixel[image.Im.GetLength(0), image.Im.GetLength(1)];
            for (int x = 0; x < image.Im.GetLength(0); x++)
            {
                for (int y = 0; y < image.Im.GetLength(1); y++)
                {
                    matriceinter[x, y] = Somme(noyau, image, x, y, n);

                }
            }
            return matriceinter;

        }

        public static void Contour(MyImage image, int precision = 10)
        {
            int[,] kernel = new int[,] { { 1, 0, -1 }, { 0, 0, 0 }, { -1, 0, 1 } };
            Pixel[,] passage = Convolution(kernel, image, 1);
            for (int i = 0; i < passage.GetLength(0); i++)
            {
                for (int j = 0; j < passage.GetLength(1); j++)
                {
                    if (passage[i, j].R <= precision && passage[i, j].G <= precision && passage[i, j].B <= precision)
                    {
                        image.Im[i, j] = new Pixel(255, 255, 255);
                    }
                    else
                    {
                        image.Im[i, j] = new Pixel(0, 0, 0);
                    }
                }
            }
        }

        public static void Renforcebord(MyImage image)
        {
            int[,] kernel = new int[,] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
            Pixel[,] passage = Convolution(kernel, image, 1);
            for (int i = 0; i < passage.GetLength(0); i++)
            {
                for (int j = 0; j < passage.GetLength(1); j++)
                {
                    image.Im[i, j] = passage[i, j];
                }
            }
        }

        public static void Flou(MyImage image)
        {
            int[,] kernel = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            Pixel[,] passage = Convolution(kernel, image, 9);
            for (int i = 0; i < passage.GetLength(0); i++)
            {
                for (int j = 0; j < passage.GetLength(1); j++)
                {
                    image.Im[i, j] = passage[i, j];
                }
            }
        }


        //TD 5

        static SortedList<int, int[]> CoordonnéeHistogrammeRGB(MyImage im)
        {
            SortedList<int, int[]> histo = new SortedList<int, int[]>();
            int vmax = 1;
            for (int i = 0; i < im.Hauteur; i++)
            {
                for (int j = 0; j < im.Largeur; j++)
                {
                    int keyR = Convert.ToInt32(im.Im[i, j].R);
                    int keyG = Convert.ToInt32(im.Im[i, j].G);
                    int keyB = Convert.ToInt32(im.Im[i, j].B);
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

        static void Remplirehisto(Pixel[,] im)
        {
            for (int i = 1; i < im.GetLength(0); i++)
            {
                for (int j = 1; j < im.GetLength(1); j++)
                {
                    if (im[i, j].R != 255 || im[i, j].G != 255 || im[i, j].B != 255)
                    {
                        for (int n = 1; n < i; n++)
                        {
                            if (im[i - n, j].R != 125 || im[i - n, j].G != 125 || im[i - n, j].B != 125)
                            {
                                if (im[i - n, j].R == 255 && im[i - n, j].G == 255 && im[i - n, j].B == 255) im[i - n, j] = im[i, j];
                                else
                                {
                                    if (im[i - n, j].R == 0) im[i - n, j].R = im[i, j].R;
                                    if (im[i - n, j].G == 0) im[i - n, j].G = im[i, j].G;
                                    if (im[i - n, j].B == 0) im[i - n, j].B = im[i, j].B;

                                    if (im[i - n, j].R == 255 && im[i - n, j].G == 255 && im[i - n, j].B == 255)
                                    {
                                        im[i - n, j] = new Pixel(125, 125, 125);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void HistogrammeRGB(MyImage histogrammecouleur)
        {
            SortedList<int, int[]> histo = CoordonnéeHistogrammeRGB(histogrammecouleur);
            Pixel[,] histogrammeRGB = new Pixel[200, 256];
            int vmax = histo.Values[histo.Count - 1][0];
            histo.Remove(256);
            int largeurcase = 256 / histo.Count;
            int avancervalue = 0;
            int multi = 0;
            if (vmax > 199)
            {
                while (vmax > 199)
                {
                    vmax -= 199;
                    multi++;
                }

            }
            else
            {
                while (vmax < 199)
                {
                    vmax++;
                    multi++;
                }
                vmax = -multi;
                multi = 1;
            }
            for (int i = 0; i < histogrammeRGB.GetLength(0); i++)
            {
                for (int j = 0; j < histogrammeRGB.GetLength(1); j++)
                {
                    if (i == 0 || j == 0)
                    {
                        histogrammeRGB[i, j] = new Pixel(0, 0, 0);
                    }
                    else
                    {
                        if (largeurcase == 0)
                        {
                            avancervalue++;
                            largeurcase = 256 / histo.Count;
                        }
                        largeurcase--;
                        if (avancervalue < histo.Count)
                        {
                            int[] val = histo.Values[avancervalue];

                            if (val[0] < vmax)
                            {
                                val[0] = vmax;
                            }
                            if (val[1] < vmax)
                            {
                                val[1] = vmax;
                            }
                            if (val[2] < vmax)
                            {
                                val[2] = vmax;
                            }

                            if (val[0] == val[1])
                            {
                                histogrammeRGB[(val[0] - vmax) / multi, j] = new Pixel(255, 255, 0);
                            }
                            if (val[0] == val[2])
                            {
                                histogrammeRGB[(val[0] - vmax) / multi, j] = new Pixel(255, 0, 255);
                            }
                            if (val[1] == val[2])
                            {
                                histogrammeRGB[(val[1] - vmax) / multi, j] = new Pixel(0, 255, 255);
                            }
                            if (val[1] == val[0] && val[1] == val[2])
                            {
                                histogrammeRGB[(val[0] - vmax) / multi, j] = new Pixel(125, 125, 125);
                            }
                            else
                            {
                                histogrammeRGB[(val[0] - vmax) / multi, j] = new Pixel(255, 0, 0);
                                histogrammeRGB[(val[1] - vmax) / multi, j] = new Pixel(0, 255, 0);
                                histogrammeRGB[(val[2] - vmax) / multi, j] = new Pixel(0, 0, 255);
                            }
                        }
                    }
                    if (histogrammeRGB[i, j] == null)
                    {
                        histogrammeRGB[i, j] = new Pixel(255, 255, 255);
                    }

                }
                largeurcase = 256 / histo.Count;
                avancervalue = 0;
            }
            Remplirehisto(histogrammeRGB);
            //Verif();
            histogrammecouleur.Im = histogrammeRGB;
            histogrammecouleur.ChangerTailleImage(true);
        }

        static bool AppartienEnsemble(int i, int j)
        {
            double xn = 0;
            double yn = 0;
            double a = (j * 2.3) / 320 - 1.80;
            double b = (i * 1.78) / 200 - 0.89;
            bool ensemble = true;
            for (int n = 0; n < 50; n++)
            {
                double temp = xn;
                xn = xn * xn - yn * yn + a;
                yn = 2 * temp * yn + b;
                if ((xn * xn + yn * yn) > 4) ensemble = false;
            }
            return ensemble;
        }

        public static void FractaleMandelbrot(MyImage fractale)
        {
            fractale.Im = new Pixel[200, 320];

            fractale.ChangerTailleImage(true);
            for (int i = 0; i < fractale.Hauteur; i++)
            {
                for (int j = 0; j < fractale.Largeur; j++)
                {
                    bool ensemble = AppartienEnsemble(i, j);

                    if (ensemble == true) fractale.Im[i, j] = new Pixel(0, 0, 0);

                    else fractale.Im[i, j] = new Pixel(255, 255, 255);
                }
            }
        }

        public static MyImage CacherImage(MyImage im1, MyImage im2) //cache im2 dans im1
        {
            MyImage imcachee = null;
            if (im1.Hauteur >= im2.Hauteur && im1.Largeur >= im2.Largeur)
            {
                string a; string b; byte R; byte G; byte B;
                imcachee = new MyImage(im1);
                for (int i = 0; i < im1.Hauteur; i++)
                {
                    for (int j = 0; j < im1.Largeur; j++)
                    {
                        a = Convert.ToString(im1.Im[i, j].R, 2);
                        while (a.Length < 8) { a = a.Insert(0, "0"); }
                        if (i < im2.Hauteur && j < im2.Largeur)
                        {
                            b = Convert.ToString(im2.Im[i, j].R, 2);
                            while (b.Length < 8) { b = b.Insert(0, "0"); }
                        }
                        else { b = "0000"; }
                        a = a.Insert(4, b); a = a.Remove(8);
                        R = Convert.ToByte(a, 2);

                        a = Convert.ToString(im1.Im[i, j].G, 2);
                        while (a.Length < 8) { a = a.Insert(0, "0"); }
                        if (i < im2.Hauteur && j < im2.Largeur)
                        {
                            b = Convert.ToString(im2.Im[i, j].G, 2);
                            while (b.Length < 8) { b = b.Insert(0, "0"); }
                        }
                        else { b = "0000"; }
                        a = a.Insert(4, b); a = a.Remove(8);
                        G = Convert.ToByte(a, 2);

                        a = Convert.ToString(im1.Im[i, j].B, 2);
                        while (a.Length < 8) { a = a.Insert(0, "0"); }
                        if (i < im2.Hauteur && j < im2.Largeur)
                        {
                            b = Convert.ToString(im2.Im[i, j].B, 2);
                            while (b.Length < 8) { b = b.Insert(0, "0"); }
                        }
                        else { b = "0000"; }
                        a = a.Insert(4, b); a = a.Remove(8);
                        B = Convert.ToByte(a, 2);
                        imcachee.Im[i, j] = new Pixel(R, G, B);
                    }
                }
            }
            return imcachee;
        }

        public static MyImage RevelerImage(MyImage image)
        {
            MyImage imcachee = new MyImage(image);
            string a; byte R; byte G; byte B;
            for (int i = 0; i < imcachee.Hauteur; i++)
            {
                for (int j = 0; j < imcachee.Largeur; j++)
                {
                    a = Convert.ToString(image.Im[i, j].R, 2);
                    while (a.Length < 8) { a = a.Insert(0, "0"); }
                    a = a.Substring(4, 4); a = a.Insert(4, "0000");
                    R = Convert.ToByte(a, 2);

                    a = Convert.ToString(image.Im[i, j].G, 2);
                    while (a.Length < 8) { a = a.Insert(0, "0"); }
                    a = a.Substring(4, 4); a = a.Insert(4, "0000");
                    G = Convert.ToByte(a, 2);

                    a = Convert.ToString(image.Im[i, j].B, 2);
                    while (a.Length < 8) { a = a.Insert(0, "0"); }
                    a = a.Substring(4, 4); a = a.Insert(4, "0000");
                    B = Convert.ToByte(a, 2);
                    imcachee.Im[i, j] = new Pixel(R, G, B);
                }
            }
            return imcachee;
        }

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
