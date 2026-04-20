using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : Singleton<InteractionSystem>
{
    public bool PlayerIsDragging { get; set; } = false;
    public bool PlayerCanInteract()
    {
    if(!ActionSystem.instance.isPerforming)return true;
        return false;
    }
    public bool PlayerCanHovering()
    {
        if(!PlayerIsDragging)return true;
        return false;
    }
}
