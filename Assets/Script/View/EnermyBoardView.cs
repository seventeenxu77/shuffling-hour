using System.Collections;
using System.Collections.Generic;
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
}
