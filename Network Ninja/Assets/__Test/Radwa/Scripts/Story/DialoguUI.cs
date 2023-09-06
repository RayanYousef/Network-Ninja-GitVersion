using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class DialoguUI : MonoBehaviour
{
    [Header("UI Dialogue Panel")]
    public Image background;
    public SpriteRenderer rightSpeakerImg;
    public SpriteRenderer leftSpeakerImg;

    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtParagraph;

    public static DialoguUI instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(gameObject);
    }


}
