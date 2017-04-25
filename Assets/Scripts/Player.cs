using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private Rigidbody rb;
	private Animator animator;
	private Renderer rend;
	private int maxBoosts = 12;
	private int currentBoosts = 0;
	private float GroundDistance = 0.5f;
	private int jumpCount = 0;
	private int maxJumpCount = 2;
	private float speed = 6f;
	private float jumpSpeed = 6f;

	private Color primaryColor = Color.red;
	private Color secondaryColor = Color.cyan;

	void Start () {
		rb = GetComponent<Rigidbody>();
		rend = GetComponent<Renderer>();
		animator = GetComponent<Animator> ();
		rend.material.color = primaryColor;
	}

	void Update () {
		if (IsGrounded ()) {
			jumpCount = 0;
		}
		if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount) {
			rb.velocity = new Vector3 (rb.velocity.x, jumpSpeed, 0);
			currentBoosts = 0;
			jumpCount++;
		}

		if (Input.GetButtonDown ("ChangeColor")) {
			animator.SetTrigger ("ColorChange");
			if (rend.material.color == primaryColor) {
				rend.material.color = secondaryColor;
			} else {
				rend.material.color = primaryColor;
			}
		}
	}

	bool IsGrounded () {
		return Physics.Raycast (transform.position, - Vector3.up, GroundDistance + 0.1f);
	}

	void FixedUpdate () {
		rb.velocity = new Vector3 (speed, rb.velocity.y, 0);
		if (Input.GetButton ("Jump") && currentBoosts < maxBoosts) {
			rb.AddForce (new Vector3 (0, 12f, 0));
			currentBoosts++;
		}
	}

	void OnCollisionStay (Collision collision) {
		if (collision.gameObject.CompareTag ("PrimaryColor") && CurrentColor() == secondaryColor ||
			collision.gameObject.CompareTag ("SecondaryColor") && CurrentColor() == primaryColor
		) {
			Debug.Log ("DEAD");
		}
	}

	public Color CurrentColor() {
		return rend.material.color;
	}
}
