using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeispielUtilityPresenter.ViewInterfaces;

namespace Beispiel_Export.UserControls
{
    public partial class BatchSelection : UserControl, IBatchSelection
    {

        public BatchSelection()
        {
            InitializeComponent();
        }

        public Dictionary<string, object> selectedBatches { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler selectBatch;
        public event EventHandler selectSample;
        public event EventHandler selectAllSamples;
        public event EventHandler deselectAllSamples;
        public event EventHandler deselectSample;
        public event EventHandler deselectBatch;
    }
}
