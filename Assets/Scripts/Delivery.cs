using UnityEngine;

public class Delivery : MonoBehaviour
{
    /// <summary>
    /// TODO more fields
    /// </summary>
    public City APoint, BPoint;

    public Delivery(City aPoint, City bPoint)
    {
        APoint = aPoint;
        BPoint = bPoint;
    }

    public override string ToString()
    {
        return $"Delivery from {APoint} to {BPoint}";
    }
}
