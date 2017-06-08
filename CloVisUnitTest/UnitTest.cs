
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resume;
using ResumeElements;
using System.Diagnostics;

namespace CloVisUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddDataToElementList()
        {
            var list = new Deprecated_ElementList<Element>("test");
            var data = new Deprecated_Data<string>("data", "value", -1, "", true);
            list.Add(data);
            Assert.IsTrue(data.Categories.Contains(list));
        }

        [TestMethod]
        public void AddListToDataCategory()
        {
            var list = new Deprecated_ElementList<Element>("test");
            var data = new Deprecated_Data<string>("data", "value", -1, "", true);
            data.AddCategory(list);
            Assert.IsTrue(data.Categories.Contains(list));
            Assert.IsTrue(list.Contains(data));
        }
    }
}
