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

    private void Start()
    {
        pathController = GetComponentInParent<PathController>();
    }
    private void Update()
    {

        buttonPressed = Input.GetKey(KeyCode.E);
        if (playerIsHere == true && buttonPressed)
        {
            pathController.PlayerEnteredPath(state);
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObjectsManager.Instance.Player)
        {
            playerIsHere = true;
            rUIManager.instance.InGameUI.prompt.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameObjectsManager.Instance.Player)
        {
            playerIsHere = false;
            rUIManager.instance.InGameUI.prompt.enabled = false;

        }
    }
}
