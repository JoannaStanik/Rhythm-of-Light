using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [Header("Panele")]
    public GameObject mainMenuPanel;
    public GameObject songListPanel;
    //public GameObject GoldenDifficultyPanel;

    public void OpenSongList()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (songListPanel) songListPanel.SetActive(true);
    }

    public void BackToMain()
    {
        if (songListPanel) songListPanel.SetActive(false);
        if (mainMenuPanel) mainMenuPanel.SetActive(true);
    }

    //public void OpenDifficultyForGolden()
    //{
    //    if (songListPanel) songListPanel.SetActive(false);
    //    if (GoldenDifficultyPanel) GoldenDifficultyPanel.SetActive(true);
    //}

    //public void BackToSongList()
    //{
    //    if (GoldenDifficultyPanel) GoldenDifficultyPanel.SetActive(false);
    //    if (songListPanel ) songListPanel.SetActive(true);
    //}

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
