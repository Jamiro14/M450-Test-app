using System.Net;
using System.Net.Http.Json;
using BIE.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bie.IntegrationTests;

[TestClass]
public class LearnerIntegrationTests
{
    [TestMethod]
    public async Task TouristAt30DaysIsSavedAsApproved()
    {
        using var app = new BieFactory();
        using var client = app.CreateClient();
        var entry = new Entry("Sam Test", "Orion", "Tourismus", 30, false, false, false, true);
        var response = await client.PostAsJsonAsync("/api/cases", entry);
        var records = await client.GetFromJsonAsync<List<Case>>("/api/cases");

        // R7: Ein gültiger Fall wird mit HTTP 201 gespeichert.
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        // R8: Der gespeicherte Fall lässt sich wieder auslesen.
        Assert.IsNotNull(records);
        Assert.AreEqual(1, records.Count);
        var saved = records[0];
        Assert.AreEqual(entry, saved.Applicant);
        Assert.IsTrue(saved.Id > 0);

        // R3: 30 Tage Tourismus ergeben "approved".
        Assert.AreEqual("approved", saved.Decision.Status);
    }
}
