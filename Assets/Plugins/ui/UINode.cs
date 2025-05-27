using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UINode
{
    public string name;
    public GameObject gameObject;
    public UINode[] children;

    public bool activeSelf => gameObject.activeSelf;
    public void SetActive(bool active) => gameObject.SetActive(active);
    public T GetComponent<T>() where T : Component => gameObject.GetComponent<T>();

    protected virtual void FillCustomFields(GameObject gameObject) { }

    public static implicit operator GameObject(UINode node) => node.gameObject;
    public static implicit operator RectTransform(UINode node) => (RectTransform)node.gameObject.transform;

    public static string NormalizeName(string name) =>
        name.Replace(" ", "_").Replace('(', '_').Replace(')', '_').Replace('{', '_').Replace('}', '_')
        .Replace('[', '_').Replace(']', '_').Replace('-', '_').TrimStart("0123456789".ToArray());

    public static T FromCode<T>(GameObject gameObject) where T : UINode, new() => (T)FromCode(gameObject, typeof(T));

    private static UINode FromCode(GameObject gameObject, Type type)
    {
        UINode node = (UINode)Activator.CreateInstance(type);
        node.name = gameObject.name;
        node.gameObject = gameObject;

        var children = Enumerable.Range(0, node.gameObject.transform.childCount)
            .Select(x => node.gameObject.transform.GetChild(x).gameObject)
            .GroupBy(x => NormalizeName(x.name))
            .SelectMany(x =>
            {
                if (x.Count() == 1) return new[] { (x.Key, x.First()) };
                else
                {
                    int i = 0;
                    return x.Select(y => (x.Key + "_" + i++, y)).ToArray();
                }
            }).ToArray();

        int i = 0;
        node.children = new UINode[children.Length];

        foreach (var (name, child) in children)
        {
            var field = type.GetField(name);

            if (field != null)
            {
                Type childType = type.GetField(name).FieldType;
                var childNode = FromCode(child, childType);
                type.GetField(name).SetValue(node, childNode);
                node.children[i++] = childNode;
            }
        }

        node.FillCustomFields(gameObject);

        return node;
    }

    public virtual void Reset()
    {
        foreach (var child in children)
            if (child != null)
                child.Reset();
    }
}

public class UIButtonNode : UINode
{
    private Button _button;

    public bool Pressed { get; private set; }

    protected override void FillCustomFields(GameObject gameObject)
    {
        Pressed = false;
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(() => Pressed = true);
    }

    public override void Reset()
    {
        base.Reset();
        Pressed = false;
    }
}

public class UITextNode : UINode
{
#if TMPro
    private TMPro.TextMeshProUGUI _text;
#else
    private Text _text;
#endif

    public string Text
    {
        get => _text.text;
        set => _text.text = value;
    }

    protected override void FillCustomFields(GameObject gameObject) =>
#if TMPro
        _text = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
#else
        _text = gameObject.GetComponent<Text>();
#endif

}

public class UIImageNode : UINode
{
    private Image _image;

    public Image ImageComponent => _image;

    protected override void FillCustomFields(GameObject gameObject)
    {
        _image = gameObject.GetComponent<Image>();
    }
}