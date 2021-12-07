using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

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

    public Vector2 startPosition, endPosition;
    public float scrollSecond;
    public int maxFontSize;
    public GameObject TextPrefab, attachParent;
    public string[] fontName;
    public List<TextType> texts;
    private bool isFromContructor;

    [HideInInspector]public TextMeshProUGUI ingameText;

    public CreditRoll(Vector2 startPosition, Vector2 endPosition, float seconds, GameObject parent, int maxFontSize, List<TextType> texts)
    {
        isFromContructor = true;

        this.startPosition = startPosition;
        this.endPosition = endPosition;
        scrollSecond = seconds;
        attachParent = parent;
        this.maxFontSize = maxFontSize;
        this.texts = texts;
    }

    private async void Start()
    {
        if (!isFromContructor)
        {
            CreateRollingText();
            ingameText.text = GenerateTextLogic();
            Show();
            bool finished = await ScrollCredit(startPosition , endPosition, scrollSecond);
        }
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
        ingameText.transform.position = startPosition;
        ingameText.transform.position += new Vector3(attachParent.transform.position.x, attachParent.transform.position.y);
        ingameText.fontSize = maxFontSize;
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

    public async Task<bool> ScrollCredit(Vector2 startPosition, Vector2 destination, float seconds)
    {
        await WaitScrollCredit(startPosition, destination, seconds);
        return true;
    }
    private async Task WaitScrollCredit(Vector2 startPosition, Vector2 destination, float seconds)
    {
        float endTime = Time.timeSinceLevelLoad + seconds;
        float currentTime = Time.timeSinceLevelLoad;
        float percentage = 0;
        Vector3 scrollStartPosition = startPosition;
        scrollStartPosition.x += attachParent.transform.position.x;
        scrollStartPosition.y += attachParent.transform.position.y;
        Vector3 endPosition = destination;
        endPosition.x += attachParent.transform.position.x;
        endPosition.y += attachParent.transform.position.y;

        try
        {
            do
            {
                currentTime += Time.deltaTime;
                percentage = currentTime / endTime;
                ingameText.transform.position = Vector3.Lerp(scrollStartPosition, endPosition, percentage);
                await Task.Yield();
            } while (percentage <= 1);
        }
        catch (MissingReferenceException e)
        {
            Debug.Log("Credit Roll System:\t---Credit Scrolling Was Ended Before It Finished---\n" + e);
        }
    }
}
