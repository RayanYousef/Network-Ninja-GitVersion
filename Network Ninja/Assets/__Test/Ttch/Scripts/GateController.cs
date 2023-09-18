using System.Net;
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


    public Animator anim1, anim2;


    [Header("Area")]
    [SerializeField] rArea nextArea;

    public GateStates State { get => state; set => state = value; }
    public PathController PathController { get => pathController; set => pathController = value; }
    public rArea NextArea { get => nextArea; set => nextArea = value; }

    private void Start()
    {
        pathController = GetComponentInParent<PathController>();
        anim1 = this.GetComponentInChildren<Animator>();


        //anim2 = endPoint.GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        buttonPressed = CS_PathButton.IsPressed;

        if (playerIsHere == true && buttonPressed)
        { 
            if(rUIManager.Instance.UiPassword.CheckPasswordPanel.activeSelf)
            {
                rUIManager.Instance.UiPassword.CheckPasswordPanel.SetActive(false);
                rUIManager.Instance.SetInteractivePanelState = false;
                return;
            }

            if (GameManager.Instance.BossEntered == false && !rUIManager.Instance.SetInteractivePanelState)
            {
                GameObjectsManager.Instance.CurrentGate.NextArea = this.nextArea;

                if (nextArea.AreaType == AreaType.Base)
                {
                    rUIManager.Instance.UiPassword.ShowCheckPasswordPanel();
                }
                else
                {
                    rAreasManager.Instance.CurrentArea.PlayerInside = false;
                    pathController.PlayerEnteredPath(state);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObjectsManager.Instance.Player)
        {
            if (anim1 != null)
            {
                anim1.Play("Base Layer.Open");
            }
            playerIsHere = true;
            GameObjectsManager.Instance.CurrentGate = this;
            //rUIManager.instance.InGameUI.prompt.enabled = true;
           // rUIManager.Instance.InGameUI.prompt.gameObject.SetActive(true);
            if (CS_PathButton.PathButton != null)
                CS_PathButton.PathButton.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObjectsManager.Instance.Player)
        {
            if (anim1 != null)
            {
                anim1.Play("Base Layer.Close");
            }
            playerIsHere = false;
            //rUIManager.instance.InGameUI.prompt.enabled = false;
            //rUIManager.Instance.InGameUI.prompt.gameObject.SetActive(false);
            if(CS_PathButton.PathButton!=null)
            CS_PathButton.PathButton.SetActive(false);

        }
    }
}
