using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	[Header("Camera")]
	public Camera mainCamera;

	[Header("Movement")]
	public float speed = 4.5f;
	public LayerMask whatIsGround;
	public float turnSmoothing;

	[Header("Life Settings")]
	public float playerHealth = 1f;




	//Rigidbody playerRigidbody;
	CharacterController controller;
	bool isDead;
	private Vector3 newPosition;


	void Awake()
	{
		//playerRigidbody = GetComponent<Rigidbody>();
		controller = GetComponent<CharacterController>();

	}

    private void Start()
    {
		newPosition = transform.position;
    }

    void Update()
	{
		if (isDead)
			return;

		//Arrow Key Input
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		Vector3 inputDirection = new Vector3(h, 0, v);

		//Camera Direction
		var cameraForward = mainCamera.transform.forward;
		var cameraRight = mainCamera.transform.right;

		cameraForward.y = 0f;
		cameraRight.y = 0f;

		//Try not to use var for roadshows or learning code
		Vector3 desiredDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;

		//Why not just pass the vector instead of breaking it up only to remake it on the other side?

		//NewMove(desiredDirection);
		MoveThePlayer(desiredDirection);
		TurnThePlayer();
		

	}

	void MoveThePlayer(Vector3 desiredDirection)
	{

		Vector3 movement = new Vector3(desiredDirection.x, 0f, desiredDirection.z);
		movement = movement.normalized * speed * Time.deltaTime;
		//transform.position += movement;

		//transform.position = Vector3.Lerp(transform.position, transform.position + movement, smoothing );
		//playerRigidbody.velocity = movement;
		//playerRigidbody.MovePosition(transform.position + movement);
		controller.Move(movement);
	}

	void TurnThePlayer()
	{
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, whatIsGround))
		{
			Vector3 playerToMouse = hit.point - transform.position;
			
			playerToMouse.y = 0f;
			playerToMouse.Normalize();

			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

			transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * turnSmoothing);

		}
	}

	//Player Collision
	void OnTriggerEnter(Collider theCollider)
	{
		
	}


}
