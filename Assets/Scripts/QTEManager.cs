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
    private bool _active = false;

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

        for (int i = 0; i < count; i++)
        {
            var icon = Instantiate(promptPrefab, promptRoot);
            icon.SetKey(sequence.steps[i].key);
            icon.SetAlpha(0f);

            var rt = icon.GetComponent<RectTransform>();
            Vector2 pos = rt.anchoredPosition;
            pos.y = topY;
            rt.anchoredPosition = pos;


            _icons[i] = icon;
            _rects[i] = rt;
        }
    }

    void Update()
    {
        if (!_active || sequence == null || sequence.steps == null)
            return;

        float t = music ? music.MusicTime : Time.time;

        for (int i = 0; i < sequence.steps.Length; i++)
        {
            if (_resolved[i]) 
                continue;

            var step = sequence.steps[i];
            float delta = t - step.time;

            float travelStart = step.time - appearWindow;
            float travelEnd = step.time;

            float fallT = Mathf.InverseLerp(travelStart, travelEnd, t);
            fallT = Mathf.Clamp01(fallT);

            Vector2 pos = _rects[i].anchoredPosition;
            pos.y = Mathf.Lerp(topY, hitY, fallT);
            _rects[i].anchoredPosition = pos;

            _icons[i].SetAlpha(fallT);

            if (Input.GetKeyDown(step.key))
            {
                float early = -sequence.allowedEarly;
                float late = sequence.allowedLate;

                if (delta >= early && delta <= late)
                {
                    _resolved[i] = true;
                    _icons[i].SetHit();

                    if (score != null)
                    {
                        float deltaAbs = Mathf.Abs(delta);
                        score.RegisterHit(deltaAbs, sequence.perfectWindow, sequence.goodWindow);
                    }

                    RemovePrompt(i);
                }
                else
                {
                    _resolved[i] = true;
                    _icons[i].SetMiss();
                    if (score != null)
                        score.RegisterMiss();

                    RemovePrompt(i);
                }
            }

            if (delta > sequence.allowedLate && !_resolved[i])
            {
                _resolved[i] = true;
                _icons[i].SetMiss();
                if (score != null)
                    score.RegisterMiss();

                RemovePrompt(i);
            }
        }
    }

    void RemovePrompt(int i)
    {
        if (_icons != null && i >= 0 && i < _icons.Length && _icons[i] != null)
        {
            Destroy(_icons[i].gameObject, removeDelay);
        }
    }

    public void StartQTE()
    {
        _active = true;

        if (score != null)
            score.ResetScore();

        if (music != null)
            music.Play();
    }

    public void StopQTE()
    {
        _active = false;
        if (music != null)
            music.Stop();
    }
}
