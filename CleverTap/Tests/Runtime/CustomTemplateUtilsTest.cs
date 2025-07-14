using System.Collections.Generic;
using CleverTapSDK.Native;
using NUnit.Framework;

public class CustomTemplateUtilsTest
{
    [Test]
    public void SyncPayload_ShouldReturnCorrectPayloadStructureAndOrder()
    {
        var builder = new InAppTemplateBuilder();
        builder.SetName("TestTemplate 1");

        builder.AddArgument("b", TemplateArgumentType.Bool, false);
        builder.AddArgument("c", TemplateArgumentType.String, "1 string");
        builder.AddArgument("d", TemplateArgumentType.String, "2 string");
        builder.AddDictionaryArgument("e", new Dictionary<string, object>()
        {
            { "h", 7 },
            { "f", new Dictionary<string, object>()
                {
                    { "c", 4 },
                    { "e", "6 string" },
                    { "d", 5.9599 },
                }
            }
        });

        builder.AddArgument("l", TemplateArgumentType.String, "9 string");
        builder.AddArgument("k", TemplateArgumentType.Bool, true);
        builder.AddArgument("e.w", TemplateArgumentType.Number, 8);
        builder.AddArgument("e.f.a", TemplateArgumentType.Number, 3);
        builder.AddDictionaryArgument("a", new Dictionary<string, object>()
        {
            { "n", "12 string" },
            { "m", "11 string" }
        });

        var template = builder.Build();

        var builder2 = new InAppTemplateBuilder();
        builder2.SetName("TestTemplate 2");

        builder2.AddArgument("b", TemplateArgumentType.Bool, false);
        builder2.AddArgument("c", TemplateArgumentType.String, "1 string");
        builder2.AddArgument("a.d", TemplateArgumentType.Number, 5);
        builder2.AddArgument("a.c.a", TemplateArgumentType.Number, 4.1);
        builder2.AddDictionaryArgument("a", new Dictionary<string, object>()
        {
            { "b", "3 string" },
            { "a", "2 string" }
        });

        var template2 = builder2.Build();

        var templates = new HashSet<CustomTemplate> { template, template2 };
        var payload = CustomTemplateUtils.GetSyncTemplatesPayload(templates);

        Assert.AreEqual("templatePayload", payload["type"]);
        var definitions = (Dictionary<string, object>)payload["definitions"];
        Assert.AreEqual(2, definitions.Count);

        var expectedPayload = new Dictionary<string, object>
        {
            ["type"] = "templatePayload",
            ["definitions"] = new Dictionary<string, object>
            {
                ["TestTemplate 1"] = new Dictionary<string, object>
                {
                    ["type"] = UnityNativeConstants.CustomTemplates.TEMPLATE_TYPE,
                    ["vars"] = new Dictionary<string, object>
                    {
                        ["a.m"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = "11 string",
                            ["order"] = 11,
                            ["type"] = "string"
                        },
                        ["a.n"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = "12 string",
                            ["order"] = 12,
                            ["type"] = "string"
                        },
                        ["b"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = false,
                            ["order"] = 0,
                            ["type"] = "boolean"
                        },
                        ["c"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = "1 string",
                            ["order"] = 1,
                            ["type"] = "string"
                        },
                        ["d"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = "2 string",
                            ["order"] = 2,
                            ["type"] = "string"
                        },
                        ["e.f.a"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = 3,
                            ["order"] = 3,
                            ["type"] = "number"
                        },
                        ["e.f.c"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = 4,
                            ["order"] = 4,
                            ["type"] = "number"
                        },
                        ["e.f.d"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = 5.9599,
                            ["order"] = 5,
                            ["type"] = "number"
                        },
                        ["e.f.e"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = "6 string",
                            ["order"] = 6,
                            ["type"] = "string"
                        },
                        ["e.h"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = 7,
                            ["order"] = 7,
                            ["type"] = "number"
                        },
                        ["e.w"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = 8,
                            ["order"] = 8,
                            ["type"] = "number"
                        },
                        ["k"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = true,
                            ["order"] = 10,
                            ["type"] = "boolean"
                        },
                        ["l"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = "9 string",
                            ["order"] = 9,
                            ["type"] = "string"
                        }
                    }
                },
                ["TestTemplate 2"] = new Dictionary<string, object>
                {
                    ["type"] = UnityNativeConstants.CustomTemplates.TEMPLATE_TYPE,
                    ["vars"] = new Dictionary<string, object>
                    {
                        ["b"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = false,
                            ["order"] = 0,
                            ["type"] = "boolean"
                        },
                        ["c"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = "1 string",
                            ["order"] = 1,
                            ["type"] = "string"
                        },
                        ["a.a"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = "2 string",
                            ["order"] = 2,
                            ["type"] = "string"
                        },
                        ["a.b"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = "3 string",
                            ["order"] = 3,
                            ["type"] = "string"
                        },
                        ["a.c.a"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = 4.1,
                            ["order"] = 4,
                            ["type"] = "number"
                        },
                        ["a.d"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = 5,
                            ["order"] = 5,
                            ["type"] = "number"
                        }
                    }
                }
            }
        };

        CollectionAssert.AreEqual(expectedPayload, payload);
    }

