using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeispielUtilityPresenter.ViewInterfaces
{
    public interface IMainView
    {
        event EventHandler exitApplication;
        event EventHandler exportData;
        event EventHandler setStartDate;
        event EventHandler setEndDate;
        event EventHandler getBatches;
        event EventHandler loadData;
        
        string savepath { get; set; }
    }
}
