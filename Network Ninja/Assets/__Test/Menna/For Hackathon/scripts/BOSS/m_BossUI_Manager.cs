using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class m_BossUI_Manager : MonoBehaviour
{
  // [SerializeField] private Slider healthBarSprite;

    [SerializeField] private Image bloodSplatter;
    [SerializeField] private Color transparentColor;
    [SerializeField] private Color color;

    //public m_CombatManager combatManager;

    private void Awake()
    {
        color = new Color(188f, 0f, 0f, 1f);
        transparentColor = new Color(0f, 0f, 0f, 0f);
    }

    //public void UpdateHealthBar(float maxHealth, float currentHealth)
    //{
    //    healthBarSprite.value = currentHealth / maxHealth;
    //}

    public IEnumerator DoFade()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 5f)
        {
            bloodSplatter.color = Color.Lerp(color, transparentColor, (elapsedTime / 5));
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}
