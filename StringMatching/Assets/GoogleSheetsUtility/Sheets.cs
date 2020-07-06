using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
//using UnityEngine;

public class Sheets
{
    private static string[] scopes = { SheetsService.Scope.Spreadsheets }; // Change this if you're accessing Drive or Docs
    private static string applicationName = "Advanced Tools Scores";
    private static string spreadsheetId = "190FxHv17blSqYKPJbDBVcaQIwmTg-4AUrzuhphR5j3M";
    private static SheetsService service;
    private static string path = Environment.CurrentDirectory + "/Assets/Credentials";


    public static void ConnectToGoogle()
    {
        GoogleCredential credential;

        //Debug.Log(Environment.CurrentDirectory);
        // Put your credentials json file in the root of the solution and make sure copy to output dir property is set to always copy 
        using (var stream = new FileStream(Path.Combine(path + "/credentials.json"),
            FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);
        }

        // Create Google Sheets API service.
        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = applicationName
        });
        ClearRequest();
    }
    public static bool AddScoreEntry(string algorithim,string word,string dataSet,float time,float preprocessingTime,bool found)
    {
        try
        {
            string range = "Scores!A2:F";
            var dataValueRange = new ValueRange();
            dataValueRange.Range = range;
            var oblist = new List<object>() {algorithim,word, dataSet,time,preprocessingTime,found};
            dataValueRange.Values = new List<IList<object>> { oblist };

            SpreadsheetsResource.ValuesResource.AppendRequest update = service.Spreadsheets.Values.Append(dataValueRange, spreadsheetId, range);
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
            AppendValuesResponse result2 = update.Execute();

            Console.WriteLine("done!");
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    public static void ClearRequest()
    {
        string range = "Scores!A2:F";  // TODO: Update placeholder value.
        ClearValuesRequest requestBody = new ClearValuesRequest();
        SpreadsheetsResource.ValuesResource.ClearRequest request = service.Spreadsheets.Values.Clear(requestBody, spreadsheetId, range);
        ClearValuesResponse response = request.Execute();
    }

}

public class ScoreboardObject
{
    public object score;
    public object name;
}

