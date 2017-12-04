using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PhanMemNoiSoi
{
    class Helper
    {
        Dictionary<int, string> dateFormat = new Dictionary<int, string>()
        {
            { 0, "dd/MM/yyyy"},
            { 1, "yyyy/MM/dd"},
            { 2, "d/M/yy"},
            { 3, "dd-MM-yyyy"},
            { 4, "yyyy-MM-dd"},
            { 5, "d-M-yy"},
        };

        /// <summary>
        /// Save jpeg image to disk using save dialog
        /// </summary>
        /// <param name="image"></param>
        public void SaveImageCapture(System.Drawing.Image image)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.FileName = "Image";
            s.DefaultExt = ".Jpg";
            s.Filter = "Image (.jpg)|*.jpg";

            if (s.ShowDialog() == DialogResult.OK)
            {
                string filename = s.FileName;
                FileStream fstream = new FileStream(filename, FileMode.Create);
                image.Save(fstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                fstream.Close();
            }
        }

        /// <summary>
        /// Save image to special disk location
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filePath"></param>
        public void SaveImageCapture(System.Drawing.Image image, string filePath)
        {
            // Save Image
            FileStream fstream = new FileStream(filePath, FileMode.Create);
            image.Save(fstream, System.Drawing.Imaging.ImageFormat.Jpeg);
            fstream.Close();
        }

        /// <summary>
        /// Create file on disk if does exits
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool createFile(string filePath)
        {
            bool exitCode = false;
            try
            {
                if (!File.Exists(filePath))
                {
                    FileStream fs = File.Create(filePath);
                    fs.Close();
                    exitCode = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return exitCode;
        }

        /// <summary>
        /// Delete a file on disk
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool deleteFile(string filePath)
        {
            bool exitCode = false;
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    exitCode = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return exitCode;
        }

        /// <summary>
        /// Execute a command by cmd.exe 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public int ExecuteCommand(string command, string parameter)
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo();
            processInfo.FileName = "cmd.exe";
            processInfo.Arguments = "/c " + command + " " + parameter;

            Console.WriteLine(processInfo.Arguments);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");

            process.Close();
            return exitCode;
        }

        /// <summary>
        /// auto set row index for data grid view
        /// </summary>
        /// <param name="dgv"></param>
        public void setRowNumberDgvSick(DataGridView dgv)
        {
            for(int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Cells["STT"].Value = String.Format("{0}", i + 1);
            }           
        }

        /// <summary>
        /// auto set row index for data grid view
        /// </summary>
        /// <param name="dgv"></param>
        public void setRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);

            }
            dgv.RowHeadersWidth = dgv.Width / 12;
            dgv.RowHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11f);
        }

        /// <summary>
        /// Get saved image folder path from patient id  
        /// </summary>
        /// <param name="patientId"></param>
        public string getImgFolderById(string patientId)
        {
            string result = null;
            string base_path = Properties.Settings.Default.imageFolder;
            Patient patient = new Patient().getPatientByNum(patientId);
            result = base_path + "\\" + patient.CreateTimeProperty.Year
                                    + patient.CreateTimeProperty.Month + "\\"
                                    + patient.CreateTimeProperty.Day + "\\"
                                    + patient.NumProperty + "\\";
            return result;
        }

        public bool DBConnectionStatus(string connString)
        {
            bool isOK = false;
            SqlConnection sqlConn = null;
            try
            {
                using (sqlConn =
                    new SqlConnection(connString))
                {
                    sqlConn.Open();
                    if (sqlConn.State == ConnectionState.Open)
                    {
                        isOK = true;
                    }
                }
            }
            catch (SqlException)
            {
                return isOK;
            }
            catch (Exception)
            {
                return isOK;
            }
            finally
            {
                if (isOK)
                {
                    sqlConn.Close();
                }
            }
            return isOK;
        }

        public Image resizeImage(string filename, Size size)
        {
            Image imgToResize = Image.FromFile(filename);
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public int getIdCheck()
        {
            string sql = "SELECT  COUNT(*) FROM SickData";
            SqlCommand comd = new SqlCommand(sql, DBConnection.Instance.sqlConn);
            int count = Convert.ToInt32(comd.ExecuteScalar());
            return count;
        }

        public string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        public void deleteFolder(string folderPath)
        {
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(folderPath);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public bool myValidateRoles(string strInput)
        {
            bool isValid = false;
            foreach (string str in Session.Instance.UserRole)
            {
                if (string.Equals(str, strInput))
                {
                    isValid = true;
                    break;
                }
            }
            return isValid;
        }

        public string getDateFormat(int index)
        {
            string strDateFormat = "";
            strDateFormat = dateFormat[index];
            return strDateFormat;
        }
    }
}

