using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beispiel_Export_Model;

namespace BeispielUtilityPresenter.ViewInterfaces
{
    public interface IBatchSelection
    {
        Dictionary<string, object> selectedBatches { get; set; }
        event EventHandler selectBatch;
        event EventHandler selectSample;
        event EventHandler selectAllSamples;
        event EventHandler deselectAllSamples;
        event EventHandler deselectSample;
        event EventHandler deselectBatch;
    }
}
