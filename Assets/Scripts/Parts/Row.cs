using UnityEngine;

public class Row : MonoBehaviour
{
    [SerializeField] Cell[] cells;

    public void SetColor(Color color)
    {
        foreach (var item in cells)
        {
            item.SetColor(color);
        } 
    }

    public void SetText(int index, string text)
    {
        cells[index].SetText(text);
    }
}
