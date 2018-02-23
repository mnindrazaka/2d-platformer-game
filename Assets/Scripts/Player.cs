using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody2D myRigidBody;
	private Animator myAnimator;

	[SerializeField]
	private float movementSpeed = 10f; 
	private bool facingRight = true;

	private bool attack;
	private bool slide;

	[SerializeField]
	private Transform[] groundPoints;

	[SerializeField]
	private float groundRadius;

	[SerializeField]
	private LayerMask whatIsGround;
	private bool isGrounded;

	[SerializeField]
	private float jumpForce;
	private bool jump;

	private bool airControl;

	// Use this for initialization
	void Start () {
		myRigidBody = this.GetComponent<Rigidbody2D> ();
		myAnimator = this.GetComponent<Animator> ();
	}
		
	// Update is called once per frame
	void Update() {
		HandleInput ();
	}

	void FixedUpdate () {
		float horizontal = Input.GetAxis ("Horizontal");
		isGrounded = IsGrounded ();

		HandleMovement (horizontal);
		HandleAttacks ();

		if (!this.myAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Slide")) {
			Flip (horizontal);
		}

		ResetValues ();
	}

	private void HandleMovement(float horizontal) {
		if (!myAnimator.GetBool ("slide") && !myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Attack") && (isGrounded || airControl)) {
			myRigidBody.velocity = new Vector2 (horizontal * movementSpeed, myRigidBody.velocity.y);
		} else if (myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")) {
			myRigidBody.velocity = Vector2.zero;
		}

		if(isGrounded && jump) {
			isGrounded = false;
			myRigidBody.AddForce (new Vector2 (0, jumpForce));
		}

		if (slide && !myAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Slide")) {
			myAnimator.SetBool ("slide", true);
		} else if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slide")) {
			myAnimator.SetBool ("slide", false);
		}

		myAnimator.SetFloat ("speed", Mathf.Abs(horizontal));
	}

	private void HandleAttacks() {
		if (attack) {
			myAnimator.SetTrigger ("attack");
		}
	}

	private void HandleInput() {
		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			attack = true;
		}

		if (Input.GetKeyDown (KeyCode.LeftControl)) {
			slide = true;
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			jump = true;
		}
	}

	private void Flip(float horizontal) {
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			facingRight = !facingRight;
			Vector3 theScale = this.transform.localScale;
			theScale.x *= -1;
			this.transform.localScale = theScale;
		}
	}

	private void ResetValues() {
		attack = false;
		slide = false;
		jump = false;
	}

	private bool IsGrounded() {
		if (myRigidBody.velocity.y <= 0) {
			foreach (Transform point in groundPoints) {
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);
				foreach (Collider2D collider in colliders) {
					if (collider.gameObject != gameObject) {
						return true;
					}
				}
			}
		}
		return false;
	}
}
