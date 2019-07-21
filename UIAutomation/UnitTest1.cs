using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System.Threading;

namespace UIAutomation
{
    [TestClass]
    public class UnitTest1
    {
        public const string windowsDriverUrl = "http://127.0.0.1:4723";
        public const string applicationPath = @"E:\next\ProcessFinder.exe";
        public static WindowsDriver<WindowsElement> session;
        public static WindowsDriver<WindowsElement> session1;

        [TestInitialize]
        public void Init_method()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Root"); //Need set capablity to Desktop to get any opened application
            appCapabilities.SetCapability("deviceName", "WindowsPC"); 
            session = new WindowsDriver<WindowsElement>(new Uri(windowsDriverUrl), appCapabilities);
            session.FindElementByName("ProcessFinder").Click();
            DesiredCapabilities appCapabilities1 = new DesiredCapabilities();
            appCapabilities1.SetCapability("app", applicationPath);
            appCapabilities1.SetCapability("deviceName", "WindowsPC");
            session1 = new WindowsDriver<WindowsElement>(new Uri(windowsDriverUrl), appCapabilities1);
        }

        /// <summary>
        /// Do a check for any process which is no running now.
        /// </summary>
        [TestMethod]
        public void FakeProcessSearch()
        {           
            session1.FindElementByAccessibilityId("textBox1");
            session1.FindElementByAccessibilityId("textBox1").SendKeys("dummy");
            session1.FindElementByAccessibilityId("button1").Click();
            Thread.Sleep(2000);
            session1.FindElementByAccessibilityId("label9");
        }

        [TestMethod]
        public void SearchProperProcess()
        {           
            session1.FindElementByAccessibilityId("textBox1").SendKeys("chrome");
            session1.FindElementByAccessibilityId("button1").Click();
            Thread.Sleep(90000);
            Assert.IsTrue(session1.FindElementByAccessibilityId("label10").Displayed);
        }
    }
}
