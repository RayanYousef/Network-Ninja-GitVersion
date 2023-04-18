//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class rSolidersManager : MonoBehaviour
//{
//    [SerializeField] rSolider meleePrefab;
//    [SerializeField] rSolider rangedPrefab;
//    [SerializeField] rSolider tankPrefab;

//    float meleeChance;
//    float rangedChance;
//    float tankChance;
//    float chance;

//    int cols = 5;
//    float spacing;

//    public void InstantiateSoliders(int rows, Soldiers soldiersType)
//    {
//        meleeChance = rangedChance = tankChance = 0;

//        switch (soldiersType)
//        {
//            case Soldiers.Melee:
//                meleeChance = 1;
//                break;
//            case Soldiers.Ranged:
//                rangedChance = 1;
//                break;
//            case Soldiers.MeleeRanged:
//                meleeChance = 0.4f;
//                rangedChance = 0.6f;
//                break;
//            case Soldiers.MeleeRangedTank:
//                meleeChance = 0.2f;
//                rangedChance = 0.3f;
//                tankChance = 0.5f;
//                break;
//        }


//        for (int i = 0; i < cols; i++)
//        {
//            for(int j = 0; j < rows; j++)
//            {
//                spacing = Random.Range(-3f, 3f);
//                //Debug.Log($"Spacing {spacing}");
//                Vector3 position = new Vector3(i * spacing, 0.5f, j * spacing);

//                chance = Random.Range(0f, 1f);
//                Debug.Log($"Chance {chance}");
//                if(chance < meleeChance)
//                {
//                    rSolider solider = Instantiate(meleePrefab, position, Quaternion.identity) as rSolider;
//                }
//                else if(chance < rangedChance)
//                {
//                    rSolider solider = Instantiate(rangedPrefab, position, Quaternion.identity) as rSolider;
//                }
//                else
//                {
//                    rSolider solider = Instantiate(tankPrefab, position, Quaternion.identity) as rSolider;
//                }
//            }
//        }
//    }
//}
