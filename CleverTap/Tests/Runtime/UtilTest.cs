using System.Collections.Generic;
using CleverTapSDK.Utilities;
using NUnit.Framework;

public class UtilTest
{
    [Test]
    public void FillInValues_ShouldCopyValuesFromSourceToDestination()
    {
        var source = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
        var destination = new Dictionary<string, int>();

        Util.FillInValues(source, destination);

        Assert.AreEqual(2, destination.Count);
        Assert.AreEqual(1, destination["a"]);
        Assert.AreEqual(2, destination["b"]);
    }

    [Test]
    public void FillInValues_ShouldHandleNestedDictionaries()
    {
        var source = new Dictionary<string, object>
        {
            { "a", 1 },
            { "b", new Dictionary<string, int> { { "c", 2 } } }
        };
        var destination = new Dictionary<string, object>();

        Util.FillInValues(source, destination);

        Assert.AreEqual(2, destination.Count);
        Assert.AreEqual(1, destination["a"]);
        Assert.IsInstanceOf<Dictionary<string, int>>(destination["b"]);
        Assert.AreEqual(2, ((Dictionary<string, int>)destination["b"])["c"]);

        // Ensure not same reference
        Assert.AreNotSame(source, destination);
        Assert.AreNotSame(source["b"], destination["b"]);
    }

    [Test]
    public void FillInValues_ShouldNotCopyCollections_WhenShouldCopyCollectionIsFalse()
    {
        var source = new Dictionary<string, object>
        {
            { "a", 1 },
            { "b", new Dictionary<string, int> { { "c", 2 } } }
        };
        var destination = new Dictionary<string, object>();

        Util.FillInValues(source, destination, shouldCopyCollection: false);

        Assert.AreEqual(2, destination.Count);
        Assert.AreEqual(1, destination["a"]);
        // Ensure the same reference is used
        Assert.AreSame(source["b"], destination["b"]);
    }

    [Test]
    public void FillInValues_ShouldNotModifyDestination_WhenSourceIsNull()
    {
        var destination = new Dictionary<string, int> { { "a", 1 } };

        Util.FillInValues(null, destination);

        Assert.AreEqual(1, destination.Count);
        Assert.AreEqual(1, destination["a"]);
    }

    [Test]
    public void FillInValues_ShouldNotModifyDestination_WhenSourceAndDestinationAreEqual()
    {
        var source = new Dictionary<string, int> { { "a", 1 } };
        var destination = new Dictionary<string, int> { { "a", 1 } };

        Util.FillInValues(source, destination);

        Assert.AreEqual(1, destination.Count);
        Assert.AreEqual(1, destination["a"]);
    }

    [Test]
    public void CreateNewDictionary_ShouldCreateNewDictionaryOfSameType()
    {
        var originalDictionary = new Dictionary<string, int>();

        var newDictionary = Util.CreateNewDictionary(originalDictionary);

        Assert.IsNotNull(newDictionary);
        Assert.IsInstanceOf<Dictionary<string, int>>(newDictionary);
        Assert.AreNotSame(originalDictionary, newDictionary);
    }

    [Test]
    public void CreateNewDictionary_ShouldReturnNull_ForNonDictionaryTypes()
    {
        var nonDictionary = new List<int>();

        var result = Util.CreateNewDictionary(nonDictionary);

        Assert.IsNull(result);
    }

    [Test]
    public void GetKeyType_ShouldReturnCorrectKeyType()
    {
        var dictionary = new Dictionary<string, int>();

        var keyType = dictionary.GetKeyType();

        Assert.AreEqual(typeof(string), keyType);
    }

    [Test]
    public void GetValueType_ShouldReturnCorrectValueType()
    {
        var dictionary = new Dictionary<string, int>();

        var valueType = dictionary.GetValueType();

        Assert.AreEqual(typeof(int), valueType);
    }

    [Test]
    public void TryGetValue_ShouldReturnTrueAndValue_WhenKeyExists()
    {
        var dictionary = new Dictionary<string, int> { { "a", 1 } };

        bool result = dictionary.TryGetValue("a", out var value);

        Assert.IsTrue(result);
        Assert.AreEqual(1, value);
    }

    [Test]
    public void TryGetValue_ShouldReturnFalse_WhenKeyDoesNotExist()
    {
        var dictionary = new Dictionary<string, int>();
        bool result = dictionary.TryGetValue("a", out var value);

        Assert.IsFalse(result);
        Assert.AreEqual(0, value);

        var dictionaryString = new Dictionary<string, string>();
        bool resultString = dictionaryString.TryGetValue("a", out var valueString);

        Assert.IsFalse(resultString);
        Assert.IsNull(valueString);
    }
}