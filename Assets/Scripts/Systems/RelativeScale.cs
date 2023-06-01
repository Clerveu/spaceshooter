using UnityEngine;

public class RelativeScale : MonoBehaviour
{
    [SerializeField] private Vector3 scaleFactor = Vector3.one;

    public void SetActive(bool active)
    {
        this.enabled = active;
    }

    public void EnableScale()
    {
        this.enabled = true;
    }

    public void DisableScale()
    {
        this.enabled = false;
    }


    void LateUpdate()
    {
        // Make sure object has a parent
        if (transform.parent != null)
        {
            transform.localScale = Vector3.Scale(transform.parent.localScale, scaleFactor);
        }
        else
        {
            Debug.LogWarning("Object does not have a parent, cannot apply relative scale.");
        }
    }
}
