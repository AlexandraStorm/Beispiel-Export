using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using BeispielUtilityPresenter.ViewInterfaces;
using BeispielUtilityPresenter;
using BeispielViewModel;
using System.IO;

namespace Beispiel_Export
{
    public partial class MainView : Form, IMainView
    {
        private readonly MainViewPresenter presenter;

        public string savepath {
            get
            {
                return btnexportlocation.EditValue.ToString();
            }
            set { }
        }

        public MainView()
        {
            InitializeComponent();
            setLocationValue();
            presenter = new MainViewPresenter(this);
            tlBatches.NodeCellStyle += TlBatches_NodeCellStyle;
        }
        private void setLocationValue()
        {
            string configFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\Lifecodes\\Beispiel.config";
            if (!File.Exists(configFile))
            {
                File.Copy($"{Environment.CurrentDirectory}\\Beispiel.config", configFile);
            }
            btnexportlocation.EditValue = getsavelocation();
        }
        public event EventHandler exitApplication;
        public event EventHandler exportData;
        public event EventHandler setStartDate;
        public event EventHandler setEndDate;
        public event EventHandler getBatches;
        public event EventHandler loadData;
        private string getsavelocation()
        {
            string configFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\Lifecodes\\Beispiel.config";
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFile;
            string retValue;
            Configuration config =  ConfigurationManager.OpenMappedExeConfiguration(fileMap,ConfigurationUserLevel.None);
            retValue = config.AppSettings.Settings["exportlocation"].Value;
            return retValue;
        }
        private void dtStartDate_EditValueChanged(object sender, EventArgs e)
        {
            EventHandler handler = setStartDate;
            string dateselected = ((DevExpress.XtraEditors.DateEdit)sender).DateTime.ToString("yyyy-MM-dd");
            handler?.Invoke(dateselected, e);
        }

        private void dtEndDate_EditValueChanged(object sender, EventArgs e)
        {
            if (dtStartDate.DateTime > dtEndDate.DateTime)
            {
                MessageBox.Show("End date cannot be before start date");
                dtEndDate.DateTime = DateTime.Now;
            }
            else
            {
                EventHandler handler = setEndDate;
                string dateselected = ((DevExpress.XtraEditors.DateEdit)sender).DateTime.ToString("yyyy-MM-dd");
                handler?.Invoke(dateselected, e);
            }
        }

        private void btnGetBatches_Click(object sender, EventArgs e)
        {
            ClearForm();
            EventHandler handler = getBatches;
            handler?.Invoke(sender, e);

            List<BatchVM> batches = presenter.batchVM;
            
            tlBatches.DataSource = batches;
            
            tlBatches.ChildListFieldName = "samples";
        }
        private void ClearForm()
        {
            tlBatches.DataSource = null;
            tlBatches.RefreshDataSource();
            dgDNA.DataSource = null;
            dgLSA.DataSource = null;
            dgPaklx.DataSource = null;
        }
        private void TlBatches_NodeCellStyle(object sender, GetCustomNodeCellStyleEventArgs e)
        {
            if (e.Column.FieldName != "name") return;
            if (e.Node.HasChildren)
            {
                e.Appearance.BackColor = (Color)e.Node.GetValue("backcolor");
                e.Appearance.BackColor2 = (Color)e.Node.GetValue("backcolor");
            }
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            btnExport.Enabled = true;
            List<BatchVM> batchVMs = (List<BatchVM>)tlBatches.DataSource;

            EventHandler handler = loadData;
            handler?.Invoke(batchVMs, e);
            dgLSA.DataSource = presenter.lsaVM;
            dgDNA.DataSource = presenter.dnaVM;
            dgPaklx.DataSource = presenter.pakVM;
            dgLMX.DataSource = presenter.lmxVM;
        }

        private void batchCheckChanged(object sender, EventArgs e)
        {
            CheckEdit edit = sender as CheckEdit;
            TreeList treeList = (TreeList)edit.Parent;
            
            switch (edit.Checked)
            {
                case true:
                    if (treeList.FocusedNode.HasChildren)
                    {
                        treeList.FocusedNode.SetValue("Selected", true);
                        foreach(TreeListNode node in treeList.FocusedNode.Nodes)
                        {
                            node.SetValue("Selected", true);
                        }
                    }
                    break;
                case false:
                    if (treeList.FocusedNode.HasChildren)
                    {
                        treeList.FocusedNode.SetValue("Selected", false);
                        foreach (TreeListNode node in treeList.FocusedNode.Nodes)
                        {
                            node.SetValue("Selected", false);
                        }
                    }
                    break;
            }
        }

        private void barEditItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            folderBrowserDialog1.SelectedPath = ConfigurationManager.AppSettings["exportlocation"];
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string configFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\Lifecodes\\Beispiel.config";
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                    fileMap.ExeConfigFilename = configFile;
                    string retValue;
                    Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                    retValue = config.AppSettings.Settings["exportlocation"].Value;
                    var settings = config.AppSettings.Settings;
                    if (settings["exportlocation"] == null)
                    {
                        settings.Add("exportlocation", folderBrowserDialog1.SelectedPath);
                    }
                    else
                    {
                        settings["exportlocation"].Value = folderBrowserDialog1.SelectedPath;
                    }
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
                }
                catch (ConfigurationErrorsException)
                {
                    Console.WriteLine("Error writing app settings");
                }
            }
            btnexportlocation.EditValue = folderBrowserDialog1.SelectedPath;
        }
        private void mnuExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Application.Exit();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            EventHandler handler = exportData;
            handler?.Invoke(sender, e);

            MessageBox.Show("Export complete");
        }
    }
}
