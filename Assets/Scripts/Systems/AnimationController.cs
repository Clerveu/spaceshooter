using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public RelativeScale relativeScale; // Reference to the RelativeScale script on the child

    public void EnableChildScale()
    {
        if (relativeScale != null)
        {
            relativeScale.EnableScale();
        }
    }

    public void DisableChildScale()
    {
        if (relativeScale != null)
        {
            relativeScale.DisableScale();
        }
    }
}
