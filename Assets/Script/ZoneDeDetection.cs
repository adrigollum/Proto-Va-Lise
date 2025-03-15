using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZoneDeDetection : MonoBehaviour
{
	public Bouge valise; // R�f�rence � l'objet valise

	public Transform zoneLivraison;  // Destination o� la valise doit �tre livr�e
	public float detectionRadius = 5f;  // Rayon de d�tection de la valise

	private void OnTriggerEnter(Collider other)
	{
		// V�rifie si l'objet entrant est la valise
		if (!valise.isHold && other.transform == valise.transform && valise.jo.tag != "Bagagiste")
		{
			Debug.Log("Valise d�tect�e !");
			valise.canpickup = false; // Emp�cher le joueur de reprendre la valise
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

		// Trouver tous les bagagistes � proximit� dans la sc�ne
		GameObject[] bagagistes = GameObject.FindGameObjectsWithTag("Bagagiste");
		if (bagagistes.Length == 0)
		{
			Debug.LogWarning("Aucun bagagiste trouv� dans la sc�ne !");
			return;
		}

		GameObject closestBagagiste = null;
		float closestDistance = Mathf.Infinity;

		// D�boguer et afficher tous les bagagistes trouv�s
		Debug.Log("Liste des bagagistes dans la sc�ne :");
		foreach (GameObject bagagiste in bagagistes)
		{
			Debug.Log("Bagagiste trouv� : " + bagagiste.name + " � la position " + bagagiste.transform.position);
		}

		// Cherche le bagagiste le plus proche
		foreach (GameObject bagagiste in bagagistes)
		{
			float distance = Vector3.Distance(bagagiste.transform.position, valiseTransform.position);
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestBagagiste = bagagiste; // Mettre � jour le garde le plus proche
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
			Debug.Log("Aucun bagagiste proche trouv�.");
		}
	}
}
