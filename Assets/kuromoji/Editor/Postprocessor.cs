using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace takuyaa.kuromoji.Editor
{
    public class Postprocessor
    {
        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.WebGL) return;

            // kuromoji.js / dictフォルダを build path に複製する

            // build path に "kuromoji" フォルダなかったら作成
            var root = Path.Combine(pathToBuiltProject, "kuromoji");
            if(!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            CopyBuild(root);
            CopyDict(root);
        }

        /// <summary>
        /// build/kuromoji.js を複製
        /// </summary>
        private static void CopyBuild(string root)
        {
            var build = Path.Combine(root, "build");
            if (!Directory.Exists(build))
            {
                Directory.CreateDirectory(build);
            }
            var sourceFileName = Path.Combine(Application.dataPath, "kuromoji/libs/build/kuromoji.js");
            var destFileName = Path.Combine(build, "kuromoji.js");
            File.Copy(sourceFileName, destFileName, true);
        }

        /// <summary>
        /// dict を複製
        /// </summary>
        private static void CopyDict(string root)
        {
            var dict = Path.Combine(root, "dict");
            if (!Directory.Exists(dict))
            {
                Directory.CreateDirectory(dict);
            }

            var files = Directory.GetFiles(Path.Combine(Application.dataPath, "kuromoji/libs/dict"));
            foreach (var file in files.Where(v=> !v.EndsWith(".meta")))
            {
                var destFileName = Path.Combine(dict, Path.GetFileName(file));
                File.Copy(file, destFileName, true);
            }
        }
    }
}