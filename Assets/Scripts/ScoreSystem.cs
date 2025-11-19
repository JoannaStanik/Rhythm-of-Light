using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    [Header("UI")]
    public Text scoreText;
    public Text comboText;
    public Text feedbackText;

    [Header("Wartoœci")]
    public int score;
    public int combo;
    public int maxCombo;

    public void ResetScore()
    {
        score = 0;
        combo = 0;
        maxCombo = 0;
        UpdateUI();
        ClearFeedback();
    }

    // wywo³ywane przy trafieniu (QTEManager przekazuje dok³adnoœæ)
    public void RegisterHit(float deltaAbs, float perfectWindow, float goodWindow)
    {
        int points;
        string message;

        if (deltaAbs <= perfectWindow)
        {
            points = 100;
            message = "Perfect!";
        }
        else if (deltaAbs <= goodWindow)
        {
            points = 70;
            message = "Good!";
        }
        else
        {
            points = 50;
            message = "Ok";
        }

        score += points;
        combo++;
        if (combo > maxCombo) maxCombo = combo;

        ShowFeedback(message);
        UpdateUI();
    }

    // wywo³ywane przy pudle
    public void RegisterMiss()
    {
        combo = 0;
        ShowFeedback("Miss");
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText)
            scoreText.text = $"Score: {score}";

        if (comboText)
        {
            if (combo > 1)
                comboText.text = $"Combo: {combo}";
            else
                comboText.text = "";
        }
    }

    void ShowFeedback(string msg)
    {
        if (!feedbackText) return;

        feedbackText.text = msg;
        CancelInvoke(nameof(ClearFeedback));
        Invoke(nameof(ClearFeedback), 0.5f); // czyœci po 0.5 s
    }

    void ClearFeedback()
    {
        if (feedbackText)
            feedbackText.text = "";
    }
}
