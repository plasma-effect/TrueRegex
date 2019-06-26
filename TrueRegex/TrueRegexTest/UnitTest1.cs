using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrueRegex;
using System.Diagnostics;
using static System.Linq.Enumerable;
using static TrueRegex.Predefined;

namespace TrueRegexTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AtomicTest()
        {
            {
                var expr = new Atomic(char.IsLower);
                Assert.AreEqual(expr.Match("a"), true);
                Assert.AreEqual(expr.Match("A"), false);
                Assert.AreEqual(expr.Match(""), false);
            }
            {
                var expr = Chars.Create('1', '2', '3', '4', '5');
                Assert.AreEqual(expr.Match("1"), true);
                Assert.AreEqual(expr.Match("2"), true);
                Assert.AreEqual(expr.Match("3"), true);
                Assert.AreEqual(expr.Match("4"), true);
                Assert.AreEqual(expr.Match("5"), true);
                Assert.AreEqual(expr.Match("6"), false);
                Assert.AreEqual(expr.Match(""), false);
            }
        }

        [TestMethod]
        public void RepeatTest()
        {
            var baseExpr = Chars.Create('a');
            {
                var expr = ~baseExpr;
                Assert.AreEqual(expr.Match("aaa"), true);
                Assert.AreEqual(expr.Match("aab"), false);
                Assert.AreEqual(expr.Match(""), true);
            }
            {
                var expr = +baseExpr;
                Assert.AreEqual(expr.Match("aaa"), true);
                Assert.AreEqual(expr.Match("aab"), false);
                Assert.AreEqual(expr.Match(""), false);
            }
        }

        [TestMethod]
        public void SequenceTest()
        {
            {
                var expr = Chars.Create('a', 'b') + Chars.Create('a', 'b');
                Assert.AreEqual(expr.Match("aa"), true);
                Assert.AreEqual(expr.Match("ab"), true);
                Assert.AreEqual(expr.Match("ba"), true);
                Assert.AreEqual(expr.Match("bb"), true);
                Assert.AreEqual(expr.Match("aaa"), false);
                Assert.AreEqual(expr.Match("a"), false);
                Assert.AreEqual(expr.Match("ac"), false);
                Assert.AreEqual(expr.Match(""), false);
            }
        }
        
        [TestMethod]
        public void OptionalTest()
        {
            {
                var expr = -Chars.Create('a');
                Assert.AreEqual(expr.Match("a"), true);
                Assert.AreEqual(expr.Match("b"), false);
                Assert.AreEqual(expr.Match(""), true);
            }
        }

        [TestMethod]
        public void SelectTest()
        {
            {
                var expr =Chars.Create('a') | Chars.Create('b') | Chars.Create('c');
                Assert.AreEqual(expr.Match("a"), true);
                Assert.AreEqual(expr.Match("b"), true);
                Assert.AreEqual(expr.Match("c"), true);
                Assert.AreEqual(expr.Match("d"), false);
                Assert.AreEqual(expr.Match("ab"), false);
                Assert.AreEqual(expr.Match(""), false);
            }
        }

        [TestMethod]
        public void NotTest()
        {
            {
                var expr = !Number;
                Assert.AreEqual(expr.Match("123"), false);
                Assert.AreEqual(expr.Match("12a"), true);
                Assert.AreEqual(expr.Match("a12"), true);
                Assert.AreEqual(expr.Match(""), true);
            }
        }

        [TestMethod]
        public void PredefinedTest()
        {
            {
                var expr = String.Create("abc");
                Assert.AreEqual(expr.Match("abc"), true);
                Assert.AreEqual(expr.Match("ab"), false);
                Assert.AreEqual(expr.Match("abd"), false);
                Assert.AreEqual(expr.Match(""), false);
            }
            {
                var expr = String.Create("");
                Assert.AreEqual(expr.Match("a"), false);
                Assert.AreEqual(expr.Match(""), true);
            }
            {
                var expr = Number;
                Assert.AreEqual(expr.Match("123"), true);
                Assert.AreEqual(expr.Match("12a"), false);
                Assert.AreEqual(expr.Match(""), false);
            }
            {
                var expr = Name;
                Assert.AreEqual(expr.Match("123"), true);
                Assert.AreEqual(expr.Match("abc"), true);
                Assert.AreEqual(expr.Match("ab1"), true);
                Assert.AreEqual(expr.Match("1ab"), true);
                Assert.AreEqual(expr.Match("1,2"), false);
                Assert.AreEqual(expr.Match(""), false);
            }
        }

        [TestMethod]
        public void FirstAndLastTest()
        {
            {
                var expr = +String.Create("a");
                Assert.AreEqual(expr.FirstMatch("aaa"), 1);
                Assert.AreEqual(expr.LastMatch("aaa"), 3);
                Assert.AreEqual(expr.LastMatch("aab"), 2);
                Assert.AreEqual(expr.FirstMatch("baa"), null);
                Assert.AreEqual(expr.LastMatch("baa"), null);
            }
        }
    }
}
