using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardViewHoversystem : Singleton<CardViewHoversystem>
{
    [SerializeField]Cardview cardviewhover;
    public void show(Card card,Vector3 position)
    {
        cardviewhover.gameObject.SetActive(true);
        cardviewhover.Setup(card);
        cardviewhover.transform.position=position;
    }
    public void hide()
    {
        cardviewhover.gameObject.SetActive(false);
    }
}
