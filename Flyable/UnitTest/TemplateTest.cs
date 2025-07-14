using System.Diagnostics;
using System.Runtime.CompilerServices;
using Flyable.StatusCode;
using Flyable.Utils;
using Flyable.Wraps;
using Microsoft.OpenApi.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Flyable.UnitTest;

[TestClass]
public class TemplateTest
{
    [TestMethod]
    public void TestGetTemplate()
    {
        var template = new TemplateTranslate((ActionType)9, "sunhao", VerifyCodeGenerator.VerifyCode.ToString()).Template;
        Debug.WriteLine(template);
        Debug.WriteLine(template);
        Assert.IsNotNull(template);
    }

    [TestMethod]
    public void TestGetTemplate2()
    {
        const UserLevel level = UserLevel.FeatherStart;
        Assert.IsTrue(level < UserLevel.FeatherSoar);
    }
}