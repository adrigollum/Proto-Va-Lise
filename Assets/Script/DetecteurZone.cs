using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetecteurZone : MonoBehaviour
{
	public Bouge valise; // Référence à l'objet valise
	public float detectionRadius = 0f;  // Rayon de détection du joueur
	public Transform closestGuard; // Référence au garde le plus proche

	private void OnTriggerEnter(Collider other)
	{
		// Vérifier si l'objet entrant est le joueur avec la valise
		if (valise.isHold && other.transform == valise.jo.transform)
		{
			Debug.Log("Le joueur avec la valise est entré dans la zone !");
			FindClosestGuard(other.transform); // Trouver le garde le plus proche
			valise.canpickup = false; // Empêcher le joueur de reprendre la valise
			valise.DropValise();
		}
	}

	void FindClosestGuard(Transform detectorTransform)
	{
		float closestDistance = float.MaxValue; // Distance minimale
		Transform closestGuard = null; // Le garde le plus proche

		// Trouver tous les objets avec le tag "Guard"
		Guarde[] guards = FindObjectsOfType<Guarde>();

		foreach (Guarde guard in guards)
		{
			// Calculer la distance entre le détecteur et chaque garde
			float distance = Vector3.Distance(guard.transform.position, detectorTransform.position);

			// Si c'est le garde le plus proche
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestGuard = guard.transform; // Mettre à jour le garde le plus proche
			}
		}

		if (closestGuard != null)
		{
			// Si un garde a été trouvé, faire réagir le garde
			closestGuard.GetComponent<Guarde>().ReactToPlayerWithValise();
		}
	}
}