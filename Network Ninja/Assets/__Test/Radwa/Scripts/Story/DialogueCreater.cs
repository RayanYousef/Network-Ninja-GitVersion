using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DialogueCreater : MonoBehaviour
{
    DialoguUI dialoguUI;
    TextArchitect architect;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float talkingSpeed;
    private bool isHeroineSpeaking;
    
    [SerializeField] int lineIndex;
    [SerializeField] Line[] lines;

    private Line currentLine;

    private string heroineName = "Daivolo";
    private string allyName = "Antivirus";

    void Start()
    {
        dialoguUI = DialoguUI.instance;
        currentLine = lines[lineIndex];
        architect = new TextArchitect(dialoguUI.txtParagraph);
        architect.speed = 0.5f;
        SetDialogue();
    }

    void Update()
    {
        if (lineIndex < lines.Length - 1 && lines[lineIndex].isSkippable)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(architect.isBuilding)
                {
                    if (!architect.hurryUp)
                        architect.hurryUp = true;
                    else
                        architect.ForceComplete();
                }
                else
                {
                    lineIndex++;
                    SetDialogue();
                }
            }
        }
    }

    public void SetDialogue()
    {
        currentLine = lines[lineIndex];

        PlayVoice();

        Emotion e = Array.Find(currentLine.lineSpeaker.arrayOfEmotions,
            x => x.emotionType == currentLine.lineEmotion);

        if (currentLine.lineSpeaker.characterName == heroineName)
        {
            isHeroineSpeaking = true;

            dialoguUI.txtName.text = heroineName;

            if(currentLine.lineEmotion == EMOTION.Talking)
            {
                StartCoroutine(LeftTalking(e));
            }
            else
                dialoguUI.leftSpeakerImg.sprite = e.emotionSprite[0];
        }
        else
        {
            isHeroineSpeaking = false;

            dialoguUI.txtName.text = allyName;
            if (currentLine.lineEmotion == EMOTION.Talking)
            {
                StartCoroutine(RightTalking(e));
            }
            else
                dialoguUI.rightSpeakerImg.sprite = e.emotionSprite[0];
        }

        if (currentLine.hasTextEffect)
        {
            foreach (TextEffects tes in currentLine.textEffects)
            {
                tes.ApplyTextEffects(architect, currentLine);
            }
        }


        if (currentLine.hasCamEffect)
        {
            CameraEffects ces = currentLine.camEffects;
            ces.ApplyCamEffects();
        }

    }
    private IEnumerator LeftTalking(Emotion e)
    {
        int len = e.emotionSprite.Length;
        for(int i = 0; i < len; i = (i+1) % len)
        {
            dialoguUI.leftSpeakerImg.sprite = e.emotionSprite[i];
            if (!isHeroineSpeaking)
            {
                dialoguUI.leftSpeakerImg.sprite = e.emotionSprite[0];
                yield break;
            }
            yield return new WaitForSeconds(talkingSpeed);
        }
    } 
    
    private IEnumerator RightTalking(Emotion e)
    {
        int len = e.emotionSprite.Length;
        for(int i = 0; i < len; i = (i+1) % len)
        {
            dialoguUI.rightSpeakerImg.sprite = e.emotionSprite[i];
            if (isHeroineSpeaking)
            {
                dialoguUI.rightSpeakerImg.sprite = e.emotionSprite[0];
                yield break;

            }
            yield return new WaitForSeconds(talkingSpeed);
        }
    }

    private void PlayVoice()
    {
        if(currentLine.voice != null)
        {
            if(audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource.PlayOneShot(currentLine.voice);
        }
    }

    private void ControlFontSize()
    {
        if (lines[lineIndex].isChangeFontSize)
        {
            dialoguUI.txtParagraph.fontSizeMax = lines[lineIndex].fontSize;
        }
        else
        {
            dialoguUI.txtParagraph.fontSizeMax = lines[lineIndex].DefualtFontSize;
        }
    }


}
