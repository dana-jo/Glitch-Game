using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LocalAIService : MonoBehaviour, IAIChatService
{
    [Header("Local LLM Settings (Ollama)")]
    [SerializeField] private string _apiUrl = "http://localhost:11434/api/generate";
    [SerializeField] private string _modelName = "phi3"; // اسم النموذج الذي قمتِ بتنزيله

    public void AskQuestion(string prompt, Action<string> onAnswerReceived)
    {
        StartCoroutine(SendLocalRequestRoutine(prompt, onAnswerReceived));
    }

    private IEnumerator SendLocalRequestRoutine(string prompt, Action<string> onAnswerReceived)
    {
        // 1. تجهيز الطلب لـ Ollama
        OllamaRequest requestData = new OllamaRequest
        {
            model = _modelName,
            prompt = prompt,
            stream = false // لترجيع الإجابة كاملة دفعة واحدة
        };

        string jsonPayload = JsonUtility.ToJson(requestData);

        // 2. إرسال الطلب عبر localhost
        using (UnityWebRequest request = new UnityWebRequest(_apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log($"[LocalAIService] Sending prompt to local Phi-3 model...");

            yield return request.SendWebRequest();

            // 3. معالجة الرد
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"[LocalAIService] Connection Error: {request.error}\n{request.downloadHandler.text}");
                onAnswerReceived?.Invoke("تعذر الاتصال بـ Phi-3! تأكدي من أن برنامج Ollama يعمل في الخلفية.");
            }
            else
            {
                string rawJson = request.downloadHandler.text;
                Debug.Log($"[LocalAIService] Raw Response: {rawJson}");

                OllamaResponse responseData = JsonUtility.FromJson<OllamaResponse>(rawJson);

                if (responseData != null && !string.IsNullOrEmpty(responseData.response))
                {
                    onAnswerReceived?.Invoke(responseData.response.Trim());
                }
                else
                {
                    onAnswerReceived?.Invoke("وصلت استجابة فارغة من النموذج المحلي.");
                }
            }
        }
    }

    #region JSON Serialization Classes
    [Serializable]
    private class OllamaRequest
    {
        public string model;
        public string prompt;
        public bool stream;
    }

    [Serializable]
    private class OllamaResponse
    {
        public string response;
    }
    #endregion
}