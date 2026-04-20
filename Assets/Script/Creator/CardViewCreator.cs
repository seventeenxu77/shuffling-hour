using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private Cardview Cardprefab;
    public Cardview CardViewCreate( Card card,Vector3 pos,Quaternion rotation)
    {
        Cardview cardview=Instantiate(Cardprefab,pos,rotation);
        cardview.transform.localScale=Vector3.zero;
        cardview.transform.DOScale(Vector3.one,0.15f);
        cardview.Setup(card);
        return cardview;
    }
}
