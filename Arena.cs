// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.Arena
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpacewarMonoGame
{
  public class Arena
  {
    private float lastBonus = 0.0f;
    public List<IUpdateableDrawable> ArenaObjects;
    public List<IUpdateableDrawable> ArenaObjectsToBeAdded;
    private Particule scoreParticule;
    private bool reboot;

    public World World { get; }

    public CollisionHandler CollisionHandler { get; }

    public bool Headless { get; set; }

    public Arena(ContentHolder content)
    {
      this.ArenaObjects = new List<IUpdateableDrawable>();
      this.ArenaObjectsToBeAdded = new List<IUpdateableDrawable>();
      this.World = new World(Vector2.Zero);
      this.CollisionHandler = new CollisionHandler(content, this);
      this.Headless = false;
      for (int playerIndex = 0; playerIndex < 2; ++playerIndex)
        this.ArenaObjects.Add((IUpdateableDrawable) new PlayerShip(content, this.World, this, playerIndex));
      this.CreateArenaBodies(content);
    }

    public void CreateArenaBodies(ContentHolder content)
    {
      Body rectangle1 = BodyFactory.CreateRectangle(this.World, content.Parameters.GetItem<Vector2Xml>("ARENA_SIZE").X, 6f, 1f, (object) null);
      rectangle1.FixtureList.First<Fixture>().UserData = (object) new Border();
      rectangle1.IsStatic = true;
      rectangle1.SetTransform(new Vector2(content.Parameters.GetItem<Vector2Xml>("ARENA_SIZE").X / 2f, -3f), 0.0f);
      rectangle1.Restitution = 1f;
      Body rectangle2 = BodyFactory.CreateRectangle(this.World, content.Parameters.GetItem<Vector2Xml>("ARENA_SIZE").X, 6f, 1f, (object) null);
      rectangle2.FixtureList.First<Fixture>().UserData = (object) new Border();
      rectangle2.IsStatic = true;
      rectangle2.SetTransform(new Vector2(content.Parameters.GetItem<Vector2Xml>("ARENA_SIZE").X / 2f, content.Parameters.GetItem<Vector2Xml>("ARENA_SIZE").Y + 3f), 0.0f);
      rectangle2.Restitution = 1f;
      Body rectangle3 = BodyFactory.CreateRectangle(this.World, 6f, content.Parameters.GetItem<Vector2Xml>("ARENA_SIZE").Y, 1f, (object) null);
      rectangle3.FixtureList.First<Fixture>().UserData = (object) new Border();
      rectangle3.IsStatic = true;
      rectangle3.SetTransform(new Vector2(-3f, content.Parameters.GetItem<Vector2Xml>("ARENA_SIZE").Y / 2f), 0.0f);
      rectangle3.Restitution = 1f;
      Body rectangle4 = BodyFactory.CreateRectangle(this.World, 6f, content.Parameters.GetItem<Vector2Xml>("ARENA_SIZE").Y, 1f, (object) null);
      rectangle4.FixtureList.First<Fixture>().UserData = (object) new Border();
      rectangle4.IsStatic = true;
      rectangle4.SetTransform(new Vector2(content.Parameters.GetItem<Vector2Xml>("ARENA_SIZE").X + 3f, content.Parameters.GetItem<Vector2Xml>("ARENA_SIZE").Y / 2f), 0.0f);
      rectangle4.Restitution = 1f;
      Body circle = BodyFactory.CreateCircle(this.World, (float) ((double) content.Parameters.GetItem<float>("PLANETOID_SIZE") * (double) content.Parameters.GetItem<float>("PLANETOID_MARGIN") / 2.0), 1f, (object) null);
      circle.FixtureList.First<Fixture>().UserData = (object) new Planetoid();
      circle.IsStatic = true;
      circle.SetTransform(content.Parameters.GetItem<Vector2Xml>("PLANETOID_POSITION"), 0.0f);
      circle.Restitution = 0.5f;
    }

    public void CreateBullet(ContentHolder content, int playerIndex, Vector2 position, Vector2 velocity, float rotation)
    {
      this.ArenaObjectsToBeAdded.Add((IUpdateableDrawable) new Bullet(this.World, content, this, playerIndex, position, velocity, rotation));
    }

    public void CreateBomb(ContentHolder content, int playerIndex, Vector2 position, Vector2 velocity)
    {
      this.ArenaObjectsToBeAdded.Add((IUpdateableDrawable) new Bomb(this.World, content, this, playerIndex, position, velocity));
    }

    public void CreateHeatBullet(ContentHolder content, int playerIndex, Vector2 position, Vector2 velocity, float rotation)
    {
      this.ArenaObjectsToBeAdded.Add((IUpdateableDrawable) new BulletHeatSeeking(this.World, content, this, playerIndex, position, velocity, rotation));
    }

    public void CreatePlasma(ContentHolder content, int playerIndex, Vector2 position, Vector2 velocity, float rotation)
    {
      this.ArenaObjectsToBeAdded.Add((IUpdateableDrawable) new PlasmaBall(this.World, content, this, playerIndex, position, velocity, rotation));
    }

    public void CreateDamage(ContentHolder content, Vector2 position)
    {
      if (this.Headless)
        return;
      int num1 = 5;
      for (int index = 0; index < num1; ++index)
      {
        Particule p = new Particule(content, content.GetParticuleStateList("damage"));
        p.spriteName = "particle";
        p.size = content.Parameters.GetItem<Vector2Xml>("SIZE_DAMAGE");
        p.blendMode = BlendState.Additive;
        p.position = position;
        double num2 = Parameters.RNG.NextDouble() * Math.PI * 2.0;
        p.velocity.X = (float) (Math.Cos(num2) * (double) content.Parameters.GetItem<float>("VELOCITY_DAMAGE") * Parameters.RNG.NextDouble());
        p.velocity.Y = (float) (Math.Sin(num2) * (double) content.Parameters.GetItem<float>("VELOCITY_DAMAGE") * Parameters.RNG.NextDouble());
        content.ParticulEngine.AddParticule(p);
      }
    }

    public void CreateDamageLong(ContentHolder content, Vector2 position)
    {
      if (this.Headless)
        return;
      for (int index = 0; index < 1; ++index)
      {
        Particule p = new Particule(content, content.GetParticuleStateList("damage_long"));
        p.spriteName = "particle_big";
        p.size = content.Parameters.GetItem<Vector2Xml>("SIZE_DAMAGE").ToVector() * 2f;
        p.position = position;
        double num = Parameters.RNG.NextDouble() * Math.PI * 2.0;
        p.velocity.X = (float) (Math.Cos(num) * 4.0 * Parameters.RNG.NextDouble());
        p.velocity.Y = (float) (Math.Sin(num) * 4.0 * Parameters.RNG.NextDouble());
        p.blendMode = BlendState.Additive;
        content.ParticulEngine.AddParticule(p);
      }
    }

    public void CreateSmokeLong(ContentHolder content, Vector2 position)
    {
      if (this.Headless)
        return;
      for (int index = 0; index < 1; ++index)
      {
        Particule p = new Particule(content, content.GetParticuleStateList("smoke_long"));
        p.spriteName = "particle_big";
        p.size = content.Parameters.GetItem<Vector2Xml>("SIZE_DAMAGE").ToVector() * 2f;
        p.depth = content.Parameters.GetItem<float>("DEPTH_DEFAULT") - 1f / 1000f;
        p.position = position;
        double num = Parameters.RNG.NextDouble() * Math.PI * 2.0;
        p.velocity.X = (float) (Math.Cos(num) * 4.0 * Parameters.RNG.NextDouble());
        p.velocity.Y = (float) (Math.Sin(num) * 4.0 * Parameters.RNG.NextDouble());
        p.blendMode = BlendState.NonPremultiplied;
        content.ParticulEngine.AddParticule(p);
      }
    }

    public void CreateExplosion(ContentHolder content, Vector2 position, int nbShrapnels, int nbFireTrail, int nbSmokeTrail, bool kinetic)
    {
      this.CreateExplosion(content, position, Vector2.Zero, nbShrapnels, nbFireTrail, nbSmokeTrail, kinetic);
    }

    public void CreateExplosion(ContentHolder content, Vector2 position, Vector2 velocity, int nbShrapnels, int nbFireTrail, int nbSmokeTrail, bool kinetic)
    {
      for (int index = 0; index < nbShrapnels; ++index)
      {
        double num1 = Parameters.RNG.NextDouble() * Math.PI * 2.0;
        float num2 = content.Parameters.GetItem<float>("VELOCITY_BOMBSHRAPNEL_MIN") + (float) Parameters.RNG.NextDouble() * (content.Parameters.GetItem<float>("VELOCITY_BOMBSHRAPNEL_MAX") - content.Parameters.GetItem<float>("VELOCITY_BOMBSHRAPNEL_MIN"));
        Vector2 velocity1 = velocity + new Vector2((float) Math.Cos(num1) * num2, (float) Math.Sin(num1) * num2);
        this.ArenaObjectsToBeAdded.Add((IUpdateableDrawable) new BombShrapnel(this.World, content, this, position, velocity1)
        {
          isKinetic = kinetic,
          fireTrail = (index < nbFireTrail),
          smokeTrail = (index >= nbFireTrail && index < nbFireTrail + nbSmokeTrail)
        });
      }
    }

    public void CreateDeathExplosion(ContentHolder content, Vector2 position, Vector2 velocity)
    {
      if (this.Headless)
        return;
      this.CreateExplosion(content, position, velocity, 20, 14, 6, false);
    }

    public void CreateBombExplosion(ContentHolder content, Vector2 position)
    {
      this.CreateExplosion(content, position, content.Parameters.GetItem<int>("BOMB_NB_SHRAPNEL"), this.Headless ? 0 : 14, this.Headless ? 0 : 6, true);
    }

    public void StartReboot(ContentHolder content)
    {
      foreach (PlayerShip playerShip in this.ArenaObjects.OfType<PlayerShip>())
      {
        playerShip.Reboot = true;
        if (!playerShip.IsDead)
          ++playerShip.PlayerScore;
      }
      this.scoreParticule = new Particule(content, content.GetParticuleStateList("scores"));
      this.reboot = true;
    }

    public void EndReboot(ContentHolder content)
    {
      foreach (PhysicsObject physicsObject in this.ArenaObjects.OfType<PhysicsObject>())
        physicsObject.IsDead = true;
      this.reboot = false;
      foreach (PlayerShip playerShip in this.ArenaObjects.OfType<PlayerShip>())
      {
        if (!playerShip.IsRemoved)
          this.World.SafeRemoveBody(playerShip.playerShipBody);
        playerShip.RebootShip(content, this.World);
      }
      foreach (BulletHeatSeeking bulletHeatSeeking in this.ArenaObjects.OfType<BulletHeatSeeking>())
        bulletHeatSeeking.EndLifespan(content);
    }

    public void CreateBonus(ContentHolder content, float seconds)
    {
      this.lastBonus += seconds;
      if ((double) this.lastBonus <= (double) content.Parameters.GetItem<float>("BONUS_FREQUENCY"))
        return;
      this.lastBonus = 0.0f;
      this.ArenaObjects.Add((IUpdateableDrawable) new Bonus(this.World, content, this));
    }

    public void Update(ContentHolder content, float seconds)
    {
      foreach (IUpdateableDrawable updateableDrawable in this.ArenaObjectsToBeAdded)
        this.ArenaObjects.Add(updateableDrawable);
      this.ArenaObjectsToBeAdded.Clear();
      this.World.Step(seconds);
      foreach (IUpdateableDrawable arenaObject in this.ArenaObjects)
        arenaObject.Update(content, this, seconds);
      foreach (PhysicsObject physicsObject in this.ArenaObjects.OfType<PhysicsObject>().Where<PhysicsObject>((Func<PhysicsObject, bool>) (b => b.IsDead)))
        this.World.SafeRemoveBody(physicsObject.physicsBody);
      this.ArenaObjects.RemoveAll((Predicate<IUpdateableDrawable>) (b =>
      {
        if (b is PhysicsObject)
          return b.IsDead;
        return false;
      }));
      this.CreateBonus(content, seconds);
      foreach (PlayerShip playerShip in this.ArenaObjects.OfType<PlayerShip>().Where<PlayerShip>((Func<PlayerShip, bool>) (p =>
      {
        if (p.IsDead)
          return !p.IsRemoved;
        return false;
      })))
      {
        this.World.SafeRemoveBody(playerShip.playerShipBody);
        playerShip.IsRemoved = true;
      }
      if (!this.reboot && this.ArenaObjects.OfType<PlayerShip>().Count<PlayerShip>((Func<PlayerShip, bool>) (p => !p.IsDead)) <= 1)
        this.StartReboot(content);
      if (!this.reboot)
        return;
      this.scoreParticule.Update(content, seconds);
      if (this.scoreParticule.IsDead)
        this.EndReboot(content);
    }

    public void Draw(SpriteBatch spriteBatch, ContentHolder content, Camera camera)
    {
      foreach (IUpdateableDrawable arenaObject in this.ArenaObjects)
        arenaObject.Draw(content, spriteBatch, camera);
      Rectangle destRect;
      destRect.X = 0;
      destRect.Y = 0;
      destRect.Width = 0;
      destRect.Height = 0;
      camera.ToCamera(content.Parameters.GetItem<Vector2Xml>("PLANETOID_POSITION"), new Vector2(content.Parameters.GetItem<float>("PLANETOID_SIZE")), ref destRect);
      DrawHelper.Draw(content, spriteBatch, camera, "planetoid", Color.White, content.Parameters.GetItem<Vector2Xml>("PLANETOID_POSITION"), new Vector2(content.Parameters.GetItem<float>("PLANETOID_SIZE")), 0.0f, content.Parameters.GetItem<float>("DEPTH_PLANETOID"));
      if (!this.reboot)
        return;
      this.DrawScores(spriteBatch, content);
    }

    public void DrawScores(SpriteBatch spriteBatch, ContentHolder content)
    {
      int length = this.ArenaObjects.OfType<PlayerShip>().Count<PlayerShip>();
      string[] strArray = new string[length];
      Vector2[] vector2Array = new Vector2[length];
      float num = 10f;
      Vector2 zero1 = Vector2.Zero;
      int index1 = 0;
      foreach (PlayerShip playerShip in this.ArenaObjects.OfType<PlayerShip>())
      {
        strArray[index1] = playerShip.PlayerName + " : " + playerShip.PlayerScore.ToString();
        vector2Array[index1] = content.GetFont(content.Parameters.GetItem<string>("FONT_SCORES")).MeasureString(strArray[index1]);
        zero1.Y += vector2Array[index1].Y + num;
        ++index1;
      }
      Vector2 zero2 = Vector2.Zero;
      zero2.Y = (float) ((double) content.Parameters.GetItem<Vector2Xml>("SCREEN_SIZE").Y / 2.0 - (double) zero1.Y / 2.0 - 200.0);
      for (int index2 = 0; index2 < length; ++index2)
      {
        Color white = Color.White;
        white.A = this.scoreParticule.currentColor.A;
        white.R = content.Parameters.GetItemColorArray("PLAYER_COLORS")[index2].R;
        white.G = content.Parameters.GetItemColorArray("PLAYER_COLORS")[index2].G;
        white.B = content.Parameters.GetItemColorArray("PLAYER_COLORS")[index2].B;
        zero2.X = (float) ((double) content.Parameters.GetItem<Vector2Xml>("SCREEN_SIZE").X / 2.0 - (double) vector2Array[index2].X / 2.0);
        spriteBatch.DrawString(content.GetFont(content.Parameters.GetItem<string>("FONT_SCORES")), strArray[index2], zero2, white);
        zero2.Y += vector2Array[index2].Y + num;
      }
    }
  }
}
