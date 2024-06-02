var LibraryKuromoji = {
    $instances: [],
    kuromojiLoadScript: function (path) {
        var script = document.createElement('script');
        script.src = UTF8ToString(path);
        document.head.appendChild(script);
    },
    kuromojBuild: function (id, dict, text, cb) {
        var cacheText = UTF8ToString(text);
        var dicPath = UTF8ToString(dict);

        function callback(path) {
            var returnStr = JSON.stringify({tokenize : path});
            var bufferSize = lengthBytesUTF8(returnStr) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(returnStr, buffer, bufferSize);
            dynCall("vii", cb, [id, buffer]);
        }

        var tokenizer = instances[dicPath];

        if(typeof tokenizer === "undefined")
        {
            console.log("[SHICO] 1");
            // create builder and build it take a tokenizer
            kuromoji.builder({ dicPath:dicPath }).build(function(err, tokenizer) {
                // cache the tokenizer
                instances[dicPath] = tokenizer;

                // return result
                callback(tokenizer.tokenize(cacheText));
            });
        }
        else
        {
            console.log("[SHICO] 2");
            // use cached tokenizer
            callback(tokenizer.tokenize(cacheText));
        }
    },
    kuromojiClearCache: function () {
        instances = [];
    },
}

autoAddDeps(LibraryKuromoji, '$instances');
mergeInto(LibraryManager.library, LibraryKuromoji);
