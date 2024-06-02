using AOT;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices; // for DllImport
using UnityEngine;
using static takuyaa.kuromoji.Kuromoji;

namespace takuyaa.kuromoji
{
    [Serializable] struct TokenizeArrayWrapper { public Tokenize[] tokenize; }

    static class Plugins
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern void kuromojiLoadScript(string path);

        [DllImport("__Internal")]
        public static extern void kuromojBuild(int id, string dict, string text, Action<int, string> cb);

        [DllImport("__Internal")]
        public static extern void kuromojiClearCache();
#else
        public static void kuromojiLoadScript(string path) { }

        public static void kuromojBuild(int id, string dict, string text, Action<int, string> cb)
        {
            var tokenize = new Tokenize
            {
                word_id = 0,
                word_type = "UNKNOWN",
                word_position = 0,
                surface_form = text,
                pos = "*",
                pos_detail_1= "*",
                pos_detail_2= "*",
                pos_detail_3 = "*",
                conjugated_type = "*",
                conjugated_form = "*",
                basic_form = "*",
                reading = "*",
                pronunciation = "*"
            };
            cb?.Invoke(id, JsonUtility.ToJson(new TokenizeArrayWrapper { tokenize = new[] { tokenize } }));
        }

        public static void kuromojiClearCache() { }
#endif
    }

    public static class Kuromoji
    {
        [Serializable]
        public class Tokenize
        {
            public int word_id;             // 辞書内での単語ID
            public string word_type;        // 単語タイプ(辞書に登録されている単語ならKNOWN, 未知語ならUNKNOWN)
            public int word_position;       // 単語の開始位置
            public string surface_form;     // 表層形
            public string pos;              // 品詞
            public string pos_detail_1;     // 品詞細分類1
            public string pos_detail_2;     // 品詞細分類2
            public string pos_detail_3;     // 品詞細分類3
            public string conjugated_type;  // 活用型
            public string conjugated_form;  // 活用形
            public string basic_form;       // 基本形
            public string reading;          // 読み
            public string pronunciation;    // 発音
        }

        private static Dictionary<int, Action<Tokenize[]>> callbacks = new Dictionary<int, Action<Tokenize[]>>();

        /// <summary>
        /// kuromoji.js を読み込み
        /// path :  "./kuromoji/build/kuromoji.js"
        /// </summary>
        public static void LoadScript()
        {
            LoadScript("./kuromoji/build/kuromoji.js");
        }

        /// <summary>
        /// path 指定して kuromoji.js を読み込み
        /// </summary>
        public static void LoadScript(string path)
        {
            Plugins.kuromojiLoadScript(path);
        }

        /// <summary>
        /// ビルドする
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="text"></param>
        /// <param name="cb"></param>
        public static void Build(string text, Action<Tokenize[]> cb)
        {
            Build("./kuromoji/dict/", text, cb);
        }

        /// <summary>
        /// dict を指定してビルドする
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="text"></param>
        /// <param name="cb"></param>
        public static void Build(string dict, string text, Action<Tokenize[]> cb)
        {
            // このビルドにidを付与する
            var id = callbacks.Count + 1;
            callbacks.Add(id, cb);

            Plugins.kuromojBuild(id, dict, text, Result);
        }

        /// <summary>
        /// 解析結果受け取り
        /// </summary>
        /// <param name="id"></param>
        /// <param name="result"></param>
        [MonoPInvokeCallback(typeof(Action<int, string>))]
        private static void Result(int id, string result)
        {
            var cb = callbacks[id];
            callbacks[id] = null;
            cb?.Invoke(JsonUtility.FromJson<TokenizeArrayWrapper>(result).tokenize);
        }

        /// <summary>
        /// キャッシュした tokenizer クリアする
        /// </summary>
        public static void ClearCache()
        {
            Plugins.kuromojiClearCache();
        }
    }
}
