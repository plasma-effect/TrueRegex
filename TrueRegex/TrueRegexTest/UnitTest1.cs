using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrueRegex;
using TrueRegex.Predefind;
using System.Diagnostics;
using static System.Linq.Enumerable;

namespace TrueRegexTest
{
    [TestClass]
    public class UnitTest1
    {
        private Regex Create(Expression expr)
        {
            var regex = new Regex(expr);
            //Debug.WriteLine(regex);
            return regex;
        }

        [TestMethod]
        public void AtomicTest()
        {
            {
                var regex = Create(new Atomic(char.IsLower));
                Assert.AreEqual(regex.Match("a"), true);
                Assert.AreEqual(regex.Match("A"), false);
                Assert.AreEqual(regex.Match(""), false);
            }
            {
                var regex = Create(new Chars('1', '2', '3', '4', '5'));
                Assert.AreEqual(regex.Match("1"), true);
                Assert.AreEqual(regex.Match("2"), true);
                Assert.AreEqual(regex.Match("3"), true);
                Assert.AreEqual(regex.Match("4"), true);
                Assert.AreEqual(regex.Match("5"), true);
                Assert.AreEqual(regex.Match("6"), false);
                Assert.AreEqual(regex.Match(""), false);
            }
        }

        [TestMethod]
        public void RepeatTest()
        {
            var baseExpr = new Chars('a');
            {
                var regex = Create(~baseExpr);
                Assert.AreEqual(regex.Match("aaa"), true);
                Assert.AreEqual(regex.Match("aab"), false);
                Assert.AreEqual(regex.Match(""), true);
            }
            {
                var regex = Create(+baseExpr);
                Assert.AreEqual(regex.Match("aaa"), true);
                Assert.AreEqual(regex.Match("aab"), false);
                Assert.AreEqual(regex.Match(""), false);
            }
        }

        [TestMethod]
        public void SequenceTest()
        {
            {
                var regex = Create(Chars.Create('a', 'b') + Chars.Create('a', 'b'));
                Assert.AreEqual(regex.Match("aa"), true);
                Assert.AreEqual(regex.Match("ab"), true);
                Assert.AreEqual(regex.Match("ba"), true);
                Assert.AreEqual(regex.Match("bb"), true);
                Assert.AreEqual(regex.Match("aaa"), false);
                Assert.AreEqual(regex.Match("a"), false);
                Assert.AreEqual(regex.Match("ac"), false);
                Assert.AreEqual(regex.Match(""), false);
            }
        }
        
        [TestMethod]
        public void OptionalTest()
        {
            {
                var regex = Create(-Chars.Create('a'));
                Assert.AreEqual(regex.Match("a"), true);
                Assert.AreEqual(regex.Match("b"), false);
                Assert.AreEqual(regex.Match(""), true);
            }
        }

        [TestMethod]
        public void SelectTest()
        {
            {
                var regex = Create(Chars.Create('a') | Chars.Create('b') | Chars.Create('c'));
                Assert.AreEqual(regex.Match("a"), true);
                Assert.AreEqual(regex.Match("b"), true);
                Assert.AreEqual(regex.Match("c"), true);
                Assert.AreEqual(regex.Match("d"), false);
                Assert.AreEqual(regex.Match("ab"), false);
                Assert.AreEqual(regex.Match(""), false);
            }
        }

        [TestMethod]
        public void PredefinedTest()
        {
            {
                var regex = Create(String.Create("abc"));
                Assert.AreEqual(regex.Match("abc"), true);
                Assert.AreEqual(regex.Match("ab"), false);
                Assert.AreEqual(regex.Match("abd"), false);
                Assert.AreEqual(regex.Match(""), false);
            }
            {
                var regex = Create(new Number());
                Assert.AreEqual(regex.Match("123"), true);
                Assert.AreEqual(regex.Match("12a"), false);
                Assert.AreEqual(regex.Match(""), false);
            }
            {
                var regex = Create(new Name());
                Assert.AreEqual(regex.Match("123"), true);
                Assert.AreEqual(regex.Match("abc"), true);
                Assert.AreEqual(regex.Match("ab1"), true);
                Assert.AreEqual(regex.Match("1ab"), true);
                Assert.AreEqual(regex.Match("1,2"), false);
                Assert.AreEqual(regex.Match(""), false);
            }
        }
    }
}
