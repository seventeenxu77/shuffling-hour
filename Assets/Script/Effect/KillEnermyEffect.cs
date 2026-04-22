using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnermyEffect : Effect
{
    private int amount;
    public override GameAction GetGameAction()
    {
        DealDamageGA dealDamageGA = new(amount,new(EnermySystem.instance.enermyViews));
        return dealDamageGA;
    }
}
