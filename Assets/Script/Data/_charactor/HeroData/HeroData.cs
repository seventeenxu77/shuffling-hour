using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Hero")]
public class HeroData : ScriptableObject
{
    [SerializeField]public int maxHealth;
    [SerializeField]public Sprite sprite;
    [SerializeField]public List<CardData> deckdata;
}
