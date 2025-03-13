using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
	public List<Material> mat�riaux;

	public enum Choix { Espion, Guarde, Bag, Elec, Amb, Dict }

	public Choix role; // Appara�tra comme un menu d�roulant

	// Start is called before the first frame update
	void Start()
    {
		switch (role)
		{
			case Choix.Espion:
				if (mat�riaux.Count > 0)
				{
					GetComponent<Renderer>().material = mat�riaux[0];
				}
				break;
			case Choix.Guarde:
				if (mat�riaux.Count > 0)
				{
					GetComponent<Renderer>().material = mat�riaux[1];
				}
					break;
			case Choix.Bag:
				if (mat�riaux.Count > 0)
				{
					GetComponent<Renderer>().material = mat�riaux[2];
				}
				break;
			case Choix.Elec:
				if (mat�riaux.Count > 0)
				{

					GetComponent<Renderer>().material = mat�riaux[3];
				}
				break;
			case Choix.Amb:
				if (mat�riaux.Count > 0)
				{
					GetComponent<Renderer>().material = mat�riaux[4];
				}
				break;
			case Choix.Dict:
				if (mat�riaux.Count > 0)
				{
					GetComponent<Renderer>().material = mat�riaux[5];
				}
				break;
		}
		
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
