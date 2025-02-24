using NUnit.Framework;
using UnityEngine;

public class SampleRuntimeTest
{
    [Test]
    public void Test_Example()
    {
        int a = 2;
        int b = 3;
        Assert.AreEqual(5, a + b);
    }
}