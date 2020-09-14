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
    public partial class DateSelection : UserControl, IDateSelector
    {
        public DateSelection()
        {
            InitializeComponent();
        }

        public string StartDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string EndDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler SetStartDate;
        public event EventHandler SetEndDate;
        public event EventHandler LoadData;
        public event EventHandler ExportData;



    }
}
