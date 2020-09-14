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
    public partial class BatchData : UserControl, IBatchData
    {
        public BatchData()
        {
            InitializeComponent();
        }
    }
}
