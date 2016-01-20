using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recover
{
    class Nabor
    {
        public char[] dd = null;
        public Nabor()
        {
            ArrayList pre = new ArrayList();
            pre.Clear();
            pre.Add(0);
            for (int i = '0'; i < '9' + 1; i++)
            {
                pre.Add(i);
            }
            //========================
            
            for (int i = 'a'; i < 'z' + 1; i++)
            {
                pre.Add(i);
            }
            
            //============================
            dd = Array.ConvertAll(pre.ToArray(), Convert.ToChar);
        }
    }
}
