using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchUpSystem : MonoBehaviour
{
    [SerializeField]private List<CardData>deskdata;
    private void Start()
    {
        CardSystem.instance.Setup(deskdata);
        DrawCardGA draw=new(5);
        ActionSystem.instance.Perform(draw);
    }
}
