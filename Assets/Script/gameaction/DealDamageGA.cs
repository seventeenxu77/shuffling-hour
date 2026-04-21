using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageGA : GameAction
{
    public int amount{get;set;}
    public List<CombantantViewBase>targets=new();
    public DealDamageGA(int amount,List<CombantantViewBase>targets)
    {
        this.amount=amount;
        this.targets=new(targets);
    }
}
