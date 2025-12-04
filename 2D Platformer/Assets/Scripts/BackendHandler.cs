using System.Collections;
using HighScores;
using Unity.VisualScripting;
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
/*
        Server Communication
*/
    // 1. Fetching data from the server
    // Determine which URL to use for fetching high scores
    string urlBackendHighScores = "";
    // Pure JSON file
    public void FetchHighScoresJSONFile()
    {
        fetchCounter++;
        Debug.Log("FetchHighScoresJSONFile called.");
        urlBackendHighScores = "https://niisku.lab.fi/~kala/speedgate/highscores.json";
        StartCoroutine(GetRequestForScores(urlBackendHighScores));
    }
    // Scores from Database via PHP API
    public void FetchHighScoresJSON()
    {
        fetchCounter++;
        Debug.Log("FetchHighScoresJSON called.");
        urlBackendHighScores = "https://niisku.lab.fi/~kala/speedgate/api/highscores.php";
        StartCoroutine(GetRequestForScores(urlBackendHighScores));
    }
    // Coroutine for sending GET request and handling response
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

    // 2. Sending data to the server
    // Construct entry for the database
    public TMPro.TMP_InputField playerNameInput;
    public TMPro.TMP_InputField playerScoreInput;
    public UnityEngine.UI.Button postResultsButton;
    bool scoreInputsOk = false;
    public void PostGameResults()
    {
        checkScore();
        if (!scoreInputsOk) return;
        HighScore hsItem = new HighScore();
        hsItem.playerName = playerNameInput.text;
        hsItem.playerScore = int.Parse(playerScoreInput.text);
        Debug.Log("PostGameResults called. Player: " + hsItem.playerName + ", Score: " + hsItem.playerScore);
        Debug.Log("JSON to send: " + JsonUtility.ToJson(hsItem));
        StartCoroutine(PostRequestForScores(urlBackendHighScores, hsItem));
    }
    // Coroutine for sending POST request with game results
    IEnumerator PostRequestForScores(string uri, HighScore hsItem)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
        {
            InsertToLog("[" + fetchCounter + "] POST Request sent to " + uri);
            tmpTextLog.text = log;

            // Convert HighScore item to json string
            string jsonData = JsonUtility.ToJson(hsItem);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

            // Set uploadHandler and downloadHandler
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");

            // Request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                InsertToLog("Error encountered: " + webRequest.error);
                tmpTextLog.text = log;
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                InsertToLog("Game results posted successfully.");
                tmpTextLog.text = log;
                Debug.Log("Response: " + webRequest.downloadHandler.text);
            }
        }
    }
    void checkScore()
    {
        if (float.TryParse(playerScoreInput.text, out _) && playerNameInput.text.Trim().Length > 0)
        {
            scoreInputsOk = true;
        }
        else
        {
            scoreInputsOk = false;
        }
    }
}
