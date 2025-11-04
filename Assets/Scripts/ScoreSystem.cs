using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    [Header("UI")]
    public Text scoreText;
    public Text comboText;
    public Text feedbackText;

    int _score;
    int _combo;

    void Start()
    {
        UpdateUI();
        if (feedbackText) feedbackText.text = "";
    }

    public void RegisterHit(float deltaAbs, float perfectWindow, float goodWindow)
    {
        string fb;
        int add;

        if (deltaAbs <= perfectWindow) { fb = "PERFECT"; add = 300; _combo++; }
        else if (deltaAbs <= goodWindow) { fb = "GOOD"; add = 150; _combo++; }
        else { fb = "OK"; add = 50; _combo = Mathf.Max(0, _combo); }

        _score += add + (_combo * 5);
        ShowFeedback(fb);
        UpdateUI();
    }

    public void RegisterMiss()
    {
        _combo = 0;
        ShowFeedback("MISS");
        UpdateUI();
    }

    void ShowFeedback(string msg)
    {
        if (!feedbackText) return;
        feedbackText.text = msg;
        feedbackText.canvasRenderer.SetAlpha(1f);
        feedbackText.CrossFadeAlpha(0f, 0.4f, false);
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = _score.ToString();
        if (comboText) comboText.text = _combo > 0 ? $"COMBO {_combo}" : "";
    }
}
