using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WTMain : MonoBehaviour {
	public static FPWorld physicsWorld;

	void Start() {
		Go.defaultEaseType = EaseType.SineInOut;

		FutileParams fp = new FutileParams(true, true, false, false);
		fp.AddResolutionLevel(480f, 1.0f, 1.0f, "-res1");
		fp.AddResolutionLevel(1136f, 2.0f, 2.0f, "-res2");
		fp.AddResolutionLevel(2048f, 4.0f, 4.0f, "-res4");
		
		fp.backgroundColor = Color.black;
		fp.origin = Vector2.zero;

		Futile.instance.Init(fp);

		Futile.atlasManager.LoadAtlas("Atlases/MainSheet");
		Futile.atlasManager.LoadFont("franchise", "franchise", "Atlases/franchise", -7, -16);

		// futile done initing

		WTUtils.Init();

		physicsWorld = FPWorld.Create(64.0f);

		FSprite floorSprite = new FSprite("whiteSquare");
		floorSprite.width = Futile.screen.width * 2;
		floorSprite.height = 10;

		WTPhysicsNode floor = WTPhysicsNode.Create();
		floor.Init(new Vector2(Futile.screen.halfWidth, 5), 0, true);
		floor.AddBoxCollider(floorSprite.width, floorSprite.height);
		floor.SetupPhysicMaterial();

		FSprite leftWallSprite = new FSprite("whiteSquare");
		leftWallSprite.width = 10;
		leftWallSprite.height = Futile.screen.height * 2;

		WTPhysicsNode leftWall = WTPhysicsNode.Create();
		leftWall.Init(new Vector2(5, Futile.screen.halfHeight), 0, true);
		leftWall.AddBoxCollider(leftWallSprite.width, leftWallSprite.height);
		leftWall.SetupPhysicMaterial();

		FSprite rightWallSprite = new FSprite("whiteSquare");
		rightWallSprite.width = 10;
		rightWallSprite.height = Futile.screen.height * 2;

		WTPhysicsNode rightWall = WTPhysicsNode.Create();
		rightWall.Init(new Vector2(Futile.screen.width - 5, Futile.screen.halfHeight), 0, true);
		rightWall.AddBoxCollider(rightWallSprite.width, rightWallSprite.height);
		rightWall.SetupPhysicMaterial();

		floor.container.AddChild(floorSprite);
		Futile.stage.AddChild(floor.container);

		leftWall.container.AddChild(leftWallSprite);
		Futile.stage.AddChild(leftWall.container);

		rightWall.container.AddChild(rightWallSprite);
		Futile.stage.AddChild(rightWall.container);

		for (int i = 0; i < 100; i++) {
			FSprite s = new FSprite("whiteSquare");
			s.width = Random.Range(5, 30);
			s.height = Random.Range(5, 30);
			s.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

			WTPhysicsNode p = WTPhysicsNode.Create();
			p.Init(new Vector2(Random.Range(20, Futile.screen.width - 20), Random.Range(100, Futile.screen.height)), Random.Range(0, 360), true);
			p.AddBoxCollider(s.width, s.height);
			p.AddRigidBody();
			p.SetupPhysicMaterial();

			p.container.AddChild(s);
			Futile.stage.AddChild(p.container);
		}
	}
	
	void Update() {

	}
}