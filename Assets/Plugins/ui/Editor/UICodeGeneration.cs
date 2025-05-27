using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

/* FEATURES:
 * Boomstructuur genereren
 * GameObject aanspreken
 * Components en transform eruit halen
 * Common UIObject class
 * Dynamisch objecten toevoegen
 * Input Action => Moet in code afgehandeld of werkt ook Unity-way
 * Generate Code from UI Prefabs
 */

public static class UICodeGeneration
{
    public const string PathFormat = @"Scripts/Generated/{name}.cs";

    [MenuItem("Tools/Generate Code from selected UI GameObject")]
    public static void GenerateCodeFromUI()
    {
        GameObject selected = Selection.activeGameObject;

        if (selected)
        {
            GenerateCodeFromObject(selected);
        }

        else
        {
            Debug.LogError("Selection is not an UI object");
        }
    }

    public static void GenerateCodeFromObject(GameObject gameObject)
    {
        var tree = GenerateTreeFromObject(gameObject.name, gameObject);

        if (tree != null)
        {
            var code = tree.GenerateCode();
            string path = Path.Combine(Application.dataPath, PathFormat.Replace("{name}", tree.Name + "_Gen"));
            if (File.Exists(path)) File.Delete(path);
            code =
@"using UnityEngine;
using UnityEngine.UI;

" + code;
            File.WriteAllText(path, code);
            AssetDatabase.Refresh();
            Debug.Log("Generated code for " + tree.Name);
        }

        else
        {
            Debug.LogError("Failed to generate code for " + gameObject.name);
        }
    }

    private static string Indentation(int indent) => new string(Enumerable.Range(0, indent).Select(x => '\t').ToArray());

    private static UINodeTree GenerateTreeFromObject(string name, GameObject obj)
    {
        if (obj && obj.GetComponent<RectTransform>())
        {
            name = UINode.NormalizeName(name);

            string type = "UINode";
            bool path = false;
            if (obj.GetComponent<UIElement>()) path = true;
            if (obj.GetComponent<UIText>()) type = "UITextNode";
            if (obj.GetComponent<UIButton>()) type = "UIButtonNode";
            if (obj.GetComponent<UIImage>()) type = "UIImageNode";
            if (type != "UINode") path = true;

            var children = Enumerable.Range(0, obj.transform.childCount)
                .Select(x => obj.transform.GetChild(x).gameObject)
                .GroupBy(x => x.name)
                .SelectMany(x =>
                {
                    if (x.Count() == 1) return new[] { (x.Key, x.First()) };
                    else
                    {
                        int i = 0;
                        return x.Select(y => (x.Key + "_" + i++, y)).ToArray();
                    }
                })
                .Select(x => GenerateTreeFromObject(x.Item1, x.Item2))
                .Where(x => x.Name != null && x.Path).ToArray();

            return new UINodeTree
            {
                Name = name,
                Type = type,
                Children = children,
                Path = path || children.Length > 0
            };
        }

        return null;
    }

    public const string ClassFormat = @"public class {{Name}}_Gen : {{Base}}";
    public const string ChildFormat = @"public {{Name}}_Gen {{Name}};";

    private class UINodeTree
    {
        public string Name;
        public string Type;
        public bool Path;
        public UINodeTree[] Children;

        public string GenerateCode(int indent = 0)
        {
            string code = Indentation(indent) + ClassFormat.Replace("{{Name}}", Name).Replace("{{Base}}", Type);

            if (Children.Length > 0)
            {
                code += Environment.NewLine + Indentation(indent) + "{" + Environment.NewLine;
                code += string.Join(Environment.NewLine, Children.Select(x => Indentation(indent + 1) +
                    ChildFormat.Replace("{{Name}}", x.Name))) + Environment.NewLine + Environment.NewLine;
                code += string.Join(Environment.NewLine, Children.Select(x => x.GenerateCode(indent + 1))) + Environment.NewLine;
                code += Indentation(indent) + "}";
            }

            else
            {
                code += " { }";
            }

            return code;
        }
    }
}