using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class ColourChanger : MonoBehaviour
{
    public Slider Red;
    public Slider Green;
    public Slider Blue;
    public Image PlayerBody;
    public TMP_Text RedText;
    public TMP_Text GreenText;
    public TMP_Text BlueText;
    public TMP_InputField RedInput;
    public TMP_InputField GreenInput;
    public TMP_InputField BlueInput;
    public TMP_InputField Name;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerBody.color = new Color(Red.value, Green.value, Blue.value);
        RedText.text = Mathf.RoundToInt((255 * Red.value)).ToString();
        BlueText.text = Mathf.RoundToInt((255 * Blue.value)).ToString();
        GreenText.text = Mathf.RoundToInt((255 * Green.value)).ToString();
    }

    public void SetRed()
    {
        int RedColour;
        int.TryParse(RedInput.text, out RedColour);;
        Red.value = (float)RedColour/255;
    }
    public void SetGreen()
    {
        int GreenColour;
        int.TryParse(GreenInput.text, out GreenColour);
        Green.value = (float)GreenColour/255;
    }
    public void SetBlue()
    {
        int BlueColour;
        int.TryParse(BlueInput.text, out BlueColour);
        Blue.value = (float)BlueColour/255;
    }

    public void RandomColour()
    {
        Red.value = (float)Random.Range(0, 255)/255;
        Blue.value = (float)Random.Range(0, 255)/255;
        Green.value = (float)Random.Range(0, 255)/255;
    }
    public void done()
    {
        if(Name.text != "")
        {
            PlayerValues.PlayerName = Name.text;
        }
        else
        {
            PlayerValues.PlayerName = "John";
        }
        PlayerValues.PlayerColor = PlayerBody.color;
        float[] colour;
        colour = new float[3];
        colour[0] = PlayerBody.color.r;
        colour[1] = PlayerBody.color.g;
        colour[2] = PlayerBody.color.b;
        InventorySaveItem[] New = new InventorySaveItem[20];
        PlayerValues.Inventory = New;
        SaveAndLoad.SaveCharacter(Name.text, colour, New);
        SceneManager.LoadScene("CharacterSelection");
    }

}

public static class PlayerValues
{
    public static Color PlayerColor { get; set; }
    public static string PlayerName { get; set; }
    public static InventorySaveItem[] Inventory { get; set; }
}
