namespace Beispiel_Export.UserControls
{
    partial class BatchData
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.uxgcBatchData = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.uxgcBatchData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // uxgcBatchData
            // 
            this.uxgcBatchData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxgcBatchData.Location = new System.Drawing.Point(0, 0);
            this.uxgcBatchData.MainView = this.gridView1;
            this.uxgcBatchData.Name = "uxgcBatchData";
            this.uxgcBatchData.Size = new System.Drawing.Size(684, 295);
            this.uxgcBatchData.TabIndex = 0;
            this.uxgcBatchData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.uxgcBatchData;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // BatchData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uxgcBatchData);
            this.Name = "BatchData";
            this.Size = new System.Drawing.Size(684, 295);
            ((System.ComponentModel.ISupportInitialize)(this.uxgcBatchData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl uxgcBatchData;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}
