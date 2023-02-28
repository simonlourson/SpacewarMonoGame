// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.BulletHeatSeeking
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace SpacewarMonoGame
{
  public class BulletHeatSeeking : PhysicsObject
  {
    public float currentTime;
    public int playerIndex;
    private ParticleEmiter trailEmiter;

    public BulletHeatSeeking(World world, ContentHolder content, Arena arena, int playerIndex, Vector2 position, Vector2 velocity, float rotation)
      : base(content, arena)
    {
      this.size = content.Parameters.GetItem<Vector2Xml>("SIZE_BULLETHEAT");
      this.depth = content.Parameters.GetItem<float>("DEPTH_BULLET");
      this.color = content.Parameters.GetItemColorArray("PLAYER_COLORS")[playerIndex];
      this.rotation = rotation;
      this.playerIndex = playerIndex;
      this.CreateTrailEmiter(content);
      this.physicsBody = BodyFactory.CreateCircle(world, content.Parameters.GetItem<Vector2Xml>("BULLET_SIZE").X / 2f, 1f, position, (object) this);
      this.physicsBody.IgnoreCCD = true;
      this.physicsBody.Friction = 0.0f;
      this.physicsBody.BodyType = BodyType.Dynamic;
      this.physicsBody.SetTransform(position, 0.0f);
      this.physicsBody.LinearVelocity = velocity;
      this.physicsBody.FixtureList.First<Fixture>().UserData = (object) this;
      this.physicsBody.OnCollision += new OnCollisionEventHandler(arena.CollisionHandler.heatbulletBody_OnCollision);
    }

    public void CreateTrailEmiter(ContentHolder content)
    {
      ParticuleStateList particuleStates = content.GetParticuleStateList("ship_trail").Clone();
      foreach (ParticuleState particuleState in particuleStates.ParticuleStates)
      {
        particuleState.targetColor.R = this.color.R;
        particuleState.targetColor.G = this.color.G;
        particuleState.targetColor.B = this.color.B;
      }
      this.trailEmiter = new ParticleEmiter(content, content.ParticulEngine, particuleStates, 0.015f);
      this.trailEmiter.spriteName = "particle";
      this.trailEmiter.size = content.Parameters.GetItem<Vector2Xml>("SIZE_TRAIL");
      this.trailEmiter.blendMode = BlendState.Additive;
    }

    public void EndLifespan(ContentHolder content)
    {
      this.currentTime = content.Parameters.GetItem<float>("BULLETHEAT_LIFESPAN");
    }

    public override void Update(ContentHolder content, Arena arena, float seconds)
    {
      base.Update(content, arena, seconds);
      this.currentTime += seconds;
      if ((double) this.currentTime > (double) content.Parameters.GetItem<float>("BULLETHEAT_LIFESPAN"))
      {
        this.IsDead = true;
        arena.CreateDamage(content, this.position);
      }
      PlayerShip target = arena.ArenaObjects.OfType<PlayerShip>().Where<PlayerShip>((Func<PlayerShip, bool>) (p => p.playerIndex != this.playerIndex)).First<PlayerShip>();
      Vector2 vector2_1 = target.position - this.position;
      float num1 = (float) (((double) (MathHelper.ToDegrees((float) Math.Atan2((double) vector2_1.Y, (double) vector2_1.X)) + 90f) - (double) this.rotation) % 360.0);
      float num2 = 1f;
      if ((double) num1 < 0.0 && (double) Math.Abs(num1) < (double) num1 + 360.0 || (double) num1 > 0.0 && (double) Math.Abs(num1 - 360f) < (double) num1)
        num2 = -1f;
      if (!target.IsDead)
        this.rotation += num2 * content.Parameters.GetItem<float>("BULLETHEAT_TURN_PER_SECOND") * seconds;
      Matrix rotationZ = Matrix.CreateRotationZ(MathHelper.ToRadians(this.rotation));
      Vector2 vector2_2 = Vector2.Transform(new Vector2(0.0f, -1f), rotationZ) * content.Parameters.GetItem<float>("BULLETHEAT_IMPULSE");
      this.trailEmiter.Emit(content, this.position + Vector2.Transform(new Vector2(0.0f, 1.5f), rotationZ), Vector2.Zero);
      this.physicsBody.LinearVelocity = vector2_2;
    }

    public override void ChangeColor(Color color)
    {
      base.ChangeColor(color);
      foreach (ParticuleState particuleState in this.trailEmiter.particuleStates.ParticuleStates)
      {
        particuleState.targetColor.R = color.R;
        particuleState.targetColor.G = color.G;
        particuleState.targetColor.B = color.B;
      }
    }

    public override void Draw(ContentHolder content, SpriteBatch spriteBatch, Camera camera)
    {
      DrawHelper.Draw(content, spriteBatch, camera, "heatbullet", Color.White, this.position, this.size, this.rotation, this.depth + 1f / 1000f);
      DrawHelper.Draw(content, spriteBatch, camera, "heatbullet_color", this.color, this.position, this.size, this.rotation, this.depth);
    }
  }
}
