using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Text label;

    public void SetColor(Color color)
    {
        image.color = color;
    }

    public void SetText(string text)
    {
        label.text = text;
    }
}
