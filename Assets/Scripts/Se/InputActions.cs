using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Se {

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

	[Serializable]
	public class Bindings {
		// Displayed in the Editor, and filled in by settings when loading
		// NOTE: They are only public so that they show up in the inspector, but gameplay code MUST refrain from using them directly.
		//
		// The following initializations are just "reasonable defaults" for starting with; they are supposedly overwritten
		// by settings when the game loads. Also, some of said "reasonable default" might be weird and subject to change.
		public ButtonBinding MenuBack = new ButtonBinding(KeyCode.Backspace, XbButton.B);
		public ButtonBinding MenuConfirm = new ButtonBinding(KeyCode.Return, XbButton.A);
		public ButtonBinding PauseMenu = new ButtonBinding(KeyCode.Escape, XbButton.Start);
		public ButtonBinding Jump = new ButtonBinding(KeyCode.Space, XbButton.A);
		public ButtonBinding Dodge = new ButtonBinding(KeyCode.LeftShift, XbButton.RB);
		public ButtonBinding BasicAttack = new ButtonBinding(KeyCode.F, XbButton.X);
		public ButtonBinding MagnetImpact = new ButtonBinding(KeyCode.E, XbButton.LB);
		public ButtonBinding JumpQuake = new ButtonBinding(KeyCode.A, XbButton.Y);
	}

	public static class InputActions {

		public static Bindings Bindings = new Bindings();

		// Properties for use by gameplay code!
		public static bool Jumps { get { return Bindings.Jump.WasJustPressed; } }
		public static bool Dodges { get { return Bindings.Dodge.WasJustPressed; } }
		public static bool IsPunching { get { return Bindings.BasicAttack.IsHeldDown; } }
		public static bool IsAirKicking { get { return IsPunching; } }
		public static bool StartsChargingMagnetImpact { get { return Bindings.MagnetImpact.WasJustPressed; } }
		public static bool StillChargesMagnetImpact { get { return Bindings.MagnetImpact.IsHeldDown; } }
		public static bool UnleashesMagnetImpact { get { return Bindings.MagnetImpact.WasJustReleased; } }
		public static bool StartsChargingJumpQuake { get { return Bindings.JumpQuake.WasJustPressed; } }
		public static bool StillChargesJumpQuake { get { return Bindings.JumpQuake.IsHeldDown; } }
		public static bool UnleashesJumpQuake { get { return Bindings.JumpQuake.WasJustReleased; } }
		public static bool Pauses { get { return Bindings.PauseMenu.WasJustPressed; } }
		public static bool Resumes { get { return Pauses; } }
	
		static Vector2 movementDirectionRaw { 
			get {
				var v = XbInput.GetAxis2D (XbAxis2D.LStick);
				if (v != Vector2.zero) return v;
				v = XbInput.GetAxis2D (XbAxis2D.DPad);
				if (v != Vector2.zero) return v;
				return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			}
		}
		public static Vector2 MovementDirection { 
			get {
				return Vector2.ClampMagnitude (movementDirectionRaw, 1f);
			}
		}
		public static Vector2 CameraMovementDirection { 
			get {
				var v = XbInput.GetAxis2D (XbAxis2D.RStick);
				if (v != Vector2.zero) return v;
				return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			}
		}
	
		public static bool MenuBack { get { return Bindings.MenuBack.WasJustPressed; } }
		public static bool MenuConfirms { get { return Bindings.MenuConfirm.WasJustPressed; } }
		public static int MenuNavigationX { 
			get { 
				refreshMenuNavigationXY ();
				return menuNavigationX == prevMenuNavigationX ? 0 : MenuNavigationX;
			}
		}
		public static int MenuNavigationY { 
			get { 
				refreshMenuNavigationXY ();
				return menuNavigationY == prevMenuNavigationY ? 0 : MenuNavigationY;
			}
		}

		static int prevMenuNavigationX = 0;
		static int prevMenuNavigationY = 0;
		static int menuNavigationX = 0;
		static int menuNavigationY = 0;
		static int menuNavigationLastFrameCount = 0;

		static void refreshMenuNavigationXY() {
			// Ensure it's not done more than once per update
			if (Time.renderedFrameCount == menuNavigationLastFrameCount)
				return;
			menuNavigationLastFrameCount = Time.renderedFrameCount;

			prevMenuNavigationX = menuNavigationX;
			prevMenuNavigationY = menuNavigationY;
			var d = MovementDirection;
			menuNavigationX = Mathf.RoundToInt (d.x);
			menuNavigationY = Mathf.RoundToInt (d.y);
		}
	}
}