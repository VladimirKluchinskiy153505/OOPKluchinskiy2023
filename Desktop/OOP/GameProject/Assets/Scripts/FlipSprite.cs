using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour
{
	private FlipSprite sprite;
	private bool facingRight;
	void start()
	{
		facingRight = true;
	}
	void FixedUpdate()
	{
		float horizontal = Input.GetAxis("Horizontal");

		Flip(horizontal);
	}
	private void Flip(float horizontal)
	{
		if (horizontal > 0 && facingRight || horizontal < 0 && !facingRight)
		{
			facingRight = !facingRight;

			Vector3 theScale = transform.localScale;

			theScale.x *= -1;

			transform.localScale = theScale;
		}
	}
}
