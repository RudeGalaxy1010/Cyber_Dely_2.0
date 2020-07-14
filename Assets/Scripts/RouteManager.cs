using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

public class RouteManager : MonoBehaviour
{
    public static RouteManager Instance;
    public static bool isRouteEdit = false;

    public GameObject MarkerPrefab;
    public float MarkersHeight = 2f;
    public GameObject RouteReadyButton;

    public List<Transform> Route;

    private List<GameObject> Markers;
    private Car CurrentCar;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

        RouteReadyButton.SetActive(false);
    }

    public void OnStartRouteCreate(City startPoint, City destinationPoint, params Car[] cars)
    {
        if (cars.Length > 0)
        {
            CurrentCar = cars[0];
        }

        ///If car reached destination city
        if (startPoint == destinationPoint && CurrentCar != null)
        {
            Destroy(CurrentCar.gameObject);
            CurrentCar = null;
            return;
        }

        Route = new List<Transform>();
        Markers = new List<GameObject>();
        AddPoint(startPoint.transform);
        isRouteEdit = true;

        RouteReadyButton.SetActive(true);
    }

    public void OnEditPoint(Transform point)
    {
        if (Route.Contains(point))
        {
            ///remove last point
            if (Route.IndexOf(point) == Route.Count - 1 && Route.Count > 1)
            {
                Route.Remove(point);
                Destroy(Markers.Last());
                Markers.Remove(Markers.Last());
            }
        }
        else
        {
            ///checking for only linear route
            if (Route.Count > 0)
            {
                var lastPoint = Route.Last();
                if (lastPoint.position.x != point.position.x && lastPoint.position.z != point.position.z)
                {
                    return;
                }
            }

            ///add new point
            AddPoint(point);
        }
    }

    private void AddPoint(Transform point)
    {
        Route.Add(point);
        Markers.Add(Instantiate(MarkerPrefab, point.position + Vector3.up * MarkersHeight, Quaternion.identity));
    }

    public List<Transform> OnRouteEnd()
    {
        isRouteEdit = false;

        foreach (GameObject marker in Markers)
        {
            Destroy(marker);
        }
        Markers.Clear();
        return Route;
    }

    /// <summary>
    /// If approve button clicked
    /// spawn car
    /// </summary>
    public void OnRouteApprove()
    {
        #region checks
        if (Route.Count < 2)
        {
            return;
        }
        if (!Route.Last().TryGetComponent(out City city))
        {
            return;
        }
        #endregion

        if (CurrentCar != null)
        {
            CurrentCar.SetRoute(OnRouteEnd());
        }
        else
        {
            var car = Instantiate(GameManager.Instance.CurrentCarPrefab, GameManager.Instance.CurrentDelivery.APoint.transform.position, Quaternion.identity);
            car.GetComponent<Car>().SetRoute(OnRouteEnd());
        }

        RouteReadyButton.SetActive(false);
    }
}
