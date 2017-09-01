using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Transform;
namespace myPic2Pdf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            fillcomboBox1();
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            //判断拖入的对象格式
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //允许拖放
                e.Effect = DragDropEffects.All;
            }
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            listBox1.Items.Clear();
            //获取文件列表（文件夹会被当作文件处理）
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop); //添加拖入的文件
            foreach (string file in files)
            {
                listBox1.Items.Add(file);
            }
            this.progressBar1.Value = 0;
            this.label2.Text = "准备解析";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("请拖拽一个或多个文件夹到空白处");
                return;
            }
            mSize = ((ComboBoxItem<string, PdfSharp.PageSize>)CB_size.SelectedItem).Value;
            mIsAutoAdapt = this.checkBox1.Checked;
            this.button1.Enabled = false;
            this.checkBox1.Enabled = false;
            this.CB_size.Enabled = false;
            new System.Threading.Thread(new System.Threading.ThreadStart(runFunc)).Start();
        }
        private void runFunc()
        {
            double parRate = 100.0 / listBox1.Items.Count;
            double rate = parRate;
            double prePos = 0.0;
            foreach (object var in this.listBox1.Items)
            {
                string parentFolder = var.ToString();
                string[] directions = GetDirectories(parentFolder,"*.*", true);

                if (directions.Length == 0)
                {
                    string info = "正在转换：" + parentFolder + ".pdf";
                    SetTextMessage((int)prePos, info);
                    if (isFolderEmp(parentFolder))
                    {
                        Directory.Delete(parentFolder, true);
                        continue;
                    }
                    handleSubFolder(parentFolder);
                    prePos += rate;
                    SetTextMessage((int)prePos, info);
                }
                else
                {
                    rate = parRate / directions.Length;
                    foreach (string folder in directions)
                    {
                        string info = "正在转换：" + folder + ".pdf";
                        SetTextMessage((int)prePos, info);
                        if (isParentFolder(folder))
                        {
                            continue;
                        }
                        if (isFolderEmp(folder))
                        {
                            Directory.Delete(folder, true);
                            continue;
                        }
                        handleSubFolder(folder);
                        prePos += rate;
                        SetTextMessage((int)prePos, info);
                    }
                }
            }
            
            SetTextMessage(100, "转换完成");

        }
        private delegate void SetPos(int ipos, string info);
        private void SetTextMessage(int ipos, string info)
        {
            if (this.InvokeRequired)
            {
                SetPos setpos = new SetPos(SetTextMessage);
                this.Invoke(setpos, new object[] { ipos ,info});
            }
            else
            {
                this.progressBar1.Value = Convert.ToInt32(ipos);
                label2.Text = info;

                if (ipos == 100)
                {
                    this.button1.Enabled = true;
                    this.checkBox1.Enabled = true;
                    this.CB_size.Enabled = true;
                    //this.listBox1.Items.Clear();
                }
            }
        }
        private bool isParentFolder(string filename)
        {
            //判断是否存在文件夹  
            string[] directoryNames = Directory.GetDirectories(filename);
            return directoryNames.Length > 0;  
        }
        static public List<string> GetAllFiles(string parentDir)
        {
            List<string> _fileList = new List<string>();
            DirectoryInfo TheFolder = new DirectoryInfo(parentDir);
            foreach (FileInfo fi in TheFolder.GetFiles())
            {
                _fileList.Add(fi.FullName);
            }
            return _fileList;
        }
        private void handleSubFolder(string folder)
        {
            
            string filename = folder + ".pdf";
            List<string> files = GetAllFiles(folder);
            PdfDocument document = new PdfDocument();

            foreach (string path in files)
            {
                if (isPdf(path))
                {
                    // Open the document to import pages from it.
                    PdfDocument inputDocument = PdfReader.Open(path, PdfDocumentOpenMode.Import);
                    int count = inputDocument.PageCount;
                    for (int idx = 0; idx < count; idx++)
                    {
                        // Get the page from the external document...
                        PdfPage inputPage = inputDocument.Pages[idx];
                        // ...and add it to the output document.
                        document.AddPage(inputPage);
                    }
                }
                else if (isImg(path))
                {
                    PDFSharpImages PDFImage = new PDFSharpImages(document);
                    PdfPage page = document.AddPage();
                    page.Size = mSize;
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    PDFImage.DrawImage(gfx, path, mIsAutoAdapt);
                }
            }
            document.Save(filename);
            document.Close();

            Directory.Delete(folder, true);
        }
        private bool isPdf(string filename)
        {
            string exname = filename.Substring(filename.LastIndexOf(".") + 1);

            if (exname.Equals("pdf"))
            {
                return true;
            }

            return false;
        }
        private bool isImg(string filename)
        {
            string exname = filename.Substring(filename.LastIndexOf(".") + 1);

            if (exname.Equals("jpg") || exname.Equals("bmp") || exname.Equals("png") || exname.Equals("jpeg"))
            {
                return true;
            }

            return false;
        }
        private bool isFolderEmp(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            foreach (string file in files)
            {
                string exname = file.Substring(file.LastIndexOf(".") + 1);

                if (exname.Equals("jpg") || exname.Equals("bmp") || exname.Equals("png") || exname.Equals("jpeg") || exname.Equals("pdf"))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>  
        /// 获取指定目录及子目录中所有子目录列表  
        /// </summary>  
        /// <param name="directoryPath">指定目录的绝对路径</param>  
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。  
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>  
        /// <param name="isSearchChild">是否搜索子目录</param>  
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                return Directory.GetDirectories(
                    directoryPath, 
                    searchPattern, 
                    isSearchChild ? 
                    SearchOption.AllDirectories : 
                    SearchOption.TopDirectoryOnly);
            }
            catch
            {
                throw null;
            }
        }  
        private PdfSharp.PageSize mSize;
        private bool mIsAutoAdapt;
    }
}
