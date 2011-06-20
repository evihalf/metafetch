using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using metafetch.DataAccessors;

namespace metafetch.DataManagers.WMC
{
    /// <summary>
    /// Manages the Windows Media Center movie info cache.
    /// </summary>
    public class WMCDataManager : DataManager
    {
        private const string DVDID_HEADER = "D7F21435";
        private const int COVER_RESIZE_WIDTH = 150;
        private const int COVER_RESIZE_HEIGHT = 211;

        private string m_infoCachePath;
        private string m_imageCachePath;

        public WMCDataManager(string infoCachePath, string imageCachePath)
        {
            m_infoCachePath = infoCachePath;
            m_imageCachePath = imageCachePath;

            // Create directories if they don't exist.
            if (!Directory.Exists(infoCachePath))
                Directory.CreateDirectory(infoCachePath);

            if (!Directory.Exists(imageCachePath))
                Directory.CreateDirectory(imageCachePath);
        }

        public void Add(MovieEntry entry)
        {
            if (entry.movie == null)
                return;

            string[] idparts = GetID(entry);
            string dvdidFile = entry.moviePath + "\\" + entry.movieTag + ".dvdid.xml";
            if (!File.Exists(dvdidFile))
            {
                // Create dvdid file.
                XDocument idDoc = GenerateIDXML(entry.movie, idparts);
                idDoc.Save(dvdidFile);
            }

            string dvdinfofile = m_infoCachePath + "\\" + idparts[0] + "-" + idparts[1] + ".xml";
            if (!File.Exists(dvdinfofile))
            {
                // Create dvdinfo file.
                XDocument infoDoc = GenerateInfoXml(entry.movie, idparts);
                infoDoc.Save(dvdinfofile);
            }

            string dvdimagefile = m_imageCachePath + "\\" + idparts[0] + "-" + idparts[1] + ".jpg";
            if (!File.Exists(dvdimagefile))
            {
                // Resize and save image to image cache as JPEG.
                if (entry.movie.Images.Count > 0)
                {
                    string sourceImage = entry.movie.Images.First().path;

                    using (Bitmap originalImage = new Bitmap(sourceImage))
                    {
                        using (Bitmap resizedImage = new Bitmap(COVER_RESIZE_WIDTH, COVER_RESIZE_HEIGHT))
                        {
                            using (Graphics surface = Graphics.FromImage(resizedImage))
                            {
                                surface.SmoothingMode = SmoothingMode.AntiAlias;
                                surface.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                surface.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                surface.DrawImage(originalImage, 0, 0, resizedImage.Width, resizedImage.Height);
                            }

                            resizedImage.Save(dvdimagefile, ImageFormat.Jpeg);
                        }
                    }
                }
            }
        }

        public void Clear(MovieEntry entry)
        {
            string[] idparts = GetID(entry);

            string dvdidFile = entry.moviePath + "\\" + entry.movieTag + ".dvdid.xml";
            if (File.Exists(dvdidFile))
                File.Delete(dvdidFile);

            string dvdinfofile = m_infoCachePath + "\\" + idparts[0] + "-" + idparts[1] + ".xml";
            if (File.Exists(dvdinfofile))
                File.Delete(dvdinfofile);

            string dvdimagefile = m_imageCachePath + "\\" + idparts[0] + "-" + idparts[1] + ".jpg";
            if (File.Exists(dvdimagefile))
                File.Delete(dvdimagefile);
        }

        private static string[] GetID(MovieEntry entry)
        {
            // Concatenate the header and first 8 characters of
            // the MD5 of the full path to the movie to construct the id.
            MD5 hasher = MD5.Create();
            byte[] hashData = hasher.ComputeHash(Encoding.Default.GetBytes(entry.moviePath));

            StringBuilder hash = new StringBuilder();
            for (int i = 0; i < 4; i++)
                hash.Append(hashData[i].ToString("X2"));

            string[] idparts = new string[2];
            idparts[0] = DVDID_HEADER;
            idparts[1] = hash.ToString();

            return idparts;
        }

        public static XDocument GenerateIDXML(Movie movie, string[] idparts)
        {
            return new XDocument(
                new XElement("Disc",
                    new XElement("Name", movie.Name),
                    new XElement("ID", idparts[0] + "|" + idparts[1])));
        }

        public static XDocument GenerateInfoXml(Movie movie, string[] idparts)
        {
            string releaseDate = "";
            if (movie.Released != null)
                releaseDate = movie.Released.Value.ToString("yyyy MM dd");

            return new XDocument(
                new XElement("METADATA",
                    new XElement("MDR-DVD",
                        new XElement("dvdTitle", movie.Name),
                        new XElement("largeCoverParams", idparts[0] + "-" + idparts[1] + ".jpg"),
                        new XElement("smallCoverParams", idparts[0] + "-" + idparts[1] + ".jpg"),
                        new XElement("MPAARating", movie.Certification),
                        new XElement("genre", movie.Genre),
                        new XElement("duration", movie.Runtime),
                        new XElement("releaseDate", releaseDate),
                        new XElement("title",
                            new XElement("titleNum", 1),
                            new XElement("titleTitle", movie.Name),
                            new XElement("synopsis", movie.Overview),
                            new XElement("MPAARating", movie.Certification),
                            new XElement("genre", movie.Genre),
                            new XElement("duration", movie.Runtime)
                        )
                    ),
                    new XElement("DvdId", idparts[0] + "|" + idparts[1])
                )
            );
        }

    }
}
