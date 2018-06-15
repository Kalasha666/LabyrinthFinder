using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LFInput
{
	public class LFUserInput{

		public KeyCode exitButton = KeyCode.Escape;
		public KeyCode upButton = KeyCode.W;
		public KeyCode downButton = KeyCode.S;
		public KeyCode leftButton = KeyCode.A;
		public KeyCode rightButton = KeyCode.D;
		public KeyCode attackButton = KeyCode.Space;
		public KeyCode eatButton = KeyCode.E;

		public delegate void ExitAction();
		public ExitAction exitAction;
		public delegate void AttackAction();
		public AttackAction attackAction;
		public delegate void AttackEndAction();
		public AttackEndAction attackEndAction;
		public delegate void EatAction();
		public EatAction eatAction;

		private Vector3 _direction = Vector3.zero;

		public LFUserInput()
		{
			exitButton = KeyCode.Escape;
			upButton = KeyCode.W;
			downButton = KeyCode.S;
			leftButton = KeyCode.A;
			rightButton = KeyCode.D;
			attackButton = KeyCode.Space;
			eatButton = KeyCode.E;
			Clean (); 
		}

		public Vector3 Direction
		{
			get{return _direction;}
		}
		// Update is called once per frame
		public void Update () {
			if (Input.GetKeyDown (exitButton)) {
				Exit();
			}

			if (Input.GetKeyDown (attackButton)) {
				Attack ();
			} else if (Input.GetKeyUp (attackButton)) {
				AttackEnd ();
			}

			if (Input.GetKeyDown (eatButton)) {
				Eat ();
			}

			Vector3 newDirect = Vector3.zero;

			if (Input.GetKey (upButton))
			{
				newDirect += Vector3.up;
			}

			if (Input.GetKey (downButton))
			{
				newDirect += Vector3.down;
			}

			if (Input.GetKey (leftButton))
			{
				newDirect += Vector3.left;
			}

			if (Input.GetKey (rightButton))
			{
				newDirect += Vector3.right;
			}

			if(!Input.GetKey (rightButton) && !Input.GetKey (leftButton) && !Input.GetKey (downButton) && !Input.GetKey (upButton))
			{
				newDirect = Vector3.zero;
			}

			_direction = newDirect.normalized;
		}

		public void Clean()
		{
			exitAction = null;
			attackAction = null;
		}

		private void Exit()
		{
			if (exitAction != null) {
				exitAction ();
			}
		}

		private void Attack()
		{
			if (attackAction != null) {
				attackAction ();
			}
		}

		private void AttackEnd()
		{
			if (attackEndAction != null) {
				attackEndAction ();
			}
		}

		private void Eat()
		{
			if (eatAction != null) {
				eatAction ();
			}
		}
			
	}
}
