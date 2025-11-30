using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelRoot;
    public Text songNameText;
    public Text finalScoreText;
    public Text maxComboText;
    public GameObject targetPanelToShow;

    public void Show(int finalScore, int maxCombo, string songLabel)
    {
        if (panelRoot != null)
            panelRoot.SetActive(true);

        if (songNameText != null)
            songNameText.text = songLabel;

        if (finalScoreText != null)
            finalScoreText.text = $"Score: {finalScore}";

        if (maxComboText != null)
            maxComboText.text = $"Max combo: {maxCombo}";
    }

    public void HideImmediate()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);
    }

    public void OnBackButton()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);

        if (targetPanelToShow != null)
            targetPanelToShow.SetActive(true);
    }
}
