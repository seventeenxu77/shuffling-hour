using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformEffectGA : GameAction
{
    public Effect effect;

    public PerformEffectGA(Effect effect)
    {
        this.effect = effect;
    }
}
