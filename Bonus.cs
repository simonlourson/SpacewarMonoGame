// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.Bonus
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpacewarMonoGame
{
  public class Bonus : PhysicsObject
  {
    public float currentTime;
    public float rotationDelta;

    public Bonus(World world, ContentHolder content, Arena arena)
      : base(content, arena)
    {
      this.currentTime = 0.0f;
      this.IsDead = false;
      this.depth = content.Parameters.GetItem<float>("DEPTH_BONUS");
      this.sprite = "bonus";
      this.size = content.Parameters.GetItem<Vector2Xml>("BONUS_SIZE");
      this.rotation = (float) (Parameters.RNG.NextDouble() * Math.PI * 2.0);
      this.rotationDelta = (float) Parameters.RNG.NextDouble() * content.Parameters.GetItem<float>("BONUS_MAX_ROTATION_DELTA");
      float num = (float) (Parameters.RNG.NextDouble() * Math.PI * 2.0);
      Vector2 vector2 = new Vector2((float) Math.Cos((double) num) * content.Parameters.GetItem<float>("BONUS_MAX_VELOCITY"), (float) Math.Sin((double) num) * content.Parameters.GetItem<float>("BONUS_MAX_VELOCITY"));
      if (Parameters.RNG.NextDouble() < 0.5)
        this.rotationDelta *= -1f;
      this.position.X = (float) ((double) content.Parameters.GetItem<Vector2Xml>("BONUS_SIZE").X / 2.0 + Parameters.RNG.NextDouble() * ((double) content.Parameters.GetItem<Vector2Xml>("ARENA_SIZE").X - (double) content.Parameters.GetItem<Vector2Xml>("BONUS_SIZE").X));
      this.position.Y = (float) ((double) content.Parameters.GetItem<Vector2Xml>("BONUS_SIZE").Y / 2.0 + Parameters.RNG.NextDouble() * ((double) content.Parameters.GetItem<Vector2Xml>("ARENA_SIZE").Y - (double) content.Parameters.GetItem<Vector2Xml>("BONUS_SIZE").Y));
      this.physicsBody = BodyFactory.CreateCircle(world, content.Parameters.GetItem<Vector2Xml>("BONUS_SIZE").X / 2f, 1f, this.position, (object) this);
      this.physicsBody.IgnoreCCD = true;
      this.physicsBody.Friction = 0.0f;
      this.physicsBody.BodyType = BodyType.Dynamic;
      this.physicsBody.SetTransform(this.position, 0.0f);
      this.physicsBody.LinearVelocity = vector2;
      this.physicsBody.FixtureList.First<Fixture>().UserData = (object) this;
      this.physicsBody.OnCollision += new OnCollisionEventHandler(arena.CollisionHandler.bonusBody_OnCollision);
      this.color.A = (byte) 0;
    }

    public override void Update(ContentHolder content, Arena arena, float seconds)
    {
      base.Update(content, arena, seconds);
      this.rotation += this.rotationDelta * seconds;
      this.currentTime += seconds;
      if ((double) this.currentTime < (double) content.Parameters.GetItem<float>("BONUS_FADESPAN"))
        this.color.A = (byte) ((double) this.currentTime * (double) byte.MaxValue / (double) content.Parameters.GetItem<float>("BONUS_FADESPAN"));
      else if ((double) this.currentTime > (double) content.Parameters.GetItem<float>("BONUS_LIFESPAN") - (double) content.Parameters.GetItem<float>("BONUS_FADESPAN"))
        this.color.A = (byte) (((double) content.Parameters.GetItem<float>("BONUS_LIFESPAN") - (double) this.currentTime) * (double) byte.MaxValue / (double) content.Parameters.GetItem<float>("BONUS_FADESPAN"));
      if ((double) this.currentTime <= (double) content.Parameters.GetItem<float>("BONUS_LIFESPAN"))
        return;
      this.IsDead = true;
    }

    public static BonusType RamdomBonusType(ContentHolder content)
    {
      float num1 = 0.0f;
      foreach (BonusProbability bonusProbability in content.Parameters.GetItem<List<BonusProbability>>("BONUS_PROBABILITIES"))
        num1 += bonusProbability.BonusFrequency;
      float num2 = (float) Parameters.RNG.NextDouble();
      float num3 = 0.0f;
      foreach (BonusProbability bonusProbability in content.Parameters.GetItem<List<BonusProbability>>("BONUS_PROBABILITIES"))
      {
        num3 += bonusProbability.BonusFrequency / num1;
        if ((double) num2 <= (double) num3)
          return bonusProbability.BonusType;
      }
      throw new Exception("Math is wrong");
    }
  }
}
