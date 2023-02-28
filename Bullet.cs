// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.Bullet
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System.Linq;

namespace SpacewarMonoGame
{
  public class Bullet : PhysicsObject
  {
    public float currentTime;
    public int playerIndex;

    public Bullet(World world, ContentHolder content, Arena arena, int playerIndex, Vector2 position, Vector2 velocity, float rotation)
      : base(content, arena)
    {
      this.currentTime = 0.0f;
      this.IsDead = false;
      this.playerIndex = playerIndex;
      this.rotation = rotation;
      this.size = content.Parameters.GetItem<Vector2Xml>("BULLET_SIZE");
      this.sprite = "bullet";
      this.depth = content.Parameters.GetItem<float>("DEPTH_BULLET");
      this.color = content.Parameters.GetItemColorArray("PLAYER_COLORS")[playerIndex];
      this.physicsBody = BodyFactory.CreateCircle(world, content.Parameters.GetItem<Vector2Xml>("BULLET_SIZE").X / 2f, 1f, position, (object) this);
      this.physicsBody.IgnoreCCD = true;
      this.physicsBody.Friction = 0.0f;
      this.physicsBody.BodyType = BodyType.Dynamic;
      this.physicsBody.SetTransform(position + new Vector2(1f / 1000f), 0.0f);
      this.physicsBody.LinearVelocity = velocity;
      this.physicsBody.FixtureList.First<Fixture>().UserData = (object) this;
      this.physicsBody.OnCollision += new OnCollisionEventHandler(arena.CollisionHandler.allCollisions);
    }

    public override void Update(ContentHolder content, Arena arena, float seconds)
    {
      base.Update(content, arena, seconds);
      this.currentTime += seconds;
      if ((double) this.currentTime <= (double) content.Parameters.GetItem<float>("BULLET_LIFESPAN"))
        return;
      this.IsDead = true;
    }
  }
}
