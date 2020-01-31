using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.Sessions;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.TabItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WPFUIItems;

namespace TestStackWhiteExample
{
    [TestFixture]
    public class ActivityCenterSettingTestTestClass
    {
        private Window ndOficceMainWindow;
        private Win32Window ndOfficeSettingWindow;

        private const string TAB_OTHER = "Other";
        private const int EXPECTED_START_SLIDER_VALUE = 100;
        private const int EXPECTED_SLIDER_VALUE = 50;

        [OneTimeSetUp]
        public void OpenSettingWindowSetUp()
        {
            var ndActivityCenterDesctopButton = Desktop.Instance.Get<Button>(SearchCriteria.ByText("NetDocuments ndOffice"));
            ndActivityCenterDesctopButton.Click();

            var procInfo = new ProcessStartInfo(@"C:\Program Files (x86)\NetDocuments\ndOffice\ndOffice.exe");
            var application = Application.AttachOrLaunch(procInfo);

            var searchCriteria = SearchCriteria.ByAutomationId("NotificationAreaWindow");
            var wnd = application.GetWindow(searchCriteria, InitializeOption.NoCache);

            ndOficceMainWindow = new Win32Window(wnd.AutomationElement, WindowFactory.Desktop,
                                     InitializeOption.NoCache,
                                     new ApplicationSession().WindowSession(InitializeOption.NoCache));

            searchCriteria = SearchCriteria.ByAutomationId("SettingsMenuIcon");
            var settingImage = ndOficceMainWindow.Get<Image>(searchCriteria);
            settingImage.Click();
            Thread.Sleep(1000);

            searchCriteria = SearchCriteria.ByAutomationId("OpenSettingsMenuItem");
            var settingMenu = ndOficceMainWindow.Get(searchCriteria);
            settingMenu.Click();
            GetSettingWindow();
        }
       
        [Test]
        public void AOpenTabOtherTest()
        {

            var tab = ndOfficeSettingWindow.Get(SearchCriteria.ByAutomationId("SettingsTabControl"));
            var tabOther = tab.Get(SearchCriteria.ByAutomationId("NotificationsTabItem")) as TabPage;

            tabOther.Select();
            var actualResult = tabOther.IsSelected;
            
            //var actaulResult = Wait(() => tabOther.IsFocussed, TimeSpan.FromSeconds(10));
            Assert.IsTrue(actualResult);
        }

        [Test]
        public void BChangeSlidersValueOnTabOtherTest()
        {
            var panelActivityCenter = ndOfficeSettingWindow.Get(SearchCriteria.ByAutomationId("ActivityListGroup"));
            var slider = panelActivityCenter.Get<Slider>(SearchCriteria.ByAutomationId("NotificationDocumentsNumberSlider"));
            var thumbSlider = slider.Get<Thumb>(SearchCriteria.ByAutomationId("Thumb"));

            thumbSlider.SlideHorizontally(-50);
            Thread.Sleep(1000);
            thumbSlider.SlideHorizontally(50);
            Thread.Sleep(1000);
            slider.Value = 50;
            Thread.Sleep(1000);
        }

        [Test]
        public void ChangeComboBoxOnNavigationTabTest()
        {
            var tab = ndOfficeSettingWindow.Get(SearchCriteria.ByAutomationId("SettingsTabControl"));
            var tabOther = tab.Get(SearchCriteria.ByAutomationId("NavigationViewTabItem")) as TabPage;

            tabOther.Select();
            var actualResult = tabOther.IsSelected;

            //var actaulResult = Wait(() => tabOther.IsFocussed, TimeSpan.FromSeconds(10));
            Assert.IsTrue(actualResult);

            var comboBox = tabOther.Get<ComboBox>(SearchCriteria.ByAutomationId("NdOfficedialogsDefaultLocationsCombo"));
            comboBox.Select("Outlook");
        }

        public void GetSettingWindow()
        {
            var propCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "SettingsWindow");
            var ndOfficeSettingDesctopWindowAutomationElement = ndOficceMainWindow.AutomationElement.FindFirst(TreeScope.Descendants, propCondition);
            ndOfficeSettingWindow = new Win32Window(ndOfficeSettingDesctopWindowAutomationElement, WindowFactory.Desktop,
                                                                 InitializeOption.NoCache,
                                                                 new ApplicationSession().WindowSession(InitializeOption.NoCache));
            Thread.Sleep(2000);
        }
        static bool Wait(Func<bool> checker, TimeSpan waitTime)
        {
            var till = DateTime.Now + waitTime;
            var res = false;
            do
            {
                res = checker();
            }
            while (DateTime.Now <= till || !res);

            return res;
        }

        [OneTimeTearDown]
       public void SettingWindowDeleteTearDown()
       {
            if (ndOfficeSettingWindow!= null)
            {
                ndOfficeSettingWindow.Close();
                ndOfficeSettingWindow = null;
            }
       }            
    }
}
