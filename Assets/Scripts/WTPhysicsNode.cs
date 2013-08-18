using UnityEngine;
using System.Collections;

// IMPORTANT: when controlling a WTPhysicsNode's position or rotation in code (i.e. when the physic engine is stopped), you MUST
// use "SetNewRotation" and "SetNewPosition" rather than FContainer's base "SetRotation" method and "rotation" ivar. otherwise the
// physics colliders will not be correctly positioned and rotated.

public class WTPhysicsNode : FContainer {
	public WTPhysicsComponent physicsComponent;

	public WTPhysicsNode(string name) {
		InitPhysicsComponent(name);

		ListenForUpdate(HandleUpdate);
		ListenForFixedUpdate(HandleFixedUpdate);
		ListenForLateUpdate(HandleLateUpdate);
	}

	// keeps the position of the collider correctly synced with the position of the node when controlling with code
	public void UpdatePosition() {
		if (physicsComponent.IsControlledByPhysicsEngine()) return;

		physicsComponent.SetPosition(GetPosition());
	}

	// keeps the rotation of the collider correctly synced with the position of the node when controlling with code
	public void UpdateRotation() {
		if (physicsComponent.IsControlledByPhysicsEngine()) return;

		physicsComponent.SetRotation(rotation);
	}

	private void InitPhysicsComponent(string name) {
		physicsComponent = WTPhysicsComponent.Create(name);
		physicsComponent.Init(Vector2.zero, 0, this);
		physicsComponent.SignalOnCollisionEnter += HandleOnCollisionEnter;
		physicsComponent.SignalOnTriggerEnter += HandleOnTriggerEnter;
		physicsComponent.SignalOnTriggerExit += HandleOnTriggerExit;
		physicsComponent.SignalOnTriggerStay += HandleOnTriggerStay;
		UpdatePosition();
		UpdateRotation();
	}

	// if you override this make SURE you call base.HandleUpdate
	virtual public void HandleUpdate() {
		UpdatePosition();
		UpdateRotation();

		FPDebugRenderer debugRenderer = physicsComponent.gameObject.GetComponent<FPDebugRenderer>();
		if (debugRenderer != null) debugRenderer.Update();
	}

	virtual public void HandleFixedUpdate() {

	}

	virtual public void HandleLateUpdate() {

	}

	// this will be called whenever something hits it
	virtual public void HandleOnCollisionEnter(Collision coll) {
		// take these out obviously if you want to keep this method abstract. i just put these in for ease.

		if (coll.gameObject.collider.GetType() == typeof(BoxCollider)) FSoundManager.PlaySound("boop1", 0.1f);
		if (coll.gameObject.collider.GetType() == typeof(SphereCollider)) FSoundManager.PlaySound("boop2", 0.1f);
		if (coll.gameObject.collider.GetType() == typeof(MeshCollider)) FSoundManager.PlaySound("boop3", 0.1f);
	}

	// if you set the physicsComponent to be a trigger, these methods will be called when another collider passes through
	virtual public void HandleOnTriggerEnter(Collider coll) {

	}

	virtual public void HandleOnTriggerExit(Collider coll) {

	}

	virtual public void HandleOnTriggerStay(Collider coll) {

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
