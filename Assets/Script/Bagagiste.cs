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

	// Appel�e par la zone de d�tection pour faire venir le bagagiste
	public void MoveToValise(Transform valiseTransform, Transform livraisonZone)
	{

		zoneLivraison = livraisonZone;

		// D�placer le bagagiste vers la valise
		agent.SetDestination(valise.transform.position);
		StartCoroutine(PickUpValise());
	}

	// Lorsque le bagagiste arrive � la valise, il la prend
	IEnumerator PickUpValise()
	{
		// Attendre que le bagagiste arrive � la valise
		yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending);

		Debug.Log("Bagagiste a r�cup�r� la valise.");
		valise.guardepick = true;
		valise.following = this.gameObject;
		valise.PickUpValise();
		// D�placer le bagagiste vers la zone de livraison
		agent.SetDestination(zoneLivraison.position);
		StartCoroutine(DeliverValise());
	}

	// Livrer la valise � la zone de livraison
	IEnumerator DeliverValise()
	{
		// Attendre que le bagagiste arrive � la zone de livraison
		yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending);

		Debug.Log("Bagagiste a livr� la valise.");
		valise.canpickup = true; // R�active la possibilit� de r�cup�rer la valise
		valise.DropValise();
		valise.guardepick = false;
		valise.following = valise.jo;
		// Retourner � son point de spawn
		Debug.Log("je vais a " + pointSpawn);
		agent.SetDestination(pointSpawn);
		StartCoroutine(ReturnToSpawn());
	}

	// Retourner au point de spawn une fois la livraison effectu�e
	IEnumerator ReturnToSpawn()
	{
		// Attendre que le bagagiste arrive � son point de spawn
		yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending);

		Debug.Log("Bagagiste est de retour � son point de spawn.");
	}
}
