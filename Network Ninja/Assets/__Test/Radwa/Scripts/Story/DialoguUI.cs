using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class DialoguUI : MonoBehaviour
{
    [Header("UI Dialogue Panel")]
    public Image background;
    public SpriteRenderer rightSpeakerImg;
    [Tooltip("Middle speaker Image, mostly used when one speaker is speaking.")]
    public SpriteRenderer midSpeakerImg;
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
