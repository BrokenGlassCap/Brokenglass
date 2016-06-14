using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrokenGlassTests
{
    internal class AssertUtils
    {
        public static void IEnumerableAreEqual(IEnumerable<Object> expectedObject, IEnumerable<Object> actualObject)
        {
            var expectedEnumerateor = expectedObject.GetEnumerator();
            var actualEnumerator = actualObject.GetEnumerator();
            while (expectedEnumerateor.MoveNext() && actualEnumerator.MoveNext())
            {
                Assert.AreEqual(expectedEnumerateor.Current, actualEnumerator.Current);
            }
        }
    }
}
