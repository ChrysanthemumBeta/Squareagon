using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GetProfiles : MonoBehaviour
{
    public GameObject panel;
    public Transform parent;
    private void Start()
    {
        string[] profiles = SaveAndLoad.GetAllProfiles();

        foreach (string profile in profiles)
        {
            CharacterData data = SaveAndLoad.LoadCharacter(profile);
            GameObject Panel = Instantiate(panel, parent);
            Panel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color = new Color(data.Colour[0], data.Colour[1], data.Colour[2]);
            Panel.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = data.Name;
        }
    }
}
