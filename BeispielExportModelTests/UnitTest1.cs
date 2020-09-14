using NUnit.Framework;
using System;

namespace BeispielExportModelTests
{
    public class Tests
    {
        private string connectionStringMI;
        private string connectionStringPakLx;

        [SetUp]
        public void Setup()
        {
            EvolutionDBFacade.EvolutionDBFacade.DBFacade dBFacade = new EvolutionDBFacade.EvolutionDBFacade.DBFacade($"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\LIFECODES\\External_Matchit.config");
            EvolutionDBFacade.EvolutionDBFacade.DBFacade dBFacade1 = new EvolutionDBFacade.EvolutionDBFacade.DBFacade($"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\LIFECODES\\External_MatchitPakLx.config");
            connectionStringMI = dBFacade.GetSqlConnectionString();
            connectionStringPakLx = dBFacade1.GetSqlConnectionString();
        }

        [Test]
        public void GetBatchesTest()
        {
            Beispiel_Export_Model.Batches batches = new Beispiel_Export_Model.Batches("2020-01-01", "2020-08-01");
            Assert.IsTrue(batches.FoundBatches.Count > 0);
        }
    }
}