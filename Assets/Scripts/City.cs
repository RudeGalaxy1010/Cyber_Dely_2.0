using UnityEngine;
using UnityEngine.UI;

public class City : MonoBehaviour
{
    public string Name = "City";
    /// <summary>
    /// Holder for delivery view and city name text
    /// </summary>
    public GameObject BillBoard;
    public GameObject DeliveryView;
    public Text CityNameText;

    private void Start()
    {
        ///UI Initialize
        CityNameText.text = Name;
        DeliveryView.SetActive(false);
    }

    public void ShowDelivery()
    {
        DeliveryView.SetActive(true);
    }

    private void LateUpdate()
    {
        BillBoard.transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void OnDeliveryView()
    {
        BillBoard.SetActive(false);
        GameManager.Instance.OnDeliveryShow();
    }

    public override string ToString()
    {
        return Name;
    }
}
