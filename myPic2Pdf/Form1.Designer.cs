namespace myPic2Pdf
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public struct ComboBoxItem<TKey, TValue>
        {
            private TKey key;
            private TValue value;

            public ComboBoxItem(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }

            public TKey Key
            {
                get { return key; }
            }

            public TValue Value
            {
                get { return value; }
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        #region Windows 窗体设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.CB_size = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.AllowDrop = true;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(493, 244);
            this.listBox1.TabIndex = 0;
            this.listBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
            this.listBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(149, 273);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(108, 16);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "是否自适应放大";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // CB_size
            // 
            this.CB_size.FormattingEnabled = true;
            this.CB_size.Location = new System.Drawing.Point(68, 269);
            this.CB_size.Name = "CB_size";
            this.CB_size.Size = new System.Drawing.Size(54, 20);
            this.CB_size.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 272);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "纸张大小";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 321);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(493, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(430, 269);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "开始";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 362);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "准备解析";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(527, 386);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CB_size);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.listBox1);
            this.MaximumSize = new System.Drawing.Size(543, 424);
            this.MinimumSize = new System.Drawing.Size(543, 424);
            this.Name = "Form1";
            this.Text = "Pic2Pdf";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void fillcomboBox1()
        {
            
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("A0", PdfSharp.PageSize.A0));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("A1", PdfSharp.PageSize.A1));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("A2", PdfSharp.PageSize.A2));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("A3", PdfSharp.PageSize.A3));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("A4", PdfSharp.PageSize.A4));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("A5", PdfSharp.PageSize.A5));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("RA0", PdfSharp.PageSize.RA0));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("RA1", PdfSharp.PageSize.RA1));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("RA2", PdfSharp.PageSize.RA2));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("RA3", PdfSharp.PageSize.RA3));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("RA4", PdfSharp.PageSize.RA4));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("RA5", PdfSharp.PageSize.RA5));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("B0", PdfSharp.PageSize.B0));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("B1", PdfSharp.PageSize.B1));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("B2", PdfSharp.PageSize.B2));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("B3", PdfSharp.PageSize.B3));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("B4", PdfSharp.PageSize.B4));
            this.CB_size.Items.Add(new ComboBoxItem<string, PdfSharp.PageSize>("B5", PdfSharp.PageSize.B5));

            this.CB_size.Text = "A4";
        }

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox CB_size;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;

        
    }
}

