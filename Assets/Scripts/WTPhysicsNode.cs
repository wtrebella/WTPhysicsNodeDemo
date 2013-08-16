using UnityEngine;
using System.Collections;

public class WTPhysicsNode : FContainer {
	public WTPhysicsComponent physicsComponent;

	public WTPhysicsNode(string name) {
		InitPhysicsComponent(name);

		ListenForUpdate(HandleUpdate);
		ListenForFixedUpdate(HandleFixedUpdate);
		ListenForLateUpdate(HandleLateUpdate);
	}

	public void UpdatePositions() {
		if (physicsComponent.CanMoveInCode()) {
			physicsComponent.SetPosition(GetPosition());
			physicsComponent.SetRotation(rotation);
		}
	}

	virtual protected void InitPhysicsComponent(string name) {
		physicsComponent = WTPhysicsComponent.Create(name);
		physicsComponent.Init(Vector2.zero, 0, true, this);
		HandleUpdate();
	}

	virtual public void HandleUpdate() {
		UpdatePositions();
	}

	virtual public void HandleFixedUpdate() {

	}

	virtual public void HandleLateUpdate() {

	}
}
