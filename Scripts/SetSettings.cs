using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetSettings : MonoBehaviour
{
    public Image Settings;
    public void setOutline(Sprite image)
    {
        Settings.sprite = image;
        settings.Outline = image;
        Debug.Log(image.name);
    }

    public void back()
    {
        SceneManager.LoadScene("CharacterSelection");
    }
}

public static class settings
{
    public static Sprite Outline { get; set; }
}
