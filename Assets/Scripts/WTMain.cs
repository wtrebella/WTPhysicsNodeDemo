using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTMain : MonoBehaviour {
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

		WTUtils.Init();

		FPWorld.Create(64.0f);

		FSprite squareSprite = new FSprite("coolSquare");

		WTPhysicsNode square = new WTPhysicsNode("square");
		square.AddChild(squareSprite);
		square.physicsComponent.AddRigidBody(1, 10);
		square.physicsComponent.AddBoxCollider(squareSprite.width, squareSprite.height);
		square.physicsComponent.SetupPhysicMaterial(1.0f, 0.0f, 0.0f, PhysicMaterialCombine.Maximum);
		square.SetNewPosition(WTUtils.screenCenter);

		square.physicsComponent.StartPhysics();
		square.physicsComponent.AddForce(7000, 5000);

		Futile.stage.AddChild(square);

		float wallThickness = 10;

		WTWall leftWall = new WTWall(wallThickness, Futile.screen.height);
		WTWall rightWall = new WTWall(wallThickness, Futile.screen.height);
		WTWall bottomWall = new WTWall(Futile.screen.width, wallThickness);
		WTWall topWall = new WTWall(Futile.screen.width, wallThickness);

		leftWall.SetPosition(wallThickness/2f, Futile.screen.halfHeight);
		rightWall.SetPosition(Futile.screen.width - wallThickness/2f, Futile.screen.halfHeight);
		bottomWall.SetPosition(Futile.screen.halfWidth, wallThickness/2f);
		topWall.SetPosition(Futile.screen.halfWidth, Futile.screen.height - wallThickness/2f);

		Futile.stage.AddChild(leftWall);
		Futile.stage.AddChild(rightWall);
		Futile.stage.AddChild(bottomWall);
		Futile.stage.AddChild(topWall);
	}
	
	void Update() {
//		if (frog.physicsNode.CanMoveInCode()) {
//			frog.rotation += 3;
//		}
	}
}