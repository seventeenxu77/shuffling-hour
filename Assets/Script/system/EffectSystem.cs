using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : MonoBehaviour//system使用unity生命周期，因此一定要挂载在层级面部的物品上。
{
    void OnEnable()
    {
        ActionSystem.AttachPerformer<PerformEffectGA>(PlayCardPerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<PerformEffectGA>();
    }
    private IEnumerator PlayCardPerformer(PerformEffectGA action)
    {
        Debug.Log("监听到effectga动作，执行卡牌effect对应的行为");
        Effect effect = action.effect;
        GameAction gameAction = effect.GetGameAction();
        ActionSystem.instance.AddReaction(gameAction);
        yield return null;
    }
}
