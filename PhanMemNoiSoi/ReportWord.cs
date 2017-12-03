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
        private object oTemplatePath2 = (System.Windows.Forms.Application.StartupPath + @"\Template\template.dotx");

        public ReportWord()
        {
            wordDoc = new Word.Document();
            wordApp = new Word.Application();
        }

        public bool openFile()
        {
            bool flag = false;
            try
            {
                this.wordDoc = this.wordApp.Documents.Add(ref this.oTemplatePath2, ref this.missing, ref this.missing, ref this.missing);
                flag = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            return flag;
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

        public void insertImage(string imgpath, int imgPos, Size imgSize)
        {
            int count = wordDoc.Bookmarks.Count;
            for (int i = 1; i < count + 1; i++)
            {
                if (i == imgPos)
                {
                    object oRange = wordDoc.Bookmarks[i].Range;
                    object saveWithDocument = true;
                    object missing = Type.Missing;
                    Image image = Image.FromFile(imgpath);
                    Image image2 = this.helper.ResizeImage(image, imgSize.Width, imgSize.Height);
                    string str2 = Log.Instance.GetTempPath() + "tmp.jpg";
                    image2.Save(str2);
                    this.wordDoc.InlineShapes.AddPicture(str2, ref missing, true, ref oRange);
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
