using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Se {
	public class InputActions : MonoBehaviour {
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
		}
		// Displayed in the Editor, and filled in by settings when loading
		// NOTE: They are only public so that they show up in the inspector, but gameplay code MUST refrain from using them directly.
		public ButtonBinding MenuBackBinding, MenuConfirmBinding, PauseMenuBinding;
		public ButtonBinding JumpBinding, DodgeBinding, BasicAttackBinding, MagnetImpactBinding, JumpQuakeBinding;
	
		// Properties for use by gameplay code!
		public bool Jumps { get { return JumpBinding.WasJustPressed; } }
		public bool IsPunching { get { return BasicAttackBinding.IsHeldDown; } }
		public bool IsAirKicking { get { return IsPunching; } }
		public bool Dodges { get { return DodgeBinding.WasJustPressed; } }
		public bool Pauses { get { return PauseMenuBinding.WasJustPressed; } }
		public bool Resumes { get { return Pauses; } }
		public bool StartsChargingMagnetImpact { get { return MagnetImpactBinding.WasJustPressed; } }
		public bool StillChargesMagnetImpact { get { return MagnetImpactBinding.IsHeldDown; } }
		public bool UnleashesMagnetImpact { get { return MagnetImpactBinding.WasJustReleased; } }
		public bool StartsChargingJumpQuake { get { return JumpQuakeBinding.WasJustPressed; } }
		public bool StillChargesJumpQuake { get { return JumpQuakeBinding.IsHeldDown; } }
		public bool UnleashesJumpQuake { get { return JumpQuakeBinding.WasJustReleased; } }
	
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