using UnityEngine;


public enum GateStates
{
    Start,
    End
}

public class GateController : MonoBehaviour
{
    [SerializeField] GateStates state;
    public bool spacePressed;
    PathController pathController;

    private void Start()
    {
        pathController = GetComponentInParent<PathController>();
    }
    private void Update()
    {
        spacePressed = Input.GetKey(KeyCode.Space);
    }
    void OnTriggerStay(Collider other)
    {
        if (spacePressed)
        {
            pathController.GateState(state, other);
        }
    }
}
