using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Card
{
    private readonly CardData carddata;
    public string Title=>carddata.name;
    public Sprite Image=>carddata.image;
    public string Description=>carddata.Description;
    public int Mano{get;private set;}
    public List<Effect> Effects=>carddata.effects;
    public Card(CardData _card)
    {
        carddata=_card;
        Mano=_card.Mano;
    }
}