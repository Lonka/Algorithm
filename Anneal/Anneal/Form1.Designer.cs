namespace Anneal
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lsb_Data = new System.Windows.Forms.ListBox();
            this.btn_Execute = new System.Windows.Forms.Button();
            this.lsb_Ans = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lsb_Data
            // 
            this.lsb_Data.FormattingEnabled = true;
            this.lsb_Data.ItemHeight = 12;
            this.lsb_Data.Location = new System.Drawing.Point(12, 23);
            this.lsb_Data.Name = "lsb_Data";
            this.lsb_Data.Size = new System.Drawing.Size(371, 412);
            this.lsb_Data.TabIndex = 0;
            // 
            // btn_Execute
            // 
            this.btn_Execute.Location = new System.Drawing.Point(389, 23);
            this.btn_Execute.Name = "btn_Execute";
            this.btn_Execute.Size = new System.Drawing.Size(75, 23);
            this.btn_Execute.TabIndex = 1;
            this.btn_Execute.Text = "執行";
            this.btn_Execute.UseVisualStyleBackColor = true;
            this.btn_Execute.Click += new System.EventHandler(this.btn_Execute_Click);
            // 
            // lsb_Ans
            // 
            this.lsb_Ans.FormattingEnabled = true;
            this.lsb_Ans.ItemHeight = 12;
            this.lsb_Ans.Location = new System.Drawing.Point(389, 47);
            this.lsb_Ans.Name = "lsb_Ans";
            this.lsb_Ans.Size = new System.Drawing.Size(283, 388);
            this.lsb_Ans.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 449);
            this.Controls.Add(this.lsb_Ans);
            this.Controls.Add(this.btn_Execute);
            this.Controls.Add(this.lsb_Data);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lsb_Data;
        private System.Windows.Forms.Button btn_Execute;
        private System.Windows.Forms.ListBox lsb_Ans;
    }
}

