namespace HoeGraphForm
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.wiiChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.kinectChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.rgbImage = new System.Windows.Forms.PictureBox();
            this.barLabel = new System.Windows.Forms.Label();
            this.rgbBar = new System.Windows.Forms.TrackBar();
            this.jointSpeed = new System.Windows.Forms.Label();
            this.wiiAccel = new System.Windows.Forms.Label();
            this.thresholdBar = new System.Windows.Forms.TrackBar();
            this.thresholdLabel = new System.Windows.Forms.Label();
            this.kinectFreq = new System.Windows.Forms.Label();
            this.wiiFreq = new System.Windows.Forms.Label();
            this.wiiFrame = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.wiiChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kinectChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgbImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgbBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdBar)).BeginInit();
            this.SuspendLayout();
            // 
            // wiiChart
            // 
            chartArea1.Name = "ChartArea1";
            this.wiiChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.wiiChart.Legends.Add(legend1);
            this.wiiChart.Location = new System.Drawing.Point(647, 400);
            this.wiiChart.Name = "wiiChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.wiiChart.Series.Add(series1);
            this.wiiChart.Size = new System.Drawing.Size(640, 300);
            this.wiiChart.TabIndex = 0;
            this.wiiChart.Text = "WiiRemote";
            // 
            // kinectChart
            // 
            chartArea2.Name = "ChartArea1";
            this.kinectChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.kinectChart.Legends.Add(legend2);
            this.kinectChart.Location = new System.Drawing.Point(644, 63);
            this.kinectChart.Name = "kinectChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.kinectChart.Series.Add(series2);
            this.kinectChart.Size = new System.Drawing.Size(640, 300);
            this.kinectChart.TabIndex = 1;
            this.kinectChart.Text = "Kinect";
            // 
            // rgbImage
            // 
            this.rgbImage.Location = new System.Drawing.Point(1, 94);
            this.rgbImage.Name = "rgbImage";
            this.rgbImage.Size = new System.Drawing.Size(640, 480);
            this.rgbImage.TabIndex = 2;
            this.rgbImage.TabStop = false;
            // 
            // barLabel
            // 
            this.barLabel.AutoSize = true;
            this.barLabel.Location = new System.Drawing.Point(-1, 30);
            this.barLabel.Name = "barLabel";
            this.barLabel.Size = new System.Drawing.Size(11, 12);
            this.barLabel.TabIndex = 3;
            this.barLabel.Text = "0";
            // 
            // rgbBar
            // 
            this.rgbBar.Location = new System.Drawing.Point(1, 45);
            this.rgbBar.Name = "rgbBar";
            this.rgbBar.Size = new System.Drawing.Size(400, 45);
            this.rgbBar.TabIndex = 4;
            this.rgbBar.ValueChanged += new System.EventHandler(this.rgbBar_ValueChanged);
            // 
            // jointSpeed
            // 
            this.jointSpeed.AutoSize = true;
            this.jointSpeed.Location = new System.Drawing.Point(647, 26);
            this.jointSpeed.Name = "jointSpeed";
            this.jointSpeed.Size = new System.Drawing.Size(64, 12);
            this.jointSpeed.TabIndex = 5;
            this.jointSpeed.Text = "JointSpeed:";
            // 
            // wiiAccel
            // 
            this.wiiAccel.AutoSize = true;
            this.wiiAccel.Location = new System.Drawing.Point(642, 382);
            this.wiiAccel.Name = "wiiAccel";
            this.wiiAccel.Size = new System.Drawing.Size(51, 12);
            this.wiiAccel.TabIndex = 6;
            this.wiiAccel.Text = "WiiAccel:";
            // 
            // thresholdBar
            // 
            this.thresholdBar.Location = new System.Drawing.Point(818, 9);
            this.thresholdBar.Maximum = 400;
            this.thresholdBar.Name = "thresholdBar";
            this.thresholdBar.Size = new System.Drawing.Size(444, 45);
            this.thresholdBar.TabIndex = 7;
            this.thresholdBar.ValueChanged += new System.EventHandler(this.thresholdBar_ValueChanged);
            // 
            // thresholdLabel
            // 
            this.thresholdLabel.AutoSize = true;
            this.thresholdLabel.Location = new System.Drawing.Point(647, 9);
            this.thresholdLabel.Name = "thresholdLabel";
            this.thresholdLabel.Size = new System.Drawing.Size(54, 12);
            this.thresholdLabel.TabIndex = 8;
            this.thresholdLabel.Text = "threshold:";
            // 
            // kinectFreq
            // 
            this.kinectFreq.AutoSize = true;
            this.kinectFreq.Location = new System.Drawing.Point(647, 42);
            this.kinectFreq.Name = "kinectFreq";
            this.kinectFreq.Size = new System.Drawing.Size(60, 12);
            this.kinectFreq.TabIndex = 9;
            this.kinectFreq.Text = "Frequency:";
            // 
            // wiiFreq
            // 
            this.wiiFreq.AutoSize = true;
            this.wiiFreq.Location = new System.Drawing.Point(642, 397);
            this.wiiFreq.Name = "wiiFreq";
            this.wiiFreq.Size = new System.Drawing.Size(60, 12);
            this.wiiFreq.TabIndex = 10;
            this.wiiFreq.Text = "Frequency:";
            // 
            // wiiFrame
            // 
            this.wiiFrame.AutoSize = true;
            this.wiiFrame.Location = new System.Drawing.Point(644, 367);
            this.wiiFrame.Name = "wiiFrame";
            this.wiiFrame.Size = new System.Drawing.Size(36, 12);
            this.wiiFrame.TabIndex = 11;
            this.wiiFrame.Text = "frame:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 701);
            this.Controls.Add(this.wiiFrame);
            this.Controls.Add(this.wiiFreq);
            this.Controls.Add(this.kinectFreq);
            this.Controls.Add(this.thresholdLabel);
            this.Controls.Add(this.thresholdBar);
            this.Controls.Add(this.wiiAccel);
            this.Controls.Add(this.jointSpeed);
            this.Controls.Add(this.rgbBar);
            this.Controls.Add(this.barLabel);
            this.Controls.Add(this.rgbImage);
            this.Controls.Add(this.kinectChart);
            this.Controls.Add(this.wiiChart);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.wiiChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kinectChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgbImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgbBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart wiiChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart kinectChart;
        private System.Windows.Forms.PictureBox rgbImage;
        private System.Windows.Forms.Label barLabel;
        private System.Windows.Forms.TrackBar rgbBar;
        private System.Windows.Forms.Label jointSpeed;
        private System.Windows.Forms.Label wiiAccel;
        private System.Windows.Forms.TrackBar thresholdBar;
        private System.Windows.Forms.Label thresholdLabel;
        private System.Windows.Forms.Label kinectFreq;
        private System.Windows.Forms.Label wiiFreq;
        private System.Windows.Forms.Label wiiFrame;
    }
}

