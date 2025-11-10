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
    [Tooltip("Ile sekund przed czasem kroku ikonka zaczyna być widoczna")]
    public float appearWindow = 1f;

    private PromptIcon[] _icons;
    private bool[] _resolved;
    private bool _active = false;

    void Start()
    {
        if (sequence == null || sequence.steps == null || sequence.steps.Length == 0)
        {
            Debug.LogWarning("QTEManager: Brak sekwencji kroków!");
            return;
        }

        _icons = new PromptIcon[sequence.steps.Length];
        _resolved = new bool[sequence.steps.Length];

        for (int i = 0; i < sequence.steps.Length; i++)
        {
            var icon = Instantiate(promptPrefab, promptRoot);
            icon.SetKey(sequence.steps[i].key);
            icon.SetAlpha(0f);
            _icons[i] = icon;
        }
    }

    void Update()
    {
        if (!_active)
            return;

        float t = music ? music.MusicTime : Time.time;

        for (int i = 0; i < sequence.steps.Length; i++)
        {
            if (_resolved[i]) continue;

            var step = sequence.steps[i];
            float delta = t - step.time;

            float alpha = Mathf.InverseLerp(appearWindow, 0f, Mathf.Abs(delta));
            alpha = Mathf.Clamp01(1f - alpha);
            _icons[i].SetAlpha(alpha);

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
                }
                else
                {
                    _resolved[i] = true;
                    _icons[i].SetMiss();
                    if (score != null)
                        score.RegisterMiss();
                }
            }

            if (delta > sequence.allowedLate && !_resolved[i])
            {
                _resolved[i] = true;
                _icons[i].SetMiss();
                if (score != null)
                    score.RegisterMiss();
            }
        }
    }

    public void StartQTE()
    {
        _active = true;
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
