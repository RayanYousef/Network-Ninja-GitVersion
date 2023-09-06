using UnityEngine;

public enum TXT_EFFECT { TypeWriting, Fade }

[System.Serializable]

public class TextEffects
{
    public TXT_EFFECT TextEffectType;

    [Range(-1, 1)]
    public int shakeVal;

    [Range(-1, 1)]
    public float speedVal;

    public void ApplyTextEffects(TextArchitect architect, Line currentLine)
    {
        switch (TextEffectType)
        {
            case TXT_EFFECT.TypeWriting:
            architect.buildMethod = TextArchitect.BuildMethod.typewriter;
                break;

            case TXT_EFFECT.Fade:
                architect.buildMethod = TextArchitect.BuildMethod.fade;
                break;

            default:
                break;
        }

        if (architect.isBuilding)
        {
            architect.ForceComplete();
        }
        else
        {
            if (currentLine.lineType == LINETYPE.New)
                architect.Build(currentLine.says);
            else
                architect.Append(currentLine.says);
        }
    }
}
