using UnityEngine;
using System.Collections;

public class WTPhysicsRay {
	public static bool Raycast(WTPhysicsRay ray, out WTPhysicsRaycastHit raycastHit, float distance) {
		raycastHit = new WTPhysicsRaycastHit();
		return Physics.Raycast(ray.ray, out raycastHit.raycastHit, distance * FPhysics.POINTS_TO_METERS);
	}

	public Ray ray;

	public WTPhysicsRay(WTPhysicsNode node, Vector2 originInPercents, Vector3 direction) {
		if (originInPercents.x < 0 || originInPercents.x > 1 || originInPercents.y < 0 || originInPercents.y > 1) throw new FutileException("bad originInPercents");

		float xOrigin = node.physicsComponent.collider.bounds.min.x + originInPercents.x * node.physicsComponent.collider.bounds.size.x;
		float yOrigin = node.physicsComponent.collider.bounds.min.y + originInPercents.y * node.physicsComponent.collider.bounds.size.y;

		xOrigin = Mathf.Min(xOrigin, node.physicsComponent.collider.bounds.max.x);
		yOrigin = Mathf.Min(yOrigin, node.physicsComponent.collider.bounds.max.y);

		ray = new Ray(new Vector3(xOrigin, yOrigin, node.physicsComponent.collider.bounds.center.z), direction);

	}
}

public class WTPhysicsRaycastHit {
	public RaycastHit raycastHit;

	public WTPhysicsRaycastHit() {

	}

	public WTPhysicsNode GetPhysicsNode() {
		return raycastHit.collider.gameObject.GetComponent<WTPhysicsComponent>().container;
	}

	public Vector2 GetPoint() {
		return new Vector2(raycastHit.point.x * FPhysics.METERS_TO_POINTS, raycastHit.point.y * FPhysics.METERS_TO_POINTS);
	}
}
