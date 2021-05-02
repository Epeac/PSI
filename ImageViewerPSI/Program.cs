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

        public static void NoirEtBlanc(MyImage image)
        {
            int a = 0;
            for (int i = 0; i < image.Im.GetLength(0); i++)
            {
                for (int j = 0; j < image.Im.GetLength(1); j++)
                {
                    a = (image.Im[i,j].R + image.Im[i, j].G + image.Im[i, j].B) / 3;
                    image.Im[i, j].R = Convert.ToByte(a);
                    image.Im[i, j].G = Convert.ToByte(a);
                    image.Im[i, j].B = Convert.ToByte(a);
                }
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

        public static void Repoussage(MyImage image)
        {
            int[,] kernel = new int[,] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
            Pixel[,] passage = Convolution(kernel, image, 1);
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

        //QR CODe

        public static List<char> Tabledeconv()
        {
            List<char> table = new List<char>();
            for (int i = 0; i < 10; i++)
            {
                table.Add(Convert.ToChar(Convert.ToString(i)));
            }
            for (int i = 65; i <= 90; i++)
            {
                table.Add(Convert.ToChar(i));
            }
            table.Add(' ');
            table.Add('$');
            table.Add('%');
            table.Add('*');
            table.Add('+');
            table.Add('-');
            table.Add('.');
            table.Add('/');
            table.Add(':');
            return table;
        }

        public static List<string> Encode(string phrase)
        {
            List<string> rep = new List<string>();
            int n = 2;
            List<char> table = Tabledeconv();
            rep.Add("0010");
            rep.Add(Convert.ToString(phrase.Length, 2));
            while (rep[1].Length < 9)
            {
                rep[1] = "0" + rep[1];
            }
            for (int i = 0; i < phrase.Length; i += 2)
            {
                if (i < (phrase.Length - 1))
                {
                    int temps = table.IndexOf(phrase[i]) * 45 + table.IndexOf(phrase[i + 1]);
                    rep.Add(Convert.ToString(temps, 2));
                    while (rep[n].Length < 11)
                    {
                        rep[n] = "0" + rep[n];
                    }
                }
                else
                {
                    int temps = table.IndexOf(phrase[i]);
                    rep.Add(Convert.ToString(temps, 2));
                    while (rep[n].Length < 6)
                    {
                        rep[n] = "0" + rep[n];
                    }
                }
                n++;
            }
            return rep;
        }

        public static string Correction(List<string> donnée, int version)
        {
            version *= 8;
            string chainecoorecte = "";
            int longueur = 0;
            for (int i = 0; i < donnée.Count; i++)
            {
                longueur += donnée[i].Length;
            }
            if (longueur < version)
            {
                if (longueur <= version)
                {
                    donnée[donnée.Count - 1] += "0000";
                    longueur += 4;
                }
                else
                {
                    while (longueur < version)
                    {
                        donnée[donnée.Count - 1] += "0";
                        longueur++;
                    }
                }
                int temps = longueur / 8;
                while (longueur - 8 * temps != 0)
                {
                    longueur++;
                    temps = longueur / 8;
                    donnée[donnée.Count - 1] += "0";
                }
                bool alterne = false;
                while (longueur < version)
                {
                    if (alterne == false)
                    {
                        donnée.Add("11101100");
                        alterne = true;
                    }
                    else
                    {
                        donnée.Add("00010001");
                        alterne = false;
                    }
                    longueur += 8;
                }
            }
            for (int i = 0; i < donnée.Count; i++)
            {
                chainecoorecte += donnée[i];
            }
            return chainecoorecte;
        }

        public static Pixel[,] QRcodeV1(int version)
        {
            if (version == 19) version = 0;
            else version = 4;
            Pixel[,] img = new Pixel[21 + version, 21 + version];
            int bel = img.GetLength(0);
            Pixel noir = new Pixel(0, 0, 0);
            Pixel blanc = new Pixel(255, 255, 255);
            bool alterne1 = true;
            bool alterne2 = true;
            string mask = "111011111000100";
            int indexmask1 = 0;
            int indexmask2 = 0;
            for (int i = 0; i < img.GetLength(0); i++)
            {
                for (int j = 0; j < img.GetLength(1); j++)
                {
                    if ((j < 7 && (i == 0 || i == 6 || i == 20 + version || i == 14 + version))
                         || (j >= 14 + version && (i == 20 + version || i == 14 + version))
                         || (i < 7 && (j == 0 || j == 6))
                         || (i >= 14 + version && (j == 20 + version || j == 0 || j == 6 || j == 14 + version))
                         || ((j >= 2 && j <= 4) && ((i >= 2 && i <= 4) || (i >= 16 + version && i <= 18 + version)))
                         || ((j >= 16 + version && j <= 18 + version) && (i >= 16 + version && i <= 18 + version)))
                    {
                        img[i, j] = noir;
                    }
                    if ((j == 7 && (i <= 7 || i >= 13 + version))
                         || (j <= 7 && (i == 13 + version || i == 7))
                         || (j == 13 + version && i >= 13 + version)
                         || (j >= 13 + version && i == 13 + version)
                         || ((j >= 1 && j <= 5) && (i == 1 || i == 5 || i == 15 + version || i == 19 + version))
                         || ((j == 1 || j == 5) && ((i >= 15 + version && i <= 19 + version) || (i >= 1 && i <= 5)))
                         || ((j == 15 + version || j == 19 + version) && (i >= 15 + version && i <= 19 + version))
                         || ((j >= 15 + version && j <= 19 + version) && (i == 15 + version || i == 19 + version)))
                    {
                        img[i, j] = blanc;
                    }
                    if (j == 6 && (i >= 8 && i <= 12 + version))
                    {
                        if (alterne1 == true)
                        {
                            img[i, j] = noir;
                            alterne1 = false;
                        }
                        else
                        {
                            img[i, j] = blanc;
                            alterne1 = true;
                        }
                    }
                    if (i == 14 + version && (j >= 8 && j <= 12 + version))
                    {
                        if (alterne2 == true)
                        {
                            img[i, j] = noir;
                            alterne2 = false;
                        }
                        else
                        {
                            img[i, j] = blanc;
                            alterne2 = true;
                        }
                    }
                    if ((i == 12 + version && (j <= 8 && j != 6))
                         || (j == 8 && (i > 12 + version && i != 14 + version)))
                    {
                        if (mask[indexmask1] == '1') img[i, j] = noir;
                        else img[i, j] = blanc;
                        indexmask1++;
                    }
                    if ((j == 8 && i < 7) || (i == 12 + version && j >= 13 + version))
                    {
                        if (mask[indexmask2] == '1') img[i, j] = noir;
                        else img[i, j] = blanc;
                        indexmask2++;
                    }
                    if (i == 7 && j == 8) img[i, j] = noir;
                    if (version == 4)
                    {
                        if (((i == 4 || i == 8) && (j >= 16 && j <= 20))
                            || ((j == 16 || j == 20) && (i >= 4 && i <= 8))
                            || (i == 6 && j == 18))
                        {
                            img[i, j] = noir;
                        }
                        if (((j == 17 || j == 19) && (i >= 5 && i <= 7))
                            || ((i == 5 || i == 7) && (j >= 17 && j <= 19)))
                        {
                            img[i, j] = blanc;
                        }
                    }

                }
            }
            return img;
        }

        public static void RemplirQrcod(Pixel[,] im, string chaine)
        {
            Pixel noir = new Pixel(0, 0, 0);
            Pixel blanc = new Pixel(255, 255, 255);
            int indexchaine = 0;
            bool alterne = false;
            for (int j = im.GetLength(1) - 1; j > 0; j -= 2)
            {
                if (j == 6) j--;
                for (int i = 0; i < im.GetLength(0); i++)
                {

                    int ligne;
                    if (alterne == true) ligne = im.GetLength(0) - 1 - i;
                    else ligne = i;


                    if (im[ligne, j] == null)
                    {



                        if (chaine[indexchaine] == '1')
                        {
                            if (Masque0(j, ligne, im.GetLength(0) - 1) == true) im[ligne, j] = noir;
                            else im[ligne, j] = blanc;
                        }
                        else
                        {
                            if (Masque0(j, ligne, im.GetLength(0) - 1) == true) im[ligne, j] = blanc;
                            else im[ligne, j] = noir;
                        }
                        indexchaine++;

                    }

                    if (im[ligne, j - 1] == null)
                    {
                        if (indexchaine >= chaine.Length)
                        {
                            im[ligne, j - 1] = new Pixel(0, 0, 255);
                            im[ligne, j] = new Pixel(255, 0, 0);
                        }
                        else
                        {
                            if (chaine[indexchaine] == '1')
                            {
                                if (Masque0(j - 1, ligne, im.GetLength(0) - 1) == true) im[ligne, j - 1] = noir;
                                else im[ligne, j - 1] = blanc;
                            }
                            else
                            {
                                if (Masque0(j - 1, ligne, im.GetLength(0) - 1) == true) im[ligne, j - 1] = blanc;
                                else im[ligne, j - 1] = noir;
                            }
                            indexchaine++;
                        }
                    }
                }
                if (alterne == true) alterne = false;
                else alterne = true;
            }
        }

        public static bool Masque0(int colonne, int ligne, int côté)
        {
            int verif = (côté - ligne) + (côté - colonne);
            while (verif >= 2)
            {
                verif -= 2;
            }
            return verif != 0;
        }

        public static Pixel[,] Contour(Pixel[,] im)
        {
            Pixel[,] rep = new Pixel[im.GetLength(0) + 8, im.GetLength(1) + 8];
            for (int i = 0; i < rep.GetLength(0); i++)
            {
                for (int j = 0; j < rep.GetLength(1); j++)
                {
                    if (i < 4 || i > (im.GetLength(0) + 3) || j < 4 || j > (im.GetLength(1) + 3)) rep[i, j] = new Pixel(255, 255, 255);
                    else rep[i, j] = im[i - 4, j - 4];

                }
            }
            return rep;
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
