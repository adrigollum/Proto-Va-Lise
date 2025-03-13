using UnityEngine;

public class allume : MonoBehaviour
{
	private Renderer objectRenderer; // Référence au renderer de l'objet
	public float detectionRadius ; // Distance de détection
	public Bouge valise;
	private Material outlineMaterial;
	private bool isHighlighted = false;

	public string rolejo;
	private bool canGive = false;

	void Start()
	{
		actualise();
		detectionRadius = valise.interactionDistance;
	}

	void Update()
	{
		rolejo = valise.jo.GetComponent<Role>().role.ToString();
		relat();

		if (valise.jo.transform != this.transform && canGive)
		{
			float distance = Vector3.Distance(valise.jo.transform.position, transform.position);


			if (distance <= detectionRadius)
			{
				float distanceToJo = Vector3.Distance(transform.position, valise.jo.transform.position);

				// Afficher la distance au moment du calcul dans la console
				Debug.Log("Distance e : " + distanceToJo);
				if (!isHighlighted)
				{
					ActivateOutline(true);
				}
			}
			else if (distance > detectionRadius && isHighlighted)
			{
				ActivateOutline(false);
			}
		}
		else if (valise.jo.transform != this.transform)
		{
			ActivateOutline(false);
		}
	}



	void ActivateOutline(bool state)
	{
		isHighlighted = state;
		outlineMaterial.SetFloat("_OutlineWidth", state ? 0.05f : 0f);
	}

	void actualise()
	{
		objectRenderer = this.GetComponent<Renderer>();

		if (objectRenderer != null)
			outlineMaterial = objectRenderer.material;
	}

	void relat()
	{
		switch (GetComponent<Role>().role)
		{
			case Role.Choix.Espion:
				switch (rolejo)
				{
					case "Espion":
						canGive = true;
						break;
					case "Guarde":
						canGive = false;
						break;
					case "Elec":
						canGive = false;
						break;
					case "Dict":
						canGive = true;
						break;
					case "Bag":
						canGive = true;
						break;
					case "Amb":
						canGive = true;
						break;
				}
				break;
			case Role.Choix.Guarde:
				canGive = false;
				break;
			case Role.Choix.Elec:
				switch (rolejo)
				{
					case "Espion":
						canGive = true;
						break;
					case "Guarde":
						canGive = false;
						break;
					case "Elec":
						canGive = true;
						break;
					case "Dict":
						canGive = false;
						break;
					case "Bag":
						canGive = false;
						break;
					case "Amb":
						canGive = false;
						break;
				}
				break;
			case Role.Choix.Dict:
				switch (rolejo)
				{
					case "Espion":
						canGive = false;
						break;
					case "Guarde":
						canGive = false;
						break;
					case "Elec":
						canGive = false;
						break;
					case "Dict":
						canGive = true;
						break;
					case "Bag":
						canGive = false;
						break;
					case "Amb":
						canGive = true;
						break;
				}
				break;
			case Role.Choix.Bag:
				switch (rolejo)
				{
					case "Espion":
						canGive = true;
						break;
					case "Guarde":
						canGive = false;
						break;
					case "Elec":
						canGive = true;
						break;
					case "Dict":
						canGive = true;
						break;
					case "Bag":
						canGive = true;
						break;
					case "Amb":
						canGive = true;
						break;
				}
				break;
			case Role.Choix.Amb:
				switch (rolejo)
				{
					case "Espion":
						canGive = false;
						break;
					case "Guarde":
						canGive = false;
						break;
					case "Elec":
						canGive = false;
						break;
					case "Dict":
						canGive = true;
						break;
					case "Bag":
						canGive = true;
						break;
					case "Amb":
						canGive = true;
						break;
				}
				break;
		}
	}
}
