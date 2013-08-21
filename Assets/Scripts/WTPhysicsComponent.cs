using UnityEngine;
using System.Collections.Generic;
using System;

public class WTPhysicsComponent : MonoBehaviour
{
	public event Action<Collision> SignalOnCollisionEnter;
	public event Action<Collision> SignalOnCollisionStay;
	public event Action<Collision> SignalOnCollisionExit;
	public event Action<Collider> SignalOnTriggerEnter;
	public event Action<Collider> SignalOnTriggerExit;
	public event Action<Collider> SignalOnTriggerStay;

	public static WTPhysicsComponent Create(string name) {
		GameObject physicsNodeGO = new GameObject(name);
		WTPhysicsComponent physicsNode = physicsNodeGO.AddComponent<WTPhysicsComponent>();
		return physicsNode;
	}
	
	public WTPhysicsNode container;

	public FPNodeLink nodeLink;

	public void Init(Vector2 startPos, float startRotation, WTPhysicsNode container) {
		this.container = container;
		container.rotation = startRotation;

		SetPosition(startPos);
		gameObject.transform.rotation = Quaternion.Euler(0, 0, startRotation);
		gameObject.transform.parent = FPWorld.instance.transform;

		nodeLink = gameObject.AddComponent<FPNodeLink>();
		nodeLink.Init(container, true);
	}

	// once you call this, you should NOT try to control the position or rotation of the node in code
	// all movement will be determined by the physics engine
	public void StartPhysics() {
		if (rigidbody == null) return;

		rigidbody.isKinematic = false;
	}

	// once you call this, the physics will stop and you'll be able to control its position and rotation in code
	public void StopPhysics() {
		if (rigidbody == null) return;

		rigidbody.isKinematic = true;
	}
	
	public void SetIsTrigger(bool isTrigger) {
		gameObject.GetComponent<Collider>().isTrigger = isTrigger;
	}

	public Rect GetGlobalHitBox() {
		return new Rect(gameObject.collider.bounds.min.x * FPhysics.METERS_TO_POINTS,
		                gameObject.collider.bounds.min.y * FPhysics.METERS_TO_POINTS,
		                gameObject.collider.bounds.size.x * FPhysics.METERS_TO_POINTS,
		                gameObject.collider.bounds.size.y * FPhysics.METERS_TO_POINTS);
	}

	public void Destroy() {
		UnityEngine.Object.Destroy(gameObject);

		container.RemoveFromContainer();
	}

	public Rigidbody AddRigidBody(float angularDrag, float mass) {
		Rigidbody rb = gameObject.AddComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
		rb.angularDrag = angularDrag;
		rb.mass = mass;
		rb.maxAngularVelocity = 10000;
		rb.isKinematic = true;
		return rb;
	}

	public FPPolygonalCollider AddPolygonalCollider(Vector2[] vertices, bool withDebugView = false) {
		FPPolygonalCollider fp = gameObject.AddComponent<FPPolygonalCollider>();
		fp.Init(new FPPolygonalData(vertices, false));

		if (withDebugView) FPDebugRenderer.Create(gameObject, Futile.stage, 0x00FF00, false);

		return fp;
	}

	public SphereCollider AddSphereCollider(float radius, bool withDebugView = false) {
		SphereCollider sc = gameObject.AddComponent<SphereCollider>();
		sc.radius = radius * FPhysics.POINTS_TO_METERS;

		if (withDebugView) FPDebugRenderer.Create(gameObject, Futile.stage, 0x00FF00, false);

		return sc;
	}

	public BoxCollider AddBoxCollider(Vector2 size, bool withDebugView = false) {
		BoxCollider bc = gameObject.AddComponent<BoxCollider>();
		bc.size = new Vector3(size.x * FPhysics.POINTS_TO_METERS, size.y * FPhysics.POINTS_TO_METERS, FPhysics.DEFAULT_Z_THICKNESS);

		if (withDebugView) FPDebugRenderer.Create(gameObject, Futile.stage, 0x00FF00, false);

		return bc;
	}

	public BoxCollider AddBoxCollider(float x, float y, bool withDebugView = false) {
		return AddBoxCollider(new Vector2(x, y), withDebugView);
	}

	public PhysicMaterial SetupPhysicMaterial(float bounciness, float dynamicFriction, float staticFriction, PhysicMaterialCombine frictionCombine = PhysicMaterialCombine.Average) {
		if (collider == null) throw new FutileException("must have collider before adding physicMaterial");
		
		PhysicMaterial physicMaterial = new PhysicMaterial();
		physicMaterial.bounciness = bounciness;
		physicMaterial.dynamicFriction = dynamicFriction;
		physicMaterial.staticFriction = staticFriction;
		physicMaterial.frictionCombine = frictionCombine;
		collider.material = physicMaterial;
		return physicMaterial;
	}

	void OnCollisionEnter(Collision coll) {
		if (SignalOnCollisionEnter != null) SignalOnCollisionEnter(coll);
	}

	void OnCollisionStay(Collision coll) {
		if (SignalOnCollisionStay != null) SignalOnCollisionStay(coll);
	}

	void OnCollisionExit(Collision coll) {
		if (SignalOnCollisionExit != null) SignalOnCollisionExit(coll);
	}

	void OnTriggerEnter(Collider coll) {
		if (SignalOnTriggerEnter != null) SignalOnTriggerEnter(coll);
	}

	void OnTriggerExit(Collider coll) {
		if (SignalOnTriggerExit != null) SignalOnTriggerExit(coll);
	}

	void OnTriggerStay(Collider coll) {
		if (SignalOnTriggerStay != null) SignalOnTriggerStay(coll);
	}

	public void AddForce(float xForce, float yForce, ForceMode forceMode = ForceMode.Force) {
		if (rigidbody == null) return;

		rigidbody.AddForce(new Vector3(xForce, yForce, 0), forceMode);
	}

	public void AddForceAtPosition(float xForce, float yForce, float xPosition, float yPosition, ForceMode forceMode = ForceMode.Force) {
		if (rigidbody == null) return;

		rigidbody.AddForceAtPosition(new Vector3(xForce, yForce, 0), new Vector2(xPosition, yPosition) * FPhysics.POINTS_TO_METERS, forceMode);
	}

	public void SetPosition(Vector2 pos) {
		if (!IsControlledByPhysicsEngine()) {
			gameObject.transform.position = new Vector3(pos.x * FPhysics.POINTS_TO_METERS,pos.y * FPhysics.POINTS_TO_METERS,0);
		}
		else {
			Debug.Log("can't move because it's not kinematic!");
		}
	}

	public void SetRotation(float rot) {
		if (!IsControlledByPhysicsEngine()) {
			gameObject.transform.rotation = Quaternion.Euler(0, 0, -rot);
		}
		else {
			Debug.Log("can't rotate because it's not kinematic!");
		}
	}

	public bool IsControlledByPhysicsEngine() {
		return !((rigidbody != null && rigidbody.isKinematic) || rigidbody == null);
	}
}
