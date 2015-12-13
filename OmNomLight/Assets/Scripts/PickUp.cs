using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
    public float range = 0.5f;
	public LayerMask layerMask;

    bool carrying = false;
    private GetCarried carriedObject;
	public bool isStandingInFrontOfFridge = false;

	// the last time something was eaten
	private float lastFoodTime = 0.0f;
	private const float EAT_COOLDOWN_SECONDS = 1.0f; 

    void Update()
    {
		if(Input.GetButtonDown("Pick Up") || Input.GetButtonDown("Pick Up Controller"))
        {
            if(!carrying)//currently not carrying anything
            {
				RaycastHit2D hit = Physics2D.CircleCast(transform.position, range, Vector3.back,range,layerMask);

                //check the pickup zone
                if (hit)
                {
                    carriedObject = hit.collider.GetComponent<GetCarried>();

                    //an object that can be carried is inside the pickup zone
                    if (carriedObject)
                    {
						if( gameObject.CompareTag("Monster") && Input.GetButtonDown("Pick Up Controller"))
							MonsterAction(carriedObject);
						else if(gameObject.CompareTag("Human") && Input.GetButtonDown("Pick Up") )
							HumanAction(carriedObject);
                    }
                }
            }
			else if (carriedObject && gameObject.CompareTag("Human") && Input.GetButtonDown("Pick Up") )
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

	private void MonsterAction(GetCarried n_object)
	{
		//Debug.Log("Monster Action");
		Collectible tmp_food = n_object.GetComponent<Collectible>();

		// only if the object really is food; prevent eating the Candle
		if( n_object.GetComponent<Collectible>() != null )
		{
			// When Human holds the object, let the monster not eat stuff
			if( GameObject.FindWithTag("Human").GetComponent<PickUp>().GetCarriedObject() != n_object.GetComponent<GetCarried>() 
				&& Time.timeSinceLevelLoad - lastFoodTime > EAT_COOLDOWN_SECONDS )
			{
				// Play on random music clip
				int tmp_random = Random.Range(0,4);
				if( tmp_random == 0 )
					gameObject.GetComponent<AudioSource>().clip = gameObject.GetComponent<PlayerController>().audioEating1;
				else if( tmp_random == 1)
					gameObject.GetComponent<AudioSource>().clip = gameObject.GetComponent<PlayerController>().audioEating2;
				else if( tmp_random == 2 )
					gameObject.GetComponent<AudioSource>().clip = gameObject.GetComponent<PlayerController>().audioEating3;
				else
					gameObject.GetComponent<AudioSource>().clip = gameObject.GetComponent<PlayerController>().audioEating4;
				gameObject.GetComponent<AudioSource>().Play();

				lastFoodTime = Time.timeSinceLevelLoad;

				GameObject.FindObjectOfType<GameState>().MonsterEatsOneFood();

				n_object.gameObject.SetActive(false);

				Destroy(n_object.gameObject, 0.3f);
			}
		}
	}

	private void HumanAction(GetCarried n_object)
	{
		GetComponent<AudioSource>().clip = GetComponent<PlayerController>().audioPickUp;
		GetComponent<AudioSource>().Play();

		carriedObject.startCarrying(transform);
		carrying = true;
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
