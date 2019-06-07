using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrueRegex;
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
            Debug.WriteLine(regex);
            return regex;
        }

        [TestMethod]
        public void AllTest()
        {
            AtomicTest();
        }

        [TestMethod]
        public void AtomicTest()
        {
            {
                var regex = Create(new Atomic(char.IsLower));
                Assert.AreEqual(regex.Match("a"), true);
                Assert.AreEqual(regex.Match("A"), false);
            }
            {
                var regex = Create(new Chars('1', '2', '3', '4', '5'));
                Assert.AreEqual(regex.Match("1"), true);
                Assert.AreEqual(regex.Match("2"), true);
                Assert.AreEqual(regex.Match("3"), true);
                Assert.AreEqual(regex.Match("4"), true);
                Assert.AreEqual(regex.Match("5"), true);
                Assert.AreEqual(regex.Match("6"), false);
            }
        }

        [TestMethod]
        public void RepeatTest()
        {
            {
                var regex = Create(new ZeroRepeat(new Chars('a')));
                Assert.AreEqual(regex.Match("aaa"), true);
                Assert.AreEqual(regex.Match("aab"), false);
                Assert.AreEqual(regex.Match(""), true);
            }
            {
                var regex = Create(new OneRepeat(new Chars('a')));
                Assert.AreEqual(regex.Match("aaa"), true);
                Assert.AreEqual(regex.Match("aab"), false);
                Assert.AreEqual(regex.Match(""), false);
            }
        }
    }
}
