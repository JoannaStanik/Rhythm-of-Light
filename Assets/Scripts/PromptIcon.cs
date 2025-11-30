using UnityEngine;
using UnityEngine.UI;

public class PromptIcon : MonoBehaviour
{
    [Header("UI")]
    public Text label;            // Tekst z nazw¹ klawisza (np. "A")
    public Image background;      // Opcjonalnie t³o do podœwietlania
    public Image iconImage;

    [Header("kolory")]
    public Color idleColor = Color.white;
    public Color hitColor = Color.green;
    public Color missColor = Color.red;

    [Header("Klawisze")]
    public Sprite spriteW;
    public Sprite spriteA;
    public Sprite spriteS;
    public Sprite spriteD;
    public Sprite defaultSprite;

    private KeyCode _key;

    public void SetKey(KeyCode key)
    {
        _key = key;

        if (label) 
            label.text = key.ToString();

        if (iconImage)
            iconImage.sprite = GetSpriteForKey(key);


        SetIdle();
    }

    private Sprite GetSpriteForKey(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W: return spriteW;
            case KeyCode.A: return spriteA;
            case KeyCode.S: return spriteS;
            case KeyCode.D: return spriteD;
            default: return defaultSprite;
        }
    }

    public void SetIdle()
    {
        if (label)
        {
            var c = idleColor;
            c.a = label.color.a;
            label.color = c;
        }

        if (background)
        {
            var c = idleColor;
            c.a = background.color.a;
            background.color = c;
        }

        if (iconImage)
        {
            var c = Color.white;
            c.a = iconImage.color.a;
            iconImage.color = c;
        }
    }

    public void SetHit()
    {
        if (label)
        {
            var c = hitColor;
            c.a = label.color.a;
            label.color = c;
        }

        if (background)
        {
            var c = hitColor;
            c.a = background.color.a;
            background.color = c;
        }

        if (iconImage)
        {
            var c = hitColor;
            c.a = iconImage.color.a;
            iconImage.color = c;
        }
    }

    public void SetMiss()
    {
        if (label)
        {
            var c = missColor;
            c.a = label.color.a;
            label.color = c;
        }

        if (background)
        {
            var c = missColor;
            c.a = background.color.a;
            background.color = c;
        }

        if (iconImage)
        {
            var c = missColor;
            c.a = iconImage.color.a;
            iconImage.color = c;
        }
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

        if (iconImage)
        {
            var c = iconImage.color; c.a = a; iconImage.color = c;
        }
    }
}
