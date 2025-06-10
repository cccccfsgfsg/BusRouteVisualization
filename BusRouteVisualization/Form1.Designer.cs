namespace BusRouteVisualization
{
    partial class Маршрут
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.picMap = new System.Windows.Forms.PictureBox();
            this.btnLoadRoute = new System.Windows.Forms.Button();
            this.btnAnimate = new System.Windows.Forms.Button();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.cmbRoutes = new System.Windows.Forms.ComboBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.btnAddPoint = new System.Windows.Forms.Button();
            this.btnCreateRoute = new System.Windows.Forms.Button();
            this.txtRouteName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picMap)).BeginInit();
            this.SuspendLayout();
            // 
            // picMap
            // 
            this.picMap.Location = new System.Drawing.Point(10, 10);
            this.picMap.Name = "picMap";
            this.picMap.Size = new System.Drawing.Size(644, 368);
            this.picMap.TabIndex = 0;
            this.picMap.TabStop = false;
            // 
            // btnLoadRoute
            // 
            this.btnLoadRoute.Location = new System.Drawing.Point(664, 83);
            this.btnLoadRoute.Name = "btnLoadRoute";
            this.btnLoadRoute.Size = new System.Drawing.Size(124, 23);
            this.btnLoadRoute.TabIndex = 1;
            this.btnLoadRoute.Text = "Загрузить маршрут";
            this.btnLoadRoute.UseVisualStyleBackColor = true;
            this.btnLoadRoute.Click += new System.EventHandler(this.btnLoadRoute_Click);
            // 
            // btnAnimate
            // 
            this.btnAnimate.Location = new System.Drawing.Point(664, 112);
            this.btnAnimate.Name = "btnAnimate";
            this.btnAnimate.Size = new System.Drawing.Size(124, 25);
            this.btnAnimate.TabIndex = 2;
            this.btnAnimate.Text = "Анимация маршрута";
            this.btnAnimate.UseVisualStyleBackColor = true;
            this.btnAnimate.Click += new System.EventHandler(this.btnAnimate_Click);
            // 
            // cmbRoutes
            // 
            this.cmbRoutes.FormattingEnabled = true;
            this.cmbRoutes.Location = new System.Drawing.Point(664, 31);
            this.cmbRoutes.Name = "cmbRoutes";
            this.cmbRoutes.Size = new System.Drawing.Size(124, 21);
            this.cmbRoutes.TabIndex = 3;
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 444);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(800, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // btnAddPoint
            // 
            this.btnAddPoint.Location = new System.Drawing.Point(59, 398);
            this.btnAddPoint.Name = "btnAddPoint";
            this.btnAddPoint.Size = new System.Drawing.Size(104, 30);
            this.btnAddPoint.TabIndex = 5;
            this.btnAddPoint.Text = "Добавить точку";
            this.btnAddPoint.UseVisualStyleBackColor = true;
            this.btnAddPoint.Click += new System.EventHandler(this.BtnAddPoint_Click);
            // 
            // btnCreateRoute
            // 
            this.btnCreateRoute.Location = new System.Drawing.Point(233, 398);
            this.btnCreateRoute.Name = "btnCreateRoute";
            this.btnCreateRoute.Size = new System.Drawing.Size(130, 30);
            this.btnCreateRoute.TabIndex = 6;
            this.btnCreateRoute.Text = "Добавить маршрут";
            this.btnCreateRoute.UseVisualStyleBackColor = true;
            this.btnCreateRoute.Click += new System.EventHandler(this.BtnCreateRoute_Click);
            // 
            // txtRouteName
            // 
            this.txtRouteName.BackColor = System.Drawing.Color.Tan;
            this.txtRouteName.Location = new System.Drawing.Point(435, 398);
            this.txtRouteName.Name = "txtRouteName";
            this.txtRouteName.Size = new System.Drawing.Size(100, 20);
            this.txtRouteName.TabIndex = 7;
            // 
            // Маршрут
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Bisque;
            this.ClientSize = new System.Drawing.Size(800, 466);
            this.Controls.Add(this.txtRouteName);
            this.Controls.Add(this.btnCreateRoute);
            this.Controls.Add(this.btnAddPoint);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.cmbRoutes);
            this.Controls.Add(this.btnAnimate);
            this.Controls.Add(this.btnLoadRoute);
            this.Controls.Add(this.picMap);
            this.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Name = "Маршрут";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picMap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picMap;
        private System.Windows.Forms.Button btnLoadRoute;
        private System.Windows.Forms.Button btnAnimate;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.ComboBox cmbRoutes;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Button btnAddPoint;
        private System.Windows.Forms.Button btnCreateRoute;
        private System.Windows.Forms.TextBox txtRouteName;
    }

}

