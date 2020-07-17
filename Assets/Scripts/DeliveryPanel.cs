using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryPanel : MonoBehaviour
{
    public Text CityA_Text, CityB_Text;
    public Text PackageName_Text, PackageDescription_Text;
    public Image CarPreview_Image;
    public Text CarDescription_Text;
    [Space(10)]
    public List<Car> CarPrefabs = new List<Car>();
    public Car CurrentCarPrefab;

    #region Camera Callbacks

    private CameraControl MainCam;

    private void Awake()
    {
        MainCam = FindObjectOfType<CameraControl>();
    }

    private void OnEnable()
    {
        MainCam.canMove = false;
    }

    private void OnDisable()
    {
        MainCam.canMove = true;
    }
    #endregion

    public void SetValues(Delivery delivery)
    {
        CityA_Text.text = delivery.APoint.ToString();
        CityB_Text.text = delivery.BPoint.ToString();

        PackageName_Text.text = delivery.PackageName;
        ///TODO get description
        PackageDescription_Text.text = "description";

        CurrentCarPrefab = null;
        OnSwitchCar(true);
    }

    public void OnSwitchCar(bool isNext)
    {
        int index = 0;
        if (CurrentCarPrefab != null)
        {
            index = CarPrefabs.IndexOf(CurrentCarPrefab);
            if (isNext)
            {
                if (index == CarPrefabs.Count - 1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
            }
            else
            {
                if (index == 0)
                {
                    index = CarPrefabs.Count - 1;
                }
                else
                {
                    index--;
                }
            }
        }

        CurrentCarPrefab = CarPrefabs[index];
        CarPreview_Image.sprite = CurrentCarPrefab.PreviewSprite;
        CarDescription_Text.text = $"{CurrentCarPrefab.Name} - {CurrentCarPrefab.Description}";
        GameManager.Instance.CurrentCarPrefab = CurrentCarPrefab.gameObject;
    }
}
