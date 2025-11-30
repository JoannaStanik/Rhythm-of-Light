using UnityEngine;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    [Header("Odniesienia")]
    public MusicManager music;
    public DanceSequence sequence;
    public RectTransform promptRoot;
    public PromptIcon promptPrefab;
    public ScoreSystem score;
    public GameObject scorePanel;
    public ResultScreen resultScreen;

    [Header("Wizualne zbliżanie się kroku")]
    public float appearWindow = 1f;

    [Header("Ruch ikon (spadanie)")]
    public float topY = 300f;

    [Tooltip("Pozycja Y linii trafienia")]
    public float hitY = 0f;

    [Header("Usuwanie ikon")]
    public float removeDelay = 0.2f;

    private PromptIcon[] _icons;
    private RectTransform[] _rects;
    private bool[] _resolved;
    private float[] _stepTimes;

    private bool _active = false;
    private int _resolvedCount = 0;

    void Start()
    {
        if (sequence == null || sequence.steps == null || sequence.steps.Length == 0)
        {
            Debug.LogWarning("QTEManager: Brak sekwencji kroków!");
            return;
        }

        int count = sequence.steps.Length;
        _icons = new PromptIcon[count];
        _rects = new RectTransform[count];
        _resolved = new bool[count];
        _stepTimes = new float[count];

        float runningTime = 0f;
        for (int i = 0; i < count; i++)
        {
            runningTime += sequence.steps[i].delay;
            _stepTimes[i] = runningTime;

            var icon = Instantiate(promptPrefab, promptRoot);
            icon.SetKey(sequence.steps[i].key);
            icon.SetAlpha(0f);

            var rt = icon.GetComponent<RectTransform>();
            Vector2 pos = rt.anchoredPosition;
            pos.y = topY;
            rt.anchoredPosition = pos;

            _icons[i] = icon;
            _rects[i] = rt;
            _resolved[i] = false;
        }

        if (scorePanel != null)
            scorePanel.SetActive(false);

        if (resultScreen != null)
            resultScreen.HideImmediate();
    }

    void Update()
    {
        if (!_active || sequence == null || sequence.steps == null)
            return;

        float t = music ? music.MusicTime : Time.time;
        bool keyConsumedThisFrame = false;

        for (int i = 0; i < sequence.steps.Length; i++)
        {
            if (_resolved[i])
                continue;

            float stepTime = _stepTimes[i];
            float delta = t - stepTime;

            float travelStart = stepTime - appearWindow;
            float travelEnd = stepTime;

            float fallT = Mathf.InverseLerp(travelStart, travelEnd, t);
            fallT = Mathf.Clamp01(fallT);

            RectTransform rt = _rects[i];
            if (rt == null)
                continue;

            Vector2 pos = rt.anchoredPosition;
            pos.y = Mathf.Lerp(topY, hitY, fallT);
            rt.anchoredPosition = pos;

            if (_icons[i] != null)
                _icons[i].SetAlpha(fallT);

            if (!keyConsumedThisFrame && Input.GetKeyDown(sequence.steps[i].key))
            {
                float early = -sequence.allowedEarly;
                float late = sequence.allowedLate;

                if (delta >= early && delta <= late)
                {
                    RegisterHitForIndex(i, delta);
                }
                else
                {
                    RegisterMissForIndex(i);
                }

                keyConsumedThisFrame = true;
            }

            if (!_resolved[i] && delta > sequence.allowedLate)
            {
                RegisterMissForIndex(i);
            }
        }

        if (_active && _resolvedCount >= sequence.steps.Length)
        {
            EndQTE();
        }
    }

    void RegisterHitForIndex(int i, float delta)
    {
        if (_resolved[i]) return;

        _resolved[i] = true;
        _resolvedCount++;

        if (_icons[i] != null)
            _icons[i].SetHit();

        if (score != null)
        {
            float deltaAbs = Mathf.Abs(delta);
            score.RegisterHit(deltaAbs, sequence.perfectWindow, sequence.goodWindow);
        }

        RemovePrompt(i);
    }

    void RegisterMissForIndex(int i)
    {
        if (_resolved[i]) return;

        _resolved[i] = true;
        _resolvedCount++;

        if (_icons[i] != null)
            _icons[i].SetMiss();

        if (score != null)
            score.RegisterMiss();

        RemovePrompt(i);
    }

    void RemovePrompt(int i)
    {
        if (_icons != null && i >= 0 && i < _icons.Length && _icons[i] != null)
        {
            Destroy(_icons[i].gameObject, removeDelay);
        }

        _icons[i] = null;
        _rects[i] = null;
    }


    public void StartQTE()
    {
        if (sequence == null || sequence.steps == null || sequence.steps.Length == 0)
        {
            Debug.LogWarning("QTEManager: próba startu bez sekwencji.");
            return;
        }

        _active = true;
        _resolvedCount = 0;

        for (int i = 0; i < _resolved.Length; i++)
            _resolved[i] = false;

        if (score != null)
            score.ResetScore();

        if (scorePanel != null)
            scorePanel.SetActive(true);

        if (resultScreen != null)
            resultScreen.HideImmediate();

        if (music != null)
            music.Play();
    }

    public void StopQTE()
    {
        if (!_active) return;

        _active = false;

        if (music != null)
            music.Stop();

        if (scorePanel != null)
            scorePanel.SetActive(false);
    }

    void EndQTE()
    {
        if (!_active) return;

        _active = false;

        if (music != null)
            music.Stop();

        if (scorePanel != null)
            scorePanel.SetActive(false);

        if (resultScreen != null && score != null)
        {
            int finalScore = score.CurrentScore;
            int maxCombo = score.MaxComboValue;
            string songLabel = sequence != null && !string.IsNullOrEmpty(sequence.songName)
                ? sequence.songName
                : "Song";

            resultScreen.Show(finalScore, maxCombo, songLabel);
        }
    }
}
