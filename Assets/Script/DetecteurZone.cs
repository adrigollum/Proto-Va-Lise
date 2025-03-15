using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetecteurZone : MonoBehaviour
{
	public Bouge valise; // R�f�rence � l'objet valise
	public float detectionRadius = 0f;  // Rayon de d�tection du joueur
	public Transform closestGuard; // R�f�rence au garde le plus proche

	private void OnTriggerEnter(Collider other)
	{
		// V�rifier si l'objet entrant est le joueur avec la valise
		if (valise.isHold && other.transform == valise.jo.transform)
		{
			Debug.Log("Le joueur avec la valise est entr� dans la zone !");
			FindClosestGuard(other.transform); // Trouver le garde le plus proche
			valise.canpickup = false; // Emp�cher le joueur de reprendre la valise
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
			// Calculer la distance entre le d�tecteur et chaque garde
			float distance = Vector3.Distance(guard.transform.position, detectorTransform.position);

			// Si c'est le garde le plus proche
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestGuard = guard.transform; // Mettre � jour le garde le plus proche
			}
		}

		if (closestGuard != null)
		{
			// Si un garde a �t� trouv�, faire r�agir le garde
			closestGuard.GetComponent<Guarde>().ReactToPlayerWithValise();
		}
	}
}