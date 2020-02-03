using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.InputDevices;
using TestStack.White.Sessions;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.TabItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WPFUIItems;
using TestStack.White.WindowsAPI;

namespace TestStackWhiteExample
{
    [TestFixture]
    public class ActivityCenterSettingTestClass
    {
        private Window ndOficceMainWindow;
        private Win32Window ndOfficeSettingWindow;

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
            GetWindowByUIAutomation();
        }
       
        [Test, Order(1)]
        public void OpenTabOtherTest()
        {

            var tab = ndOfficeSettingWindow.Get<Tab>(SearchCriteria.ByAutomationId("SettingsTabControl"));
            var tabOther = tab.Get<TabPage>(SearchCriteria.ByAutomationId("NotificationsTabItem"));

            tabOther.Select();
            var actualResult = tabOther.IsSelected;
            
            //var actaulResult = Wait(() => tabOther.IsFocussed, TimeSpan.FromSeconds(10));
            Assert.IsTrue(actualResult);
        }

        [Test, Order(2)]
        public void ChangeSlidersValueOnTabOtherTest()
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

        [Test, Order(3)]
        public void ChangeComboBoxOnNavigationTabTest()
        {
            var tab = ndOfficeSettingWindow.Get<Tab>(SearchCriteria.ByAutomationId("SettingsTabControl"));
            var tabOther = tab.Get<TabPage>(SearchCriteria.ByAutomationId("NavigationViewTabItem"));

            tabOther.Select();
            var actualResult = tabOther.IsSelected;

            //var actaulResult = Wait(() => tabOther.IsFocussed, TimeSpan.FromSeconds(10));
            Assert.IsTrue(actualResult);

            var comboBox = tabOther.Get<ComboBox>(SearchCriteria.ByAutomationId("NdOfficedialogsDefaultLocationsCombo"));
            comboBox.Select(1);
            Thread.Sleep(2000);
            comboBox.Select(0);
            Thread.Sleep(2000);
        }

        [Test, Order(4)]
        public void ChangeHostTextBoxOnNetworkTab()
        {
            var tab = ndOfficeSettingWindow.Get<Tab>(SearchCriteria.ByAutomationId("SettingsTabControl"));
            var tabNetwork = tab.Get<TabPage>(SearchCriteria.ByAutomationId("HostTabItem"));
            tabNetwork.Select();

            var proxyGroup = tabNetwork.Get<GroupBox>(SearchCriteria.ByAutomationId("ProxyGroupItem"));
            var httpProxyRadioButton = proxyGroup.Get<RadioButton>(SearchCriteria.ByAutomationId("HttpProxyRadio"));
            httpProxyRadioButton.Select();

            var hostValueTextBox = proxyGroup.Get<TextBox>(SearchCriteria.ByAutomationId("ProxyHostValueTextBox"));
            
            Mouse.Instance.Location = hostValueTextBox.Bounds.BottomLeft;
            hostValueTextBox.ClickAtCenter();

            Keyboard.Instance.Enter("TEST TEXT");
            Thread.Sleep(5000);
            Keyboard.Instance.Enter(string.Empty);

            string testString = "Test Text";
            var lines = testString.Replace("\r\n", "\n").Split('\n');
            Keyboard.Instance.Send(lines[0], new NullActionListener());
            foreach (var line in lines.Skip(1))
            {
                Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                Keyboard.Instance.Send(line, hostValueTextBox.ActionListener);                   
            }            

            Thread.Sleep(5000);

            var systemProxyRadioButton = proxyGroup.Get<RadioButton>(SearchCriteria.ByAutomationId("SystemProxyRadio"));
            systemProxyRadioButton.Select();         
        }

        public void GetWindowByUIAutomation()
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
