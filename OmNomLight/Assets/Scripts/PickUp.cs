using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
    public float range = 0.5f;
	public LayerMask layerMask;

    bool carrying = false;
    private GetCarried carriedObject;
	public bool isStandingInFrontOfFridge = false;

    void Update()
    {
		
        if(Input.GetButtonDown("Pick Up"))
        {
			
            if(!carrying)//currently not carrying anything
            {
				RaycastHit2D hit = Physics2D.CircleCast(transform.position, range, Vector3.back,range,layerMask);

                //check the pickup zone
                if (hit)
                {
                    carriedObject = hit.collider.GetComponent<GetCarried>();

                    Debug.Log(hit.collider.gameObject.name);

                    //an object that can be carried is inside the pickup zone
                    if (carriedObject)
                    {
						GetComponent<AudioSource>().clip = GetComponent<PlayerController>().audioPickUp;
						GetComponent<AudioSource>().Play();

                        carriedObject.startCarrying(transform);
                        carrying = true;
                    }
                }
            }
            else if (carriedObject)
            {
				// only drop the object when not directly in front of fridge,
				// as one wants to put the food into the fridge in this case
				if( !isStandingInFrontOfFridge )
				{
					StopCarrying();
				}
            }
        }
    }

	public void StopCarrying()
	{
		//drop the carried object
		carriedObject.stopCarrying();
		carriedObject = null;
		carrying = false;
	}


	public GetCarried GetCarriedObject()
	{
		return carriedObject;
	}

}
