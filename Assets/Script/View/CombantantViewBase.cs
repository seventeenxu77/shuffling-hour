using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombantantViewBase : MonoBehaviour
{
    [SerializeField]private SpriteRenderer spriteRenderer;
    [SerializeField]private TMP_Text HealthText;
    [SerializeField]public int MaxHealth{get;private set;}
    [SerializeField]public int CurrentHealth{get;private set;}
    public void SetupBase(SpriteRenderer sprite,int health)
    {
        spriteRenderer = sprite;
        MaxHealth = health;
        CurrentHealth = health;
        UpdateHealthText();
    }
    public void UpdateHealthText()
    {
        HealthText.text = CurrentHealth.ToString();
    }


}
