using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyWindowsMediaPlayer.Model;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestMyWindowsMediaPlayer
{
    [TestClass]
    public class TestXmlBDD
    {
        [TestMethod]
        public void TestMethod1()
        {
            IBDD ibdd = new XmlBDD();

            var mediaList = new List<String>();

            foreach (var i in Enumerable.Range(0, 42))
               mediaList.Add(i.ToString());
            ibdd.AddMedia(mediaList, "the answer to life the universe and everything");
        }
        [TestMethod]
        public void TestMethod2()
        {
            IBDD ibdd = new XmlBDD("42.xml");

            var mediaList = new List<String>();

            foreach (var i in Enumerable.Range(0, 42))
                mediaList.Add(i.ToString());
            ibdd.AddMedia(mediaList, "the answer to life the universe and everything");
        }
    }
}
