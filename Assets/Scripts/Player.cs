using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody2D myRigidBody;
	private Animator myAnimator;

	public float movementSpeed = 10f; 
	private bool facingRight = true;

	// Use this for initialization
	void Start () {
		myRigidBody = this.GetComponent<Rigidbody2D> ();
		myAnimator = this.GetComponent<Animator> ();
	}
		
	// Update is called once per frame
	void FixedUpdate () {
		float horizontal = Input.GetAxis ("Horizontal");
		HandleMovement (horizontal);
		flip (horizontal);
	}

	void HandleMovement(float horizontal) {
		myAnimator.SetFloat ("speed", Mathf.Abs(horizontal));
		myRigidBody.velocity = new Vector2 (horizontal * movementSpeed, myRigidBody.velocity.y);
	}

	void flip(float horizontal) {
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			facingRight = !facingRight;
			Vector3 theScale = this.transform.localScale;
			theScale.x *= -1;
			this.transform.localScale = theScale;
		}
	}
}
