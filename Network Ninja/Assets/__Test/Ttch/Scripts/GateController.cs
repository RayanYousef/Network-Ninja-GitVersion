using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public enum GateStates
{
    Start,
    End
}

public class GateController : MonoBehaviour
{
    [SerializeField] GateStates state;
    public bool buttonPressed;
    public bool playerIsHere;
    PathController pathController;

    [Header("Area")]
    [SerializeField] rArea nextArea;

    public GateStates State { get => state; set => state = value; }
    public PathController PathController { get => pathController; set => pathController = value; }
    public rArea NextArea { get => nextArea; set => nextArea = value; }

    private void Start()
    {
        pathController = GetComponentInParent<PathController>();
    }
    private void Update()
    {
        buttonPressed = Input.GetKeyDown(KeyCode.E);
        if (playerIsHere == true && buttonPressed && rUIManager.Instance.UiPassword.CheckPasswordPanel.activeSelf)
        {
            rUIManager.Instance.UiPassword.CheckPasswordPanel.SetActive(false);
            rUIManager.instance.IsAnyInteractivePanelEnabled = false;
            return;
        }

        if (playerIsHere == true && buttonPressed && GameManager.Instance.BossEntered==false && !rUIManager.Instance.IsAnyInteractivePanelEnabled)
        {
            GameObjectsManager.Instance.CurrentGate.NextArea = this.nextArea;

            if (nextArea.AreaType == AreaType.Base)
            {
                rUIManager.instance.UiPassword.ShowCheckPasswordPanel();
            }
            else
            {
                rAreasManager.Instance.CurrentArea.PlayerInside = false;
                pathController.PlayerEnteredPath(state);
            }
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObjectsManager.Instance.Player)
        {

            playerIsHere = true;
            GameObjectsManager.Instance.CurrentGate = this;
            //rUIManager.instance.InGameUI.prompt.enabled = true;
            rUIManager.instance.InGameUI.prompt.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObjectsManager.Instance.Player)
        {
            playerIsHere = false;
            //rUIManager.instance.InGameUI.prompt.enabled = false;
            rUIManager.instance.InGameUI.prompt.gameObject.SetActive(false);
        }
    }
}
