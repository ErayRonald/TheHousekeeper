using UnityEngine;
using UnityEngine.UI;

#if TMPro
[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
#else
[RequireComponent(typeof(Text))]
#endif
public class UIText : MonoBehaviour { }
