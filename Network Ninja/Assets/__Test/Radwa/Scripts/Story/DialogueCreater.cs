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

        Emotion e = Array.Find(currentLine.lineSpeaker.arrayOfEmotions,
            x => x.emotionType == currentLine.lineEmotion);
        

        if (currentLine.lineSpeaker.characterName == heroineName)
        {
            dialoguUI.txtName.text = heroineName;
            if (e.emotionSprite != null)
                dialoguUI.leftSpeakerImg.sprite = e.emotionSprite[0];
        }
        else
        {
            dialoguUI.txtName.text = allyName;
            if (e.emotionSprite != null)
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
