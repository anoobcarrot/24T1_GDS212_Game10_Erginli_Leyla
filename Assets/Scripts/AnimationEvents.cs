using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public GameObject targetGameObject;

    public void CallSetPunchingFalse()
    {
        if (targetGameObject != null)
        {
            targetGameObject.SendMessage("SetPunchingFalse", SendMessageOptions.RequireReceiver);
        }
        else
        {
            Debug.LogError("Target GameObject is null!");
        }
    }

    public void CallSetCastingFalse()
    {
        if (targetGameObject != null)
        {
            targetGameObject.SendMessage("SetCastingFalse", SendMessageOptions.RequireReceiver);
        }
        else
        {
            Debug.LogError("Target GameObject is null!");
        }
    }

    public void CallSetCastingAttackFalse()
    {
        if (targetGameObject != null)
        {
            targetGameObject.SendMessage("SetCastingAttackFalse", SendMessageOptions.RequireReceiver);
        }
        else
        {
            Debug.LogError("Target GameObject is null!");
        }
    }
}

