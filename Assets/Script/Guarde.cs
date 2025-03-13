using System.Collections;
using UnityEngine;
using UnityEngine.AI; // Pour que le garde puisse se d�placer


public class Guarde : MonoBehaviour
{
	public float detectionRadius = 5f;  // Distance de d�tection
	public float detectionAngle = 90f;  // Angle du c�ne de vision
	public int resolution = 20;         // Nombre de points pour dessiner le c�ne
	public Color visionColor = new Color(0, 0, 1, 0.3f); // Couleur bleue semi-transparente
	public float tiltAngle = -15f;

	public Bouge valise;

	private LineRenderer lineRenderer;

	private NavMeshAgent agent; // Pour le d�placement du garde


	void Start()
	{
		valise = this.gameObject.GetComponent<allume>().valise;

		agent = GetComponent<NavMeshAgent>(); // R�cup�rer le composant NavMeshAgent

		// Cr�ation et configuration du LineRenderer
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.positionCount = resolution + 3; // +3 pour fermer le c�ne vers le garde
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
			Debug.Log("Joueur d�tect� avec la valise ! Il doit la l�cher.");
			valise.canpickup = false; // Emp�cher le joueur de reprendre la valise
			valise.DropValise();

			MoveToValise();
		}
	}

	void MoveToValise()
	{
		agent.SetDestination(valise.transform.position); // D�placer le garde vers la valise
		StartCoroutine(PickUpValise());
	}

	IEnumerator PickUpValise()
	{
		yield return new WaitUntil(() => agent.remainingDistance < valise.interactionDistance); // Attendre d'arriver � la valise
		
		Debug.Log("Le garde a r�cup�r� la valise !");
		valise.following = this.gameObject;
		valise.PickUpValise();

		StartCoroutine(DisablePickupForSeconds(10)); // Bloque la prise pendant 10 secondes
	}

	IEnumerator DisablePickupForSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		valise.canpickup = true; // R�active la possibilit� de r�cup�rer la valise
		valise.DropValise();
		valise.following = valise.jo;
		Debug.Log("Le joueur peut maintenant reprendre la valise.");
	}

	void DrawVisionCone()
	{
		lineRenderer.positionCount = resolution + 3;
		lineRenderer.SetPosition(0, transform.position); // Origine du c�ne

		float stepAngleSize = (detectionAngle * 2) / resolution;

		for (int i = 0; i <= resolution; i++)
		{
			float angle = -detectionAngle + stepAngleSize * i;
			Vector3 direction = Quaternion.Euler(tiltAngle, angle, 0) * transform.forward; // Inclinaison vers le bas
			Vector3 vertex = transform.position + direction * detectionRadius;
			lineRenderer.SetPosition(i + 1, vertex);
		}

		// Fermeture du c�ne vers le garde
		lineRenderer.SetPosition(resolution + 2, transform.position);
	}
}
