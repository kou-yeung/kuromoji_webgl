using UnityEngine;
using UnityEngine.UI;
using takuyaa.kuromoji;
using System.Collections.Generic;

public class Sample : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Row rowPrefab;

    List<GameObject> rows = new List<GameObject>();

    void Start()
    {
        Kuromoji.LoadScript();
    }

    public void OnClick()
    {
        var text = inputField.text;
        Kuromoji.Build(text, res =>
        {
            // 既存表示物破棄する
            rows.ForEach(GameObject.Destroy);
            rows.Clear();

            // タイトル作成
            {
                var row = GameObject.Instantiate(rowPrefab, scrollRect.content);
                rows.Add(row.gameObject);
                var texts = new[] { "表層形", "品詞", "品詞細分類 1", "品詞細分類 2", "品詞細分類 3", "活用型", "活用形", "基本形", "読み", "発音" };

                ColorUtility.TryParseHtmlString("#F5F5F5", out var c);
                row.SetColor(c);
                for (int i = 0; i < texts.Length; i++)
                {
                    row.SetText(i, texts[i]);
                }
            }

            // 結果作成
            for (int i = 0; i < res.Length; i++)
            {
                var row = GameObject.Instantiate(rowPrefab, scrollRect.content);
                rows.Add(row.gameObject);

                var item = res[i];
                var colorText = i % 2 == 0 ? "#FFFFFF" : "#F9F9F9";
                ColorUtility.TryParseHtmlString(colorText, out var c);
                row.SetColor(c);

                var texts = new[]
                {
                    item.surface_form, item.pos, item.pos_detail_1, item.pos_detail_2, item.pos_detail_3,
                    item.conjugated_type, item.conjugated_form, item.basic_form, item.reading, item.pronunciation
                };

                for (int index = 0; index < texts.Length; index++)
                {
                    row.SetText(index, texts[index]);
                }
            }
        });
    }
}
