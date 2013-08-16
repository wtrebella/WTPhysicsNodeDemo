using UnityEngine;
using System.Collections;

public static class WTUtils {
	public static Vector2 screenCenter {get; private set;}
	public static PhysicMaterial defaultPhysicMaterial {get; private set;}
	public static float defaultRigidBodyAngularDrag {get; private set;}
	public static float defaultRigidBodyMass {get; private set;}

	public static void Init() {
		screenCenter = new Vector2(Futile.screen.halfWidth, Futile.screen.halfHeight);

		defaultPhysicMaterial = new PhysicMaterial();
		defaultPhysicMaterial.bounciness = 0.9f;
		defaultPhysicMaterial.dynamicFriction = 0.1f;
		defaultPhysicMaterial.staticFriction = 0.1f;
		defaultPhysicMaterial.frictionCombine = PhysicMaterialCombine.Maximum;

		defaultRigidBodyAngularDrag = 1.0f;
		defaultRigidBodyMass = 10.0f;
	}
}
