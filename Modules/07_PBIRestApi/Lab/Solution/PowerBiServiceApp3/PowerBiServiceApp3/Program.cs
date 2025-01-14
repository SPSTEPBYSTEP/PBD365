﻿using System;
using Microsoft.Identity.Client;
using Microsoft.PowerBI.Api.V2;
using Microsoft.Rest;

class Program {

  // update the following four constants with the values from your envirionment
  const string appWorkspaceId = "";
  const string clientId = "";
  const string clientSecret = "";
  const string tenantName = "MY_TENANT.onMicrosoft.com";

  // endpoint for tenant-specific authority 
  const string tenantSpecificAuthority = "https://login.microsoftonline.com/" + tenantName;

  // Power BI Service API Root URL
  const string urlPowerBiRestApiRoot = "https://api.powerbi.com/";

static string GetAppOnlyAccessToken() {
  Console.WriteLine("Acquiring app-only access token");

  var appConfidential = ConfidentialClientApplicationBuilder.Create(clientId)
                          .WithClientSecret(clientSecret)
                          .WithAuthority(tenantSpecificAuthority)
                          .Build();

  string[] scopesDefault = new string[] { "https://analysis.windows.net/powerbi/api/.default" };

  var authResult = appConfidential.AcquireTokenForClient(scopesDefault).ExecuteAsync().Result;

  return authResult.AccessToken;
}

static void Main() {
  DisplayAppWorkspaceAssets();
}

  static void DisplayAppWorkspaceAssets() {

    string AccessToken = GetAppOnlyAccessToken();
    var pbiClient = new PowerBIClient(new Uri(urlPowerBiRestApiRoot),
                                      new TokenCredentials(AccessToken, "Bearer"));

    Console.WriteLine();
    Console.WriteLine("Dashboards:");
    var dashboards = pbiClient.Dashboards.GetDashboardsInGroup(appWorkspaceId).Value;
    foreach (var dashboard in dashboards) {
      Console.WriteLine(" - " + dashboard.DisplayName + " [" + dashboard.Id + "]");
    }

    Console.WriteLine();
    Console.WriteLine("Reports:");
    var reports = pbiClient.Reports.GetReportsInGroup(appWorkspaceId).Value;
    foreach (var report in reports) {
      Console.WriteLine(" - " + report.Name + " [" + report.Id + "]");
    }

    Console.WriteLine();
    Console.WriteLine("Datasets:");
    var datasets = pbiClient.Datasets.GetDatasetsInGroup(appWorkspaceId).Value;
    foreach (var dataset in datasets) {
      Console.WriteLine(" - " + dataset.Name + " [" + dataset.Id + "]");
    }

    Console.WriteLine();
  }

}