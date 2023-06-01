using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public RelativeScale relativeScale; // Reference to the RelativeScale script on the child

    public void EnableChildScale()
    {
        Debug.Log("EnableChildScale Called");
        if (relativeScale != null)
        {
            relativeScale.EnableScale();
        }
    }

    public void DisableChildScale()
    {
        Debug.Log("DisableChildScale Called");
        if (relativeScale != null)
        {
            relativeScale.DisableScale();
        }
    }
}
