using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroView : CombantantViewBase
{
    public void SetUp(HeroData data)
    {
        SetupBase(data.sprite, data.maxHealth);
    }
}
