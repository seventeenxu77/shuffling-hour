using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnermyView : CombantantViewBase
{
    [SerializeField]private TMP_Text attack;
    public int Attackpower{get;private set;}
    public void SetUp(EnermyData data)
    {
        Attackpower=data.attackPower;
        UpdateAttackText();
        SetupBase(data.sprite,data.maxHealth);
    }
    void UpdateAttackText()
    {
        attack.text="Attack:" + Attackpower.ToString();
    }
}
