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
        public void TestInstatiaton()
        {
            Parser p = new Parser(codes);
            Assert.IsNotNull(p, "Could not instantiate Parser!");
        }
        [TestMethod]
        public void TestAddChildren()
        {
            Parser p = new Parser(codes);

            String tests = "ABAABACEFACDF123I22dAB";
            char[] bTest = tests.ToCharArray();
            for (int i = 0; i < tests.Length; i++)
            {
                eRETURN er = p.Parse(bTest[i]);
                if (i == 1)
                {
                    Assert.AreEqual(er, eRETURN.DECODED);
                }
                 Trace.WriteLine(Environment.NewLine +  i + "\t" + er.ToString());
            }

        }
    }
}
