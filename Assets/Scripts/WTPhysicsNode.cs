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

	public void UpdatePosition() {
		if (!physicsComponent.CanMoveInCode()) return;

		physicsComponent.SetPosition(GetPosition());
	}

	public void UpdateRotation() {
		if (!physicsComponent.CanMoveInCode()) return;

		physicsComponent.SetRotation(rotation);
	}

	virtual protected void InitPhysicsComponent(string name) {
		physicsComponent = WTPhysicsComponent.Create(name);
		physicsComponent.Init(Vector2.zero, 0, true, this);
		physicsComponent.SignalOnCollisionEnter += HandleOnCollisionEnter;
		HandleUpdate();
	}

	virtual public void HandleUpdate() {
		UpdatePosition();
		UpdateRotation();
	}

	virtual public void HandleFixedUpdate() {

	}

	virtual public void HandleLateUpdate() {

	}

	virtual public void HandleOnCollisionEnter(Collision coll) {
		FSoundManager.PlaySound("boop", 0.3f);
	}

	public void SetNewPosition(float xNew, float yNew) {
		SetPosition(xNew, yNew);
		UpdatePosition();
	}

	public void SetNewPosition(Vector2 position) {
		SetNewPosition(position.x, position.y);
	}

	public void SetNewRotation(float rot) {
		rotation = rot;
		UpdateRotation();
	}
}
