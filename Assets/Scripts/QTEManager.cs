using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QTEManager : MonoBehaviour
{
    [Header("Odniesienia")]
    public MusicManager music;
    public DanceSequence sequence;
    public RectTransform promptRoot;
    public PromptIcon promptPrefab;
    public ScoreSystem score;

    [Header("Wizualne zbli¿anie siê kroku")]
    [Tooltip("Im wiêksza wartoœæ, tym szybciej prompty staj¹ siê widoczne przed czasem kroku.")]
    public float appearWindow = 1.0f;

    PromptIcon[] _icons;
    bool[] _resolved;

    void Start()
    {
        if (sequence == null || sequence.steps == null || sequence.steps.Length == 0)
        {
            Debug.LogWarning("QTEManager: Brak sekwencji kroków!");
            enabled = false;
            return;
        }

        _icons = new PromptIcon[sequence.steps.Length];
        _resolved = new bool[sequence.steps.Length];

        for (int i = 0; i < sequence.steps.Length; i++)
        {
            var icon = Instantiate(promptPrefab, promptRoot);
            icon.SetKey(sequence.steps[i].key);
            icon.SetAlpha(0.15f);
            _icons[i] = icon;
        }
    }

    void Update()
    {
        float t = music ? music.MusicTime : Time.time;

        for (int i = 0; i < sequence.steps.Length; i++)
        {
            if (_resolved[i]) continue;

            var step = sequence.steps[i];
            float delta = t - step.time;

            float a = Mathf.InverseLerp(-appearWindow, 0f, -Mathf.Abs(delta));
            a = Mathf.Clamp01(a + 0.15f);
            _icons[i].SetAlpha(a);

            // Wejœcie gracza
            if (Input.GetKeyDown(step.key))
            {
                float early = -sequence.allowedEarly;
                float late = sequence.allowedLate;

                if (delta >= early && delta <= late)
                {
                    // Trafienie
                    _resolved[i] = true;
                    _icons[i].SetHit();
                    float deltaAbs = Mathf.Abs(delta);
                    score.RegisterHit(deltaAbs, sequence.perfectWindow, sequence.goodWindow);
                }
                else
                {
                    // Z³y timing = Miss
                    _resolved[i] = true;
                    _icons[i].SetMiss();
                    score.RegisterMiss();
                }
            }

            // Zbyt póŸno — Miss
            if (delta > sequence.allowedLate && !_resolved[i])
            {
                _resolved[i] = true;
                _icons[i].SetMiss();
                score.RegisterMiss();
            }
        }
    }
}
