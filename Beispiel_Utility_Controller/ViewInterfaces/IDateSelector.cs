using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeispielUtilityPresenter.ViewInterfaces
{
    public interface IDateSelector
    {
        string StartDate { get; set; }
        string EndDate { get; set; }

        event EventHandler SetStartDate;
        event EventHandler SetEndDate;
        event EventHandler LoadData;
        event EventHandler ExportData;
    }
}
