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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.wiiChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.kinectChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.rgbImage = new System.Windows.Forms.PictureBox();
            this.barLabel = new System.Windows.Forms.Label();
            this.rgbBar = new System.Windows.Forms.TrackBar();
            this.jointSpeed = new System.Windows.Forms.Label();
            this.wiiAccel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.wiiChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kinectChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgbImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgbBar)).BeginInit();
            this.SuspendLayout();
            // 
            // wiiChart
            // 
            chartArea5.Name = "ChartArea1";
            this.wiiChart.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.wiiChart.Legends.Add(legend5);
            this.wiiChart.Location = new System.Drawing.Point(647, 400);
            this.wiiChart.Name = "wiiChart";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.wiiChart.Series.Add(series5);
            this.wiiChart.Size = new System.Drawing.Size(640, 300);
            this.wiiChart.TabIndex = 0;
            this.wiiChart.Text = "WiiRemote";
            // 
            // kinectChart
            // 
            chartArea6.Name = "ChartArea1";
            this.kinectChart.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.kinectChart.Legends.Add(legend6);
            this.kinectChart.Location = new System.Drawing.Point(644, 63);
            this.kinectChart.Name = "kinectChart";
            series6.ChartArea = "ChartArea1";
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            this.kinectChart.Series.Add(series6);
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
            this.jointSpeed.Location = new System.Drawing.Point(645, 45);
            this.jointSpeed.Name = "jointSpeed";
            this.jointSpeed.Size = new System.Drawing.Size(64, 12);
            this.jointSpeed.TabIndex = 5;
            this.jointSpeed.Text = "JointSpeed:";
            // 
            // wiiAccel
            // 
            this.wiiAccel.AutoSize = true;
            this.wiiAccel.Location = new System.Drawing.Point(645, 385);
            this.wiiAccel.Name = "wiiAccel";
            this.wiiAccel.Size = new System.Drawing.Size(51, 12);
            this.wiiAccel.TabIndex = 6;
            this.wiiAccel.Text = "WiiAccel:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 701);
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
    }
}

