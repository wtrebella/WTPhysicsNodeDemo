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

	public bool CompareTag(string tagToCompare) {
		return physicsComponent.gameObject.CompareTag(tagToCompare);
	}

	private void InitPhysicsComponent(string name) {
		physicsComponent = WTPhysicsComponent.Create(name);
		physicsComponent.Init(Vector2.zero, 0, this);
		physicsComponent.SignalOnCollisionEnter += HandleOnCollisionEnter;
		physicsComponent.SignalOnCollisionStay += HandleOnCollisionStay;
		physicsComponent.SignalOnCollisionExit += HandleOnCollisionExit;
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

	}

	virtual public void HandleOnCollisionStay(Collision coll) {

	}

	virtual public void HandleOnCollisionExit(Collision coll) {

	}

	// if you set the physicsComponent to be a trigger, these methods will be called when another collider passes through
	virtual public void HandleOnTriggerEnter(Collider coll) {

	}

	virtual public void HandleOnTriggerExit(Collider coll) {

	}

	virtual public void HandleOnTriggerStay(Collider coll) {

	}

	override public float x {
		get {return _x;}
		set {
			_x = value;
			_isMatrixDirty = true;
			UpdatePosition();
		}
	}

	override public float y {
		get {return _y;}
		set {
			_y = value;
			_isMatrixDirty = true;
			UpdatePosition();
		}
	}

	override public float rotation {
		get {return _rotation;}
		set {
			_rotation = value;
			_isMatrixDirty = true;
			UpdateRotation();
		}
	}
}
