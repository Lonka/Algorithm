namespace BackPropagationNeuralNetwork
{
    partial class ElectricityForecast
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_Work2 = new System.Windows.Forms.Label();
            this.lbl_Work1 = new System.Windows.Forms.Label();
            this.lbl_Holiday2 = new System.Windows.Forms.Label();
            this.lbl_Holiday1 = new System.Windows.Forms.Label();
            this.lbl_Season2 = new System.Windows.Forms.Label();
            this.lbl_Season1 = new System.Windows.Forms.Label();
            this.lbl_Avg_Temperature2 = new System.Windows.Forms.Label();
            this.lbl_Avg_Temperature1 = new System.Windows.Forms.Label();
            this.lbl_Avg_Illumination2 = new System.Windows.Forms.Label();
            this.lbl_Avg_Illumination1 = new System.Windows.Forms.Label();
            this.lbl_Total_Relatively_kWh2 = new System.Windows.Forms.Label();
            this.lbl_Total_Relatively_kWh1 = new System.Windows.Forms.Label();
            this.lbl_Total_kWh2 = new System.Windows.Forms.Label();
            this.lbl_Total_kWh1 = new System.Windows.Forms.Label();
            this.txt_LearningRate = new System.Windows.Forms.TextBox();
            this.txt_MomentumFactor = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_Forecast = new System.Windows.Forms.Button();
            this.cb_Work = new System.Windows.Forms.ComboBox();
            this.cb_Holiday = new System.Windows.Forms.ComboBox();
            this.cb_Season = new System.Windows.Forms.ComboBox();
            this.cb_Total_kWh = new System.Windows.Forms.ComboBox();
            this.cb_Avg_Illumination = new System.Windows.Forms.ComboBox();
            this.cb_Total_Relatively_kWh = new System.Windows.Forms.ComboBox();
            this.cb_Avg_Temperature = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.label10 = new System.Windows.Forms.Label();
            this.lbl_Mae = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_Work2);
            this.groupBox1.Controls.Add(this.lbl_Work1);
            this.groupBox1.Controls.Add(this.lbl_Holiday2);
            this.groupBox1.Controls.Add(this.lbl_Holiday1);
            this.groupBox1.Controls.Add(this.lbl_Season2);
            this.groupBox1.Controls.Add(this.lbl_Season1);
            this.groupBox1.Controls.Add(this.lbl_Avg_Temperature2);
            this.groupBox1.Controls.Add(this.lbl_Avg_Temperature1);
            this.groupBox1.Controls.Add(this.lbl_Avg_Illumination2);
            this.groupBox1.Controls.Add(this.lbl_Avg_Illumination1);
            this.groupBox1.Controls.Add(this.lbl_Total_Relatively_kWh2);
            this.groupBox1.Controls.Add(this.lbl_Total_Relatively_kWh1);
            this.groupBox1.Controls.Add(this.lbl_Total_kWh2);
            this.groupBox1.Controls.Add(this.lbl_Total_kWh1);
            this.groupBox1.Controls.Add(this.txt_LearningRate);
            this.groupBox1.Controls.Add(this.txt_MomentumFactor);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.btn_Forecast);
            this.groupBox1.Controls.Add(this.cb_Work);
            this.groupBox1.Controls.Add(this.cb_Holiday);
            this.groupBox1.Controls.Add(this.cb_Season);
            this.groupBox1.Controls.Add(this.cb_Total_kWh);
            this.groupBox1.Controls.Add(this.cb_Avg_Illumination);
            this.groupBox1.Controls.Add(this.cb_Total_Relatively_kWh);
            this.groupBox1.Controls.Add(this.cb_Avg_Temperature);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(0, 365);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(335, 238);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "設定";
            // 
            // lbl_Work2
            // 
            this.lbl_Work2.AutoSize = true;
            this.lbl_Work2.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Work2.Location = new System.Drawing.Point(275, 173);
            this.lbl_Work2.Name = "lbl_Work2";
            this.lbl_Work2.Size = new System.Drawing.Size(35, 15);
            this.lbl_Work2.TabIndex = 28;
            this.lbl_Work2.Text = "工作2";
            this.lbl_Work2.Visible = false;
            // 
            // lbl_Work1
            // 
            this.lbl_Work1.AutoSize = true;
            this.lbl_Work1.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Work1.Location = new System.Drawing.Point(191, 166);
            this.lbl_Work1.Name = "lbl_Work1";
            this.lbl_Work1.Size = new System.Drawing.Size(35, 15);
            this.lbl_Work1.TabIndex = 27;
            this.lbl_Work1.Text = "工作1";
            this.lbl_Work1.Visible = false;
            // 
            // lbl_Holiday2
            // 
            this.lbl_Holiday2.AutoSize = true;
            this.lbl_Holiday2.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Holiday2.Location = new System.Drawing.Point(275, 130);
            this.lbl_Holiday2.Name = "lbl_Holiday2";
            this.lbl_Holiday2.Size = new System.Drawing.Size(35, 15);
            this.lbl_Holiday2.TabIndex = 26;
            this.lbl_Holiday2.Text = "假日2";
            this.lbl_Holiday2.Visible = false;
            // 
            // lbl_Holiday1
            // 
            this.lbl_Holiday1.AutoSize = true;
            this.lbl_Holiday1.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Holiday1.Location = new System.Drawing.Point(191, 126);
            this.lbl_Holiday1.Name = "lbl_Holiday1";
            this.lbl_Holiday1.Size = new System.Drawing.Size(35, 15);
            this.lbl_Holiday1.TabIndex = 25;
            this.lbl_Holiday1.Text = "假日1";
            this.lbl_Holiday1.Visible = false;
            // 
            // lbl_Season2
            // 
            this.lbl_Season2.AutoSize = true;
            this.lbl_Season2.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Season2.Location = new System.Drawing.Point(275, 89);
            this.lbl_Season2.Name = "lbl_Season2";
            this.lbl_Season2.Size = new System.Drawing.Size(35, 15);
            this.lbl_Season2.TabIndex = 24;
            this.lbl_Season2.Text = "季節2";
            this.lbl_Season2.Visible = false;
            // 
            // lbl_Season1
            // 
            this.lbl_Season1.AutoSize = true;
            this.lbl_Season1.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Season1.Location = new System.Drawing.Point(191, 85);
            this.lbl_Season1.Name = "lbl_Season1";
            this.lbl_Season1.Size = new System.Drawing.Size(35, 15);
            this.lbl_Season1.TabIndex = 23;
            this.lbl_Season1.Text = "季節1";
            this.lbl_Season1.Visible = false;
            // 
            // lbl_Avg_Temperature2
            // 
            this.lbl_Avg_Temperature2.AutoSize = true;
            this.lbl_Avg_Temperature2.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Avg_Temperature2.Location = new System.Drawing.Point(119, 217);
            this.lbl_Avg_Temperature2.Name = "lbl_Avg_Temperature2";
            this.lbl_Avg_Temperature2.Size = new System.Drawing.Size(57, 15);
            this.lbl_Avg_Temperature2.TabIndex = 22;
            this.lbl_Avg_Temperature2.Text = "平均溫度2";
            this.lbl_Avg_Temperature2.Visible = false;
            // 
            // lbl_Avg_Temperature1
            // 
            this.lbl_Avg_Temperature1.AutoSize = true;
            this.lbl_Avg_Temperature1.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Avg_Temperature1.Location = new System.Drawing.Point(20, 211);
            this.lbl_Avg_Temperature1.Name = "lbl_Avg_Temperature1";
            this.lbl_Avg_Temperature1.Size = new System.Drawing.Size(57, 15);
            this.lbl_Avg_Temperature1.TabIndex = 21;
            this.lbl_Avg_Temperature1.Text = "平均溫度1";
            this.lbl_Avg_Temperature1.Visible = false;
            // 
            // lbl_Avg_Illumination2
            // 
            this.lbl_Avg_Illumination2.AutoSize = true;
            this.lbl_Avg_Illumination2.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Avg_Illumination2.Location = new System.Drawing.Point(112, 173);
            this.lbl_Avg_Illumination2.Name = "lbl_Avg_Illumination2";
            this.lbl_Avg_Illumination2.Size = new System.Drawing.Size(68, 15);
            this.lbl_Avg_Illumination2.TabIndex = 20;
            this.lbl_Avg_Illumination2.Text = "平均日照量2";
            this.lbl_Avg_Illumination2.Visible = false;
            // 
            // lbl_Avg_Illumination1
            // 
            this.lbl_Avg_Illumination1.AutoSize = true;
            this.lbl_Avg_Illumination1.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Avg_Illumination1.Location = new System.Drawing.Point(20, 166);
            this.lbl_Avg_Illumination1.Name = "lbl_Avg_Illumination1";
            this.lbl_Avg_Illumination1.Size = new System.Drawing.Size(68, 15);
            this.lbl_Avg_Illumination1.TabIndex = 19;
            this.lbl_Avg_Illumination1.Text = "平均日照量1";
            this.lbl_Avg_Illumination1.Visible = false;
            // 
            // lbl_Total_Relatively_kWh2
            // 
            this.lbl_Total_Relatively_kWh2.AutoSize = true;
            this.lbl_Total_Relatively_kWh2.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Total_Relatively_kWh2.Location = new System.Drawing.Point(112, 130);
            this.lbl_Total_Relatively_kWh2.Name = "lbl_Total_Relatively_kWh2";
            this.lbl_Total_Relatively_kWh2.Size = new System.Drawing.Size(68, 15);
            this.lbl_Total_Relatively_kWh2.TabIndex = 18;
            this.lbl_Total_Relatively_kWh2.Text = "相對總用電2";
            this.lbl_Total_Relatively_kWh2.Visible = false;
            // 
            // lbl_Total_Relatively_kWh1
            // 
            this.lbl_Total_Relatively_kWh1.AutoSize = true;
            this.lbl_Total_Relatively_kWh1.Font = new System.Drawing.Font("微軟正黑體", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Total_Relatively_kWh1.Location = new System.Drawing.Point(20, 125);
            this.lbl_Total_Relatively_kWh1.Name = "lbl_Total_Relatively_kWh1";
            this.lbl_Total_Relatively_kWh1.Size = new System.Drawing.Size(68, 15);
            this.lbl_Total_Relatively_kWh1.TabIndex = 17;
            this.lbl_Total_Relatively_kWh1.Text = "相對總用電1";
            this.lbl_Total_Relatively_kWh1.Visible = false;
            // 
            // lbl_Total_kWh2
            // 
            this.lbl_Total_kWh2.AutoSize = true;
            this.lbl_Total_kWh2.Font = new System.Drawing.Font("微軟正黑體", 8F);
            this.lbl_Total_kWh2.Location = new System.Drawing.Point(115, 87);
            this.lbl_Total_kWh2.Name = "lbl_Total_kWh2";
            this.lbl_Total_kWh2.Size = new System.Drawing.Size(68, 15);
            this.lbl_Total_kWh2.TabIndex = 16;
            this.lbl_Total_kWh2.Text = "即時總用電2";
            this.lbl_Total_kWh2.Visible = false;
            // 
            // lbl_Total_kWh1
            // 
            this.lbl_Total_kWh1.AutoSize = true;
            this.lbl_Total_kWh1.Font = new System.Drawing.Font("微軟正黑體", 8F);
            this.lbl_Total_kWh1.Location = new System.Drawing.Point(20, 83);
            this.lbl_Total_kWh1.Name = "lbl_Total_kWh1";
            this.lbl_Total_kWh1.Size = new System.Drawing.Size(68, 15);
            this.lbl_Total_kWh1.TabIndex = 15;
            this.lbl_Total_kWh1.Text = "即時總用電1";
            this.lbl_Total_kWh1.Visible = false;
            // 
            // txt_LearningRate
            // 
            this.txt_LearningRate.Location = new System.Drawing.Point(104, 25);
            this.txt_LearningRate.Name = "txt_LearningRate";
            this.txt_LearningRate.Size = new System.Drawing.Size(50, 25);
            this.txt_LearningRate.TabIndex = 14;
            this.txt_LearningRate.Text = "0.6";
            // 
            // txt_MomentumFactor
            // 
            this.txt_MomentumFactor.Location = new System.Drawing.Point(261, 25);
            this.txt_MomentumFactor.Name = "txt_MomentumFactor";
            this.txt_MomentumFactor.Size = new System.Drawing.Size(50, 25);
            this.txt_MomentumFactor.TabIndex = 13;
            this.txt_MomentumFactor.Text = "0.7";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(191, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 18);
            this.label9.TabIndex = 3;
            this.label9.Text = "慣性因子";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 18);
            this.label8.TabIndex = 2;
            this.label8.Text = "學習比率";
            // 
            // btn_Forecast
            // 
            this.btn_Forecast.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Forecast.Location = new System.Drawing.Point(193, 188);
            this.btn_Forecast.Name = "btn_Forecast";
            this.btn_Forecast.Size = new System.Drawing.Size(118, 27);
            this.btn_Forecast.TabIndex = 2;
            this.btn_Forecast.Text = "預測";
            this.btn_Forecast.UseVisualStyleBackColor = true;
            this.btn_Forecast.Click += new System.EventHandler(this.btn_Forecast_Click);
            // 
            // cb_Work
            // 
            this.cb_Work.DisplayMember = "Text";
            this.cb_Work.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Work.Enabled = false;
            this.cb_Work.FormattingEnabled = true;
            this.cb_Work.Location = new System.Drawing.Point(232, 145);
            this.cb_Work.Name = "cb_Work";
            this.cb_Work.Size = new System.Drawing.Size(79, 25);
            this.cb_Work.TabIndex = 12;
            this.cb_Work.ValueMember = "Value";
            // 
            // cb_Holiday
            // 
            this.cb_Holiday.DisplayMember = "Text";
            this.cb_Holiday.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Holiday.Enabled = false;
            this.cb_Holiday.FormattingEnabled = true;
            this.cb_Holiday.Location = new System.Drawing.Point(232, 104);
            this.cb_Holiday.Name = "cb_Holiday";
            this.cb_Holiday.Size = new System.Drawing.Size(79, 25);
            this.cb_Holiday.TabIndex = 11;
            this.cb_Holiday.ValueMember = "Value";
            // 
            // cb_Season
            // 
            this.cb_Season.DisplayMember = "Text";
            this.cb_Season.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Season.Enabled = false;
            this.cb_Season.FormattingEnabled = true;
            this.cb_Season.Location = new System.Drawing.Point(232, 61);
            this.cb_Season.Name = "cb_Season";
            this.cb_Season.Size = new System.Drawing.Size(79, 25);
            this.cb_Season.TabIndex = 10;
            this.cb_Season.ValueMember = "Value";
            // 
            // cb_Total_kWh
            // 
            this.cb_Total_kWh.DisplayMember = "Text";
            this.cb_Total_kWh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Total_kWh.FormattingEnabled = true;
            this.cb_Total_kWh.Location = new System.Drawing.Point(104, 61);
            this.cb_Total_kWh.Name = "cb_Total_kWh";
            this.cb_Total_kWh.Size = new System.Drawing.Size(79, 25);
            this.cb_Total_kWh.TabIndex = 9;
            this.cb_Total_kWh.ValueMember = "Value";
            // 
            // cb_Avg_Illumination
            // 
            this.cb_Avg_Illumination.DisplayMember = "Text";
            this.cb_Avg_Illumination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Avg_Illumination.FormattingEnabled = true;
            this.cb_Avg_Illumination.Location = new System.Drawing.Point(104, 145);
            this.cb_Avg_Illumination.Name = "cb_Avg_Illumination";
            this.cb_Avg_Illumination.Size = new System.Drawing.Size(79, 25);
            this.cb_Avg_Illumination.TabIndex = 8;
            this.cb_Avg_Illumination.ValueMember = "Value";
            // 
            // cb_Total_Relatively_kWh
            // 
            this.cb_Total_Relatively_kWh.DisplayMember = "Text";
            this.cb_Total_Relatively_kWh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Total_Relatively_kWh.FormattingEnabled = true;
            this.cb_Total_Relatively_kWh.Location = new System.Drawing.Point(104, 104);
            this.cb_Total_Relatively_kWh.Name = "cb_Total_Relatively_kWh";
            this.cb_Total_Relatively_kWh.Size = new System.Drawing.Size(79, 25);
            this.cb_Total_Relatively_kWh.TabIndex = 7;
            this.cb_Total_Relatively_kWh.ValueMember = "Value";
            // 
            // cb_Avg_Temperature
            // 
            this.cb_Avg_Temperature.DisplayMember = "Text";
            this.cb_Avg_Temperature.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Avg_Temperature.FormattingEnabled = true;
            this.cb_Avg_Temperature.Location = new System.Drawing.Point(104, 190);
            this.cb_Avg_Temperature.Name = "cb_Avg_Temperature";
            this.cb_Avg_Temperature.Size = new System.Drawing.Size(79, 25);
            this.cb_Avg_Temperature.TabIndex = 2;
            this.cb_Avg_Temperature.ValueMember = "Value";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Enabled = false;
            this.label7.Location = new System.Drawing.Point(190, 148);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 18);
            this.label7.TabIndex = 6;
            this.label7.Text = "工作";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(190, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 18);
            this.label6.TabIndex = 5;
            this.label6.Text = "假日";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(190, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 18);
            this.label5.TabIndex = 4;
            this.label5.Text = "季節";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 193);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "平均溫度";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "平均日照量";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "相對總用電";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "即時總用電";
            // 
            // chart1
            // 
            chartArea2.Area3DStyle.PointDepth = 50;
            chartArea2.Area3DStyle.PointGapDepth = 50;
            chartArea2.Area3DStyle.Rotation = 20;
            chartArea2.Area3DStyle.WallWidth = 1;
            chartArea2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            chartArea2.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.LeftRight;
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Top;
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend2.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend2.IsTextAutoFit = false;
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series3.BorderWidth = 3;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.LegendText = "Actual Value";
            series3.Name = "Series1";
            series3.YValueMembers = "Target";
            series4.BorderWidth = 3;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Legend = "Legend1";
            series4.LegendText = "Forecase Value";
            series4.Name = "Series2";
            series4.YValueMembers = "Target_Forecast";
            this.chart1.Series.Add(series3);
            this.chart1.Series.Add(series4);
            this.chart1.Size = new System.Drawing.Size(972, 365);
            this.chart1.TabIndex = 3;
            this.chart1.Text = "chart2";
            title2.Font = new System.Drawing.Font("微軟正黑體", 16F);
            title2.Name = "title1";
            title2.Text = "每日用電預測趨勢圖";
            this.chart1.Titles.Add(title2);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(335, 365);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(637, 238);
            this.dataGridView1.TabIndex = 0;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Right;
            this.dataGridView2.Location = new System.Drawing.Point(972, 0);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(33, 603);
            this.dataGridView2.TabIndex = 4;
            this.dataGridView2.Visible = false;
            // 
            // dataGridView3
            // 
            this.dataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Dock = System.Windows.Forms.DockStyle.Right;
            this.dataGridView3.Location = new System.Drawing.Point(1005, 0);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowHeadersVisible = false;
            this.dataGridView3.RowTemplate.Height = 24;
            this.dataGridView3.Size = new System.Drawing.Size(40, 603);
            this.dataGridView3.TabIndex = 5;
            this.dataGridView3.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(12, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(170, 20);
            this.label10.TabIndex = 6;
            this.label10.Text = "Mean absolute error :";
            // 
            // lbl_Mae
            // 
            this.lbl_Mae.AutoSize = true;
            this.lbl_Mae.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_Mae.Location = new System.Drawing.Point(188, 9);
            this.lbl_Mae.Name = "lbl_Mae";
            this.lbl_Mae.Size = new System.Drawing.Size(49, 20);
            this.lbl_Mae.TabIndex = 7;
            this.lbl_Mae.Text = "value";
            // 
            // ElectricityForecast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 603);
            this.Controls.Add(this.lbl_Mae);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView3);
            this.Name = "ElectricityForecast";
            this.Text = "ElectricityForecast";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cb_Avg_Temperature;
        private System.Windows.Forms.Button btn_Forecast;
        private System.Windows.Forms.ComboBox cb_Total_Relatively_kWh;
        private System.Windows.Forms.ComboBox cb_Avg_Illumination;
        private System.Windows.Forms.ComboBox cb_Total_kWh;
        private System.Windows.Forms.ComboBox cb_Season;
        private System.Windows.Forms.ComboBox cb_Holiday;
        private System.Windows.Forms.ComboBox cb_Work;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_LearningRate;
        private System.Windows.Forms.TextBox txt_MomentumFactor;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lbl_Work2;
        private System.Windows.Forms.Label lbl_Work1;
        private System.Windows.Forms.Label lbl_Holiday2;
        private System.Windows.Forms.Label lbl_Holiday1;
        private System.Windows.Forms.Label lbl_Season2;
        private System.Windows.Forms.Label lbl_Season1;
        private System.Windows.Forms.Label lbl_Avg_Temperature2;
        private System.Windows.Forms.Label lbl_Avg_Temperature1;
        private System.Windows.Forms.Label lbl_Avg_Illumination2;
        private System.Windows.Forms.Label lbl_Avg_Illumination1;
        private System.Windows.Forms.Label lbl_Total_Relatively_kWh2;
        private System.Windows.Forms.Label lbl_Total_Relatively_kWh1;
        private System.Windows.Forms.Label lbl_Total_kWh2;
        private System.Windows.Forms.Label lbl_Total_kWh1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbl_Mae;

    }
}

