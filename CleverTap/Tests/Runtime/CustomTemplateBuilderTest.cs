using System.Collections.Generic;
using CleverTapSDK.Native;
using NUnit.Framework;
using System.Linq;
using static CleverTapSDK.Native.UnityNativeConstants;

public class CustomTemplateBuilderTest
{
    #region InAppTemplateBuilder Tests

    [Test]
    public void SetName_ValidName_SetsSuccessfully()
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");

        var template = builder.Build();

        Assert.AreEqual("TestTemplate", template.Name);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void SetName_InvalidName_ThrowsException(string invalidName)
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        var ex = Assert.Throws<CleverTapTemplateException>(() => builder.SetName(invalidName));
        StringAssert.Contains("Template Name cannot be null", ex.Message);
    }

    [Test]
    public void SetName_AlreadySet_ThrowsException()
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");

        var ex = Assert.Throws<CleverTapTemplateException>(() => builder.SetName("TestTemplate"));
        StringAssert.Contains("Template Name already set", ex.Message);
    }

    [TestCase("validName")]
    [TestCase("valid Name")]
    [TestCase("valid.Name")]
    [TestCase("valid.two.Name")]
    public void AddArgument_ValidArgument_AddsSuccessfully(string validName)
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");
        builder.AddArgument(validName, TemplateArgumentType.String, "value");
        var template = builder.Build();

        Assert.AreEqual(1, template.Arguments.Count);
        Assert.AreEqual(validName, template.Arguments[0].Name);
    }

    [Test]
    public void AddArgument_MaintainsOrder()
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");
        builder.AddArgument("string", TemplateArgumentType.String, "value");
        builder.AddArgument("number", TemplateArgumentType.Number, 1);
        builder.AddArgument("bool", TemplateArgumentType.Bool, true);
        var template = builder.Build();

        Assert.AreEqual(3, template.Arguments.Count);
        Assert.AreEqual("string", template.Arguments[0].Name);
        Assert.AreEqual("number", template.Arguments[1].Name);
        Assert.AreEqual("bool", template.Arguments[2].Name);
    }

    [Test]
    public void AddArgument_AddsWithCorrectValue()
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");
        builder.AddArgument("string", TemplateArgumentType.String, "some, string.");
        builder.AddArgument("number", TemplateArgumentType.Number, 1);
        builder.AddArgument("number1", TemplateArgumentType.Number, 999999999.95f);
        builder.AddArgument("number2", TemplateArgumentType.Number, 55d);
        builder.AddArgument("bool", TemplateArgumentType.Bool, true);
        builder.AddArgument("bool2", TemplateArgumentType.Bool, false);
        var template = builder.Build();

        Assert.AreEqual("some, string.", template.Arguments.First(a => a.Name == "string").Value);
        Assert.AreEqual(1, template.Arguments.First(a => a.Name == "number").Value);
        Assert.AreEqual(999999999.95f, template.Arguments.First(a => a.Name == "number1").Value);
        Assert.AreEqual(55d, template.Arguments.First(a => a.Name == "number2").Value);
        Assert.AreEqual(true, template.Arguments.First(a => a.Name == "bool").Value);
        Assert.AreEqual(false, template.Arguments.First(a => a.Name == "bool2").Value);
    }

    [Test]
    public void AddArgument_DuplicateName_ThrowsException()
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");

        builder.AddArgument("arg1", TemplateArgumentType.String, "value");

        var ex = Assert.Throws<CleverTapTemplateException>(() =>
            builder.AddArgument("arg1", TemplateArgumentType.String, "value2"));

        StringAssert.Contains("already added", ex.Message);
    }

    [Test]
    public void AddArgument_NameWithDotStartOrEnd_ThrowsException()
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");

        var ex1 = Assert.Throws<CleverTapTemplateException>(() =>
            builder.AddArgument(".invalid", TemplateArgumentType.String, "value"));
        var ex2 = Assert.Throws<CleverTapTemplateException>(() =>
            builder.AddArgument("invalid.", TemplateArgumentType.String, "value"));

        StringAssert.Contains("cannot start or end", ex1.Message);
        StringAssert.Contains("cannot start or end", ex2.Message);
    }

    [Test]
    public void AddDictionaryArgument_ValidNestedDictionary_AddsAllArguments()
    {
        var dict = new Dictionary<string, object>
        {
            { "key1", "value1" },
            { "key2", 42 },
            { "key3", true },
            { "nested", new Dictionary<string, object> { { "innerKey", 3.14 } } }
        };

        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");

        builder.AddDictionaryArgument("arg", dict);
        var template = builder.Build();

        Assert.AreEqual(4, template.Arguments.Count);
        Assert.IsTrue(template.Arguments.Exists(a => a.Name == "arg.key1"));
        Assert.IsTrue(template.Arguments.Exists(a => a.Name == "arg.key2"));
        Assert.IsTrue(template.Arguments.Exists(a => a.Name == "arg.key3"));
        Assert.IsTrue(template.Arguments.Exists(a => a.Name == "arg.nested.innerKey"));
    }

    [Test]
    public void AddDictionaryArgument_NullOrEmptyDictionary_ThrowsException()
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");

        var ex1 = Assert.Throws<CleverTapTemplateException>(() => builder.AddDictionaryArgument("arg", null));
        var ex2 = Assert.Throws<CleverTapTemplateException>(() => builder.AddDictionaryArgument("arg", new Dictionary<string, object>()));

        StringAssert.Contains("cannot be null or empty", ex1.Message);
        StringAssert.Contains("cannot be null or empty", ex2.Message);
    }

    [Test]
    public void AddArgument_NameConflictsWithParent_ThrowsException()
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");

        builder.AddArgument("parent.child", TemplateArgumentType.String, "value");

        var ex = Assert.Throws<CleverTapTemplateException>(() =>
            builder.AddArgument("parent", TemplateArgumentType.String, "value2"));

        StringAssert.Contains("already added", ex.Message);
    }

    [Test]
    public void AddArgument_NameWithParent_AddedSuccessfully()
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");

        builder.AddArgument("parent.child1", TemplateArgumentType.String, "value");
        builder.AddArgument("parent.child2", TemplateArgumentType.String, "value");

        var dict = new Dictionary<string, object>
        {
            { "child3", true },
            { "child4", 10 }
        };
        builder.AddDictionaryArgument("parent", dict);

        builder.AddArgument("parent.child5.child1", TemplateArgumentType.String, "value");

        var template = builder.Build();
        Assert.IsTrue(template.Arguments.Exists(a => a.Name == "parent.child1"));
        Assert.IsTrue(template.Arguments.Exists(a => a.Name == "parent.child2"));
        Assert.IsTrue(template.Arguments.Exists(a => a.Name == "parent.child3"));
        Assert.IsTrue(template.Arguments.Exists(a => a.Name == "parent.child4"));
        Assert.IsTrue(template.Arguments.Exists(a => a.Name == "parent.child5.child1"));
    }

    [Test]
    public void AddArgument_DuplicateDictionaryArgument_ThrowsException()
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");

        var dict1 = new Dictionary<string, object>
        {
            { "a", 0 }
        };
        builder.AddDictionaryArgument("parent", dict1);

        var dict2 = new Dictionary<string, object>
        {
            { "b", 0 }
        };
        builder.AddDictionaryArgument("parent", dict2);

        var ex = Assert.Throws<CleverTapTemplateException>(() =>
            builder.AddDictionaryArgument("parent", dict1));

        StringAssert.Contains("already added", ex.Message);
    }

    [Test]
    public void AddFileArgument_AddsFileTypeArgument()
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");

        builder.AddFileArgument("fileKey");
        var template = builder.Build();

        Assert.AreEqual(1, template.Arguments.Count);
        Assert.AreEqual("fileKey", template.Arguments[0].Name);
        Assert.AreEqual(TemplateArgumentType.File, template.Arguments[0].Type);
    }

    [Test]
    public void AddFileArgument_AddsActionTypeArgument()
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");

        builder.AddActionArgument("Open action");
        var template = builder.Build();

        Assert.AreEqual(1, template.Arguments.Count);
        Assert.AreEqual("Open action", template.Arguments[0].Name);
        Assert.AreEqual(TemplateArgumentType.Action, template.Arguments[0].Type);
    }

    [Test]
    public void Build_WithoutName_ThrowsException()
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        var ex = Assert.Throws<CleverTapTemplateException>(() => builder.Build());
        StringAssert.Contains("CustomTemplate must have a name", ex.Message);
    }

    [Test]
    public void Build_InAppTemplateBuilder_CreatesCorrectTemplate()
    {
        InAppTemplateBuilder builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate");
        builder.AddArgument("arg", TemplateArgumentType.String, "test");

        var template = builder.Build();

        Assert.AreEqual("TestTemplate", template.Name);
        Assert.AreEqual(CustomTemplates.TEMPLATE_TYPE, template.TemplateType);
        Assert.IsTrue(template.IsVisual);
        Assert.AreEqual(1, template.Arguments.Count);
    }

    #endregion

    #region AppFunctionBuilder Tests

    [Test]
    public void AppFunctionBuilder_WithVisualFlag_SetsIsVisual()
    {
        var builder = new AppFunctionBuilder(true);
        builder.SetName("VisualFunction");
        var template = builder.Build();

        Assert.IsTrue(template.IsVisual);
        Assert.AreEqual(CustomTemplates.FUNCTION_TYPE, template.TemplateType);
    }

    [Test]
    public void AppFunctionBuilder_WithNonVisualFlag_SetsIsVisualFalse()
    {
        var builder = new AppFunctionBuilder(false);
        builder.SetName("NonVisualFunction");
        var template = builder.Build();

        Assert.IsFalse(template.IsVisual);
    }

    [Test]
    public void Build_AppFunctionBuilder_WithArguments_ReturnsCorrectTemplate()
    {
        var builder = new AppFunctionBuilder(true);
        builder.SetName("FunctionWithArgs");
        builder.AddArgument("arg1", TemplateArgumentType.Number, 123);
        builder.AddArgument("arg2", TemplateArgumentType.Bool, true);

        var template = builder.Build();

        Assert.AreEqual("FunctionWithArgs", template.Name);
        Assert.AreEqual(2, template.Arguments.Count);
        Assert.AreEqual(CustomTemplates.FUNCTION_TYPE, template.TemplateType);
        Assert.IsTrue(template.IsVisual);
    }

    [Test]
    public void AddDictionaryArgument_ToAppFunctionBuilder_WorksCorrectly()
    {
        var builder = new AppFunctionBuilder(true);
        builder.SetName("FunctionWithDict");

        var dict = new Dictionary<string, object>
        {
            { "flag", true },
            { "count", 10 }
        };

        builder.AddDictionaryArgument("settings", dict);
        var template = builder.Build();

        Assert.AreEqual(2, template.Arguments.Count);
        Assert.IsTrue(template.Arguments.Exists(a => a.Name == "settings.flag"));
        Assert.IsTrue(template.Arguments.Exists(a => a.Name == "settings.count"));
    }

    #endregion
}
