using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnermySystem : Singleton<EnermySystem>
{
    [SerializeField]private EnermyBoardView enermyBoardView;
    public List<EnermyView>enermyViews=>enermyBoardView.enermyViews;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnermyTurnGA>(EnermyTurnPerform);
        ActionSystem.AttachPerformer<AttackHeroGA>(AttackHeroPerform);
        ActionSystem.AttachPerformer<KillEnermyGA>(KillEnermyPerform);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnermyTurnGA>();
        ActionSystem.DetachPerformer<AttackHeroGA>();
        ActionSystem.DetachPerformer<KillEnermyGA>();

    }
    public void SetUp(List<EnermyData> enermyDatas)
    {
        foreach(EnermyData data in enermyDatas)
        {
            enermyBoardView.addenermy(data);
        }
    }
    private IEnumerator EnermyTurnPerform(EnermyTurnGA ga)
    {
        foreach(EnermyView enermyView in enermyBoardView.enermyViews)
        {
            AttackHeroGA attackHeroGA=new AttackHeroGA(enermyView);
            ActionSystem.instance.AddReaction(attackHeroGA);
        }
        yield return null;
    }
    private IEnumerator AttackHeroPerform(AttackHeroGA ga)
    {
        EnermyView enermyView=ga.enermyView;
        Tween tween=enermyView.transform.DOMoveX(enermyView.transform.position.x-1f,0.15f);
        yield return tween.WaitForCompletion();
        tween=enermyView.transform.DOMoveX(enermyView.transform.position.x+1f,0.25f);
        DealDamageGA dealDamageGA=new(ga.enermyView.Attackpower,new(){HeroSystem.instance.heroView});
        ActionSystem.instance.AddReaction(dealDamageGA);
        yield return new WaitForSeconds(0.5f);
    }
    private IEnumerator KillEnermyPerform(KillEnermyGA ga)
    {
        yield return enermyBoardView.removeenermy(ga.enermyView);
    }
}
