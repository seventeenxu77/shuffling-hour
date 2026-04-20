using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 卡牌动作系统
/// </summary>
public class ActionSystem : Singleton<ActionSystem>
{
    private List<GameAction> reactions = null;

    /// <summary>
    /// 是否正在执行动作
    /// </summary>
    public bool isPerforming { get; private set; } = false;

    private static Dictionary<Type, List<Action<GameAction>>> preSubs = new();
    private static Dictionary<Type, List<Action<GameAction>>> postSubs = new();
    private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();

    // 存储原始委托和包装委托的映射
    private static Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> preWrapperMap = new();
    private static Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> postWrapperMap = new();

    // 立即反应队列，要插队处理的队列（使用链表保证插入顺序）
    private LinkedList<GameAction> immediateReactions = new LinkedList<GameAction>();

    // 立即反应队列是否正在执行（避免立即执行队列中再执行立即执行队列）
    private bool isInImmediateReaction = false;

    /// <summary>
    /// 执行一个卡牌动作
    /// </summary>
    /// <param name="action">要执行的动作</param>
    /// <param name="OnPerformFinished">执行完成后的回调</param>
    public void Perform(GameAction action, Action OnPerformFinished = null)
    {
        if (isPerforming) return;

        isPerforming = true;

        // 以防事件意外中断导致此状态未重置
        isInImmediateReaction = false;

        StartCoroutine(Flow(action, () =>
        {
            isPerforming = false;
            OnPerformFinished?.Invoke();
        }));
    }

    /// <summary>
    /// 添加一个反应动作到当前执行流程
    /// </summary>
    /// <param name="gameAction">要添加的动作</param>
    public void AddReaction(GameAction gameAction)
    {
        reactions?.Add(gameAction);
    }

    /// <summary>
    /// 动作执行流程
    /// </summary>
    /// <param name="action">要执行的动作</param>
    /// <param name="OnFlowfinished">流程完成后的回调</param>
    /// <returns></returns>
    private IEnumerator Flow(GameAction action, Action OnFlowfinished = null)
    {
        reactions = action.PreReactions;
        PerformSubscribers(action, preSubs);
        yield return PerformReactions();

        reactions = action.PerformReactions;
        yield return PerformPerformer(action);
        yield return PerformReactions();

        reactions = action.PostReactions;
        PerformSubscribers(action, postSubs);
        yield return PerformReactions();

        OnFlowfinished?.Invoke();
    }

    /// <summary>
    /// 执行所有反应动作
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformReactions()
    {
        foreach (var reaction in reactions)
        {
            yield return PerformImmediateReactions();
            yield return Flow(reaction);
        }
        yield return PerformImmediateReactions();
    }

    /// <summary>
    /// 执行动作的执行器
    /// </summary>
    /// <param name="action">要执行的动作</param>
    /// <returns></returns>
    private IEnumerator PerformPerformer(GameAction action)
    {
        Type type = action.GetType();
        if (performers.ContainsKey(type))
        {
            yield return PerformImmediateReactions();
            yield return performers[type](action);
        }
        yield return PerformImmediateReactions();
    }

    /// <summary>
    /// 执行立即反应队列（插队队列）
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformImmediateReactions()
    {
        if (isInImmediateReaction) yield break;
        isInImmediateReaction = true;

        while (immediateReactions.Count > 0)
        {
            var reaction = immediateReactions.First.Value;
            immediateReactions.RemoveFirst();

            yield return Flow(reaction);
        }

        isInImmediateReaction = false;
    }

    /// <summary>
    /// 执行订阅者
    /// </summary>
    /// <param name="action">要执行的动作</param>
    /// <param name="subs">订阅者字典</param>
    private void PerformSubscribers(GameAction action, Dictionary<Type, List<Action<GameAction>>> subs)
    {
        Type currentType = action.GetType();

        // 遍历当前类型及其所有基类（直到GameAction）
        while (currentType != null && currentType != typeof(object))
        {
            if (subs.TryGetValue(currentType, out var subscribers))
            {
                // 创建副本避免修改
                var subscribersCopy = subscribers.ToArray();
                foreach (var sub in subscribersCopy)
                {
                    sub(action);
                }
            }

            // 向上遍历基类
            currentType = currentType.BaseType;
        }
    }

