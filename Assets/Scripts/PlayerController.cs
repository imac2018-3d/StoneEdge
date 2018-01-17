using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float FallVelocityThreshold = -10.0f;
	public Canvas TransitionCanvas = null;

	private Rigidbody rb;
	private Vector3 lastValidPos;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		lastValidPos = rb.transform.position;
	}

	void Warp()
	{
		TransitionCanvas.GetComponent<Transition>().close(
			() =>
			{
				rb.velocity = Vector3.zero;
				transform.position = lastValidPos;
				transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
				TransitionCanvas.GetComponent<Transition>().open();
			}
		);
	}

	void CheckFall()
	{
		if (rb.velocity.y <= FallVelocityThreshold)
		{
			RaycastHit hitInfo;
			if (!Physics.Raycast(rb.transform.position, rb.velocity, out hitInfo))
			{
				Warp();
			}
		}
	}

	void CheckLocation()
	{
		RaycastHit hitInfo;
		if (Physics.Linecast(rb.transform.position, rb.transform.position+rb.velocity*Time.deltaTime*5, out hitInfo))
		{
			if (hitInfo.transform.GetComponent<UnreachableArea>() != null)
			{
				Warp();
			}
		}
	}

	void FixedUpdate() {
		CheckFall();
		CheckLocation();
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		RaycastHit hitInfo;
		Vector3 movement = new Vector3 (moveHorizontal/2.0f, 0.0f, moveVertical/2.0f);
		if (movement.sqrMagnitude > 0)
		{
			if (Physics.Raycast(rb.transform.position + movement + rb.velocity * 0.01f, -Vector3.up, out hitInfo))
			{
				if (hitInfo.transform.GetComponent<UnreachableArea>() == null &&
					Vector3.Dot(Vector3.up, rb.transform.up) > 0.8f &&
						(hitInfo.point - transform.position).sqrMagnitude < 1.0f)
					lastValidPos = rb.transform.position;
			}
		}
		rb.MovePosition (transform.position + movement);
	}
}
