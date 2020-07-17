using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// TODO fix bug with car moving by grass (update check not only by x and z)
/// </summary>
public class RouteManager : MonoBehaviour
{
    public static RouteManager Instance;
    public static bool isRouteEdit = false;

    public GameObject MarkerPrefab;
    public GameObject EndMarkerPrefab;
    public float EndMarkerHeight = 4f;
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
            GameManager.Instance.GenerateDelivery();
            return;
        }

        Route = new List<Transform>();
        Markers = new List<GameObject>();
        isRouteEdit = true;

        ///add first point
        AddPoint(startPoint.transform);

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
<<<<<<< HEAD
                if (Route.Count > 1)
                {
                    var maxDelta = Vector3.Distance(lastPoint.position, point.transform.position);
                    var checkDelta = maxDelta;
                    
                    foreach (var anchor in point.GetComponent<RouteEditable>().ExitAnchors)
                    {
                        foreach (var otherAnchor in lastPoint.GetComponent<RouteEditable>().ExitAnchors)
                        {
                            checkDelta = Vector3.Distance(anchor.position, otherAnchor.position);
                            if (checkDelta < maxDelta)
                            {
                                goto checkComplete;
                            }
                        }
                    }
                    return;
=======
                ///TODO solve problem with rotations!!!
                if (Route.Count > 1)
                {
                    var deltaX = lastPoint.position.x - point.position.x;
                    var deltaZ = lastPoint.position.z - point.position.z;
                    Debug.Log(deltaX + " " + deltaZ);
                    if (deltaX > 0)
                    {
                        if (point.GetComponent<RouteEditable>().Right == Exit.no)
                        {
                            return;
                        }
                    }
                    else if (deltaX < 0)
                    {
                        if (point.GetComponent<RouteEditable>().Left == Exit.no)
                        {
                            return;
                        }
                    }
                    else if (deltaZ > 0)
                    {
                        if (point.GetComponent<RouteEditable>().Forward == Exit.no)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (point.GetComponent<RouteEditable>().Backward == Exit.no)
                        {
                            return;
                        }
                    }
>>>>>>> f9f5d78a3a28d091d67ee0651184ba15b53c43da
                }
            }

            checkComplete:
            ///add new point
            AddPoint(point);
        }
    }

    private void AddPoint(Transform point)
    {
        Route.Add(point);
        if (Markers.Count == 0)
        {
            ///Spawn end marker
            var position = GameManager.Instance.CurrentDelivery.BPoint.transform.position + Vector3.up * EndMarkerHeight;
            Markers.Add(Instantiate(EndMarkerPrefab, position, Quaternion.identity));
        }
        Markers.Add(Instantiate(MarkerPrefab, point.position + Vector3.up * MarkersHeight, Quaternion.identity));
    }

    public void OnRouteCancel()
    {
        OnRouteEnd();
        GameManager.Instance.GenerateDelivery();
    }

    public List<Transform> OnRouteEnd()
    {
        isRouteEdit = false;

        foreach (GameObject marker in Markers)
        {
            Destroy(marker);
        }
        Markers.Clear();
        RouteReadyButton.SetActive(false);
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
    }
}
