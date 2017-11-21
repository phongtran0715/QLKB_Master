using System;
using System.Drawing;
using System.Reflection;
using Word = Microsoft.Office.Interop.Word;

namespace PhanMemNoiSoi
{
    class ReportWord
    {
        Helper helper = new Helper();
        Object missing = Missing.Value;
        Word.Application wordApp;
        Word.Document wordDoc;
        Object oTemplatePath = System.Windows.Forms.Application.StartupPath + "\\Template\\template.dotx";

        public ReportWord()
        {
            wordDoc = new Word.Document();
            wordApp = new Word.Application();
        }

        public void openFile(int type)
        {
            try
            {
                wordDoc = wordApp.Documents.Add(ref oTemplatePath, ref missing, ref missing, ref missing);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        ~ReportWord()
        {
            try
            {
                if (wordDoc != null)
                {
                    wordDoc.Close();
                    wordDoc = null;
                }
                if (wordApp != null)
                {
                    wordApp.Quit();
                    wordApp = null;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.LogMessageToFile(ex.ToString());
            }
        }

        public void saveFile(string path, bool isOpen)
        {
            object readOnly = true;
            object isVisible = true;
            wordApp.Visible = true;
            object fileName = path;

            try
            {
                wordDoc.SaveAs(path);

                if (isOpen)
                {
                    wordApp.Documents.Open(ref fileName, ref missing, ref readOnly,
                                                ref missing, ref missing, ref missing,
                                                ref missing, ref missing, ref missing,
                                                ref missing, ref missing, ref missing,
                                                ref missing, ref missing, ref missing, ref missing);
                    wordDoc.Activate();
                    wordApp.Activate();
                    wordApp.Options.SaveNormalPrompt = false;
                    wordApp.Options.SavePropertiesPrompt = false;
                    wordDoc.Windows.Application.WindowState = Word.WdWindowState.wdWindowStateMaximize;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void insertText(string key, string value)
        {
            foreach (Word.Field myMergeField in wordDoc.Fields)
            {
                Word.Range rngFieldCode = myMergeField.Code;

                String fieldText = rngFieldCode.Text;

                if (fieldText.StartsWith(" MERGEFIELD"))
                {

                    Int32 endMerge = fieldText.IndexOf("\\");

                    Int32 fieldNameLength = fieldText.Length - endMerge;

                    String fieldName = fieldText.Substring(11, endMerge - 11);

                    fieldName = fieldName.Trim();

                    if (fieldName == key)
                    {
                        if (value == null || value == "") { value = " "; }
                        myMergeField.Select();
                        wordApp.Selection.TypeText(value);
                    }
                }
            }
        }

        public void insertImage(string imgpath, int imgPos)
        {
            int count = wordDoc.Bookmarks.Count;
            for (int i = 1; i < count + 1; i++)
            {
                if (i == imgPos)
                {
                    object oRange = wordDoc.Bookmarks[i].Range;
                    object saveWithDocument = true;
                    object missing = Type.Missing;
                    string pictureName = imgpath;
                    int imgW = Properties.Settings.Default.imgWidth;
                    int imgH = Properties.Settings.Default.imgHeight;
                    System.Drawing.Size size = new System.Drawing.Size(imgW, imgH);
                    //Image imgToResize = helper.resizeImage(imgpath, size);
                    Image img = Image.FromFile(pictureName);
                    Image imgToResize = helper.ResizeImage(img, size.Width, size.Height);
                    //string tmpImgPath = Environment.CurrentDirectory + "\\Images\\tmp.jpg";
                    string tmpImgPath = Log.Instance.GetTempPath() + "tmp.jpg";
                    imgToResize.Save(tmpImgPath);
                    wordDoc.InlineShapes.AddPicture(tmpImgPath, ref missing, ref saveWithDocument, ref oRange);
                    break;
                }
            }
        }

        private void DocumentBeforeClose()
        {
            //Do some thing
        }
    }
}
