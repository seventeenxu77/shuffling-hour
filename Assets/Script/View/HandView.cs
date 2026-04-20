using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    private readonly List<Cardview> cards = new List<Cardview>();
    public IEnumerator Addcard(Cardview card)
    {
        cards.Add(card);
        yield return UpdateCardPos(0.15f);
    }
    public Cardview RemoveCard(Card card)
    {
        Cardview cardview=GetCardview(card);
        if (!cardview)return null;
        cards.Remove(cardview);
        StartCoroutine(UpdateCardPos(0.15f));//这里进行弃牌后排序的动画
        return cardview;
    }
    public Cardview GetCardview(Card card)
    {
        return cards.Where(Cardview=>Cardview.card==card).FirstOrDefault();
    }
    public IEnumerator UpdateCardPos(float duration)
    {
        if(cards.Count==0)yield break;
        Spline spline = splineContainer.Spline;
        float CardSpacing = 1f /10f;
        float FirstPos = 0.5f-CardSpacing * (cards.Count - 1) / 2;
        for (int i = 0; i < cards.Count; i++)
        {
            float p=FirstPos + CardSpacing * i;
            Vector3 targetPos= spline.EvaluatePosition(p);
            Vector3 forward =spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);//up是z轴正方向，表现在卡牌的背面指向深处
            Quaternion targetRot = Quaternion.LookRotation(-up,Vector3.Cross(-up,forward).normalized);
            //前面参数表示正面朝向，后面餐宿表示上方向，因为这里forward向右，所以叉乘结果是卡牌应该在的位置的应该有的上方朝向。
            cards[i].transform.DOMove(targetPos+transform.position + 0.01f*i*Vector3.back,duration);//避免卡牌重叠，线条本地位置+全局位置。
            cards[i].transform.DORotate(targetRot.eulerAngles,duration);//欧拉角存储的是三个轴的角度，代替了之前两个向量的表示方法，现在一个向量表示了。
        }
        yield return new WaitForSeconds(duration);
    }
}
