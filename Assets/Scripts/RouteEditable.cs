using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RouteEditable : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (RouteManager.isRouteEdit)
        {
            RouteManager.Instance.OnEditPoint(transform);
        }
    }
}
