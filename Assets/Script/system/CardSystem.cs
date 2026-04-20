using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    public List<Card>drawPile=new List<Card>();//注意这里要初始化
    public List<Card>hand=new List<Card>();
    public List<Card>discardPile=new List<Card>();
    public Transform drawPilePos;
    public Transform discradPilePos;

    public HandView handview;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardGA>(DrawcardPerformer);
        ActionSystem.AttachPerformer<DiscardCardGA>(DiscardcardPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
        ActionSystem.SubscribeReaction<EnermyTurnGA>(EnermyTurnPreReaction,ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnermyTurnGA>(EnermyTurnPostReaction,ReactionTiming.POST);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardGA>();
        ActionSystem.DetachPerformer<DiscardCardGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
        ActionSystem.UnsubscribeReaction<EnermyTurnGA>(EnermyTurnPreReaction,ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnermyTurnGA>(EnermyTurnPostReaction,ReactionTiming.POST);
    }
    public void Setup(List<CardData> carddata)
    {
        foreach(var data in carddata)
        {
            Card card=new(data);
            drawPile.Add(card);
        }
    }
    private IEnumerator DrawcardPerformer(DrawCardGA drawactionga)
    {
        int actualdraw=Mathf.Min(drawactionga.Amount,drawPile.Count);
        int notdraw=drawactionga.Amount-actualdraw;
        for(int i=0;i<actualdraw;i++)yield return DrawCard();
        if (notdraw > 0)
        {
            deckfill();
            for(int i=0;i<notdraw;i++)yield return DrawCard();
        }
    }
    private IEnumerator DiscardcardPerformer(DiscardCardGA action)
    {
        foreach(var card in hand)
        {
            discardPile.Add(card);
            Cardview cardview=handview.RemoveCard(card);
            yield return DisCardCard(cardview);
        }
        hand.Clear();//注意不要在foreach内部使用hand.Remove(card);否则遍历失败
    }
    private IEnumerator PlayCardPerformer(PlayCardGA action)
    {
        Card card=action.card;
        hand.Remove(card);
        discardPile.Add(card);
        Cardview cardview=handview.RemoveCard(card);
        yield return DisCardCard(cardview);
        ManoSpendGA manoSpendGA=new(card.Mano);
        ActionSystem.instance.AddReaction(manoSpendGA);
        foreach(Effect effect in card.Effects)
        {
            PerformEffectGA gameaction=new(effect);
            ActionSystem.instance.AddReaction(gameaction);
            Debug.Log("添加effectga进入反应队列");
        }
    }
    private IEnumerator DrawCard()
    {
        Card card=drawPile.draw();
        hand.Add(card);
        Cardview cardview=CardViewCreator.instance.CardViewCreate(card,drawPilePos.position,drawPilePos.rotation);
        yield return handview.Addcard(cardview);
    }
    private void deckfill()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
    }
    private IEnumerator DisCardCard(Cardview card)
    {
        Debug.Log("丢弃卡牌");
        //这里的动画我觉得处理的不太好，因为这里只是恰好因为两个时间一致，但是如果时间不一致就可能出现移动着突然消失的效果，应该用sequence会好一点。
        card.transform.DOScale(Vector3.zero,0.15f);
        Tween tween=card.transform.DOMove(discradPilePos.position,0.15f);
        yield return tween.WaitForCompletion();
        Destroy(card.gameObject);//这里一定要记住摧毁卡牌

    }
    private void EnermyTurnPreReaction(EnermyTurnGA enermyturnga)
    {
        DiscardCardGA discard=new();
        ActionSystem.instance.AddReaction(discard);
    }
    private void EnermyTurnPostReaction(EnermyTurnGA enermyturnga)
    {
        DrawCardGA draw=new(5);
        ActionSystem.instance.AddReaction(draw);

    }
}
