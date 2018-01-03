using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Se {
	public abstract class FsmState {
		public abstract FsmState OnUpdate(GameObject go);
		public void OnEnterState(GameObject go) {}
		public void OnLeaveState(GameObject go) {}
	}

	public class Fsm {
		Fsm () {}
		public Fsm(FsmState initialState) {
			currentState = initialState;
		}

		FsmState currentState;

		public void OnStart (GameObject gameObject) {
			currentState.OnEnterState(gameObject);
		}
		public void OnUpdate (GameObject gameObject) {
			var nextState = currentState.OnUpdate(gameObject);
			if(nextState == currentState)
				return;
			currentState.OnLeaveState(gameObject);
			nextState.OnEnterState(gameObject);
			currentState = nextState;
		}
	}
}