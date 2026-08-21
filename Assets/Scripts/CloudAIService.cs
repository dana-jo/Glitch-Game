using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class CloudAIService : MonoBehaviour, IAIChatService
{
    [Header("Gemini API Settings")]
    [SerializeField] private string _apiKey = "YOUR_API_KEY_HERE";
    [SerializeField] private string _modelName = "gemini-1.5-flash";

    [Header("Network Settings")]
    [SerializeField]
    private int _timeoutSeconds = 15;
    private string ApiUrl => $"https://generativelanguage.googleapis.com/v1beta/models/{_modelName}:generateContent?key={_apiKey}";

    public void AskQuestion(string prompt, Action<string> onAnswerReceived)
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogError("[CloudAIService] GameObject is inactive! Cannot start WebRequest.");
            return;
        }

        StartCoroutine(SendGeminiRequestRoutine(prompt, onAnswerReceived));
    }

    private IEnumerator SendGeminiRequestRoutine(string prompt, Action<string> onAnswerReceived)
    {
        string safePrompt = prompt.Replace("\\", "\\\\")
                               .Replace("\"", "\\\"")
                               .Replace("\r", "")
                               .Replace("\n", "\\n");

        GeminiRequest requestPayload = new GeminiRequest
        {
            contents = new GeminiContent[]
            {
                new GeminiContent
                {
                    parts = new GeminiPart[] { new GeminiPart { text = safePrompt } }
                }
            },
            generationConfig = new GenerationConfig { maxOutputTokens = 2048 }
        };

        string jsonPayload = JsonUtility.ToJson(requestPayload);

        using (UnityWebRequest request = new UnityWebRequest(ApiUrl, "POST"))
        {
            request.certificateHandler = new BypassCertificate();
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = _timeoutSeconds;

            Debug.Log("<color=yellow>[TEST 1 - API] Sending request to Gemini...</color>");

            yield return request.SendWebRequest();

            Debug.Log("<color=yellow>[TEST 2 - API] Request Finished. Response Code: " + request.responseCode + "</color>");

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[CloudAIService] Error: {request.error} | Details: {request.downloadHandler.text}");
                onAnswerReceived?.Invoke("System Error: Connection failed.");
            }
            else
            {
                Debug.Log($"<color=cyan>[TEST 3 - RAW JSON]:</color> {request.downloadHandler.text}");
                ParseResponse(request.downloadHandler.text, onAnswerReceived);
            }
        }
    }

    private void ParseResponse(string rawJson, Action<string> onAnswerReceived)
    {
        try
        {
            GeminiResponse responseData = JsonUtility.FromJson<GeminiResponse>(rawJson);

            if (responseData != null && responseData.candidates != null && responseData.candidates.Length > 0)
            {
                string aiText = responseData.candidates[0].content.parts[0].text;

                Debug.Log($"<color=green>[TEST 4 - PARSED AI RESPONSE]:</color> {aiText.Trim()}");

                onAnswerReceived?.Invoke(aiText.Trim());
            }
            else
            {
                Debug.LogWarning("[CloudAIService] Candidates array is null or empty.");
                onAnswerReceived?.Invoke("System Error: Received empty response.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[CloudAIService] Parsing Error: {e.Message}");
            onAnswerReceived?.Invoke("System Error: Failed to read AI response.");
        }
    }

 
    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    #region JSON Serialization Classes
    [Serializable] private class GeminiResponse { public Candidate[] candidates; }
    [Serializable] private class Candidate { public Content content; }
    [Serializable] private class Content { public Part[] parts; }
    [Serializable] private class Part { public string text; }

    
    [Serializable]
    private class GeminiRequest
    {
        public GeminiContent[] contents;
        public GenerationConfig generationConfig;
    }

    [Serializable] private class GeminiContent { public GeminiPart[] parts; }
    [Serializable] private class GeminiPart { public string text; }
    [Serializable] private class GenerationConfig { public int maxOutputTokens; }
    #endregion

}