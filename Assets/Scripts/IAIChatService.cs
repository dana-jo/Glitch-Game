using System;

public interface IAIChatService
{
    /// <summary>
    /// Sends a prompt to the AI service and returns the response via callback.
    /// </summary>
    /// <param name="prompt">The formatted prompt (including anti-spoiler context).</param>
    /// <param name="onAnswerReceived">Callback invoked when the AI responds.</param>
    void AskQuestion(string prompt, Action<string> onAnswerReceived);
}