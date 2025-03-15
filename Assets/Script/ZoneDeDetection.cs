using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZoneDeDetection : MonoBehaviour
{
	public Bouge valise; // Référence à l'objet valise

	public Transform zoneLivraison;  // Destination où la valise doit être livrée
	public float detectionRadius = 5f;  // Rayon de détection de la valise

	private void OnTriggerEnter(Collider other)
	{
		// Vérifie si l'objet entrant est la valise
		if (!valise.isHold && other.transform == valise.transform && valise.jo.tag != "Bagagiste")
		{
			Debug.Log("Valise détectée !");
			valise.canpickup = false; // Empêcher le joueur de reprendre la valise
									  // Trouve le bagagiste le plus proche
			FindClosestBagagiste(other.transform);
		}
	}

	void FindClosestBagagiste(Transform valiseTransform)
	{
		if (valiseTransform == null)
		{
			Debug.LogError("Transform de la valise est null !");
			return;
		}

		// Trouver tous les bagagistes à proximité dans la scène
		GameObject[] bagagistes = GameObject.FindGameObjectsWithTag("Bagagiste");
		if (bagagistes.Length == 0)
		{
			Debug.LogWarning("Aucun bagagiste trouvé dans la scène !");
			return;
		}

		GameObject closestBagagiste = null;
		float closestDistance = Mathf.Infinity;

		// Déboguer et afficher tous les bagagistes trouvés
		Debug.Log("Liste des bagagistes dans la scène :");
		foreach (GameObject bagagiste in bagagistes)
		{
			Debug.Log("Bagagiste trouvé : " + bagagiste.name + " à la position " + bagagiste.transform.position);
		}

		// Cherche le bagagiste le plus proche
		foreach (GameObject bagagiste in bagagistes)
		{
			float distance = Vector3.Distance(bagagiste.transform.position, valiseTransform.position);
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestBagagiste = bagagiste; // Mettre à jour le garde le plus proche
			}
		}

		if (closestBagagiste != null)
		{
			Debug.Log(closestBagagiste.name);
			// Invoque le bagagiste pour venir chercher la valise
			closestBagagiste.GetComponent<Bagagiste>().MoveToValise(valiseTransform, zoneLivraison);
		}
		else
		{
			Debug.Log("Aucun bagagiste proche trouvé.");
		}
	}
}
