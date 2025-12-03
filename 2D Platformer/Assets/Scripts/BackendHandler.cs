using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BackendHandler : MonoBehaviour
{
    bool updateHighScoreTextArea = false;
    public TMPro.TMP_Text highScoreTextArea;
    // High Scores Table
    const string jsonTestStr = "{ " +
        "\"scores\": [ " +
            "{ \"id\": 1, \"playerName\": \"Alice\", \"PlayerScore\": 1500, \"PlayTime\": \"02:30\" }, " +
            "{ \"id\": 2, \"playerName\": \"Bob\", \"PlayerScore\": 1200, \"PlayTime\": \"03:15\" }, " +
            "{ \"id\": 3, \"playerName\": \"Charlie\", \"PlayerScore\": 900, \"PlayTime\": \"04:00\" } " +
        "] }";

    // Logging
    public TMPro.TMP_Text tmpTextLog;
    string log = "";
    string InsertToLog(string s)
    {
        return log = "[" + fetchCounter + "] " + s + "\n" + log;
    }
    string GetLog()
    {
        return log;
    }

    // Variables
    private int fetchCounter = 0;
    HighScores.HighScores hs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("BackendHandler started");
        // Convert test json string to HighScores object
        hs = JsonUtility.FromJson<HighScores.HighScores>(jsonTestStr);
        Debug.Log("HighScores name: " + hs.scores[0].playerName);
        // Reverse conversion back to json string
        Debug.Log("HighScores as json: " + JsonUtility.ToJson(hs));
    }

    // Update is called once per frame
    void Update()
    {
        tmpTextLog.text = log;
        if (updateHighScoreTextArea)
        {
            highScoreTextArea.text = CreateHighScoreList(); updateHighScoreTextArea = false;
        }
    }
    
    // Create a formatted high score list string from the HighScores object
    string CreateHighScoreList()
    {
        string hsList = "";
        if (hs != null)
        {
            // Limit to top 5 scores
            int len = (hs.scores.Length < 5) ? hs.scores.Length : 5;
            for (int i = 0; i < len; i++)
            {
                hsList += string.Format("[ {0} ] | {1} | {2} | {3}\n", (i + 1), // ("[ {0} ] | {1,-15} | {2,5} | {3,-15}
                    hs.scores[i].playerName,
                    hs.scores[i].playerScore,
                    hs.scores[i].playTime);
            }
        }
        return hsList;
    }

        // Backend handling
    string urlBackendHighScoresFile = "";
    public void FetchHighScoresJSONFile()
    {
        fetchCounter++;
        Debug.Log("FetchHighScoresJSONFile called.");
        // Get the JSON file from backend server
        urlBackendHighScoresFile = "https://niisku.lab.fi/~kala/speedgate/highscores.json";
        StartCoroutine(GetRequestForScores(urlBackendHighScoresFile));
    }
    public void FetchHighScoresJSON()
    {
        fetchCounter++;
        Debug.Log("FetchHighScoresJSON called.");
        // Get the highscores from server database via PHP API
        urlBackendHighScoresFile = "https://niisku.lab.fi/~kala/speedgate/api/highscores.php";
        StartCoroutine(GetRequestForScores(urlBackendHighScoresFile));
    }

    IEnumerator GetRequestForScores(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            InsertToLog("["+fetchCounter+"] Request sent to " + uri);
            tmpTextLog.text = log;

            // Set downloadHandler for json
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            // Request and wait for a response
            yield return webRequest.SendWebRequest();

            // Get raw data and convert it into string
            string resultStr = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);

            if (webRequest.isNetworkError)
            {
                InsertToLog("Error encountered: " + webRequest.error);
                tmpTextLog.text = log;
                Debug.Log("Error: " + webRequest.error);
            } else
            {
                // Create HighScore item from json string
                Debug.Log("Received json string: " + resultStr);
                hs = JsonUtility.FromJson<HighScores.HighScores>(resultStr);
                updateHighScoreTextArea = true;
                InsertToLog("HighScores fetched.");
                tmpTextLog.text = log;
                Debug.Log("Received(UTF8): " + resultStr);
                Debug.Log("Received(HS): " + JsonUtility.ToJson(hs));
            }
        }
    }
}
