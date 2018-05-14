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
using System.Text;
using System.Security.Cryptography;

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
        public void setRowNumber(DataGridView dgv, int ratioSize = 12)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            }
            dgv.RowHeadersWidth = dgv.Width / ratioSize;
            dgv.RowHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12f);
        }

        public string creatPatientFolder(string sickNum)
        {
            string BASE_IMG_FOLDER = Properties.Settings.Default.imageFolder;
            string folderPath = null;
            //check base folder is exist
            if (!Directory.Exists(BASE_IMG_FOLDER))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(BASE_IMG_FOLDER);
                    Properties.Settings.Default.imageFolder = BASE_IMG_FOLDER;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Không thể khởi tạo thưu mục lưu ảnh :" + BASE_IMG_FOLDER + ".\n Vui lòng thay đổi cài đặt thư mục lưu ảnh", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

            }
            //check month folder
            folderPath = BASE_IMG_FOLDER + "\\"
                                  + DateTime.Today.Year.ToString()
                                  + DateTime.Today.Month.ToString();
            bool exists = System.IO.Directory.Exists(folderPath);
            if (!exists)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Không thể khởi tạo thưu mục lưu ảnh :" + BASE_IMG_FOLDER + ".\n Vui lòng thay đổi cài đặt thư mục lưu ảnh", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            //check day folder
            folderPath += "\\" + DateTime.Today.Day.ToString();
            exists = System.IO.Directory.Exists(folderPath);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            //check patient folder
            folderPath += "\\" + sickNum + "\\";
            exists = System.IO.Directory.Exists(folderPath);
            if (!exists)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Không thể khởi tạo thưu mục lưu ảnh :" + BASE_IMG_FOLDER + ".\n Vui lòng thay đổi cài đặt thư mục lưu ảnh", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            return folderPath;
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
            int count = 0;
            try
            {
                string sql = "SELECT  MAX(Num) FROM SickData";
                SqlCommand comd = new SqlCommand(sql, DBConnection.Instance.sqlConn);
                object resultObj = comd.ExecuteScalar();
                if (resultObj != null)
                {
                    count = Convert.ToInt32(comd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return count;
        }

        public int getDbVersion()
        {
            int version = 0;
            try
            {
                string sql = "SELECT  Version FROM DbVersion";
                SqlCommand comd = new SqlCommand(sql, DBConnection.Instance.sqlConn);
                object resultObj = comd.ExecuteScalar();
                if (resultObj != null)
                {
                    version = Convert.ToInt32(comd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return version;
        }

        public bool setDbVersion(int version)
        {
            bool isSuccess = false;
            try
            {
                string query = "UPDATE dbo.DbVersion SET Version = @Version";
                SqlCommand command = new SqlCommand(query, DBConnection.Instance.sqlConn);
                command.Parameters.Add("@Version", SqlDbType.Int).Value = version;
                command.ExecuteNonQuery();
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return isSuccess;
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

        public string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes)
        {
            // If salt is not specified, generate it.
            if (saltBytes == null)
            {
                // Define min and max salt sizes.
                int minSaltSize = 4;
                int maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
            new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash;

            // Make sure hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Initialize appropriate hashing algorithm class.
            switch (hashAlgorithm.ToUpper())
            {

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
            saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        public bool VerifyHash(string plainText, string hashAlgorithm, string hashValue)
        {

            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            // We must know size of hash (without salt).
            int hashSizeInBits, hashSizeInBytes;

            // Make sure that hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Size of hash is based on the specified algorithm.
            switch (hashAlgorithm.ToUpper())
            {

                case "SHA384":
                    hashSizeInBits = 384;
                    break;

                case "SHA512":
                    hashSizeInBits = 512;
                    break;

                default: // Must be MD5
                    hashSizeInBits = 128;
                    break;
            }

            // Convert size of hash from bits to bytes.
            hashSizeInBytes = hashSizeInBits / 8;

            // Make sure that the specified hash value is long enough.
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            // Allocate array to hold original salt bytes retrieved from hash.
            byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];

            // Copy salt from the end of the hash to the new array.
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            // Compute a new hash string.
            string expectedHashString = ComputeHash(plainText, hashAlgorithm, saltBytes);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            return (hashValue == expectedHashString);
        }

    }
}

