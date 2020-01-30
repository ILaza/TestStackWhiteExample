
using NUnit.Framework;
using System.IO;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;

namespace TestStackWhiteExample
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {

            var applicationDirectory = TestContext.CurrentContext.TestDirectory;
            Application application = Application.Launch("notepad.exe");
            //Window window = application.GetWindow()
        }
    }
}
