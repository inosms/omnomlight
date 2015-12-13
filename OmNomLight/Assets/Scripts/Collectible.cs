using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.CompareTag("Monster"))
        {
            OnPickup();

			// Play on random music clip
			int tmp_random = Random.Range(0,4);
			if( tmp_random == 0 )
				c.GetComponent<AudioSource>().clip = c.GetComponent<PlayerController>().audioEating1;
			else if( tmp_random == 1)
				c.GetComponent<AudioSource>().clip = c.GetComponent<PlayerController>().audioEating2;
			else if( tmp_random == 2 )
				c.GetComponent<AudioSource>().clip = c.GetComponent<PlayerController>().audioEating3;
			else
				c.GetComponent<AudioSource>().clip = c.GetComponent<PlayerController>().audioEating4;
			c.GetComponent<AudioSource>().Play();
        }
    }

    void OnPickup()
    {
		GameObject.FindObjectOfType<GameState>().MonsterEatsOneFood();

		// When Human holds the object, let the human first drop the object
		if( GameObject.FindObjectOfType<PickUp>().GetCarriedObject() == this.GetComponent<GetCarried>() )
		{
			GameObject.FindObjectOfType<PickUp>().StopCarrying();
		}

		// disable the object to prevent further collisions
		this.gameObject.SetActive(false);

        Destroy(this.gameObject, 0.3f);
    }
}
