namespace WM_Fuzzy
{
    partial class ElectricityForecast
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_Total_kWh = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_Total_Relatively_kWh = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_Avg_Illumination = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Avg_Temperature = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_betterSet = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_Target = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.lbl_Mae = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 25);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(443, 359);
            this.dataGridView2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 25);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(326, 359);
            this.dataGridView1.TabIndex = 0;
            // 
            // chart1
            // 
            chartArea1.Area3DStyle.PointDepth = 50;
            chartArea1.Area3DStyle.PointGapDepth = 50;
            chartArea1.Area3DStyle.Rotation = 20;
            chartArea1.Area3DStyle.WallWidth = 1;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.LeftRight;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Top;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.LegendText = "Actual Value";
            series1.Name = "Series1";
            series1.YValueMembers = "Target";
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.LegendText = "Forecase Value";
            series2.Name = "Series2";
            series2.YValueMembers = "Target_Forecast";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(1058, 365);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            title1.Font = new System.Drawing.Font("微軟正黑體", 16F);
            title1.Name = "title1";
            title1.Text = "每日用電預測趨勢圖";
            this.chart1.Titles.Add(title1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(345, 384);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 20);
            this.label5.TabIndex = 7;
            // 
            // txt_Total_kWh
            // 
            this.txt_Total_kWh.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.txt_Total_kWh.Location = new System.Drawing.Point(204, 24);
            this.txt_Total_kWh.Name = "txt_Total_kWh";
            this.txt_Total_kWh.Size = new System.Drawing.Size(35, 25);
            this.txt_Total_kWh.TabIndex = 9;
            this.txt_Total_kWh.Text = "3";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label7.Location = new System.Drawing.Point(7, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(156, 18);
            this.label7.TabIndex = 10;
            this.label7.Text = "Membership Region : ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txt_Total_kWh);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.groupBox1.Location = new System.Drawing.Point(3, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(271, 62);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "即時總用電";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txt_Total_Relatively_kWh);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.groupBox2.Location = new System.Drawing.Point(3, 87);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(271, 62);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "相對總用電";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label8.Location = new System.Drawing.Point(7, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(156, 18);
            this.label8.TabIndex = 14;
            this.label8.Text = "Membership Region : ";
            // 
            // txt_Total_Relatively_kWh
            // 
            this.txt_Total_Relatively_kWh.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.txt_Total_Relatively_kWh.Location = new System.Drawing.Point(204, 25);
            this.txt_Total_Relatively_kWh.Name = "txt_Total_Relatively_kWh";
            this.txt_Total_Relatively_kWh.Size = new System.Drawing.Size(35, 25);
            this.txt_Total_Relatively_kWh.TabIndex = 13;
            this.txt_Total_Relatively_kWh.Text = "3";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txt_Avg_Illumination);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.groupBox3.Location = new System.Drawing.Point(3, 149);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(271, 62);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "平均日照";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label9.Location = new System.Drawing.Point(7, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(156, 18);
            this.label9.TabIndex = 14;
            this.label9.Text = "Membership Region : ";
            // 
            // txt_Avg_Illumination
            // 
            this.txt_Avg_Illumination.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.txt_Avg_Illumination.Location = new System.Drawing.Point(204, 25);
            this.txt_Avg_Illumination.Name = "txt_Avg_Illumination";
            this.txt_Avg_Illumination.Size = new System.Drawing.Size(35, 25);
            this.txt_Avg_Illumination.TabIndex = 13;
            this.txt_Avg_Illumination.Text = "3";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.txt_Avg_Temperature);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.groupBox4.Location = new System.Drawing.Point(3, 211);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(271, 62);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "平均溫度";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label3.Location = new System.Drawing.Point(9, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 18);
            this.label3.TabIndex = 14;
            this.label3.Text = "Membership Region : ";
            // 
            // txt_Avg_Temperature
            // 
            this.txt_Avg_Temperature.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.txt_Avg_Temperature.Location = new System.Drawing.Point(204, 24);
            this.txt_Avg_Temperature.Name = "txt_Avg_Temperature";
            this.txt_Avg_Temperature.Size = new System.Drawing.Size(35, 25);
            this.txt_Avg_Temperature.TabIndex = 13;
            this.txt_Avg_Temperature.Text = "3";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkRed;
            this.button1.Font = new System.Drawing.Font("微軟正黑體", 20F);
            this.button1.ForeColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(182, 339);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 44);
            this.button1.TabIndex = 17;
            this.button1.Text = "預測";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btn_Forecast_Click);
            // 
            // btn_betterSet
            // 
            this.btn_betterSet.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.btn_betterSet.Location = new System.Drawing.Point(94, 339);
            this.btn_betterSet.Name = "btn_betterSet";
            this.btn_betterSet.Size = new System.Drawing.Size(82, 44);
            this.btn_betterSet.TabIndex = 18;
            this.btn_betterSet.Text = "較好設定";
            this.btn_betterSet.UseVisualStyleBackColor = true;
            this.btn_betterSet.Click += new System.EventHandler(this.btn_betterSet_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(775, 384);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 20);
            this.label6.TabIndex = 20;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dataGridView1);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox5.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.groupBox5.Location = new System.Drawing.Point(726, 365);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(332, 387);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "規則庫(Rule Base)";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.dataGridView2);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox6.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.groupBox6.Location = new System.Drawing.Point(277, 365);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(449, 387);
            this.groupBox6.TabIndex = 22;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "每日用電資料";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.groupBox8);
            this.groupBox7.Controls.Add(this.button2);
            this.groupBox7.Controls.Add(this.groupBox4);
            this.groupBox7.Controls.Add(this.button1);
            this.groupBox7.Controls.Add(this.btn_betterSet);
            this.groupBox7.Controls.Add(this.groupBox3);
            this.groupBox7.Controls.Add(this.groupBox2);
            this.groupBox7.Controls.Add(this.groupBox1);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox7.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.groupBox7.Location = new System.Drawing.Point(0, 365);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(277, 417);
            this.groupBox7.TabIndex = 23;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "WM Fuzzy參數設定";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label1);
            this.groupBox8.Controls.Add(this.txt_Target);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox8.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.groupBox8.Location = new System.Drawing.Point(3, 273);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(271, 62);
            this.groupBox8.TabIndex = 20;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "目標用電";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.label1.Location = new System.Drawing.Point(9, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 18);
            this.label1.TabIndex = 14;
            this.label1.Text = "Membership Region : ";
            // 
            // txt_Target
            // 
            this.txt_Target.Font = new System.Drawing.Font("微軟正黑體", 10F);
            this.txt_Target.Location = new System.Drawing.Point(204, 24);
            this.txt_Target.Name = "txt_Target";
            this.txt_Target.Size = new System.Drawing.Size(35, 25);
            this.txt_Target.TabIndex = 13;
            this.txt_Target.Text = "3";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.button2.Location = new System.Drawing.Point(6, 339);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 44);
            this.button2.TabIndex = 19;
            this.button2.Text = "初始設定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbl_Mae
            // 
            this.lbl_Mae.AutoSize = true;
            this.lbl_Mae.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Mae.Location = new System.Drawing.Point(185, 9);
            this.lbl_Mae.Name = "lbl_Mae";
            this.lbl_Mae.Size = new System.Drawing.Size(49, 20);
            this.lbl_Mae.TabIndex = 25;
            this.lbl_Mae.Text = "value";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(9, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(170, 20);
            this.label10.TabIndex = 24;
            this.label10.Text = "Mean absolute error :";
            // 
            // ElectricityForecast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1058, 752);
            this.Controls.Add(this.lbl_Mae);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chart1);
            this.Name = "ElectricityForecast";
            this.Text = "Electricity Forecast";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_Total_kWh;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_Total_Relatively_kWh;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_Avg_Illumination;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_Avg_Temperature;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_betterSet;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_Target;
        private System.Windows.Forms.Label lbl_Mae;
        private System.Windows.Forms.Label label10;

    }
}