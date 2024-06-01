using System.Text;
using UnityEngine;
using UnityEngine.UI;
using takuyaa.kuromoji;

public class Sample : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] Text resultText;
    void Start()
    {
        Kuromoji.LoadScript();
    }

    public void OnClick()
    {
        var text = inputField.text;
        Kuromoji.Build(text, res =>
        {
            var sb = new StringBuilder();
            foreach (var item in res)
            {
                sb.AppendLine($"[{item.word_type}] {item.reading}");
            }
            resultText.text = sb.ToString();
        });
    }
}
