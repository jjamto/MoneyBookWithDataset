namespace MoneyBookWithDataset
{
    partial class fItem
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label idxLabel;
            System.Windows.Forms.Label pDateLabel;
            System.Windows.Forms.Label grpLabel;
            System.Windows.Forms.Label drLabel;
            System.Windows.Forms.Label crLabel;
            System.Windows.Forms.Label remarkLabel;
            this.button1 = new System.Windows.Forms.Button();
            this.dataSet1 = new MoneyBookWithDataset.DataSet1();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.idxTextBox = new System.Windows.Forms.TextBox();
            this.pDateDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.grpTextBox = new System.Windows.Forms.TextBox();
            this.drTextBox = new System.Windows.Forms.TextBox();
            this.crTextBox = new System.Windows.Forms.TextBox();
            this.remarkTextBox = new System.Windows.Forms.TextBox();
            idxLabel = new System.Windows.Forms.Label();
            pDateLabel = new System.Windows.Forms.Label();
            grpLabel = new System.Windows.Forms.Label();
            drLabel = new System.Windows.Forms.Label();
            crLabel = new System.Windows.Forms.Label();
            remarkLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(5, 186);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(305, 48);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "DataSet1";
            this.dataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bs
            // 
            this.bs.DataMember = "Data";
            this.bs.DataSource = this.dataSet1;
            // 
            // idxLabel
            // 
            idxLabel.AutoSize = true;
            idxLabel.Location = new System.Drawing.Point(24, 16);
            idxLabel.Name = "idxLabel";
            idxLabel.Size = new System.Drawing.Size(26, 12);
            idxLabel.TabIndex = 2;
            idxLabel.Text = "Idx:";
            // 
            // idxTextBox
            // 
            this.idxTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bs, "Idx", true));
            this.idxTextBox.Location = new System.Drawing.Point(82, 13);
            this.idxTextBox.Name = "idxTextBox";
            this.idxTextBox.Size = new System.Drawing.Size(200, 21);
            this.idxTextBox.TabIndex = 3;
            // 
            // pDateLabel
            // 
            pDateLabel.AutoSize = true;
            pDateLabel.Location = new System.Drawing.Point(24, 44);
            pDateLabel.Name = "pDateLabel";
            pDateLabel.Size = new System.Drawing.Size(29, 12);
            pDateLabel.TabIndex = 4;
            pDateLabel.Text = "날짜";
            // 
            // pDateDateTimePicker
            // 
            this.pDateDateTimePicker.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bs, "PDate", true));
            this.pDateDateTimePicker.Location = new System.Drawing.Point(82, 40);
            this.pDateDateTimePicker.Name = "pDateDateTimePicker";
            this.pDateDateTimePicker.Size = new System.Drawing.Size(200, 21);
            this.pDateDateTimePicker.TabIndex = 5;
            // 
            // grpLabel
            // 
            grpLabel.AutoSize = true;
            grpLabel.Location = new System.Drawing.Point(24, 70);
            grpLabel.Name = "grpLabel";
            grpLabel.Size = new System.Drawing.Size(29, 12);
            grpLabel.TabIndex = 6;
            grpLabel.Text = "구분";
            // 
            // grpTextBox
            // 
            this.grpTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bs, "Grp", true));
            this.grpTextBox.Location = new System.Drawing.Point(82, 67);
            this.grpTextBox.Name = "grpTextBox";
            this.grpTextBox.Size = new System.Drawing.Size(200, 21);
            this.grpTextBox.TabIndex = 7;
            // 
            // drLabel
            // 
            drLabel.AutoSize = true;
            drLabel.Location = new System.Drawing.Point(24, 97);
            drLabel.Name = "drLabel";
            drLabel.Size = new System.Drawing.Size(41, 12);
            drLabel.TabIndex = 8;
            drLabel.Text = "입금액";
            // 
            // drTextBox
            // 
            this.drTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bs, "Dr", true));
            this.drTextBox.Location = new System.Drawing.Point(82, 94);
            this.drTextBox.Name = "drTextBox";
            this.drTextBox.Size = new System.Drawing.Size(200, 21);
            this.drTextBox.TabIndex = 9;
            // 
            // crLabel
            // 
            crLabel.AutoSize = true;
            crLabel.Location = new System.Drawing.Point(24, 124);
            crLabel.Name = "crLabel";
            crLabel.Size = new System.Drawing.Size(41, 12);
            crLabel.TabIndex = 10;
            crLabel.Text = "출금액";
            // 
            // crTextBox
            // 
            this.crTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bs, "Cr", true));
            this.crTextBox.Location = new System.Drawing.Point(82, 121);
            this.crTextBox.Name = "crTextBox";
            this.crTextBox.Size = new System.Drawing.Size(200, 21);
            this.crTextBox.TabIndex = 11;
            // 
            // remarkLabel
            // 
            remarkLabel.AutoSize = true;
            remarkLabel.Location = new System.Drawing.Point(24, 151);
            remarkLabel.Name = "remarkLabel";
            remarkLabel.Size = new System.Drawing.Size(29, 12);
            remarkLabel.TabIndex = 12;
            remarkLabel.Text = "비고";
            // 
            // remarkTextBox
            // 
            this.remarkTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bs, "Remark", true));
            this.remarkTextBox.Location = new System.Drawing.Point(82, 148);
            this.remarkTextBox.Name = "remarkTextBox";
            this.remarkTextBox.Size = new System.Drawing.Size(200, 21);
            this.remarkTextBox.TabIndex = 13;
            // 
            // fItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 239);
            this.Controls.Add(idxLabel);
            this.Controls.Add(this.idxTextBox);
            this.Controls.Add(pDateLabel);
            this.Controls.Add(this.pDateDateTimePicker);
            this.Controls.Add(grpLabel);
            this.Controls.Add(this.grpTextBox);
            this.Controls.Add(drLabel);
            this.Controls.Add(this.drTextBox);
            this.Controls.Add(crLabel);
            this.Controls.Add(this.crTextBox);
            this.Controls.Add(remarkLabel);
            this.Controls.Add(this.remarkTextBox);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fItem";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fItem";
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private DataSet1 dataSet1;
        private System.Windows.Forms.BindingSource bs;
        private System.Windows.Forms.TextBox idxTextBox;
        private System.Windows.Forms.DateTimePicker pDateDateTimePicker;
        private System.Windows.Forms.TextBox grpTextBox;
        private System.Windows.Forms.TextBox drTextBox;
        private System.Windows.Forms.TextBox crTextBox;
        private System.Windows.Forms.TextBox remarkTextBox;
    }
}