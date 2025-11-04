using UnityEngine;
using UnityEngine.UI;

public class PromptIcon : MonoBehaviour
{
    public Text label;            // Tekst z nazw¹ klawisza (np. "A")
    public Image background;      // Opcjonalnie t³o do podœwietlania
    public Color idleColor = Color.white;
    public Color hitColor = Color.green;
    public Color missColor = Color.red;

    public void SetKey(KeyCode key)
    {
        if (label) label.text = key.ToString();
        SetIdle();
    }

    public void SetIdle()
    {
        if (label) label.color = idleColor;
        if (background) background.color = new Color(background.color.r, background.color.g, background.color.b, 0.35f);
    }

    public void SetHit()
    {
        if (label) label.color = hitColor;
        if (background) background.color = new Color(hitColor.r, hitColor.g, hitColor.b, 0.35f);
    }

    public void SetMiss()
    {
        if (label) label.color = missColor;
        if (background) background.color = new Color(missColor.r, missColor.g, missColor.b, 0.35f);
    }

    public void SetAlpha(float a)
    {
        if (label)
        {
            var c = label.color; c.a = a; label.color = c;
        }
        if (background)
        {
            var c = background.color; c.a = a * 0.35f; background.color = c;
        }
    }
}
