using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchUpSystem : MonoBehaviour
{
    [SerializeField]private HeroData heroData;
    [SerializeField]private List<EnermyData> enermyDatas;
    private void Start()
    {
        HeroSystem.instance.Setup(heroData);
        CardSystem.instance.Setup(heroData.deckdata);
        EnermySystem.instance.SetUp(enermyDatas);
        DrawCardGA draw=new(5);
        ActionSystem.instance.Perform(draw);
    }
}
