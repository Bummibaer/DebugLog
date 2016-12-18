using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugLog;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnitTestDebugLog
{
    [TestClass]
    public class UnitTest1
    {
        string[] codes = new string[]{
            "AA",
            "AB",
            "ACD",
            "BA",
            "C",
            "EF",
            "I%2d"
            };
        [TestMethod]
        public void TestMethod1()
        {
            Tree t = new Tree();
            Assert.IsNotNull(t, "Could not instantiate Tree!");
            Parser p = new Parser(t);
            Assert.IsNotNull(p, "Could not instantiate Parser!");

        }
        [TestMethod]
        public void TestAddChildren()
        {
            Tree tree = new Tree();
            foreach (string s in codes)
            {
                char[] cs = s.ToCharArray();
                for (int i = 0; i < s.Length; i++)
                {
                    Parser.sNode sn = new Parser.sNode(cs[i]);
                    if (i == (s.Length - 1)) sn.last = true;
                    tree.AddChildren(i, sn);
                }

            }
            Parser p = new Parser(tree);

            String tests = "ABAABACEFACDF123I22dAB";
            char[] bTest = tests.ToCharArray();
            for (int i = 0; i < tests.Length; i++)
            {
                eRETURN er = p.CreateTree(bTest[i]);
                if (i == 1)
                {
                    Assert.AreEqual(er, eRETURN.DECODED);
                }
                 Trace.WriteLine(Environment.NewLine +  i + "\t" + er.ToString());
            }

        }
    }
}
