using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
	public List<Material> matériaux;

	public enum Choix { Espion, Guarde, Bag, Elec, Amb, Dict }

	public Choix role; // Apparaîtra comme un menu déroulant

	// Start is called before the first frame update
	void Start()
    {
		switch (role)
		{
			case Choix.Espion:
				if (matériaux.Count > 0)
				{
					GetComponent<Renderer>().material = matériaux[0];
				}
				break;
			case Choix.Guarde:
				if (matériaux.Count > 0)
				{
					GetComponent<Renderer>().material = matériaux[1];
				}
					break;
			case Choix.Bag:
				if (matériaux.Count > 0)
				{
					GetComponent<Renderer>().material = matériaux[2];
				}
				break;
			case Choix.Elec:
				if (matériaux.Count > 0)
				{

					GetComponent<Renderer>().material = matériaux[3];
				}
				break;
			case Choix.Amb:
				if (matériaux.Count > 0)
				{
					GetComponent<Renderer>().material = matériaux[4];
				}
				break;
			case Choix.Dict:
				if (matériaux.Count > 0)
				{
					GetComponent<Renderer>().material = matériaux[5];
				}
				break;
		}
		
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
