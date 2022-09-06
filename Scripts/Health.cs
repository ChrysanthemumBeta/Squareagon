using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int _Health = 6;
    public int MaxHealth = 6;
    public int HealthPerHeart = 2;
    public Image[] HealthBar;
    public Sprite Full;
    public Sprite Empty;
    public Sprite Half;

    private void Start()
    {
        TakeDamage(0);
    }

    public void TakeDamage(int amount)
    {
        if(amount >= _Health)
        {
            Debug.Log("Dead");
        }
        else
        {
            _Health -= amount;
        }
        UpdateHealth();
    }

    void UpdateHealth()
    {
        for (int i = 0; i < HealthBar.Length; i++)
        {
            HealthBar[i].sprite = Empty;
        }

        if(_Health % HealthPerHeart == 0)
        {
            for (int i = 0; i < _Health/HealthPerHeart; i++)
            {
                HealthBar[i].sprite = Full;
            }
        }
        else
        {
            if(_Health != 0)
            {
                int CurrentHealth = _Health - 1;
                
                for (int i = 0; i < CurrentHealth / HealthPerHeart; i++)
                {
                    HealthBar[i].sprite = Full;
                }
                HealthBar[(CurrentHealth / HealthPerHeart)].sprite = Half;

            }
        }
    }
}
