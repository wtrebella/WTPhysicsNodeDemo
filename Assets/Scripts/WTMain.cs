using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTMain : MonoBehaviour {
	List<WTPhysicsNode> physicsNodes;
	float forceDelay = 0.2f;
	float nextTime = 0;
	WTPhysicsNode square;
	WTPhysicsNode circle;
	WTPhysicsNode polygon;

	// for fun, let's have the polygon float in the air, moving and spinning, without physics for 3 seconds before it becomes controlled by physics
	float delayUntilPolygonIsControlledByPhysics = 3;

	void Start() {
		Go.defaultEaseType = EaseType.SineInOut;

		FutileParams fp = new FutileParams(true, true, false, false);
		fp.AddResolutionLevel(480f, 1.0f, 1.0f, "-res1");
		fp.AddResolutionLevel(1136f, 2.0f, 2.0f, "-res2");
		fp.AddResolutionLevel(2048f, 4.0f, 4.0f, "-res4");
		
		fp.backgroundColor = Color.white;
		fp.origin = Vector2.zero;

		Futile.instance.Init(fp);

		Futile.atlasManager.LoadAtlas("Atlases/MainSheet");
		Futile.atlasManager.LoadFont("franchise", "franchise", "Atlases/franchise", -7, -16);
		// futile done initing

		FSoundManager.PlayMusic("song");

		FPWorld.Create(64.0f);

		// make the walls super thick so stuff doesn't accidentally fly through when forces are massive
		float wallThickness = 100000;

		WTBasicWall leftWall = new WTBasicWall(wallThickness, Futile.screen.height * 10);
		WTBasicWall rightWall = new WTBasicWall(wallThickness, Futile.screen.height * 10);
		WTBasicWall bottomWall = new WTBasicWall(Futile.screen.width * 10, wallThickness);
		WTBasicWall topWall = new WTBasicWall(Futile.screen.width * 10, wallThickness);

		leftWall.SetPosition(-wallThickness/2f + 10, Futile.screen.halfHeight);
		rightWall.SetPosition(Futile.screen.width + wallThickness/2f - 10, Futile.screen.halfHeight);
		bottomWall.SetPosition(Futile.screen.halfWidth, -wallThickness/2f + 10);
		topWall.SetPosition(Futile.screen.halfWidth, Futile.screen.height + wallThickness/2f - 10);

		Futile.stage.AddChild(leftWall);
		Futile.stage.AddChild(rightWall);
		Futile.stage.AddChild(bottomWall);
		Futile.stage.AddChild(topWall);

		physicsNodes = new List<WTPhysicsNode>();

		// create the square
		FSprite squareSprite = new FSprite("coolSquare");
		square = new WTPhysicsNode("square");
		square.AddChild(squareSprite);
		square.SetNewPosition(Futile.screen.width / 4f, Futile.screen.halfHeight);
		square.physicsComponent.AddBoxCollider(squareSprite.width, squareSprite.height);
		physicsNodes.Add(square);

		// create the circle
		FSprite circleSprite = new FSprite("coolCircle");
		circle = new WTPhysicsNode("circle");
		circle.AddChild(circleSprite);
		circle.SetNewPosition(Futile.screen.width / 4f * 2f, Futile.screen.halfHeight);
		circle.physicsComponent.AddSphereCollider(circleSprite.width / 2f);
		physicsNodes.Add(circle);

		// polygon object
		FSprite polygonSprite = new FSprite("coolPolygon");
		polygon = new WTPhysicsNode("polygon");
		polygon.AddChild(polygonSprite);
		polygon.SetNewPosition(Futile.screen.width / 4f * 3f, Futile.screen.halfHeight);
		Vector2[] vertices = new Vector2[] {
			new Vector2(-27, -10),
			new Vector2(-14, 6),
			new Vector2(-20, 24),
			new Vector2(-2, 11),
			new Vector2(27, 25),
			new Vector2(-7, -8),
			new Vector2(22, -17),
			new Vector2(-26, -27)
		};
		polygon.physicsComponent.AddPolygonalCollider(vertices);
		physicsNodes.Add(polygon);

		// setup all the objects and start their physics
		foreach (WTPhysicsNode pn in physicsNodes) {
			pn.physicsComponent.AddRigidBody(1, 1);
			pn.physicsComponent.SetupPhysicMaterial(0.9f, 0.1f, 0.1f, PhysicMaterialCombine.Maximum);
			pn.physicsComponent.StartPhysics();
			pn.physicsComponent.AddForce(Random.Range(-1000, 1000), Random.Range(-1000, 1000));
			Futile.stage.AddChild(pn);
		}

		// for fun, let's stop this and have it float around a bit before being controlled by physics
		polygon.physicsComponent.StopPhysics();
	}

	void Update() {
		if (Input.anyKeyDown && Time.time > nextTime) {
			// there's gotta be some sort of time constraint or else mashing buttons will make the objects go too fast and exit the walls
			nextTime = Time.time + forceDelay;

			foreach (WTPhysicsNode pn in physicsNodes) {
				pn.physicsComponent.AddForce(Random.Range(-1000, 1000), Random.Range(-1000, 1000));
			}
		}

		if (!polygon.physicsComponent.IsControlledByPhysicsEngine()) {
			if (Time.time < delayUntilPolygonIsControlledByPhysics) {
				polygon.SetNewRotation(polygon.rotation + 1000 * Time.deltaTime);
				polygon.SetNewPosition(new Vector2(polygon.x + 10 * Time.deltaTime, polygon.y + 10 * Time.deltaTime));
			}
			else polygon.physicsComponent.StartPhysics();
		}
	}
}