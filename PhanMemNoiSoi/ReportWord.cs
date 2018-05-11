using PhanMemNoiSoi.Properties;
using System;
using System.Collections.Generic;
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
        object objEndOfDocFlag = "\\endofdoc"; /* \endofdoc is a predefined bookmark */

        public ReportWord()
        {
            wordDoc = new Word.Document();
            wordApp = new Word.Application();
        }

        public bool openTemplateFile()
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

        /*
         * https://www.codeproject.com/Tips/818331/Word-Automation-using-Csharp-Create-a-Word-Table
         */
        public void createTable(string name, string[,] data)
        {
            int numRows = data.GetLength(0);
            int numCols = data.GetLength(1);
            Word.Table objTab1; //create table object
            Word.Range objWordRng = wordDoc.Range(0, 0);
            if (objWordRng.Find.Execute("<table>"))
            {
                // range is now set to bounds of the word "<table>"
                objTab1 = wordDoc.Tables.Add(objWordRng, numRows, numCols, ref missing, ref missing); //add table object in word document
                objTab1.AllowAutoFit = false;
                objTab1.Columns[1].SetWidth(wordApp.Application.CentimetersToPoints(4f), Word.WdRulerStyle.wdAdjustNone);
                objTab1.Columns[2].SetWidth(wordApp.Application.CentimetersToPoints(12f), Word.WdRulerStyle.wdAdjustNone);
                //objTab1.Range.ParagraphFormat.SpaceAfter = 6;
                int iRow, iCols;
                for (iRow = 1; iRow <= numRows; iRow++)
                {
                    for (iCols = 1; iCols <= numCols; iCols++)
                    {
                        objTab1.Cell(iRow, iCols).Range.Text = data[iRow - 1, iCols - 1];
                    }
                }
            }
        }

        public void createImageTable(string name, List<String> images)
        {
            if(images == null)
            {
                return;
            }
            int[] dimension = getImgTableSize(images.Count);
            int numRows = dimension[0];
            int numCols = dimension[1];
            Size imgSize = getImgSize(images.Count);
            Word.Table objTab1; //create table object
            Word.Range objWordRng = wordDoc.Range(0, 0);
            if (objWordRng.Find.Execute("<images>"))
            {
                // range is now set to bounds of the word "<table>"
                objTab1 = wordDoc.Tables.Add(objWordRng, numRows, numCols, ref missing, ref missing);
                objTab1.AllowAutoFit = false;
                objTab1.BottomPadding = 5;
                int iRow, iCols;
                int imgIndex = 0;
                for (iRow = 1; iRow <= numRows; iRow++)
                {
                    for (iCols = 1; iCols <= numCols; iCols++)
                    {
                        if(imgIndex >= images.Count)
                        {
                            break;
                        }
                        object oRange = objTab1.Cell(iRow, iCols).Range;
                        string tmpPath = Log.Instance.GetTempPath() + "tmp.jpg";
                        Image image;
                        if (Session.Instance.ActiveLicense == false)
                        {
                            image = Properties.Resources.img_default;
                        }
                        else
                        {
                            image = Image.FromFile(images[imgIndex]);
                        }  
                        Image resizedImg = this.helper.ResizeImage(image, imgSize.Width, imgSize.Height);
                        resizedImg.Save(tmpPath);
                        this.wordDoc.InlineShapes.AddPicture(tmpPath, ref missing, true, ref oRange);
                        imgIndex++;
                    }
                }
            }
        }

        private Size getImgSize(int numImgs)
        {
            Size imgSize = new Size();
            switch(numImgs)
            {
                case 0:
                case 1:
                case 2:
                    imgSize.Width = Settings.Default.img2Width;
                    imgSize.Height = Settings.Default.img2Height;
                    break;
                case 3:
                    imgSize.Width = Settings.Default.img3Width;
                    imgSize.Height = Settings.Default.img3Height;
                    break;
                default:
                    imgSize.Width = Settings.Default.img4Width;
                    imgSize.Height = Settings.Default.img4Heigh;
                    break;

            }
            return imgSize;
        }

        private int [] getImgTableSize(int numImgs)
        {
            int[] dimension = new int[2];
            switch (numImgs)
            {
                case 0:
                    dimension[0] = 0;
                    dimension[1] = 0;
                    break;
                case 1:
                    dimension[0] = 1;
                    dimension[1] = 1;
                    break;
                case 2:
                    dimension[0] = 1;
                    dimension[1] = 2;
                    break;
                case 3:
                    dimension[0] = 1;
                    dimension[1] = 3;
                    break;
                case 4:
                    dimension[0] = 2;
                    dimension[1] = 2;
                    break;
                default:
                    dimension[0] = 2;
                    dimension[1] = 3;
                    break;
            }
            return dimension;
        }
        private void DocumentBeforeClose()
        {
            //Do some thing
        }
    }
}
