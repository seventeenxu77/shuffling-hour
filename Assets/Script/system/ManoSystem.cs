using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManoSystem : Singleton<ManoSystem>
{
    [SerializeField] private ManoUI manoUI;
    private const int maxMano=3;//如果要赋予常量，必须用const，因为const是在编译期，而现在直接赋值实例还没有值。
    private int currentMano=maxMano;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<ManoSpendGA>(ManoSpendPerformer);
        ActionSystem.AttachPerformer<ManorefillGA>(ManoFillPerformer);
        ActionSystem.SubscribeReaction<EnermyTurnGA>(EnermyTurnPostReaction,ReactionTiming.POST);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<ManoSpendGA>();
        ActionSystem.DetachPerformer<ManorefillGA>();
        ActionSystem.UnsubscribeReaction<EnermyTurnGA>(EnermyTurnPostReaction,ReactionTiming.POST);
    }
    private IEnumerator ManoSpendPerformer(ManoSpendGA action)
    {
        currentMano -= action.Amount;
        manoUI.UpdateManoText(currentMano);
        yield return null;
    }
    private IEnumerator ManoFillPerformer(ManorefillGA action)
    {
        currentMano = maxMano;
        manoUI.UpdateManoText(currentMano);
        yield return null;
    }
    public bool HasMano(int amount)
    {
        return currentMano >= amount;
    }

    private void EnermyTurnPostReaction(EnermyTurnGA enermyturnga)
    {
        ManorefillGA refill=new();
        ActionSystem.instance.AddReaction(refill);
    }
}
