using NUnit.Framework;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WPFUIItems;
using System.Windows.Automation;
using TestStack.White.Factory;
using TestStack.White.Sessions;
using System.Threading;
using TestStack.White.UIItems.Scrolling;

namespace TestStackWhiteExample
{
    [TestFixture]
    public class DatePickerInOpenDialogTestClass
    {
        private Window advancedSearchDialogWindow;
        [OneTimeSetUp]
        public void GetAdvancedSearchDialogWindowSetUp()
        {
            var cond = new PropertyCondition(AutomationElement.NameProperty, "Open");
            var automationElement = AutomationElement.RootElement.FindFirst(TreeScope.Descendants, cond);
            var openWindow = new Win32Window(automationElement, WindowFactory.Desktop,
                                                                 InitializeOption.NoCache,
                                                                 new ApplicationSession().WindowSession(InitializeOption.NoCache));
            Thread.Sleep(1000);
            var pane = openWindow.Get(SearchCriteria.ByAutomationId("RepositoryBrowsControlHeaderPanel"));
            var searchTextBox = pane.Get<TextBox>(SearchCriteria.ByAutomationId("SearchTextBox"));
            var expandMenuSearch = searchTextBox.Get<Image>(SearchCriteria.ByAutomationId("ExpandMenuIcon"));
            expandMenuSearch.Click();
            //var listMenuItem = openWindow.Get(SearchCriteria.ByAutomationId("ContextMenuScrollViewer"));
            var menuItemAdvancedSearch = openWindow.Get(SearchCriteria.ByText("Advanced Search"));
            menuItemAdvancedSearch.Click();
            var propCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, "AdvancedSearchDialog");
            automationElement = openWindow.AutomationElement.FindFirst(TreeScope.Descendants, propCondition);
            advancedSearchDialogWindow = new Win32Window(automationElement, WindowFactory.Desktop,
                                                                 InitializeOption.NoCache,
                                                                 new ApplicationSession().WindowSession(InitializeOption.NoCache));
        }

        [Test]
        public void DatePickerTest()
        {            
            var scroll = advancedSearchDialogWindow.Get<VScrollBar>(SearchCriteria.ByAutomationId("VerticalScrollBar"));
            scroll.ScrollDown();
            var dateRangeEditor = advancedSearchDialogWindow.Get(SearchCriteria.ByAutomationId("DateRangeEditor"));
            var datePicker = dateRangeEditor.Get<DateTimePicker>(SearchCriteria.ByAutomationId("CreationDateFromDatePicker"));
            //datePicker.SetDate(new System.DateTime(2019, 12, 12), DateFormat.YearMonthDay);
            //var buttonCalendar = datePicker. (SearchCriteria.ByAutomationId("DateTimePicker"));
        }     
    }
}
