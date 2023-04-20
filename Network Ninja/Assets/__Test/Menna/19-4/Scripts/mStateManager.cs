//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class mStateManager : MonoBehaviour
//{
//    enum States { Idle, Chase, Attack };
//    States currentState;

//    mIdle mIdle;
//    private GameObject Player;
//    private Rigidbody RB;
//    void Start()
//    {
//        Player = GameObjectsManager.Instance.Player;
//        RB = GetComponent<Rigidbody>();
//        currentState = States.Idle;
//    }

//    void Update()
//    {
//        ChangeState(currentState);
//    }

//    private void ChangeState(States currentState)
//    {
//        switch (currentState)
//        {
//            case States.Idle: Debug.Log("Idle"); mIdle.Idle(); break;
//            case States.Chase: Debug.Log("CHASE"); Chase(); break;
//            case States.Attack: Debug.Log("ATTACK"); Attack(); break;
//        }
//    }


//}
