using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody2D myRigidBody;
	private Animator myAnimator;

	public float movementSpeed = 10f; 
	private bool facingRight = true;

	public bool attack;

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
		Flip (horizontal);
		HandleAttacks ();

		ResetValues ();
	}

	private void HandleMovement(float horizontal) {
		if (!myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")) {
			myRigidBody.velocity = new Vector2 (horizontal * movementSpeed, myRigidBody.velocity.y);
		} else {
			myRigidBody.velocity = Vector2.zero;
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
	}
}
