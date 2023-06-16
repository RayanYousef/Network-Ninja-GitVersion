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

    private void Update()
    {
        transform.Rotate(0, 25 * Time.deltaTime, 0, Space.Self);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CS_DamageObject>(out CS_DamageObject dmgObject))
        {
            if(dmgObject.MyStatsManager.Team == CharacterTeam.Player)
            {
                switch (stat)
                {
                    case StatToChange.Health:
                        dmgObject.MyStatsManager.Heal(dmgObject.MyStatsManager.Stats.MaxHealth / 6);
                        break;

                    case StatToChange.Energy:
                        dmgObject.MyStatsManager.AddtoEnergy(dmgObject.MyStatsManager.Stats.DefaultEnergy / 4);
                        break;

                    case StatToChange.MovementSpeed:
                        dmgObject.MyStatsManager.BuffMoveSpeed(0.5f);
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
