using System.Collections.Generic;
using System.Linq;
using CleverTapSDK.Native;
using NUnit.Framework;

public class JsonTemplateProducerTest
{
    [Test]
    public void DefineTemplates_NullInput_ThrowsException()
    {
        var producer = new JsonTemplateProducer(null);
        var ex = Assert.Throws<CleverTapTemplateException>(() => producer.DefineTemplates());
        Assert.That(ex.Message, Does.Contain("cannot be null or empty"));
    }

    [Test]
    public void DefineTemplates_InvalidJson_ThrowsException()
    {
        var producer = new JsonTemplateProducer("this is not valid json");
        var ex = Assert.Throws<CleverTapTemplateException>(() => producer.DefineTemplates());
        Assert.That(ex.Message, Does.Contain("Error parsing JSON"));
    }

    [Test]
    public void DefineTemplates_UnknownTemplateType_ThrowsException()
    {
        string json = @"{
                ""TestTemplate"": {
                    ""type"": ""unknown_type"",
                    ""arguments"": {}
                }
            }";

        var producer = new JsonTemplateProducer(json);
        var ex = Assert.Throws<CleverTapTemplateException>(() => producer.DefineTemplates());
        Assert.That(ex.Message, Does.Contain("Unknown template type"));
    }

    [Test]
    public void DefineTemplates_InAppTemplate_WithValidArguments_ReturnsTemplate()
    {
        string json = @"{
                ""TestTemplate"": {
                    ""type"": ""template"",
                    ""arguments"": {
                        ""title"": { ""type"": ""string"", ""value"": ""Welcome"" },
                        ""count"": { ""type"": ""number"", ""value"": 5 },
                        ""flag"": { ""type"": ""boolean"", ""value"": true }
                    }
                }
            }";

        var producer = new JsonTemplateProducer(json);
        var templates = producer.DefineTemplates();

        Assert.AreEqual(1, templates.Count);
        var template = new List<CustomTemplate>(templates)[0];
        Assert.AreEqual("TestTemplate", template.Name);
        Assert.AreEqual(3, template.Arguments.Count);

        Assert.AreEqual("Welcome", template.Arguments.First(a => a.Name == "title").Value);
        Assert.AreEqual(5, template.Arguments.First(a => a.Name == "count").Value);
        Assert.AreEqual(true, template.Arguments.First(a => a.Name == "flag").Value);
    }

    [Test]
    public void DefineTemplates_Function_WithIsVisualTrue_ReturnsTemplate()
    {
        string json = @"{
                ""Function1"": {
                    ""type"": ""function"",
                    ""isVisual"": true,
                    ""arguments"": {
                        ""param1"": { ""type"": ""number"", ""value"": 10 }
                    }
                }
            }";

        var producer = new JsonTemplateProducer(json);
        var templates = producer.DefineTemplates();

        Assert.AreEqual(1, templates.Count);
        var template = new List<CustomTemplate>(templates)[0];
        Assert.AreEqual("Function1", template.Name);
        Assert.IsTrue(template.IsVisual);
    }

    [Test]
    public void DefineTemplates_WithFunctionsAndTemplates_ReturnsTemplates()
    {
        string json = @"{
                ""TestTemplate"": {
                    ""type"": ""template"",
                    ""arguments"": {
                        ""title"": { ""type"": ""string"", ""value"": ""Welcome"" },
                    }
                },
                ""TestFunction"": {
                    ""type"": ""function"",
                    ""isVisual"": true,
                    ""arguments"": {
                    }
                },
                ""TestTemplate2"": {
                    ""type"": ""template"",
                    ""arguments"": {
                        ""title2"": { ""type"": ""string"", ""value"": ""Welcome 2"" },
                    }
                },
                ""TestFunction1"": {
                    ""type"": ""function"",
                    ""isVisual"": false,
                    ""arguments"": {
                        ""title"": { ""type"": ""string"", ""value"": ""Welcome Function"" }
                    }
                }
            }";

        var producer = new JsonTemplateProducer(json);
        var templates = producer.DefineTemplates();

        Assert.AreEqual(4, templates.Count);
    }

    [Test]
    public void DefineTemplates_FileArgument_WithValue_ThrowsException()
    {
        string json = @"{
                ""TestTemplate"": {
                    ""type"": ""template"",
                    ""arguments"": {
                        ""image"": { ""type"": ""file"", ""value"": ""not-empty"" }
                    }
                }
            }";

        var producer = new JsonTemplateProducer(json);
        var ex = Assert.Throws<CleverTapTemplateException>(() => producer.DefineTemplates());
        Assert.That(ex.Message, Does.Contain("File arguments should not specify a value"));
    }

    [Test]
    public void DefineTemplates_ActionArgument_InFunction_ThrowsException()
    {
        string json = @"{
                ""Function1"": {
                    ""type"": ""function"",
                    ""isVisual"": false,
                    ""arguments"": {
                        ""action1"": { ""type"": ""action"", ""value"": """" }
                    }
                }
            }";

        var producer = new JsonTemplateProducer(json);
        var ex = Assert.Throws<CleverTapTemplateException>(() => producer.DefineTemplates());
        Assert.That(ex.Message, Does.Contain("Function templates cannot have action arguments"));
    }

    [Test]
    public void DefineTemplates_NestedDictionary_WorksCorrectly()
    {
        string json = @"{
                ""ComplexTemplate"": {
                    ""type"": ""template"",
                    ""arguments"": {
                        ""settings"": {
                            ""type"": ""object"",
                            ""value"": {
                                ""enabled"": { ""type"": ""boolean"", ""value"": true },
                                ""threshold"": { ""type"": ""number"", ""value"": 1.5 },
                                ""nested"": {
                                    ""type"": ""object"",
                                    ""value"": {
                                        ""label"": { ""type"": ""string"", ""value"": ""Deep"" }
                                    }
                                }
                            }
                        }
                    }
                }
            }";

        var producer = new JsonTemplateProducer(json);
        var templates = producer.DefineTemplates();
        Assert.AreEqual(1, templates.Count);
        var template = new List<CustomTemplate>(templates)[0];
        Assert.AreEqual(3, template.Arguments.Count);
    }

    [Test]
    public void DefineTemplates_UnsupportedNestedType_ThrowsException()
    {
        string json = @"{
                ""InvalidTemplate"": {
                    ""type"": ""template"",
                    ""arguments"": {
                        ""settings"": {
                            ""type"": ""object"",
                            ""value"": {
                                ""invalid"": { ""type"": ""file"", ""value"": """" }
                            }
                        }
                    }
                }
            }";

        var producer = new JsonTemplateProducer(json);
        var ex = Assert.Throws<CleverTapTemplateException>(() => producer.DefineTemplates());
        Assert.That(ex.Message, Does.Contain("Unsupported nested argument type"));
    }
}

