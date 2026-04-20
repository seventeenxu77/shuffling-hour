using System.Collections;
using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Card")]
public class CardData : ScriptableObject
{
    
    [field:SerializeField] public string Description {get;private set;}
    [field:SerializeField] public int Mano {get;private set;}
    [field:SerializeField] public Sprite image {get;private set;}
    [field:SerializeReference,SR] public List<Effect> effects {get;private set;}//抽象类为了在面板更改序列化
}
