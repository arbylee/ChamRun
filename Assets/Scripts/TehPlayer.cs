using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TehPlayer : MonoBehaviour {
	public GameObject deathParticles;
	public AudioClip deathClip;
	private Rigidbody rb;
	private Animator animator;
	private Renderer[] renderers;
	private int maxBoosts = 12;
	private int currentBoosts = 0;
	private float GroundDistance = 0.5f;
	private int jumpCount = 0;
	private int maxJumpCount = 2;
	private float minSpeed = 11f;
	private float jumpSpeed = 6f;
	private float velocityFactor = 0f;

	private Color primaryColor = Color.red;
	private Color secondaryColor = Color.cyan;
	private Color currentColor;

	void Start () {
		rb = GetComponent<Rigidbody>();
		renderers = GetComponentsInChildren<Renderer>();
		animator = GetComponent<Animator> ();
		foreach (Renderer rend in renderers) {
			rend.material.color = primaryColor;
		}
		currentColor = primaryColor;
		rb.velocity = new Vector3 (minSpeed, rb.velocity.y, 0);

	}

	void Update () {
//		Debug.DrawRay (transform.position, -Vector3.up * 0.5f, Color.blue, 2); 
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
			if (currentColor == primaryColor) {
				foreach (Renderer rend in renderers) {
					rend.material.color = secondaryColor;
				}
				currentColor = secondaryColor;
			} else {
				foreach (Renderer rend in renderers) {
					rend.material.color = primaryColor;
				}
				currentColor = primaryColor;
			}
		}
	}

	bool IsGrounded () {
		return Physics.Raycast (transform.position, - Vector3.up, GroundDistance);
	}

	void FixedUpdate () {
		transform.position = new Vector3 (transform.position.x, transform.position.y, 0);

		float newSpeed = Mathf.SmoothDamp (rb.velocity.x, minSpeed, ref velocityFactor, 2f);
		rb.velocity = new Vector3 (newSpeed, rb.velocity.y, 0);
		if (Input.GetButton ("Jump") && currentBoosts < maxBoosts) {
			rb.AddForce (new Vector3 (0, 12f, 0));
			currentBoosts++;
		}
	}

	void OnCollisionStay (Collision collision) {
		if (collision.gameObject.CompareTag ("PrimaryColor") && currentColor == secondaryColor ||
			collision.gameObject.CompareTag ("SecondaryColor") && currentColor == primaryColor
		) {
			Die ();
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Powerup")) {
			rb.velocity = new Vector3 (25f, rb.velocity.y, 0);
			Destroy (other.gameObject);
		} else if (other.CompareTag ("OutOfBounds")) {
			Die ();
		}
	}

	void Die() {
		Instantiate (deathParticles, this.transform.position, Quaternion.identity);
		AudioSource audioSource = GameObject.FindGameObjectWithTag ("AudioSource").GetComponent<AudioSource>();
		audioSource.clip = deathClip;
		audioSource.Play ();
		Destroy (this.gameObject);
		GameManager.Instance.Restart ();
	}
}
