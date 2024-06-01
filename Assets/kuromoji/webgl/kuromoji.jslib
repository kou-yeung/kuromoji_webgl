mergeInto(LibraryManager.library, {

  kuromojiLoadScript: function (path) {
    var script = document.createElement('script');
    script.src = UTF8ToString(path);
    document.head.appendChild(script);
  },

  kuromojBuild: function (id, dict, text, cb) {
    var cacheText = UTF8ToString(text);
    kuromoji.builder({ dicPath: UTF8ToString(dict) }).build(function(err, tokenizer) {
        // tokenizer is ready
        var path = tokenizer.tokenize(cacheText);

        var returnStr = JSON.stringify({tokenize : path});
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        
        dynCall("vii", cb, [id, buffer]);
    });
  },
});
