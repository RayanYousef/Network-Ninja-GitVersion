using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatToChange
{
    Health, Energy, MovementSpeed, None
}
public class BreakableObject : MonoBehaviour
{
    [SerializeField] public StatToChange stat = StatToChange.None;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CS_DamageObject>(out CS_DamageObject dmgObject))
        {
            if(dmgObject.MyStatsManager.Team == CharacterTeam.Player)
            {
                switch (stat)
                {
                    case StatToChange.Health:
                        dmgObject.MyStatsManager.Heal(400);
                        break;

                    case StatToChange.Energy:
                        dmgObject.MyStatsManager.BuffEnergy();
                        break;

                    case StatToChange.MovementSpeed:
                        dmgObject.MyStatsManager.BuffMoveSpeed(1.3f);
                        break;

                    case StatToChange.None:
                        break;
                    default:
                        break;
                }

                this.gameObject.SetActive(false);
            }
        }
    }
}
