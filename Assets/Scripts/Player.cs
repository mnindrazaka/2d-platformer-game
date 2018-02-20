using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody2D myRigidBody;
	private Animator myAnimator;

	public float movementSpeed = 10f; 
	private bool facingRight = true;

	private bool attack;
	private bool slide;

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
		HandleMovement (horizontal);
		HandleAttacks ();

		if (!this.myAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Slide")) {
			Flip (horizontal);
		}

		ResetValues ();
	}

	private void HandleMovement(float horizontal) {
		if (!myAnimator.GetBool ("slide") && !myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")) {
			myRigidBody.velocity = new Vector2 (horizontal * movementSpeed, myRigidBody.velocity.y);
		} else if (myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")) {
			myRigidBody.velocity = Vector2.zero;
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
	}
}
