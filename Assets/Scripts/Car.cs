using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float Speed = 10f;
    public List<Transform> Route = new List<Transform>();

    private Transform NextPoint;

    public void SetRoute(List<Transform> route)
    {
        Route = new List<Transform>();

        ///Edits height
        foreach (Transform point in route)
        {
            point.position = new Vector3(point.position.x, transform.position.y, point.position.z);
        }

        Route.AddRange(route);
        NextPoint = Route[1];
    }

    public void Update()
    {
        if (NextPoint == null)
        {
            return;
        }


        if (transform.position != NextPoint.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, NextPoint.position, Time.deltaTime * Speed);
        }
        else
        {
            if (NextPoint != Route.Last())
            {
                NextPoint = Route[Route.IndexOf(NextPoint) + 1];
            }
            else
            {
                ///If reached the end of the route
                if (!RouteManager.isRouteEdit)
                {
                    var ACity = Route.Last().GetComponent<City>();
                    var BCity = GameManager.Instance.CurrentDelivery.BPoint;
                    RouteManager.Instance.OnStartRouteCreate(ACity, BCity, new Car[] { this });
                }
            }
        }
    }
}
