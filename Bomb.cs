// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.Bomb
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace SpacewarMonoGame
{
  public class Bomb : PhysicsObject
  {
    public bool explodeNext;
    public float currentTime;
    public int playerIndex;

    public Bomb(World world, ContentHolder content, Arena arena, int playerIndex, Vector2 position, Vector2 velocity)
      : base(content, arena)
    {
      this.currentTime = 0.0f;
      this.IsDead = false;
      this.explodeNext = false;
      this.playerIndex = playerIndex;
      this.size = content.Parameters.GetItem<Vector2Xml>("SIZE_BOMB");
      this.depth = content.Parameters.GetItem<float>("DEPTH_BULLET");
      this.color = content.Parameters.GetItemColorArray("PLAYER_COLORS")[playerIndex];
      this.physicsBody = BodyFactory.CreateCircle(world, content.Parameters.GetItem<Vector2Xml>("SIZE_BOMB").X / 2f, 1f, position, (object) this);
      this.physicsBody.IgnoreCCD = true;
      this.physicsBody.Friction = 0.0f;
      this.physicsBody.BodyType = BodyType.Dynamic;
      this.physicsBody.SetTransform(position + new Vector2(1f / 1000f), 0.0f);
      this.physicsBody.LinearVelocity = velocity;
      this.physicsBody.FixtureList.First<Fixture>().UserData = (object) this;
      this.physicsBody.OnCollision += new OnCollisionEventHandler(arena.CollisionHandler.bombBody_OnCollision);
    }

    public override void Update(ContentHolder content, Arena arena, float seconds)
    {
      base.Update(content, arena, seconds);
      this.currentTime += seconds;
      if ((double) this.currentTime <= (double) content.Parameters.GetItem<float>("BOMB_LIFESPAN") && !this.explodeNext)
        return;
      this.IsDead = true;
      arena.CreateBombExplosion(content, this.position);
    }

    public override void Draw(ContentHolder content, SpriteBatch spriteBatch, Camera camera)
    {
      DrawHelper.Draw(content, spriteBatch, camera, "bomb", Color.White, this.position, this.size, this.rotation, this.depth - 1f / 1000f);
      DrawHelper.Draw(content, spriteBatch, camera, "bomb_color", this.color, this.position, this.size, this.rotation, this.depth - 1f / 1000f);
    }
  }
}
