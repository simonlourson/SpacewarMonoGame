// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.PlayerShip
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpacewarMonoGame
{
  public class PlayerShip : IUpdateableDrawable
  {
    public Body playerShipBody;
    public int playerIndex;
    private Arena arena;
    public Vector2 position;
    public Vector2 velocity;
    private Color playerColor;
    private StatusBar energyBar;
    private StatusBar lifeBar;
    private AmmoHud ammoHud;
    private float energy;
    private float life;
    public bool Reboot;
    public bool IsRemoved;
    public PlayerInfoAI PlayerInfoAI;
    public string PlayerName;
    public int PlayerScore;
    public float rotation;
    private float eregenRemaining;
    private float lregenRemaining;
    private float shieldRemaining;
    private float shieldPulse;
    public bool collideWithMoon;
    public bool collideWithBorders;
    private ParticleEmiter trailEmiter;
    private ParticleEmiter eregenEmiter;
    private ParticleEmiter lregenEmiter;
    private Dictionary<FrequencyCheckerType, FrequencyChecker> frequencyCheckers;

    public bool IsDead { get; set; }

    public PlayerShip(ContentHolder content, World world, Arena arena, int playerIndex)
    {
      this.playerIndex = playerIndex;
      this.arena = arena;
      this.PlayerInfoAI = new PlayerInfoAI();
      this.PlayerName = content.Parameters.GetItem<string[]>("PLAYER_NAMES")[playerIndex];
      this.PlayerScore = 0;
      this.energyBar = new StatusBar(content.Parameters, content.Parameters.GetItemVector2Array("ENERGY_BAR_POSITIONS")[playerIndex], nameof (energy), content.Parameters.GetItem<bool[]>("INVERTED_BARS")[playerIndex]);
      this.lifeBar = new StatusBar(content.Parameters, content.Parameters.GetItemVector2Array("LIFE_BAR_POSITIONS")[playerIndex], nameof (life), content.Parameters.GetItem<bool[]>("INVERTED_BARS")[playerIndex]);
      this.ammoHud = new AmmoHud(content.Parameters.GetItemVector2Array("AMMOHUD_POSITIONS")[playerIndex], content.Parameters.GetItem<bool[]>("INVERTED_BARS")[playerIndex], content.Parameters.GetItemColorArray("PLAYER_COLORS")[playerIndex], playerIndex);
      this.playerColor = content.Parameters.GetItemColorArray("PLAYER_COLORS")[playerIndex];
      this.frequencyCheckers = new Dictionary<FrequencyCheckerType, FrequencyChecker>();
      this.frequencyCheckers.Add(FrequencyCheckerType.BULLET, new FrequencyChecker(content.Parameters.GetItem<float>("FREQUENCY_BULLET")));
      this.frequencyCheckers.Add(FrequencyCheckerType.REGEN, new FrequencyChecker(content.Parameters.GetItem<float>("FREQUENCY_REGEN")));
      this.frequencyCheckers.Add(FrequencyCheckerType.HEAT_SEEKING, new FrequencyChecker(content.Parameters.GetItem<float>("FREQUENCY_HEATBULLET")));
      this.frequencyCheckers.Add(FrequencyCheckerType.BOMB, new FrequencyChecker(content.Parameters.GetItem<float>("FREQUENCY_BOMB")));
      this.frequencyCheckers.Add(FrequencyCheckerType.PLASMA, new FrequencyChecker(content.Parameters.GetItem<float>("FREQUENCY_PLASMA")));
      this.CreateTrailEmiter(content);
      this.eregenEmiter = new ParticleEmiter(content, content.ParticulEngine, content.GetParticuleStateList("regen"), 0.2f);
      this.eregenEmiter.spriteName = "eregen";
      this.eregenEmiter.depth = content.Parameters.GetItem<float>("DEPTH_REGEN");
      this.lregenEmiter = new ParticleEmiter(content, content.ParticulEngine, content.GetParticuleStateList("regen"), 0.2f);
      this.lregenEmiter.spriteName = "lregen";
      this.lregenEmiter.depth = content.Parameters.GetItem<float>("DEPTH_REGEN");
      this.RebootShip(content, world);
    }

    public void CreateTrailEmiter(ContentHolder content)
    {
      ParticuleStateList particuleStates = content.GetParticuleStateList("ship_trail").Clone();
      foreach (ParticuleState particuleState in particuleStates.ParticuleStates)
      {
        particuleState.targetColor.R = this.playerColor.R;
        particuleState.targetColor.G = this.playerColor.G;
        particuleState.targetColor.B = this.playerColor.B;
      }
      this.trailEmiter = new ParticleEmiter(content, content.ParticulEngine, particuleStates, 0.015f);
      this.trailEmiter.spriteName = "particle";
      this.trailEmiter.size = content.Parameters.GetItem<Vector2Xml>("SIZE_TRAIL");
      this.trailEmiter.blendMode = BlendState.Additive;
    }

    public void RebootShip(ContentHolder content, World world)
    {
      if (content.Parameters.GetItem<bool>("RANDOM_STARTING_POSITIONS")) {
        this.position.X = (float) ((double) content.Parameters.GetItem<Vector2Xml>("SHIP_SIZE").X / 2.0 + Parameters.RNG.NextDouble() * ((double) content.Parameters.GetItem<Vector2Xml>("ARENA_SIZE").X - (double) content.Parameters.GetItem<Vector2Xml>("SHIP_SIZE").X));
        this.position.Y = (float) ((double) content.Parameters.GetItem<Vector2Xml>("SHIP_SIZE").Y / 2.0 + Parameters.RNG.NextDouble() * ((double) content.Parameters.GetItem<Vector2Xml>("ARENA_SIZE").Y - (double) content.Parameters.GetItem<Vector2Xml>("SHIP_SIZE").Y));
        this.rotation = (float) Parameters.RNG.NextDouble() * 360;
      }
      else {
        this.position.X = content.Parameters.GetItem<Vector2Xml[]>("PLAYER_STARTING_POSITIONS")[playerIndex].X;
        this.position.Y = content.Parameters.GetItem<Vector2Xml[]>("PLAYER_STARTING_POSITIONS")[playerIndex].Y;
        this.rotation = content.Parameters.GetItem<float[]>("PLAYER_STARTING_ROTATIONS")[playerIndex];
      }
      this.ChangeEnergy(content.Parameters.GetItem<float>("ENERGY_MAX"), content);
      this.ChangeLife(content.Parameters.GetItem<float>("LIFE_MAX"), content);
      this.CreateBody(content, world);
      this.Reboot = false;
      this.IsDead = false;
      this.IsRemoved = false;
      this.shieldRemaining = 0;
      this.eregenRemaining = 0;
      this.lregenRemaining = 0;
      this.ammoHud.Reset();

      foreach (BonusType bonusType in Enum.GetValues(typeof(BonusType))) {
        this.ammoHud.ChangeAmmo(bonusType, 99);
      }
    }

    private void CreateBody(ContentHolder content, World world)
    {
      this.playerShipBody = BodyFactory.CreateCircle(world, content.Parameters.GetItem<Vector2Xml>("SHIP_SIZE").X / 2f, 1f, this.position, (object) this);
      this.playerShipBody.IgnoreCCD = true;
      this.playerShipBody.Restitution = 1f;
      this.playerShipBody.Friction = 0.0f;
      this.playerShipBody.LinearDamping = 0.8f;
      this.playerShipBody.AngularDamping = 0.0f;
      this.playerShipBody.BodyType = BodyType.Dynamic;
      this.playerShipBody.SetTransform(this.position, 0.0f);
      this.playerShipBody.OnCollision += new OnCollisionEventHandler(this.arena.CollisionHandler.allCollisions);
    }

    public void Die()
    {
    }

    public bool TakeBullet(ContentHolder content, Bullet bullet)
    {
      if (bullet.playerIndex != this.playerIndex)
      {
        if ((double) this.shieldRemaining > 0.0)
        {
          bullet.playerIndex = this.playerIndex;
          bullet.physicsBody.LinearVelocity = -bullet.physicsBody.LinearVelocity;
          bullet.currentTime = 0.0f;
          bullet.ChangeColor(content.Parameters.GetItemColorArray("PLAYER_COLORS")[this.playerIndex]);
        }
        else
        {
          bullet.IsDead = true;
          this.ChangeLife(content.Parameters.GetItem<float>("LIFE_PER_BULLET"), content);
          this.arena.CreateDamage(content, bullet.position);
        }
      }
      return false;
    }

    public bool TakeBulletHeat(ContentHolder content, BulletHeatSeeking bullet)
    {
      if (bullet.playerIndex == this.playerIndex)
        return false;
      if ((double) this.shieldRemaining > 0.0)
      {
        bullet.playerIndex = this.playerIndex;
        bullet.currentTime = 0.0f;
        bullet.ChangeColor(content.Parameters.GetItemColorArray("PLAYER_COLORS")[this.playerIndex]);
        bullet.rotation += 180f;
        return false;
      }
      bullet.IsDead = true;
      this.ChangeLife(content.Parameters.GetItem<float>("LIFE_PER_BULLETHEAT"), content);
      this.arena.CreateDamage(content, bullet.position);
      return true;
    }

    public bool TakeBomb(ContentHolder content, Bomb bomb)
    {
      if (bomb.playerIndex != this.playerIndex)
        bomb.explodeNext = true;
      return false;
    }

    public bool TakePlasma(ContentHolder content, PlasmaBall plasma)
    {
      if (plasma.playerIndex == this.playerIndex)
        return false;
      this.ChangeLife(-10f, content);
      plasma.isActive = false;
      return false;
    }

    public bool TakeShrapnel(ContentHolder content, BombShrapnel shrapnel)
    {
      this.ChangeLife(content.Parameters.GetItem<float>("BOMB_SHRAPNEL_DAMAGE") * shrapnel.physicsBody.LinearVelocity.Length() / content.Parameters.GetItem<float>("VELOCITY_BOMBSHRAPNEL_MAX"), content);
      shrapnel.IsDead = true;
      this.arena.CreateDamage(content, shrapnel.position);
      return true;
    }

    public bool TakeBonus(ContentHolder content, Bonus bonus)
    {
      BonusType newAmmo = Bonus.RamdomBonusType(content);
      this.ammoHud.ChangeAmmo(newAmmo, content.Parameters.GetItem<List<BonusProbability>>("BONUS_PROBABILITIES").Where<BonusProbability>((Func<BonusProbability, bool>) (b => b.BonusType == newAmmo)).First<BonusProbability>().BonusNbAmmo);
      this.ammoHud.SetAmmo(newAmmo);
      bonus.IsDead = true;
      return false;
    }

    public void ChangeLife(float lifeDelta, ContentHolder content)
    {
      this.life += lifeDelta;
      this.life = MathHelper.Clamp(this.life, 0.0f, content.Parameters.GetItem<float>("LIFE_MAX"));
      if ((double) this.life <= 0.0 && !this.IsDead)
      {
        this.IsDead = true;
        this.arena.CreateDeathExplosion(content, this.position, this.velocity);
      }
      this.lifeBar.value = this.life / content.Parameters.GetItem<float>("LIFE_MAX");
    }

    public float GetLife()
    {
      return this.life;
    }

    public void ChangeEnergy(float energyDelta, ContentHolder content)
    {
      this.energy += energyDelta;
      this.energy = MathHelper.Clamp(this.energy, 0.0f, content.Parameters.GetItem<float>("ENERGY_MAX"));
      this.energyBar.value = this.energy / content.Parameters.GetItem<float>("ENERGY_MAX");
    }

    public float GetEnergy()
    {
      return this.energy;
    }

    public void Update(ContentHolder content, Arena arena, float seconds)
    {
      if (this.IsDead)
        return;
      this.position = this.playerShipBody.GetWorldPoint(Vector2.Zero);
      this.velocity = this.playerShipBody.LinearVelocity;
      if (this.Reboot)
        return;
      this.PlayerInfoAI.Update(content, arena, this);
      this.ammoHud.Update(content, seconds);
      this.shieldPulse += (float) ((double) seconds * 3.14159274101257 * 2.0);
      foreach (FrequencyChecker frequencyChecker in this.frequencyCheckers.Values)
        frequencyChecker.Update(seconds);
      if (this.collideWithMoon)
      {
        this.ChangeLife(content.Parameters.GetItem<float>("LIFE_PER_SECOND_MOON_COLLISION") * seconds, content);
        this.collideWithMoon = false;
      }
      bool flag1 = false;
      bool flag2 = false;
      float num1 = content.Parameters.GetItem<float>("DEGREES_PER_SECOND") * seconds;
      if (Keyboard.GetState().IsKeyDown(content.Parameters.GetItem<Keys[]>("KEY_MOVEMENT_LEFT")[this.playerIndex]))
        this.rotation -= num1;
      if (Keyboard.GetState().IsKeyDown(content.Parameters.GetItem<Keys[]>("KEY_MOVEMENT_RIGHT")[this.playerIndex]))
        this.rotation += num1;
      if (Keyboard.GetState().IsKeyDown(content.Parameters.GetItem<Keys[]>("KEY_ACTION_MAIN")[this.playerIndex]))
      {
        flag2 = true;
        if ((double) this.energy > (double) Math.Abs(content.Parameters.GetItem<float>("ENERGY_PER_BULLET")) && this.frequencyCheckers[FrequencyCheckerType.BULLET].CheckFrequency())
        {
          this.ChangeEnergy(content.Parameters.GetItem<float>("ENERGY_PER_BULLET"), content);
          Vector2 velocity = this.velocity + content.Parameters.GetItem<float>("BULLET_VELOCITY") * Vector2.Transform(new Vector2(0.0f, -1f), Matrix.CreateRotationZ(MathHelper.ToRadians(this.rotation)));
          this.arena.CreateBullet(content, this.playerIndex, this.position, velocity, this.rotation);
        }
      }
      if (Keyboard.GetState().IsKeyDown(content.Parameters.GetItem<Keys[]>("KEY_ACTION_SECONDARY")[this.playerIndex]))
      {
        flag2 = true;
        this.DoSecondaryFire(content, seconds);
      }
      float energyDelta = content.Parameters.GetItem<float>("ENERGY_PER_SECOND_ENGINE") * seconds;
      if (Keyboard.GetState().IsKeyDown(content.Parameters.GetItem<Keys[]>("KEY_MOVEMENT_ENGINE")[this.playerIndex]))
      {
        flag1 = true;
        if ((double) this.energy > (double) Math.Abs(energyDelta))
        {
          this.ChangeEnergy(energyDelta, content);
          Matrix rotationZ = Matrix.CreateRotationZ(MathHelper.ToRadians(this.rotation));
          this.playerShipBody.LinearVelocity += Vector2.Transform(new Vector2(0.0f, -1f), rotationZ) * (content.Parameters.GetItem<float>("IMPULSE_PER_SECOND") * seconds);
          this.trailEmiter.Emit(content, this.position + Vector2.Transform(new Vector2(0.0f, 2.5f), rotationZ), Vector2.Zero);
        }
      }
      if (!flag1 && !flag2)
        this.ChangeEnergy(content.Parameters.GetItem<float>("ENERGY_PER_SECOND_REGEN") * seconds, content);
      if ((double) this.eregenRemaining > 0.0)
      {
        float num2 = Math.Min(this.eregenRemaining, seconds);
        this.eregenRemaining -= num2;
        this.ChangeEnergy(num2 * content.Parameters.GetItem<float>("EREGEN_ENERGY_PER_SECOND"), content);
        double num3 = Parameters.RNG.NextDouble() * Math.PI * 2.0;
        Vector2 velocity = new Vector2((float) (Math.Cos(num3) * (double) content.Parameters.GetItem<float>("VELOCITY_REGEN") * Parameters.RNG.NextDouble()), (float) (Math.Sin(num3) * (double) content.Parameters.GetItem<float>("VELOCITY_REGEN") * Parameters.RNG.NextDouble()));
        this.eregenEmiter.Emit(content, this.position, velocity);
      }
      if ((double) this.lregenRemaining > 0.0)
      {
        float num2 = Math.Min(this.lregenRemaining, seconds);
        this.lregenRemaining -= num2;
        this.ChangeLife(num2 * content.Parameters.GetItem<float>("LREGEN_LIFE_PER_SECOND"), content);
        double num3 = Parameters.RNG.NextDouble() * Math.PI * 2.0;
        Vector2 velocity = new Vector2((float) (Math.Cos(num3) * (double) content.Parameters.GetItem<float>("VELOCITY_REGEN") * Parameters.RNG.NextDouble()), (float) (Math.Sin(num3) * (double) content.Parameters.GetItem<float>("VELOCITY_REGEN") * Parameters.RNG.NextDouble()));
        this.lregenEmiter.Emit(content, this.position, velocity);
      }
      if ((double) this.shieldRemaining > 0.0)
      {
        this.shieldRemaining -= seconds;
        if ((double) this.shieldRemaining < 0.0)
          this.shieldRemaining = 0.0f;
      }
      if (!Keyboard.GetState().IsKeyDown(content.Parameters.GetItem<Keys[]>("KEY_DEBUG")[this.playerIndex]))
        return;
      this.ChangeLife(-200f, content);
    }

    private void DoSecondaryFire(ContentHolder content, float seconds)
    {
      if (this.ammoHud.BonusType == BonusType.NONE || this.ammoHud.RemainingAmmo <= 0)
        return;
      if (this.ammoHud.BonusType == BonusType.ENERGY && this.frequencyCheckers[FrequencyCheckerType.REGEN].CheckFrequency())
      {
        this.ammoHud.ChangeAmmo(this.ammoHud.BonusType, -1);
        this.eregenRemaining += content.Parameters.GetItem<float>("EREGEN_TIME");
      }
      if (this.ammoHud.BonusType == BonusType.LIFE && this.frequencyCheckers[FrequencyCheckerType.REGEN].CheckFrequency())
      {
        this.ammoHud.ChangeAmmo(this.ammoHud.BonusType, -1);
        this.lregenRemaining += content.Parameters.GetItem<float>("LREGEN_TIME");
      }
      if (this.ammoHud.BonusType == BonusType.TRIPE_BULLET && ((double) this.energy > (double) Math.Abs(content.Parameters.GetItem<float>("ENERGY_PER_TBULLET")) && this.frequencyCheckers[FrequencyCheckerType.BULLET].CheckFrequency()))
      {
        this.ammoHud.ChangeAmmo(this.ammoHud.BonusType, -1);
        this.ChangeEnergy(content.Parameters.GetItem<float>("ENERGY_PER_TBULLET"), content);
        Matrix rotationZ = Matrix.CreateRotationZ(MathHelper.ToRadians(this.rotation));
        Vector2 velocity = this.velocity + content.Parameters.GetItem<float>("BULLET_VELOCITY") * Vector2.Transform(new Vector2(0.0f, -1f), rotationZ);
        this.arena.CreateBullet(content, this.playerIndex, this.position + Vector2.Transform(new Vector2(0.0f, -2f), rotationZ), velocity, this.rotation);
        this.arena.CreateBullet(content, this.playerIndex, this.position + Vector2.Transform(new Vector2(2f, -0.8f), rotationZ), velocity, this.rotation);
        this.arena.CreateBullet(content, this.playerIndex, this.position + Vector2.Transform(new Vector2(-2f, -0.8f), rotationZ), velocity, this.rotation);
      }
      if (this.ammoHud.BonusType == BonusType.HEAT_SEEKING && ((double) this.energy > (double) Math.Abs(content.Parameters.GetItem<float>("ENERGY_PER_BULLETHEAT")) && this.frequencyCheckers[FrequencyCheckerType.HEAT_SEEKING].CheckFrequency()))
      {
        this.ammoHud.ChangeAmmo(this.ammoHud.BonusType, -1);
        this.ChangeEnergy(content.Parameters.GetItem<float>("ENERGY_PER_BULLETHEAT"), content);
        this.arena.CreateHeatBullet(content, this.playerIndex, this.position, Vector2.Zero, this.rotation);
      }
      if (this.ammoHud.BonusType == BonusType.BOMB && ((double) this.energy > (double) Math.Abs(content.Parameters.GetItem<float>("ENERGY_PER_BOMB")) && this.frequencyCheckers[FrequencyCheckerType.BOMB].CheckFrequency()))
      {
        this.ammoHud.ChangeAmmo(this.ammoHud.BonusType, -1);
        this.ChangeEnergy(content.Parameters.GetItem<float>("ENERGY_PER_BOMB"), content);
        Matrix rotationZ = Matrix.CreateRotationZ(MathHelper.ToRadians(this.rotation));
        Vector2 velocity = this.velocity + content.Parameters.GetItem<float>("BOMB_VELOCITY") * Vector2.Transform(new Vector2(0.0f, -1f), rotationZ);
        this.arena.CreateBomb(content, this.playerIndex, this.position, velocity);
      }
      if (this.ammoHud.BonusType == BonusType.PLASMA && ((double) this.energy > (double) Math.Abs(content.Parameters.GetItem<float>("ENERGY_PER_PLASMA")) && this.frequencyCheckers[FrequencyCheckerType.PLASMA].CheckFrequency()))
      {
        this.ammoHud.ChangeAmmo(this.ammoHud.BonusType, -1);
        this.ChangeEnergy(content.Parameters.GetItem<float>("ENERGY_PER_PLASMA"), content);
        Matrix rotationZ = Matrix.CreateRotationZ(MathHelper.ToRadians(this.rotation));
        Vector2 velocity = this.velocity + content.Parameters.GetItem<float>("PLASMA_VELOCITY") * Vector2.Transform(new Vector2(0.0f, -1f), rotationZ);
        this.arena.CreatePlasma(content, this.playerIndex, this.position + Vector2.Transform(new Vector2(0.0f, -2f), rotationZ), velocity, this.rotation);
      }
      if (this.ammoHud.BonusType != BonusType.SHIELD || ((double) this.energy <= (double) Math.Abs(content.Parameters.GetItem<float>("ENERGY_PER_SHIELD")) || !this.frequencyCheckers[FrequencyCheckerType.REGEN].CheckFrequency()))
        return;
      this.ammoHud.ChangeAmmo(this.ammoHud.BonusType, -1);
      this.ChangeEnergy(content.Parameters.GetItem<float>("ENERGY_PER_SHIELD"), content);
      this.shieldRemaining = content.Parameters.GetItem<float>("SHIELD_DURATION");
    }

    public void Draw(ContentHolder content, SpriteBatch spriteBatch, Camera camera)
    {
      if (this.IsDead)
        return;
      this.energyBar.Draw(spriteBatch, content);
      this.lifeBar.Draw(spriteBatch, content);
      this.ammoHud.Draw(spriteBatch, content);
      Vector2 size = content.Parameters.GetItem<Vector2Xml>("SHIP_SIZE");
      DrawHelper.Draw(content, spriteBatch, camera, "playership", Color.White, this.position, size, this.rotation, content.Parameters.GetItem<float>("DEPTH_PLAYERSHIP"));
      DrawHelper.Draw(content, spriteBatch, camera, "playership_color", this.playerColor, this.position, size, this.rotation, content.Parameters.GetItem<float>("DEPTH_PLAYERSHIP") - 1f / 1000f);
      if ((double) this.shieldRemaining <= 0.0)
        return;
      float num = (float) Math.Cos((double) this.shieldPulse) * 100f;
      Color playerColor = this.playerColor;
      playerColor.R = (byte) MathHelper.Clamp((float) playerColor.R + num, 0.0f, (float) byte.MaxValue);
      playerColor.G = (byte) MathHelper.Clamp((float) playerColor.G + num, 0.0f, (float) byte.MaxValue);
      playerColor.B = (byte) MathHelper.Clamp((float) playerColor.B + num, 0.0f, (float) byte.MaxValue);
      DrawHelper.Draw(content, spriteBatch, camera, "shield", playerColor, this.position, size, this.rotation, content.Parameters.GetItem<float>("DEPTH_PLAYERSHIP") + 1f / 1000f);
    }
  }
}
