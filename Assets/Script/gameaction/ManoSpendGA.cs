using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManoSpendGA : GameAction
{
    public int Amount{get;set;}
    public ManoSpendGA(int amount)
    {
        Amount = amount;
    }
}
