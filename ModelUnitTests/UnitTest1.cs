using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private string connectionStringMI;
        private string connectionStringPakLx;

        [TestInitialize]
        public void Initialize()
        {
            EvolutionDBFacade.EvolutionDBFacade.DBFacade dBFacade = new EvolutionDBFacade.EvolutionDBFacade.DBFacade($"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\LIFECODES\\External_Matchit.config", true);
            EvolutionDBFacade.EvolutionDBFacade.DBFacade dBFacade1 = new EvolutionDBFacade.EvolutionDBFacade.DBFacade($"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\LIFECODES\\External_MatchitPakLx.config", true);
            connectionStringMI = dBFacade.GetSqlConnectionString();
            connectionStringPakLx = dBFacade1.GetSqlConnectionString();
        }
        [TestMethod]
        public void TestMethod1()
        {
            BeispielExportModel.Batches batches = new BeispielExportModel.Batches("2020-01-01", "2020-08-01");
            Assert.IsTrue(batches.FoundBatches.Count > 0);
        }
    }
}
