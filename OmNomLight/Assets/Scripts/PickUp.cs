using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
    public float range = 0.5f;

    bool carrying = false;
    private GetCarried carriedObject;


    void Update()
    {
        if(Input.GetButtonDown("Pick Up"))
        {
            if(!carrying)//currently not carrying anything
            {
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, range, Vector3.back);

                //check the pickup zone
                if (hit)
                {
                    carriedObject = hit.collider.GetComponent<GetCarried>();

                    //an object that can be carried is inside the pickup zone
                    if (carriedObject)
                    {
                        carriedObject.startCarrying(transform);
                        carrying = true;
                    }
                }
            }
            else if (carriedObject)
            {
                //drop the carried object
                carriedObject.stopCarrying();
                carrying = false;
            }
        }
    }

}
