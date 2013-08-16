using UnityEngine;
using System.Collections.Generic;
using System;

public class WTPhysicsNode : MonoBehaviour
{
	public static WTPhysicsNode Create()
	{
		GameObject physicsNodeGO = new GameObject("PhysicsNode");
		WTPhysicsNode physicsNode = physicsNodeGO.AddComponent<WTPhysicsNode>();
		return physicsNode;
	}
	
	public FContainer container;

	public FPNodeLink nodeLink;

	public void Init(Vector2 startPos, float startRotation, bool shouldLinkRotation)
	{
		container = new FContainer();
		container.rotation = startRotation;

		gameObject.transform.position = new Vector3(startPos.x * FPhysics.POINTS_TO_METERS,startPos.y * FPhysics.POINTS_TO_METERS,0);
		gameObject.transform.rotation = Quaternion.Euler(0, 0, startRotation);
		gameObject.transform.parent = WTMain.physicsWorld.transform;

		nodeLink = gameObject.AddComponent<FPNodeLink>();
		nodeLink.Init(container, shouldLinkRotation);

		container.ListenForUpdate(HandleUpdate);
		container.ListenForLateUpdate(HandleLateUpdate);
		container.ListenForFixedUpdate(HandleFixedUpdate);
	}

	public void Destroy()
	{
		UnityEngine.Object.Destroy(gameObject);

		container.RemoveFromContainer();
	}

	public Rigidbody AddRigidBody(float angularDrag, float mass) {
		Rigidbody rb = gameObject.AddComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
		rb.angularDrag = angularDrag;
		rb.mass = mass;
		return rb;
	}

	public Rigidbody AddRigidBody() {
		return AddRigidBody(WTUtils.defaultRigidBodyAngularDrag, WTUtils.defaultRigidBodyMass);
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

	public PhysicMaterial SetupPhysicMaterial(float bounciness, float dynamicFriction, float staticFriction, PhysicMaterialCombine frictionCombine) {
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

	}

	void HandleUpdate() {

	}

	void HandleLateUpdate() {

	}

	void HandleFixedUpdate() {

	}

	public Vector2 GetPos() {
		return new Vector2(transform.position.x * FPhysics.METERS_TO_POINTS, transform.position.y * FPhysics.METERS_TO_POINTS);
	}

}