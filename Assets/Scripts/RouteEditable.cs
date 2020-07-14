using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RouteEditable : MonoBehaviour
{
    public Exit Forward, Backward, Right, Left;

    private void OnMouseDown()
    {
        if (RouteManager.isRouteEdit)
        {
            RouteManager.Instance.OnEditPoint(transform);
        }
    }
}

public enum Exit 
{
    no,
    yes
}
