using UnityEngine;
using System.Collections;

public class GetCarried : MonoBehaviour 
{
    public bool beingCarried = false;

    public void startCarrying(Transform carry)
    {
        transform.SetParent(carry, true);
        beingCarried = true;
    }

    public void stopCarrying()
    {
        transform.SetParent(null, true);
        beingCarried = false;
    }
}