    /// <summary>
    /// 添加立即反应的事件（进行插队）
    /// </summary>
    /// <param name="gameAction">要添加的动作</param>
    public void AddImmediateReaction(GameAction gameAction)
    {
        immediateReactions.AddLast(gameAction);
    }

    /// <summary>
    /// 附加一个执行器到指定类型的动作
    /// </summary>
    /// <typeparam name="T">动作类型</typeparam>
    /// <param name="performer">执行器</param>
    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);
        IEnumerator wrappedPerformer(GameAction action) => performer((T)action);
        if (performers.ContainsKey(type)) performers[type] = wrappedPerformer;
        else performers.Add(type, wrappedPerformer);
    }

    /// <summary>
    /// 移除执行器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);
        if (performers.ContainsKey(type)) performers.Remove(type);
    }

    /// <summary>
    /// 移除执行器和注册到它身上的反应
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void DetachPerformerAndSubscribers<T>() where T : GameAction
    {
        Type type = typeof(T);
        DetachPerformer<T>();

        // 移除所有预订阅
        if (preSubs.ContainsKey(type))
        {
            preSubs[type].Clear();
        }
        if (preWrapperMap.ContainsKey(type))
        {
            preWrapperMap[type].Clear();
        }

        // 移除所有后订阅
        if (postSubs.ContainsKey(type))
        {
            postSubs[type].Clear();
        }
        if (postWrapperMap.ContainsKey(type))
        {
            postWrapperMap[type].Clear();
        }
    }

    /// <summary>
    /// 订阅一个动作的反应
    /// </summary>
    /// <typeparam name="T">动作类型</typeparam>
    /// <param name="reaction">反应委托</param>
    /// <param name="timing">时机（前/后）</param>
    public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> wrapperMap =
            timing == ReactionTiming.PRE ? preWrapperMap : postWrapperMap;

        Type type = typeof(T);

        // 创建包装委托
        Action<GameAction> wrappedReaction = action => reaction((T)action);

        // 确保事件类型对应的列表存在
        if (!subs.ContainsKey(type))
        {
            subs[type] = new List<Action<GameAction>>();
            wrapperMap[type] = new Dictionary<Delegate, Action<GameAction>>();
        }

        // 添加到订阅列表
        subs[type].Add(wrappedReaction);

        // 存储映射关系
        wrapperMap[type][reaction] = wrappedReaction;
    }

    /// <summary>
    /// 取消订阅一个动作的反应
    /// </summary>
    /// <typeparam name="T">动作类型</typeparam>
    /// <param name="reaction">反应委托</param>
    /// <param name="timing">时机（前/后）</param>
    public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> wrapperMap =
            timing == ReactionTiming.PRE ? preWrapperMap : postWrapperMap;

        Type type = typeof(T);

        if (!subs.ContainsKey(type) || !wrapperMap.ContainsKey(type))
            return;

        var wrapperDict = wrapperMap[type];
        // 找到对应的包装委托
        if (wrapperDict.TryGetValue(reaction, out var wrappedReaction))
        {
            // 从订阅列表中移除
            subs[type].Remove(wrappedReaction);
            // 从映射中移除
            wrapperDict.Remove(reaction);
        }
    }

    /// <summary>
    /// 停止正在执行的所有事件并清除所有反应<br/>
    /// 这句如果是在Card系统里调用，则要放到最后，因为它会中止所有的IEnumerator
    /// </summary>
    public void AbortAllActions()
    {
        // 停止所有协程（包括嵌套协程）
        StopAllCoroutines();

        // 清空反应列表
        reactions?.Clear();

        // 清空立即反应列表
        immediateReactions.Clear();

        isPerforming = false;
        isInImmediateReaction = false;
    }
}

/// <summary>
/// 反应时机
/// </summary>
public enum ReactionTiming
{
    PRE,
    POST,
}