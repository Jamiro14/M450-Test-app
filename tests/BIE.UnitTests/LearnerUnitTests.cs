using BIE.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BIE.UnitTests;

[TestClass]
public class LearnerUnitTests
{
    [TestMethod]
    public void TouristAt30DaysIsApproved()
    {
        var entry = new Entry("Sam Test", "Orion", "Tourismus", 30, false, false, false, true);

        // Act
        var verdict = EntryPolicy.Evaluate(entry);

        // Assert: R3 – genau 30 Tage Tourismus sind noch erlaubt (Grenzwert).
        Assert.AreEqual("approved", verdict.Status);
    }
}
