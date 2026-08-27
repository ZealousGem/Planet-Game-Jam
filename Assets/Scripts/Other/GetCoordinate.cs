using UnityEngine;

public class GetCoordinate : MonoBehaviour
{

    private static Transform CoordTransform;

    private void Awake()
    {
        CoordTransform = transform;
    }
    public static Transform ReturnPosition()
    {
        return CoordTransform;
    }
}
