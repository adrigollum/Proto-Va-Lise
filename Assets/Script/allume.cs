using UnityEngine;

public class allume : MonoBehaviour
{
	private Renderer objectRenderer; // R�f�rence au renderer de l'objet
	public float detectionRadius = 3f; // Distance de d�tection
	public Bouge valise;
	private Material outlineMaterial;
	private bool isHighlighted = false;


	void Start()
	{
		actualise();
	}

	void Update()
	{
		if (valise.jo.transform != this.transform)
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
}
