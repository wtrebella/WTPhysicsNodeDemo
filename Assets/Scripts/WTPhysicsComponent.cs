using UnityEngine;
using System.Collections.Generic;
using System;

public class WTPhysicsComponent : MonoBehaviour
{
	public event Action<Collision> SignalOnCollisionEnter;

	public static WTPhysicsComponent Create(string name) {
		GameObject physicsNodeGO = new GameObject(name);
		WTPhysicsComponent physicsNode = physicsNodeGO.AddComponent<WTPhysicsComponent>();
		return physicsNode;
	}
	
	public FContainer container;

	public FPNodeLink nodeLink;

	public void Init(Vector2 startPos, float startRotation, bool shouldLinkRotation, FContainer container) {
		this.container = container;
		container.rotation = startRotation;

		SetPosition(startPos);
		gameObject.transform.rotation = Quaternion.Euler(0, 0, startRotation);
		gameObject.transform.parent = FPWorld.instance.transform;

		nodeLink = gameObject.AddComponent<FPNodeLink>();
		nodeLink.Init(container, shouldLinkRotation);
	}

	public void StartPhysics() {
		rigidbody.isKinematic = false;
	}

	public void StopPhysics() {
		rigidbody.isKinematic = true;
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
		rb.maxAngularVelocity = WTUtils.defaultRigidBodyMaxAngularVelocity;
		rb.isKinematic = true;
		return rb;
	}

	public Rigidbody AddRigidBody() {
		return AddRigidBody(WTUtils.defaultRigidBodyAngularDrag, WTUtils.defaultRigidBodyMass);
	}

	public FPPolygonalCollider AddPolygonalCollider(Vector2[] vertices, bool shouldDecomposeIntoConvexPolygons, bool withDebugView = false) {
		FPPolygonalCollider fp = gameObject.AddComponent<FPPolygonalCollider>();
		fp.Init(new FPPolygonalData(vertices, shouldDecomposeIntoConvexPolygons));

		if (withDebugView) FPDebugRenderer.Create(gameObject, Futile.stage, 0x00FF00, false);

		return fp;
	}

	public SphereCollider AddSphereCollider(float radius) {
		SphereCollider sc = gameObject.AddComponent<SphereCollider>();
		sc.radius = radius * FPhysics.POINTS_TO_METERS;
		return sc;
	}

	public BoxCollider AddBoxCollider(Vector2 size) {
		BoxCollider bc = gameObject.AddComponent<BoxCollider>();
		bc.size = new Vector3(size.x * FPhysics.POINTS_TO_METERS, size.y * FPhysics.POINTS_TO_METERS, FPhysics.DEFAULT_Z_THICKNESS);
		return bc;
	}

	public BoxCollider AddBoxCollider(float x, float y) {
		return AddBoxCollider(new Vector2(x, y));
	}

	public PhysicMaterial SetupPhysicMaterial() {
		PhysicMaterial pm = WTUtils.defaultPhysicMaterial;
		return SetupPhysicMaterial(pm.bounciness, pm.dynamicFriction, pm.staticFriction, pm.frictionCombine);
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

	public void AddForce(float xForce, float yForce, ForceMode forceMode = ForceMode.Force) {
		rigidbody.AddForce(new Vector3(xForce, yForce, 0), forceMode);
	}

	public void AddForceAtPosition(float xForce, float yForce, float xPosition, float yPosition, ForceMode forceMode = ForceMode.Force) {
		rigidbody.AddForceAtPosition(new Vector3(xForce, yForce, 0), new Vector2(xPosition, yPosition) * FPhysics.POINTS_TO_METERS, forceMode);
	}

	public Vector2 GetPosition() {
		return new Vector2(transform.position.x * FPhysics.METERS_TO_POINTS, transform.position.y * FPhysics.METERS_TO_POINTS);
	}

	public void SetPosition(Vector2 pos) {
		if (CanMoveInCode()) {
			gameObject.transform.position = new Vector3(pos.x * FPhysics.POINTS_TO_METERS,pos.y * FPhysics.POINTS_TO_METERS,0);
		}
		else {
			Debug.Log("can't move because it's not kinematic!");
		}
	}

	public float GetRotation() {
		return gameObject.transform.rotation.eulerAngles.z;
	}

	public void SetRotation(float rot) {
		if (CanMoveInCode()) {
			gameObject.transform.rotation = Quaternion.Euler(0, 0, -rot);
		}
		else {
			Debug.Log("can't rotate because it's not kinematic!");
		}
	}

	public bool CanMoveInCode() {
		return (rigidbody != null && rigidbody.isKinematic) || rigidbody == null;
	}
}