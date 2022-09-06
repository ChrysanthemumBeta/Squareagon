using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    public TMP_Text Name;
    public Image body;
    public GameObject self;
    public void SelectSelf()
    {
        PlayerValues.PlayerName = Name.text;
        PlayerValues.PlayerColor = body.color;
        PlayerValues.Inventory = SaveAndLoad.LoadCharacter(Name.text).Inv;
        SceneManager.LoadScene("Game");
    }

    public void New()
    {
        SceneManager.LoadScene("CharacterCustomisation");
    }

    

    public void Delete()
    {
        SaveAndLoad.DeleteProfile(Name.text);
        Destroy(self);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
}
