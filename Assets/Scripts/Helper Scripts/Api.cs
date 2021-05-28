using UnityEngine;
using System.Net;
using System.IO;
public static class Api
{
    public static string URL = "https://snake-api-xa8h7.ondigitalocean.app";
    public static LeaderboardModel[] GetLeaderboards()
    {
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(URL + "/leaderboards?_sort=score:DESC");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        return JsonHelper.FromJson<LeaderboardModel>("{ \"Items\":"+json+"}");
    }
}
