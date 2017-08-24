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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("请拖拽一个或多个文件夹到空白处");
                return;
            }
            mSize = ((ComboBoxItem<string, PdfSharp.PageSize>)CB_size.SelectedItem).Value;
            mIsAutoZoom = this.checkBox1.Checked;
            this.button1.Enabled = false;
            this.checkBox1.Enabled = false;
            this.CB_size.Enabled = false;
            new System.Threading.Thread(new System.Threading.ThreadStart(runFunc)).Start();
        }
        private void runFunc()
        {
            int rate = 100 / this.listBox1.Items.Count;
            int prePos = 0;
            foreach (object var in this.listBox1.Items)
            {
                PdfDocument document = new PdfDocument();
                string folder = var.ToString();
                string filename = folder + ".pdf";
                List<string> files = Transform.PDFSharpImages.GetAllFiles(folder);

                foreach (string path in files)
                {
                    string info = "正在转换：" + path;

                    PDFSharpImages PDFImage = new PDFSharpImages(document);
                    PdfPage page = document.AddPage();
                    page.Size = mSize;
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    PDFImage.DrawImage(gfx, path, mIsAutoZoom);
                    SetTextMessage(prePos + rate * (files.IndexOf(path) + 1) / files.Count, info);
                }
                document.Save(filename);
                prePos = prePos + rate;
                document.Close();

                Directory.Delete(folder, true);
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

        private PdfSharp.PageSize mSize;
        private bool mIsAutoZoom;
    }
}
