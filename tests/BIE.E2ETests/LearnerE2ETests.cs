using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace BIE.E2ETests;

[TestClass]
public class LearnerE2ETests : BrowserTestBase
{
    [TestMethod]
    public void TouristAt30DaysRemainsApprovedAfterReload()
    {
        Fill("Sam Browser", "Tourismus", "30");
        Driver.FindElement(By.Id("submit")).Click();

        // Auf einen Zustand warten statt Thread.Sleep: stabil und so schnell wie möglich.
        Wait.Until(d => d.FindElement(By.Id("result")).Displayed);
        var result = Driver.FindElement(By.Id("result"));
        StringAssert.Contains(result.Text, "GENEHMIGT");

        var caseId = Driver.FindElement(By.Id("result")).GetDomAttribute("data-application-id");
        Driver.Navigate().Refresh();
        var row = Wait.Until(d => d.FindElements(By.CssSelector("#application-list tr")).FirstOrDefault(r => r.GetDomAttribute("data-id") == caseId));
        var cells = row.FindElements(By.TagName("td"));

        // R9: Nach Neuladen ist der gespeicherte Fall weiterhin im Register sichtbar.
        Assert.AreEqual("Sam Browser", cells[1].Text);
        Assert.AreEqual("Orion", cells[2].Text);
        Assert.AreEqual("30 Tage", cells[3].Text);
        Assert.AreEqual("GENEHMIGT", cells[4].Text);
    }
}
