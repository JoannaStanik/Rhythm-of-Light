using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    public MusicManager music;
    public CanvasGroup startScreen;
    public CanvasGroup resultScreen;

    enum State { Start, Playing, Result }
    State _state;

    void Start()
    {
        SetState(State.Start);
    }

    void Update()
    {
        if (_state == State.Start && Input.anyKeyDown)
        {
            SetState(State.Playing);
            if (music) music.Play();
        }

        if (_state == State.Playing && music && music.MusicTime >= music.MusicLength - 0.05f)
        {
            SetState(State.Result);
            if (music) music.Stop();
        }

        if (_state == State.Result && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void SetState(State s)
    {
        _state = s;
        SetGroup(startScreen, s == State.Start);
        SetGroup(resultScreen, s == State.Result);
    }

    void SetGroup(CanvasGroup g, bool on)
    {
        if (!g) return;
        g.alpha = on ? 1f : 0f;
        g.blocksRaycasts = on;
        g.interactable = on;
    }
}
