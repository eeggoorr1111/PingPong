using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Narratore
{
    /// <summary>
    /// Require set root of namespace in Edit > Project Settings > Editor > Root Namespace
    /// </summary>
    public class NamespaceResolver : UnityEditor.AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string metaFilePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(metaFilePath);

            if (!fileName.EndsWith(".cs"))
                return;

            var actualFile = $"{Path.GetDirectoryName(metaFilePath)}\\{fileName}";
            var segmentedPath = $"{Path.GetDirectoryName(metaFilePath)}".Split(new[] { '\\' }, StringSplitOptions.None);

            var generatedNamespace = "";
            var finalNamespace = "";
            var rootNamespace = EditorSettings.projectGenerationRootNamespace;

            // In case of placing the class at the root of a folder such as (Editor, Scripts, etc...)  
            if (segmentedPath.Length <= 2)
                finalNamespace = rootNamespace;
            else
            {
                // Skipping the Assets folder and a single subfolder (i.e. Scripts, Editor, Plugins, etc...)
                for (var i = 2; i < segmentedPath.Length; i++)
                {
                    generatedNamespace +=
                        i == segmentedPath.Length - 1
                            ? segmentedPath[i]
                            : segmentedPath[i] + "."; // Don't add '.' at the end of the namespace
                }

                finalNamespace = rootNamespace + "." + generatedNamespace;
            }

            var content = File.ReadAllText(actualFile);
            var newContent = content.Replace("#NAMESPACE#", finalNamespace);

            if (content != newContent)
            {
                File.WriteAllText(actualFile, newContent);
                AssetDatabase.Refresh();
            }
        }
    }
}

