using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RouteEditable : MonoBehaviour
{
    public List<Transform> ExitAnchors = new List<Transform>(4);

    private void OnMouseDown()
    {
        if (RouteManager.isRouteEdit)
        {
            RouteManager.Instance.OnEditPoint(transform);
        }
    }
}
