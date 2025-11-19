using UnityEngine;

[CreateAssetMenu(fileName = "DanceSequence", menuName = "Rhythm/Dance Sequence")]
public class DanceSequence : ScriptableObject
{
    [System.Serializable]
    public struct Step
    {
        public KeyCode key;
        public float delay;
    }

    [Header("Kroki sekwencji (opóŸnienie po poprzednim kroku)")]
    public Step[] steps;

    [Header("Okna czasowe trafienia")]
    public float allowedEarly = 0.12f;
    [Tooltip("Maks. spóŸnienie (s)")]
    public float allowedLate = 0.12f;

    [Header("Progi oceny (wartoœci bezwzglêdne)")]
    public float perfectWindow = 0.05f;
    public float goodWindow = 0.10f;
}
