using UnityEngine;

public class XbInputShow: MonoBehaviour {
	public bool A, B, X, Y, LB, RB, Back, Start, LStickClick, RStickClick;
	public Vector2 LStick, RStick, DPad;
	[Range(0f, 1f)] public float LT, RT;

	void Update() {
		A = XbInput.GetButton (XbButton.A);
		B = XbInput.GetButton (XbButton.B);
		X = XbInput.GetButton (XbButton.X);
		Y = XbInput.GetButton (XbButton.Y);
		LB = XbInput.GetButton (XbButton.LB);
		RB = XbInput.GetButton (XbButton.RB);
		Back = XbInput.GetButton (XbButton.Back);
		Start = XbInput.GetButton (XbButton.Start);
		LStickClick = XbInput.GetButton (XbButton.LStickClick);
		RStickClick = XbInput.GetButton (XbButton.RStickClick);
		LStick = XbInput.GetAxis2D (XbAxis2D.LStick);
		RStick = XbInput.GetAxis2D (XbAxis2D.RStick);
		DPad = XbInput.GetAxis2D (XbAxis2D.DPad);
		LT = XbInput.GetAxis (XbAxis.LT);
		RT = XbInput.GetAxis (XbAxis.RT);
	}
}