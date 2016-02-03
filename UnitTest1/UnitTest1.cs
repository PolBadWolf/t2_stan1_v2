using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using test2;
using System.Collections.Generic;

namespace UnitTest1
{
    [TestClass]
    public class UnitTest1
    {
        private Dictionary<string, string> _countYears = new Dictionary<string, string>();
        private Dictionary<string, string> _countDefectsYears = new Dictionary<string, string>();
        private Dictionary<string, string> _countMonths = new Dictionary<string, string>();
        private Dictionary<string, string> _countDefectsMonths = new Dictionary<string, string>();
        private Dictionary<string, string> _countDays = new Dictionary<string, string>();
        private Dictionary<string, string> _countDefectsDays = new Dictionary<string, string>();
        private Dictionary<string, string> _countParts = new Dictionary<string, string>();
        private Dictionary<string, string> _countDefectsParts = new Dictionary<string, string>();
        private Dictionary<string, string> _countSmens = new Dictionary<string, string>();
        private Dictionary<string, string> _countDefectsSmens = new Dictionary<string, string>();

        private ArchiveControl ac = null;
        private Write_NewTube wnt = null;
        private CollectClass cc = null;

        [TestMethod]
        public void TestMethod1()
        {
            ac = new ArchiveControl();
            MainWindow.ac_m = ac;
            wnt = new Write_NewTube();
            cc = new CollectClass();

            ac._countLastIndex = wnt.lastIndex_defectsdata();
            for (int i = 0; i < 50; i++)
            {
                cc.Cyears(_countYears);
                cc.Cdyears(_countDefectsYears);
                cc.Cmonths(_countMonths);
                cc.Cdmonths(_countDefectsMonths);
                cc.Cdays(_countDays);
                cc.Cddays(_countDefectsDays);
                cc.Csmens(_countSmens);
                cc.Cdsmens(_countDefectsSmens);
                cc.Cparts(_countParts);
                cc.cdparts(_countDefectsParts);
            }
        }
    }
}
