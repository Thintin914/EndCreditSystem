using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditRoll : MonoBehaviour
{
    [System.Serializable]
    public class TextType
    {
        public int jumpLine;
        public enum FontType { Pixel, Arial, Sketch }
        public enum FontColor { white, black, red, orange, yellow, green, tint, blue, purple}
        public FontColor color;
        public FontType font;
        public float xOffset;
        public float sizePercentageReduce;
        public string message;
    }

    public int maxFontSize;
    public GameObject TextPrefab, attachParent;
    public string[] fontName;
    public List<TextType> texts;

    [HideInInspector]public TextMeshProUGUI ingameText;

    private void Start()
    {
        CreateRollingText();
        ingameText.text = GenerateTextLogic();
        Show();
    }

    public string GenerateTextLogic()
    {
        string logicString = null;
        for(int i = 0; i < texts.Count; i++)
        {
            if (texts[i].jumpLine > 0)
            {
                for (int j = 0; j < texts[i].jumpLine; j++)
                {
                    logicString += "\n";
                }
            }
            if (texts[i].font == TextType.FontType.Pixel)
            {
                logicString += "<font=\"" + fontName[0] + "\">";
            }
            else if (texts[i].font == TextType.FontType.Arial)
            {
                logicString += "<font=\"" + fontName[1] + "\">";
            }
            else
            {
                logicString += "<font=\"" + fontName[2] + "\">";
            }
            logicString += "<align=\"" + "left" + "\">";

            logicString += "<size=" + (100 - texts[i].sizePercentageReduce) + "%>";
            logicString += "<color=#" + GetColorCode(texts[i].color) + ">";
            logicString += "<space=" + texts[i].xOffset + "em>";

            logicString = logicString + texts[i].message;

            logicString += "</color>";
        }
        return logicString;
    }

    private string GetColorCode(TextType.FontColor color)
    {
        if (color == TextType.FontColor.white)
            return "FFFFFF";
        if (color == TextType.FontColor.black)
            return "000000";
        if (color == TextType.FontColor.red)
            return "FD4040";
        if (color == TextType.FontColor.orange)
            return "FDA740";
        if (color == TextType.FontColor.yellow)
            return "FDE440";
        if (color == TextType.FontColor.green)
            return "2ED54D";
        if (color == TextType.FontColor.tint)
            return "31DE96";
        if (color == TextType.FontColor.blue)
            return "376EF7";
        if (color == TextType.FontColor.purple)
            return "5D1F77";
        return "FFFFFF";
    }

    public void CreateRollingText()
    {
        if (ingameText != null)
        {
            Destroy(ingameText.gameObject);
            ingameText = null;
        }
        ingameText = Instantiate(TextPrefab, attachParent.transform).GetComponent<TextMeshProUGUI>();
    }

    public void Show()
    {
        if (ingameText != null)
            ingameText.enabled = true;
    }
    public void Hide()
    {
        if (ingameText != null)
            ingameText.enabled = false;
    }
    public void Destroy()
    {
        if (ingameText != null)
        {
            Destroy(ingameText.gameObject);
            ingameText = null;
        }
    }

}
