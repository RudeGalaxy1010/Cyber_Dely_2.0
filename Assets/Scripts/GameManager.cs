using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public DeliveryPanel DeliveryPanel;

    public List<City> Cities;
    [HideInInspector]
    public Delivery CurrentDelivery;

    public GameObject CurrentCarPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        ///initialize UI
        DeliveryPanel.gameObject.SetActive(false);

        Cities = FindObjectsOfType<City>().ToList();
        if (Cities.Count < 2)
        {
            Debug.LogError("Not enough cities!");
        }

        GenerateDelivery();
    }

    public void GenerateDelivery()
    {
        var CurrentCity = Cities[Random.Range(0, Cities.Count)];
        CurrentCity.ShowDelivery();

        var DestinationCity = CurrentCity;
        while (DestinationCity == CurrentCity)
        {
            DestinationCity = Cities[Random.Range(0, Cities.Count)];
        }

        CurrentDelivery = new Delivery(CurrentCity, DestinationCity);
    }

    /// <summary>
    /// Show Delivery panel
    /// </summary>
    public void OnDeliveryShow()
    {
        DeliveryPanel.SetValues(CurrentDelivery);
        DeliveryPanel.gameObject.SetActive(true);
        RouteManager.Instance.OnStartRouteCreate(CurrentDelivery.APoint, CurrentDelivery.BPoint);
    }
}
