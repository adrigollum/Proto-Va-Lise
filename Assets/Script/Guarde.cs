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


	void Start()
	{
		valise = this.gameObject.GetComponent<allume>().valise;

		agent = GetComponent<NavMeshAgent>(); // Récupérer le composant NavMeshAgent

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
			Debug.Log("Joueur détecté avec la valise ! Il doit la lâcher.");
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
		yield return new WaitUntil(() => agent.remainingDistance < valise.interactionDistance); // Attendre d'arriver à la valise
		
		Debug.Log("Le garde a récupéré la valise !");
		valise.following = this.gameObject;
		valise.PickUpValise();

		StartCoroutine(DisablePickupForSeconds(10)); // Bloque la prise pendant 10 secondes
	}

	IEnumerator DisablePickupForSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		valise.canpickup = true; // Réactive la possibilité de récupérer la valise
		valise.DropValise();
		valise.following = valise.jo;
		Debug.Log("Le joueur peut maintenant reprendre la valise.");
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
