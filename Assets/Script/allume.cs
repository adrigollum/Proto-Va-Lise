using UnityEngine;

public class allume : MonoBehaviour
{
	private Renderer objectRenderer; // Référence au renderer de l'objet
	public float detectionRadius = 3f; // Distance de détection
	public Bouge valise;
	private Material outlineMaterial;
	private bool isHighlighted = false;

	public string rolejo;
	private bool canGive = false;

	void Start()
	{
		actualise();
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
		else
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
			case Role.Choix.Dict:
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
		}
	}
}
