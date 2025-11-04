using UnityEngine;

[CreateAssetMenu(fileName = "DanceSequence", menuName = "Rhythm/Dance Sequence")]
public class DanceSequence : ScriptableObject
{
    [System.Serializable]
    public struct Step
    {
        public KeyCode key;
        public float time;
    }

    [Header("Kroki sekwencji (czas w sekundach)")]
    public Step[] steps;

    [Header("Okna czasowe trafienia")]
    [Tooltip("Maks. wczeœniejsze trafienie (s)")]
    public float allowedEarly = 0.12f;
    [Tooltip("Maks. spóŸnienie (s)")]
    public float allowedLate = 0.12f;

    [Header("Progi oceny (wartoœci bezwzglêdne)")]
    public float perfectWindow = 0.05f;
    public float goodWindow = 0.10f;
}
