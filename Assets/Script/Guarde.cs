using System.Collections;
using UnityEngine;
using UnityEngine.AI; // Pour que le garde puisse se déplacer


public class Guarde : MonoBehaviour
{
	public float detectionRadius = 5f;  // Distance de détection
	public float detectionAngle = 90f;  // Angle du cône de vision
	public int resolution = 20;         // Nombre de points pour dessiner le cône
	public Color visionColor = new Color(0, 0, 1, 0.3f); // Couleur bleue semi-transparente
	public float tiltAngle = -15f;

	public Bouge valise;

	private LineRenderer lineRenderer;

	private NavMeshAgent agent; // Pour le déplacement du garde

	public Transform dropOffPoint; // Point où il doit déposer la valise
	public float dropOffRadius = 2f; // Rayon autour du point où il peut déposer

	private Vector3 spawnPoint; // Point de spawn du garde


	void Start()
	{
		spawnPoint = transform.position;

		valise = this.gameObject.GetComponent<allume>().valise;

		agent = GetComponent<NavMeshAgent>(); // Récupérer le composant NavMeshAgent

		// Trouver le GameObject avec le tag "guardedepo"
		GameObject dropOffObject = GameObject.FindGameObjectWithTag("guardedepo");
		if (dropOffObject != null)
		{
			dropOffPoint = dropOffObject.transform;  // Assigner la position du dépôt
		}
		else
		{
			Debug.LogWarning("Aucun objet avec le tag 'guardedepo' trouvé !");
		}

		// Création et configuration du LineRenderer
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.positionCount = resolution + 3; // +3 pour fermer le cône vers le garde
		lineRenderer.startWidth = 0.05f;
		lineRenderer.endWidth = 0.05f;
		lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
		lineRenderer.startColor = visionColor;
		lineRenderer.endColor = visionColor;
		lineRenderer.loop = false;
	}

	void Update()
	{
		DetectPlayer();

		DrawVisionCone();

	}

	public void ReactToPlayerWithValise()
	{
		// Vérifie si le joueur a bien la valise
		if (valise.isHold)
		{
			// Faire en sorte que le garde se déplace vers la valise
			Debug.Log("Le garde réagit et se dirige vers la valise !");
			MoveToValise(); // Déplace le garde vers la valise (comportement similaire à celui lorsqu'il la récupère lui-même)
		}
	}

	void DetectPlayer()
	{
		if (valise.following == this.gameObject)
		{
			return;
		}
		Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

		Transform target = null;

		foreach (Collider col in colliders)
		{
			Vector3 directionToTarget = (col.transform.position - transform.position).normalized;
			float angle = Vector3.Angle(transform.forward, directionToTarget);

			if (valise.jo.transform == col.transform && angle < detectionAngle)
			{
				target = col.transform;

				break;
			}
		}

		if (target && valise.isHold)
		{
			
			valise.canpickup = false; // Empêcher le joueur de reprendre la valise
			valise.DropValise();

			MoveToValise();
		}
	}

	void MoveToValise()
	{
		agent.SetDestination(valise.transform.position); // Déplacer le garde vers la valise
		StartCoroutine(PickUpValise());
	}

	IEnumerator PickUpValise()
	{
		yield return new WaitUntil(() =>
			!agent.pathPending &&
			agent.remainingDistance <= agent.stoppingDistance &&
			agent.velocity.sqrMagnitude < 0.01f
		);

		if (agent.remainingDistance <= agent.stoppingDistance)
		{
			valise.guardepick = true;
			valise.following = this.gameObject;
			valise.PickUpValise();

			
			MoveToDropOff();
		}
	}

	/* IEnumerator DisablePickupForSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		valise.canpickup = true; // Réactive la possibilité de récupérer la valise
		valise.DropValise();
		valise.guardepick = false;
		valise.following = valise.jo;
	}
	*/

	IEnumerator DropValise()
	{
		yield return new WaitUntil(() =>
			!agent.pathPending &&
			agent.remainingDistance <= dropOffRadius &&
			agent.velocity.sqrMagnitude < 0.01f
		);

		if (agent.remainingDistance <= dropOffRadius)
		{
			valise.canpickup = true; // Réactive la possibilité de récupérer la valise
			valise.DropValise();
			valise.guardepick = false;
			valise.following = valise.jo;

			MoveToSpawnPoint();
		}
	}

	void MoveToSpawnPoint()
	{
		agent.SetDestination(spawnPoint); // Déplacer le garde vers son point de spawn
	}

	void MoveToDropOff()
	{
		agent.SetDestination(dropOffPoint.position);
		StartCoroutine(DropValise());
	}

	void DrawVisionCone()
	{
		lineRenderer.positionCount = resolution + 3;
		lineRenderer.SetPosition(0, transform.position); // Origine du cône

		float stepAngleSize = (detectionAngle * 2) / resolution;

		for (int i = 0; i <= resolution; i++)
		{
			float angle = -detectionAngle + stepAngleSize * i;
			Vector3 direction = Quaternion.Euler(tiltAngle, angle, 0) * transform.forward; // Inclinaison vers le bas
			Vector3 vertex = transform.position + direction * detectionRadius;
			lineRenderer.SetPosition(i + 1, vertex);
		}

		// Fermeture du cône vers le garde
		lineRenderer.SetPosition(resolution + 2, transform.position);
	}
}
