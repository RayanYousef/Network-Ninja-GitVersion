using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
public class rCard : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cardCam;
    [SerializeField] Animator anim;

    [Header("Card Canvas")]
    [SerializeField] CanvasGroup cardPanel1;
    [SerializeField] CanvasGroup cardPanel2;
    [SerializeField] float timeToFadeIn;
    [SerializeField] Image ledImg;
    [SerializeField] Button skipBtn;

    [SerializeField] TMP_Text infoTxt;

    void Start()
    {
        cardPanel1.alpha = 0;
        cardPanel2.alpha = 0;
        GameObjectsManager.Instance.CameraBrain.m_DefaultBlend.m_Time = 1.0f;
        anim.SetBool("startRotation", true);
        cardCam.enabled = true;

        skipBtn.onClick.AddListener(OnClkSkip);

        if(GameManager.Instance.GameLang == GameLang.English)
        {
            infoTxt.text = "Ransomware is a type of virus that locks up important files on your computer and asks for money to make them accessible again.";
        }
        else
        {
            infoTxt.text = "الرانسموير هو نوع من الفيروسات، اللي بيقفل الملفات الهامة في جهازك وبيطلب فلوس لفتحهم مرة تانية، قاتله لتحمي جهازك.";
        }

    }

    public void CardAppeared()
    {
        rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeInPanel(cardPanel1, timeToFadeIn));
    }

    public void CardPlaced()
    {
        // play video
        rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeInPanel(cardPanel2, timeToFadeIn / 2));
        
        // enable cursor, player controllers
        ShowCursorAndStopInputs(true);

        // light leds
        ledImg.color = Color.white;

    }

    public void OnClkSkip()
    {
        // disable card cam
        cardCam.enabled = false;

        // disable cursor, player inputs
        ShowCursorAndStopInputs(false);

        // hide card canvas
        rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeOutPanel(cardPanel1, timeToFadeIn / 2));
        rUIManager.Instance.StartCoroutine(rUIManager.Instance.FadeOutPanel(cardPanel2, timeToFadeIn / 2));

        // spawn big boss
        rAreasManager.Instance.CurrentArea.EnemySpawner.StartCoroutine(nameof(rAreasManager.Instance.CurrentArea.EnemySpawner.SpawnBossCoroutine));
        
        // hide card parent
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    private void ShowCursorAndStopInputs(bool value)
    {
        switch (value)
        {
            case true:
                GameObjectsManager.Instance.Player.GetComponent<CS_PlayerManager>().ControllerState(false);
                Cursor.lockState = CursorLockMode.Confined;
                break;

            case false:
                GameObjectsManager.Instance.Player.GetComponent<CS_PlayerManager>().ControllerState(true);
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }
}
