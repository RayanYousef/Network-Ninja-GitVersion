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

    [SerializeField] Button skipBtn;

    public static DialoguUI instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(gameObject);
    }

    private void Start()
    {
        skipBtn.onClick.AddListener(MoveToTutorial);
    }

    public void MoveToTutorial()
    {
        CS_SceneManager.Instance.LoadSceneByNumber(CS_SceneManager.Instance.TutorialScene);
    }

}
