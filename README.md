# kuromoji_webgl

[kuromoji.js](https://github.com/takuyaa/kuromoji.js)

[demo for Unity WebGL](https://kou-yeung.github.io/kuromoji_webgl/index.html)


# 初期化

```
// kuromoji.js の URL を指定して動的読み込み
takuyaa.kuromoji.LoadScript("url/to/kuromoji.js");

or

// build path に 自動的複製したkuromoji.jsを動的読み込み
takuyaa.kuromoji.LoadScript();

```

# Tokenizer

```

// dict の階層を指定して、解析する
takuyaa.kuromoji.Kuromoji.Build("/url/to/dictionary/dir/", "すもももももももものうち", res =>
{
    foreach (var item in res)
    {
        Debug.Log(item.surface_form);
    }
});

or

// build path に 自動的複製したdict を使って解析する
takuyaa.kuromoji.Kuromoji.Build("すもももももももものうち", res =>
{
    foreach (var item in res)
    {
        Debug.Log(item.surface_form);
    }
});

```

# kuromoji.js について

公式の kuromoji.js は dict 読み込み時に例外をスローしました

原因：

現在のブラウザではレスポンス時に gz を解凍しながら受信するため、既存実装はもう一回decompress() によって失敗しました

解決方法：

```
// kuromoji.js Line.8125~

var arraybuffer = this.response;
var gz = new zlib.Zlib.Gunzip(new Uint8Array(arraybuffer));
var typed_array = gz.decompress();
callback(null, typed_array.buffer);

↓ に変更する ( ResponseHeader のContent-Encoding をチェックして encoding した場合arraybufferをそのまま使う

var arraybuffer = this.response;
var contentEncoding = xhr.getResponseHeader("Content-Encoding");
// https://github.com/hexenq/kuroshiro/issues/27
// maybe auto decompress .gz in response
if (contentEncoding == "gzip") {
    callback(null, arraybuffer);
}
else
{
    var gz = new zlib.Zlib.Gunzip(new Uint8Array(arraybuffer));
    var typed_array = gz.decompress();
    callback(null, typed_array.buffer);
}
```
