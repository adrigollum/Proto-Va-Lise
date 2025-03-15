using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bagagiste : MonoBehaviour
{
	private NavMeshAgent agent;
	public Vector3 pointSpawn;  // Point de spawn du bagagiste
	

	public Bouge valise;

	private Transform zoneLivraison;

	void Start()
	{
		valise = this.gameObject.GetComponent<allume>().valise;
		pointSpawn = this.transform.position;
		Debug.Log("spawn = " + pointSpawn);
		agent = GetComponent<NavMeshAgent>();
	}

	// Appelée par la zone de détection pour faire venir le bagagiste
	public void MoveToValise(Transform valiseTransform, Transform livraisonZone)
	{

		zoneLivraison = livraisonZone;

		// Déplacer le bagagiste vers la valise
		agent.SetDestination(valise.transform.position);
		StartCoroutine(PickUpValise());
	}

	// Lorsque le bagagiste arrive à la valise, il la prend
	IEnumerator PickUpValise()
	{
		// Attendre que le bagagiste arrive à la valise
		yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending);

		Debug.Log("Bagagiste a récupéré la valise.");
		valise.guardepick = true;
		valise.following = this.gameObject;
		valise.PickUpValise();
		// Déplacer le bagagiste vers la zone de livraison
		agent.SetDestination(zoneLivraison.position);
		StartCoroutine(DeliverValise());
	}

	// Livrer la valise à la zone de livraison
	IEnumerator DeliverValise()
	{
		// Attendre que le bagagiste arrive à la zone de livraison
		yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending);

		Debug.Log("Bagagiste a livré la valise.");
		valise.canpickup = true; // Réactive la possibilité de récupérer la valise
		valise.DropValise();
		valise.guardepick = false;
		valise.following = valise.jo;
		// Retourner à son point de spawn
		Debug.Log("je vais a " + pointSpawn);
		agent.SetDestination(pointSpawn);
		StartCoroutine(ReturnToSpawn());
	}

	// Retourner au point de spawn une fois la livraison effectuée
	IEnumerator ReturnToSpawn()
	{
		// Attendre que le bagagiste arrive à son point de spawn
		yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending);

		Debug.Log("Bagagiste est de retour à son point de spawn.");
	}
}
