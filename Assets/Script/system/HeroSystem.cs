using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    [field:SerializeField]public HeroView heroView{get;private set;}
    public void Setup(HeroData data)
    {
        heroView.SetUp(data);
    }

}
