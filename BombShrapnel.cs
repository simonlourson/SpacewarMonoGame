// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.BombShrapnel
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace SpacewarMonoGame
{
  public class BombShrapnel : PhysicsObject
  {
    public float currentTime;
    public float rotationDelta;
    public int playerIndex;
    private FrequencyChecker damageChecker;
    public float lifespan;
    public bool fireTrail;
    public bool smokeTrail;
    public bool isKinetic;

    public BombShrapnel(World world, ContentHolder content, Arena arena, Vector2 position, Vector2 velocity)
      : base(content, arena)
    {
      this.currentTime = 0.0f;
      this.IsDead = false;
      this.size = content.Parameters.GetItem<Vector2Xml>("SIZE_BOMBSHRAPNEL");
      this.depth = content.Parameters.GetItem<float>("DEPTH_BULLET");
      this.damageChecker = new FrequencyChecker(0.05f);
      this.lifespan = content.Parameters.GetItem<float>("BOMB_SHRAPNEL_LIFESPAN") + (float) Parameters.RNG.NextDouble() * content.Parameters.GetItem<float>("BOMB_SHRAPNEL_LIFESPAN_VARIABLE");
      this.rotation = (float) (Parameters.RNG.NextDouble() * Math.PI * 2.0);
      this.rotationDelta = (float) Parameters.RNG.NextDouble() * content.Parameters.GetItem<float>("BONUS_MAX_ROTATION_DELTA");
      if (Parameters.RNG.NextDouble() < 0.5)
        this.rotationDelta *= -1f;
      this.sprite = "bomb_shrapnel_" + Parameters.RNG.Next(4).ToString();
      this.physicsBody = BodyFactory.CreateCircle(world, content.Parameters.GetItem<Vector2Xml>("SIZE_BOMBSHRAPNEL").X / 2f, 1f, position, (object) this);
      this.physicsBody.IgnoreCCD = true;
      this.physicsBody.Friction = 0.0f;
      this.physicsBody.BodyType = BodyType.Dynamic;
      this.physicsBody.SetTransform(position + new Vector2(0.01f * (float) Parameters.RNG.NextDouble()), 0.0f);
      this.physicsBody.LinearVelocity = velocity;
      this.physicsBody.LinearDamping = content.Parameters.GetItem<float>("BOMB_SHRAPNEL_DAMPING");
      this.physicsBody.FixtureList.First<Fixture>().UserData = (object) this;
      this.physicsBody.OnCollision += new OnCollisionEventHandler(arena.CollisionHandler.bombshrapnelBody_OnCollision);
    }

    public override void Update(ContentHolder content, Arena arena, float seconds)
    {
      base.Update(content, arena, seconds);
      this.currentTime += seconds;
      this.rotation += this.rotationDelta * seconds;
      this.damageChecker.Update(seconds);
      if ((double) this.currentTime < (double) this.lifespan - (double) content.Parameters.GetItem<float>("BOMB_SHRAPNEL_FADESPAN") && this.damageChecker.CheckFrequency())
      {
        if (this.fireTrail)
          arena.CreateDamageLong(content, this.position);
        else if (this.smokeTrail)
          arena.CreateSmokeLong(content, this.position);
      }
      if ((double) this.currentTime > (double) this.lifespan)
        this.IsDead = true;
      if ((double) this.currentTime > (double) this.lifespan - (double) content.Parameters.GetItem<float>("BOMB_SHRAPNEL_FADESPAN"))
        this.color.A = (byte) (((double) this.lifespan - (double) this.currentTime) * (double) byte.MaxValue / (double) content.Parameters.GetItem<float>("BOMB_SHRAPNEL_FADESPAN"));
      if ((double) this.currentTime <= (double) this.lifespan)
        return;
      this.IsDead = true;
    }
  }
}
