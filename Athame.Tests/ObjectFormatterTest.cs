using Athame.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Athame.Tests
{
    [TestClass]
    public class ObjectFormatterTest
    {
        private class TestData
        {
            public string Name { get; set; }
            public TestData CircularValue{ get; set; }
            public int Number { get; set; }

            public object AlwaysNullObject
            {
                get { return null; }
            }
        }

        [TestMethod]
        public void TestValidFormat()
        {
            const string validFormatString = "{Name} is the new {CircularValue.Name} times {Number}";
            var testObj = new TestData
            {
                Name = "Orange",
                CircularValue = new TestData
                {
                    Name = "Black"
                },
                Number = 15
            };
            var result = StringObjectFormatter.Format(validFormatString, testObj);
            Assert.AreEqual("Orange is the new Black times 15", result);
        }

        [TestMethod]
        public void TestFormatWithNull()
        {
            const string formatString = "While this is always {AlwaysNullObject}, this should be {CircularValue.Number}";
            // Everything will be null
            var result = StringObjectFormatter.Format(formatString, new TestData());
            Assert.AreEqual("While this is always null, this should be 0", result);
        }

        [TestMethod]
        public void TestBrokenFormat()
        {
            const string formatString = "{Name} has everything, but {Name}} may be missing something, and {{Name} is a bit forgetful";
            var testObj = new TestData {Name = "James"};
            var result = StringObjectFormatter.Format(formatString, testObj);
            Assert.AreEqual("James has everything, but {Name}} may be missing something, and {{Name} is a bit forgetful", result);
        }

        [TestMethod]
        public void TestBraceEscaping()
        {
            const string formatString = "While {Name} is comfortable, {{John}} is even more comfortable";
            var testObj = new TestData {Name = "James"};
            var result = StringObjectFormatter.Format(formatString, testObj);
            Assert.AreEqual("While James is comfortable, {John} is even more comfortable", result);
        }

        [TestMethod]
        public void TestUnknownFormatStringToken()
        {
            const string formatString = "Nobody except {Name} knows what {Unknown} is...";
            var testObj = new TestData {Name = "Alexis"};
            var result = StringObjectFormatter.Format(formatString, testObj);
            Assert.AreEqual("Nobody except Alexis knows what {Unknown} is...", result);
        }

        [TestMethod]
        public void TestFormattingLambda()
        {
            const string formatString = "Sssss! I am a {Number} year old, {Name} {CircularValue.Name}";
            var testObj = new TestData
            {
                Name = "very slippery and slithery",
                Number = 6,
                CircularValue = new TestData
                {
                    Name = "snake"
                }
            };
            var result = StringObjectFormatter.Format(formatString, testObj, o => o.ToString().Replace(' ', '_'));
            Assert.AreEqual("Sssss! I am a 6 year old, very_slippery_and_slithery snake", result);
        }

        [TestMethod]
        public void TestFormatTokenSurrounding()
        {
            const string formatString = "It's {Name}'s birthday today, and he will be ✨{Number}🎉🎂!";
            var testObj = new TestData
            {
                Name = "Colin",
                Number = 21
            };
            var result = StringObjectFormatter.Format(formatString, testObj);
            Assert.AreEqual("It's Colin's birthday today, and he will be ✨21🎉🎂!", result);
        }
    }
}
