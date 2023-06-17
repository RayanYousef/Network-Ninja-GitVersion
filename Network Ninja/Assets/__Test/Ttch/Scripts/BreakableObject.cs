using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public enum StatToChange
{
    Health, Energy, MovementSpeed, None
}
public class BreakableObject : MonoBehaviour
{
    [SerializeField] public StatToChange stat = StatToChange.None;
    [SerializeField] public TextMeshProUGUI text;

    [SerializeField] public Material healthMat;
    [SerializeField] public Material energyMat;
    [SerializeField] public Material speedMat;

    public float value;
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
                        value = dmgObject.MyStatsManager.Stats.MaxHealth / 6;
                        text.text = stat.ToString() + " + " + value;
                        text.fontSharedMaterial = healthMat;
                        //text.color = Color.green;
                        text.GetComponent<Animator>().Play("Base Layer.Collectible");
                        dmgObject.MyStatsManager.Heal(value);
                        break;

                    case StatToChange.Energy:

                        value = dmgObject.MyStatsManager.Stats.DefaultEnergy / 4;
                        text.text = stat.ToString() + " + " + value;
                        text.fontSharedMaterial = energyMat;
                        //text.color = Color.blue;
                        text.GetComponent<Animator>().Play("Base Layer.Collectible");
                        dmgObject.MyStatsManager.AddtoEnergy(value);
                        break;

                    case StatToChange.MovementSpeed:
                        text.text = stat.ToString() + "+";
                        text.fontSharedMaterial = speedMat;
                        //text.color = Color.yellow;
                        text.GetComponent<Animator>().Play("Base Layer.Collectible");
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
