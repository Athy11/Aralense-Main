using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text;
using TMPro;

public class Smartchatbot : MonoBehaviour
{
    public TMP_InputField promptField;
    public TMP_Text chatDisplay;
    public string apiKey = "AIzaSyCRr-gLLc1jsRYi_IKPody0o3ltk-RVZ0w";
    public string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";
    public ScrollRect scrollRect;
    public RectTransform content;
    public GameObject newTextPrefab;
    private string text;

    // Start is called before the first frame update
    void Start()
    {
        StartChat();
    }

    public void StartChat()
    {
        AddToChat("Arvis: Hello!");
    }

    public void OnSendBtnClick()
    {
        string playerMessage = promptField.text;
        if (string.IsNullOrEmpty(playerMessage)) return;

        AddPlayerChat($"Student: {playerMessage}");
        StartCoroutine(SendToChatgpt(playerMessage));
        promptField.text = "";
    }



    private void AddToChat(string message)
    {
        GameObject newText = Instantiate(newTextPrefab, content);
        newText.GetComponent<TypeWriting>().StartTyping(message);

        Canvas.ForceUpdateCanvases();

        scrollRect.verticalNormalizedPosition = 0;
    }

    private void AddPlayerChat(string message)
    {
        GameObject newText = Instantiate(newTextPrefab, content);
        newText.GetComponent<TMP_Text>().text = message;

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0;
    }

    private IEnumerator SendToChatgpt(string prompt)
    {
        string jsonPayload = @"
        {
          ""contents"": [
            {
              ""parts"": [
                {""text"": ""You are Arvis, a friendly smart chatbot that will help guide MHS students in learning Computer Servicing. Keep answers under 1 sentence.""},
                {""text"": """ + prompt + @"""}
              ]
            }
          ],
          ""generationConfig"": {
            ""maxOutputTokens"": 100,
            ""temperature"": 0.7,
            ""topP"": 0.9
          }
        }";

        UnityWebRequest request = new UnityWebRequest(apiUrl + "?key=" + apiKey, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log(response);

            ChatGPTResponse responseFinal = JsonUtility.FromJson<ChatGPTResponse>(response);

            string text = responseFinal.candidates[0].content.parts[0].text;

            AddToChat($"Arvis: {text}");
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            AddToChat("Arvis: Sorry, something went wrong.");
        }

    }

    [System.Serializable]

    public class ChatGPTResponse
    {
        public Candidate[] candidates;
        public UsageMetaData usageMetaData;
    }

    [System.Serializable]
    public class Candidate
    {
        public Content content;
        public string finishReason;
        public float avgLogprobs;
    }

    [System.Serializable]
    public class Content
    {
        public Part[] parts;
        public string role;
    }

    [System.Serializable]
    public class Part
    {
        public string text;
    }

    [System.Serializable]
    public class UsageMetaData
    {
        public int promptTokenCount;
        public int candidatesTokenCount;
        public int totalTokenCount;
    }

}
