
using NUnit.Framework;
using System.Diagnostics;
using System.IO;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace TestStackWhiteExample
{
    [TestFixture]
    public class TestClass
    {
        [SetUp]
        public void SetUp()
        {
            var ndACButton = Desktop.Instance.Get<Button>(SearchCriteria.ByText("NetDocuments ndOffice"));
            ndACButton.Click();

            var procInfo = new ProcessStartInfo(@"C:\Program Files (x86)\NetDocuments\ndOffice\ndOffice.exe");
            var application = Application.AttachOrLaunch(procInfo);

            var searchCriteria = SearchCriteria.ByAutomationId("NotificationAreaWindow");
            var window = application.GetWindow(searchCriteria, InitializeOption.NoCache);

            searchCriteria = SearchCriteria.ByAutomationId("SettingsMenuIcon");
            var settingMenu = window.Get<Image>(searchCriteria);
            settingMenu.Click();
        }


        [Test]
        public void ActivityCenterSettingTest()
        {
                    









        }
    }
}