    [Test]
    public void SyncPayload_WithFunctions_ShouldReturnCorrectPayloadStructureAndOrder()
    {
        var builder = new AppFunctionBuilder(true);
        builder.SetName("TestFunction 1");
        builder.AddArgument("b", TemplateArgumentType.Number, 0);
        builder.AddArgument("a", TemplateArgumentType.String, "1 string");
        var function1 = builder.Build();

        var builder2 = new AppFunctionBuilder(false);
        builder2.SetName("TestFunction 2");
        builder2.AddArgument("b", TemplateArgumentType.Number, 1);
        builder2.AddDictionaryArgument("a", new Dictionary<string, object>()
        {
            { "c", true },
            { "d", new Dictionary<string, object>()
                {
                    { "f", "3 string" },
                    { "e", -2.9599 },
                }
            }
        });
        var function2 = builder2.Build();

        var templates = new HashSet<CustomTemplate> { function1, function2 };
        var payload = CustomTemplateUtils.GetSyncTemplatesPayload(templates);

        var expectedPayload = new Dictionary<string, object>
        {
            ["type"] = "templatePayload",
            ["definitions"] = new Dictionary<string, object>
            {
                ["TestFunction 1"] = new Dictionary<string, object>
                {
                    ["type"] = UnityNativeConstants.CustomTemplates.FUNCTION_TYPE,
                    ["vars"] = new Dictionary<string, object>
                    {
                        ["b"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = 0,
                            ["order"] = 0,
                            ["type"] = "number"
                        },
                        ["a"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = "1 string",
                            ["order"] = 1,
                            ["type"] = "string"
                        }
                    }
                },
                ["TestFunction 2"] = new Dictionary<string, object>
                {
                    ["type"] = UnityNativeConstants.CustomTemplates.FUNCTION_TYPE,
                    ["vars"] = new Dictionary<string, object>
                    {
                        ["b"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = 1,
                            ["order"] = 0,
                            ["type"] = "number"
                        },
                        ["a.c"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = true,
                            ["order"] = 1,
                            ["type"] = "boolean"
                        },
                        ["a.d.e"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = -2.9599,
                            ["order"] = 2,
                            ["type"] = "number"
                        },
                        ["a.d.f"] = new Dictionary<string, object>
                        {
                            ["defaultValue"] = "3 string",
                            ["order"] = 3,
                            ["type"] = "string"
                        },
                    }
                },             
            }
        };

        CollectionAssert.AreEqual(expectedPayload, payload);
    }

    [Test]
    public void SyncPayload_WithNoTemplates_ReturnsEmptyDefinitions()
    {
        var templates = new HashSet<CustomTemplate>();
        var payload = CustomTemplateUtils.GetSyncTemplatesPayload(templates);

        Assert.AreEqual("templatePayload", payload["type"]);
        var definitions = (Dictionary<string, object>)payload["definitions"];
        Assert.AreEqual(0, definitions.Count);
    }
}