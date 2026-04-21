using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyCreator : Singleton<EnermyCreator>
{
    [SerializeField]private EnermyView enermyviewprefab;
    public EnermyView Createenermy(EnermyData data,Vector3 position,Quaternion rotation)
    {
        EnermyView enermyView=Instantiate(enermyviewprefab,position,rotation);
        enermyView.SetUp(data);
        return enermyView;
    }
}
