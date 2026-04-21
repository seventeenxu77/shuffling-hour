using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Enermy")]
public class EnermyData : ScriptableObject
{
    [SerializeField]public int maxHealth;
    [SerializeField]public Sprite sprite;
    [SerializeField]public int attackPower;
}
