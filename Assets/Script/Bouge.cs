using UnityEngine;

public class Bouge : MonoBehaviour
{
	public float moveSpeed = 5f;
	public float rotationSpeed = 10f;

	public float valiseDist = 2f;
	public float valiseDec = 1f;
	public float valiseDistHoldable = 3f;

	public bool isHold = false;

	public GameObject jo;
	private Rigidbody rb;

	public GameObject cam;
	public float camLerpSpeed = 10f;

	public bool canGive;

	// Start is called before the first frame update
	void Start()
	{
		jo = GameObject.FindGameObjectWithTag("jo");
		rb = jo.GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E)) // Changer de personnage
		{
			TryChangeCharacter();
		}

		if (Input.GetKeyDown(KeyCode.Q)) // Desactive script Cam et active lock cam
		{
			cam.GetComponent<Cam>().enabled = !cam.GetComponent<Cam>().enabled;
		}

		if (Input.GetKeyDown(KeyCode.F) && (isHold || Vector3.Distance(jo.transform.position, transform.position) < valiseDistHoldable))
		{
			isHold = !isHold;
		}
	}

	void FixedUpdate()
	{
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveZ = Input.GetAxisRaw("Vertical");

		Vector3 joPos = jo.transform.position;

		if (isHold)
		{
			joPos = joPos + jo.transform.right * valiseDec;
			if (moveZ < 0)
			{
				moveZ = 0;
			}
		}

		Vector3 forward = new Vector3(
					joPos.x - cam.transform.position.x,
					joPos.y - cam.transform.position.y,
					joPos.z - cam.transform.position.z)
				.normalized;
		Vector3 right = Quaternion.AngleAxis(90, Vector3.up) * forward;
		Vector3 movement = new Vector3(forward.x * moveZ + right.x * moveX, 0f, forward.z * moveZ + right.z * moveX).normalized * moveSpeed;

		if (!cam.GetComponent<Cam>().enabled)
		{
			Quaternion camRot = Quaternion.LookRotation(forward, Vector3.up);
			cam.transform.rotation = Quaternion.Lerp(
				cam.transform.rotation,
				camRot,
				Time.fixedDeltaTime * camLerpSpeed * (isHold ? 2 : 1)
				);
		}

		Vector3 newPosition = rb.position + movement * Time.fixedDeltaTime;
		rb.MovePosition(newPosition);

		// Rotation du personnage vers la direction du mouvement
		if (movement != Vector3.zero)
		{
			Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
			rb.MoveRotation(Quaternion.Lerp(rb.rotation, toRotation, Time.fixedDeltaTime * 10f));
		}

		if (isHold)
		{
			gameObject.transform.position = joPos - jo.transform.forward * valiseDist;
		}

	}

	void TryChangeCharacter()
	{
		Collider[] nearbyCharacters = Physics.OverlapSphere(jo.transform.position, 3f);
		foreach (Collider col in nearbyCharacters)
		{
			if (col.GetComponent<allume>() == null)
			{
				continue;
			}
			if (col.gameObject != jo)
			{
				NextRole(col.gameObject);

				if (canGive)
				{
					changement(col.gameObject);
				}
				canGive = false;
				
				break;
			}
		}
	}

	public void changement(GameObject newjo)
	{
		jo = newjo;
		rb = jo.GetComponent<Rigidbody>();
	}

	public void NextRole(GameObject nextjo)
	{
		

		switch (jo.GetComponent<Role>().role)
		{
			case Role.Choix.Espion:
				switch (nextjo.GetComponent<Role>().role)
				{
					case Role.Choix.Espion:
						canGive = true;
						break;
					case Role.Choix.Guarde:
						canGive = false;
						break;
					case Role.Choix.Elec:
						canGive = true;
						break;
					case Role.Choix.Dict:
						canGive = true;
						break;
					case Role.Choix.Bag:
						canGive = true;
						break;
					case Role.Choix.Amb:
						canGive = true;
						break;
				}
				
				break;
			case Role.Choix.Guarde:
				canGive = false;
				break;
			case Role.Choix.Elec:
				switch (nextjo.GetComponent<Role>().role)
				{
					case Role.Choix.Espion:
						canGive = true;
						break;
					case Role.Choix.Guarde:
						canGive = false;
						break;
					case Role.Choix.Elec:
						canGive = true;
						break;
					case Role.Choix.Dict:
						canGive = true;
						break;
					case Role.Choix.Bag:
						canGive = true;
						break;
					case Role.Choix.Amb:
						canGive = true;
						break;
				}
				break;
			case Role.Choix.Dict:
				switch (nextjo.GetComponent<Role>().role)
				{
					case Role.Choix.Espion:
						canGive = true;
						break;
					case Role.Choix.Guarde:
						canGive = false;
						break;
					case Role.Choix.Elec:
						canGive = true;
						break;
					case Role.Choix.Dict:
						canGive = true;
						break;
					case Role.Choix.Bag:
						canGive = true;
						break;
					case Role.Choix.Amb:
						canGive = true;
						break;
				}
				break;
			case Role.Choix.Bag:
				switch (nextjo.GetComponent<Role>().role)
				{
					case Role.Choix.Espion:
						canGive = true;
						break;
					case Role.Choix.Guarde:
						canGive = false;
						break;
					case Role.Choix.Elec:
						canGive = true;
						break;
					case Role.Choix.Dict:
						canGive = true;
						break;
					case Role.Choix.Bag:
						canGive = true;
						break;
					case Role.Choix.Amb:
						canGive = true;
						break;
				}
				break;
			case Role.Choix.Amb:
				switch (nextjo.GetComponent<Role>().role)
				{
					case Role.Choix.Espion:
						canGive = true;
						break;
					case Role.Choix.Guarde:
						canGive = false;
						break;
					case Role.Choix.Elec:
						canGive = true;
						break;
					case Role.Choix.Dict:
						canGive = true;
						break;
					case Role.Choix.Bag:
						canGive = true;
						break;
					case Role.Choix.Amb:
						canGive = true;
						break;
				}
				break;
		}






	}
}
