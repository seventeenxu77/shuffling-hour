using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCardGA : GameAction
{
    public Card card;

    public PlayCardGA(Card card)
    {
        this.card = card;
    }

}
