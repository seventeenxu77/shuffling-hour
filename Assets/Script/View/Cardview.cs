using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Cardview : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text mamo;
    [SerializeField] private SpriteRenderer image;//这个是负责实际渲染的，而Sprite更像是一份公用的数据。
    [SerializeField] private GameObject wrapper;
    [SerializeField] private LayerMask droplayer;

    private Vector3 DragStartPos;
    private Quaternion DragStartRot;
    public Card card{set;get;}
    public void Setup(Card data)
    {
        card=data;
        title.text=data.Title;
        description.text=data.Description;
        mamo.text=data.Mano.ToString();
        image.sprite=data.Image;
    }
    void OnMouseEnter()
    {
        if(!InteractionSystem.instance.PlayerCanHovering())return;
        wrapper.SetActive(false);
        Vector3 pos=new(transform.position.x,-2,0);
        CardViewHoversystem.instance.show(card,pos);
    }
    void OnMouseExit()
    {
        if(!InteractionSystem.instance.PlayerCanHovering())return;
        CardViewHoversystem.instance.hide();
        wrapper.SetActive(true);
    }
    void OnMouseDown()
    {
        if(!InteractionSystem.instance.PlayerCanInteract())return;
        if(InteractionSystem.instance.PlayerCanInteract())
        {
            CardViewHoversystem.instance.hide();
            wrapper.SetActive(true);
            InteractionSystem.instance.PlayerIsDragging=true;
            DragStartRot=transform.rotation;
            DragStartPos=transform.position;
            transform.rotation=quaternion.Euler(0,0,0);
            transform.position=MouseUtil.GetMouseWorldPos(-1);
        }
    }
    void OnMouseUp()
    {
        if(!InteractionSystem.instance.PlayerCanInteract())return;
        Debug.DrawRay(transform.position, Vector3.forward * 10, Color.red, 10f);
        if(ManoSystem.instance.HasMano(card.Mano)&& Physics.Raycast(transform.position,Vector3.forward,out RaycastHit hit,10f,droplayer))
        {
            Debug.Log("射线命中，触发打出卡牌");//之前没有触发是因为改动的是层级面板hover那个card，没有更改预制体prefab
            PlayCardGA action=new(card);
            ActionSystem.instance.Perform(action);
        }
        else
        {
            transform.position=DragStartPos;
            transform.rotation=DragStartRot;
        }
        InteractionSystem.instance.PlayerIsDragging=false;
    }
    void OnMouseDrag()
    {
        if(!InteractionSystem.instance.PlayerCanInteract())return;
        transform.position=MouseUtil.GetMouseWorldPos(-1);

    }
}
