using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnermyBoardView : MonoBehaviour
{
    [SerializeField]private List<Transform> slots;
    [SerializeField]public List<EnermyView> enermyViews=new();
    [SerializeField]private EnermyView enermyPrefab;
    public void addenermy(EnermyData data)
    {
        Transform slot=slots[enermyViews.Count];
        EnermyView enermyView=EnermyCreator.instance.Createenermy(data,slot.position,slot.rotation);
        enermyViews.Add(enermyView);
    }
    public IEnumerator removeenermy(EnermyView enermyView)
    {
        enermyViews.Remove(enermyView);
        Tween tween=enermyView.transform.DOScale(Vector3.zero,0.25f);
        yield return tween.WaitForCompletion();
        Destroy(enermyView.gameObject);
    }
}
