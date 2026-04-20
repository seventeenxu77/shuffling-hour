using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardEffect : Effect
{    
    [SerializeField]private int amount;//设置为序列化可以在model卡牌的effect那里改抽牌数量

    public override GameAction GetGameAction()
    {
        return new DrawCardGA(amount);
    }
}
