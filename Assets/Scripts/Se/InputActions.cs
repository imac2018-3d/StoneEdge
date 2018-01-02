using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Se {

	// NOTE: I would like this to be a ScriptableObject, but it's not possible right now.
	// It needs to register for Update() events and this can only be done by being a MonoBehaviour.
	// Let's just pretend it is a singleton for now. This is enforced in the Awake() override.
	public class InputActions : MonoBehaviour {

		void Awake() {
			Exit.If(1 != FindObjectsOfType<InputActions>().Length, "There must be exactly one InputActions instance!");
		}

		[Serializable]
		public struct ButtonBinding {
			public KeyCode Keyboard;
			public XbButton XboxController;
			public List<KeyCode> ExtraAliases;
			// NOTE: No need for an extra List<XbButton>: Controller bindings don't have aliases, but Keyboard bindings may.
			// For instance, surely we would like the "MenuConfirm" action mapped to both Enter and Space keys.
			// On the other hand, this would be just the A button on a controller and we're done.
	
			public bool IsHeldDown { get { return Input.GetKey (Keyboard) || XbInput.GetButton (XboxController) || ExtraAliases.Any(Input.GetKey); } }
			public bool WasJustPressed { get { return Input.GetKeyDown (Keyboard) || XbInput.GetButtonDown(XboxController) || ExtraAliases.Any(Input.GetKeyDown); } }
			public bool WasJustReleased { get { return Input.GetKeyUp (Keyboard) || XbInput.GetButtonUp(XboxController) || ExtraAliases.Any(Input.GetKeyDown); } }

			public ButtonBinding(KeyCode c, XbButton b, params KeyCode[] a) {
				Keyboard = c;
				XboxController = b;
				ExtraAliases = a == null ? new List<KeyCode>() : a.ToList();
			}
		}

		// Displayed in the Editor, and filled in by settings when loading
		// NOTE: They are only public so that they show up in the inspector, but gameplay code MUST refrain from using them directly.
		//
		// The following initializations are just "reasonable defaults" for starting with; they are supposedly overwritten
		// by settings when the game loads. Also, some of said "reasonable default" might be weird and subject to change.
		public ButtonBinding MenuBackBinding = new ButtonBinding(KeyCode.Escape, XbButton.B, KeyCode.Backspace);
		public ButtonBinding MenuConfirmBinding = new ButtonBinding(KeyCode.Return, XbButton.A, KeyCode.Space);
		public ButtonBinding PauseMenuBinding = new ButtonBinding(KeyCode.Escape, XbButton.Start);
		public ButtonBinding JumpBinding = new ButtonBinding(KeyCode.Space, XbButton.A);
		public ButtonBinding DodgeBinding = new ButtonBinding(KeyCode.LeftShift, XbButton.RB);
		public ButtonBinding BasicAttackBinding = new ButtonBinding(KeyCode.F, XbButton.X);
		public ButtonBinding MagnetImpactBinding = new ButtonBinding(KeyCode.E, XbButton.B);
		public ButtonBinding JumpQuakeBinding = new ButtonBinding(KeyCode.A, XbButton.Y);
	
		// Properties for use by gameplay code!
		public bool Jumps { get { return JumpBinding.WasJustPressed; } }
		public bool Dodges { get { return DodgeBinding.WasJustPressed; } }
		public bool IsPunching { get { return BasicAttackBinding.IsHeldDown; } }
		public bool IsAirKicking { get { return IsPunching; } }
		public bool StartsChargingMagnetImpact { get { return MagnetImpactBinding.WasJustPressed; } }
		public bool StillChargesMagnetImpact { get { return MagnetImpactBinding.IsHeldDown; } }
		public bool UnleashesMagnetImpact { get { return MagnetImpactBinding.WasJustReleased; } }
		public bool StartsChargingJumpQuake { get { return JumpQuakeBinding.WasJustPressed; } }
		public bool StillChargesJumpQuake { get { return JumpQuakeBinding.IsHeldDown; } }
		public bool UnleashesJumpQuake { get { return JumpQuakeBinding.WasJustReleased; } }
		public bool Pauses { get { return PauseMenuBinding.WasJustPressed; } }
		public bool Resumes { get { return Pauses; } }
	
		public Vector2 MovementDirection { 
			get {
				var v = XbInput.GetAxis2D (XbAxis2D.LStick);
				if (v != Vector2.zero) return v;
				v = XbInput.GetAxis2D (XbAxis2D.DPad);
				if (v != Vector2.zero) return v;
				return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			}
		}
		public Vector2 CameraMovementDirection { 
			get {
				var v = XbInput.GetAxis2D (XbAxis2D.RStick);
				if (v != Vector2.zero) return v;
				return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			}
		}
	
		public bool MenuBack { get { return MenuBackBinding.WasJustPressed; } }
		public bool MenuConfirms { get { return MenuConfirmBinding.WasJustPressed; } }
		public int MenuNavigationX { get { return curMenuNavigationX != prevMenuNavigationX ? curMenuNavigationX : 0; } }
		public int MenuNavigationY { get { return curMenuNavigationY != prevMenuNavigationY ? curMenuNavigationY : 0; } }

		int prevMenuNavigationX = 0;
		int prevMenuNavigationY = 0;
		int curMenuNavigationX = 0;
		int curMenuNavigationY = 0;
	
		void Update() {
			prevMenuNavigationX = curMenuNavigationX;
			prevMenuNavigationY = curMenuNavigationY;
			var d = MovementDirection;
			curMenuNavigationX = Mathf.RoundToInt(d.x);
			curMenuNavigationY = Mathf.RoundToInt(d.y);
		}
	}
}