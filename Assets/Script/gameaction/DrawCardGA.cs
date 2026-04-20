using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardGA : GameAction
{
    public int Amount{get;set;}
    public DrawCardGA(int amount)
    {
        Amount=amount;
    }
}
