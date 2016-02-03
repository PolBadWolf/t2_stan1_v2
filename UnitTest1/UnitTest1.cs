using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using test2;
using System.Collections.Generic;

namespace UnitTest1
{
    [TestClass]
    public class UnitTest1
    {
        private Dictionary<string, string> _dicCount = null;
        private ArchiveControl ac = null;
        private Write_NewTube wnt = null;
        private CollectClass cc = null;

        [TestMethod]
        public void TestMethod1()
        {
            ac = new ArchiveControl(this);
            MainWindow.ac_m = ac;
            wnt = new Write_NewTube();
            cc = new CollectClass();

            _dicCount = new Dictionary<string, string>();
            ac._countLastIndex = wnt.lastIndex_defectsdata();
            for (int i = 0; i < 50; i++)
            {
                cc.Csmens(_dicCount);
            }
        }
    }
}
