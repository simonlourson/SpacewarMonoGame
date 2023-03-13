// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.CollisionHandler
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using System.Collections.Generic;

namespace SpacewarMonoGame
{
  public class CollisionHandler
  {
    private ContentHolder content;
    private Arena arena;
    private List<CollisionResponseItem> collisionResponses;

    public CollisionHandler(ContentHolder content, Arena arena)
    {
      this.content = content;
      this.arena = arena;
      this.collisionResponses = new List<CollisionResponseItem>();
      this.collisionResponses.Add(new CollisionResponseItem()
      {
        T1 = typeof (Bullet),
        T2 = typeof (Planetoid),
        ComplexResponse = new CollisionResponse(this.BulletWithPlanetoid)
      });
      this.collisionResponses.Add(new CollisionResponseItem()
      {
        T1 = typeof (Bullet),
        T2 = typeof (PlayerShip),
        ComplexResponse = new CollisionResponse(this.BulletWithPlayerShip)
      });
      this.collisionResponses.Add(new CollisionResponseItem()
      {
        T1 = typeof (PlayerShip),
        T2 = typeof (PlayerShip),
        SimpleResponse = true
      });
      this.collisionResponses.Add(new CollisionResponseItem()
      {
        T1 = typeof (PlayerShip),
        T2 = typeof (BulletHeatSeeking),
        ComplexResponse = new CollisionResponse(this.HeatSeekingBulletWithPlayerShip)
      });
      this.collisionResponses.Add(new CollisionResponseItem()
      {
        T1 = typeof (PlayerShip),
        T2 = typeof (Border),
        ComplexResponse = new CollisionResponse(this.PlayerShipWithBorder)
      });
      this.collisionResponses.Add(new CollisionResponseItem()
      {
        T1 = typeof (PlayerShip),
        T2 = typeof (Planetoid),
        ComplexResponse = new CollisionResponse(this.PlayerShipWithPlanetoid)
      });
      this.collisionResponses.Add(new CollisionResponseItem()
      {
        T1 = typeof (PlayerShip),
        T2 = typeof (Bomb),
        ComplexResponse = new CollisionResponse(this.PlayerShipWithBomb)
      });
      this.collisionResponses.Add(new CollisionResponseItem()
      {
        T1 = typeof (PlayerShip),
        T2 = typeof (BombShrapnel),
        ComplexResponse = new CollisionResponse(this.PlayerShipWithBombShrapnel)
      });
      this.collisionResponses.Add(new CollisionResponseItem()
      {
        T1 = typeof (PlayerShip),
        T2 = typeof (Bonus),
        ComplexResponse = new CollisionResponse(this.PlayerShipWithBonus)
      });
      this.collisionResponses.Add(new CollisionResponseItem()
      {
        T1 = typeof (PlayerShip),
        T2 = typeof (PlasmaBall),
        ComplexResponse = new CollisionResponse(this.PlayerShipWithPlasma)
      });
    }

    public bool allCollisions(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
      foreach (CollisionResponseItem collisionResponse in this.collisionResponses)
      {
        if (collisionResponse.T1 == fixtureA.UserData.GetType() && collisionResponse.T2 == fixtureB.UserData.GetType())
        {
          if (collisionResponse.ComplexResponse != null)
            return collisionResponse.ComplexResponse(fixtureA.UserData, fixtureB.UserData);
          return collisionResponse.SimpleResponse;
        }
      }
      return false;
    }

    public bool BulletWithPlanetoid(object _bullet, object _planetoid)
    {
      Bullet bullet = _bullet as Bullet;
      Planetoid planetoid = _planetoid as Planetoid;
      if (bullet == null || planetoid == null)
        return false;
      bullet.IsDead = true;
      this.arena.CreateDamage(this.content, bullet.position);
      return false;
    }

    public bool BulletWithPlayerShip(object _bullet, object _playerShip)
    {
      Bullet bullet = _bullet as Bullet;
      return (_playerShip as PlayerShip).TakeBullet(this.content, bullet);
    }

    public bool HeatSeekingBulletWithPlayerShip(object _playerShip, object _heatSeekingBullet)
    {
      BulletHeatSeeking bullet = _heatSeekingBullet as BulletHeatSeeking;
      return (_playerShip as PlayerShip).TakeBulletHeat(this.content, bullet);
    }

    public bool PlayerShipWithBorder(object _playerShip, object _border)
    {
      (_playerShip as PlayerShip).collideWithBorders = true;
      return true;
    }

    public bool PlayerShipWithPlanetoid(object _playerShip, object _planetoid)
    {
      (_playerShip as PlayerShip).collideWithMoon = true;
      return false;
    }

    public bool PlayerShipWithBomb(object _playerShip, object _bomb)
    {
      Bomb bomb = _bomb as Bomb;
      return (_playerShip as PlayerShip).TakeBomb(this.content, bomb);
    }

    public bool PlayerShipWithBombShrapnel(object _playerShip, object _bombShrapnel)
    {
      BombShrapnel shrapnel = _bombShrapnel as BombShrapnel;
      PlayerShip playerShip = _playerShip as PlayerShip;
      if (shrapnel.isKinetic)
        return playerShip.TakeShrapnel(this.content, shrapnel);
      return false;
    }

    public bool PlayerShipWithBonus(object _playerShip, object _bonus)
    {
      Bonus bonus = _bonus as Bonus;
      return (_playerShip as PlayerShip).TakeBonus(this.content, bonus);
    }

    public bool PlayerShipWithPlasma(object _playerShip, object _plasma)
    {
      PlasmaBall plasma = _plasma as PlasmaBall;
      return false;
    }

    public bool bombBody_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
      Bomb userData1 = fixtureA.UserData as Bomb;
      Bomb userData2 = fixtureB.UserData as Bomb;
      if (userData1 != null && userData2 != null)
      {
        userData1.explodeNext = true;
        return false;
      }
      BombShrapnel userData3 = fixtureB.UserData as BombShrapnel;
      if (userData1 != null && userData3 != null)
      {
        if (userData3.isKinetic && (double) userData3.currentTime > (double) this.content.Parameters.GetItem<float>("BOMB_SHRAPNEL_GRACE_PERIOD"))
          userData1.explodeNext = true;
        return false;
      }
      Bonus userData4 = fixtureB.UserData as Bonus;
      if (userData1 != null && userData4 != null)
        return false;
      PlasmaBall userData5 = fixtureB.UserData as PlasmaBall;
      if (userData1 != null && userData5 != null)
        return false;
      Planetoid planetoid = fixtureB.UserData as Planetoid;
      if (planetoid != null && userData1 != null) {
        userData1.explodeNext = true;
        return false;
      }
      return true;
    }

    public bool bombshrapnelBody_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
      BombShrapnel userData1 = fixtureA.UserData as BombShrapnel;
      if (!userData1.isKinetic)
        return false;
      BombShrapnel userData2 = fixtureB.UserData as BombShrapnel;
      if (userData1 != null && userData2 != null)
        return false;
      Bonus userData3 = fixtureB.UserData as Bonus;
      if (userData1 != null && userData3 != null)
        return false;
      PlasmaBall userData4 = fixtureB.UserData as PlasmaBall;
      if (userData1 != null && userData4 != null)
        return false;
      Planetoid planetoid = fixtureB.UserData as Planetoid;
      if (planetoid != null && userData1 != null) {
        this.arena.CreateDamage(this.content, userData1.position);
        userData1.IsDead = true;
        return false;
      }
      return false;
    }

    public bool bonusBody_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
      Bonus userData1 = fixtureA.UserData as Bonus;
      Bonus userData2 = fixtureB.UserData as Bonus;
      if (userData1 != null && userData2 != null)
        return true;
      PlasmaBall userData3 = fixtureB.UserData as PlasmaBall;
      if (userData1 != null && userData3 != null)
        return false;
      Planetoid planetoid = fixtureB.UserData as Planetoid;
      if (planetoid != null && userData1 != null) {
        userData1.IsDead = true;
        this.arena.CreateDamage(this.content, userData1.position);
        return false;
      }
      Border border = fixtureB.UserData as Border;
      if (border != null && userData1 != null)
        return true;
      return true;
    }

    public bool plasmaBody_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
      return false;
    }

    public bool heatbulletBody_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
    {
      BulletHeatSeeking userData1 = fixtureA.UserData as BulletHeatSeeking;
      BulletHeatSeeking userData2 = fixtureB.UserData as BulletHeatSeeking;
      if (userData1 != null && userData2 != null)
        return true;
      Bomb userData3 = fixtureB.UserData as Bomb;
      if (userData1 != null && userData3 != null)
        return false;
      BombShrapnel userData4 = fixtureB.UserData as BombShrapnel;
      if (userData1 != null && userData4 != null)
        return false;
      Bonus userData5 = fixtureB.UserData as Bonus;
      if (userData1 != null && userData5 != null)
        return false;
      PlasmaBall plasmaBall = fixtureB.UserData as PlasmaBall;
      if (userData1 != null && plasmaBall != null)
        return false;
      Planetoid planetoid = fixtureB.UserData as Planetoid;
      if (planetoid != null && userData1 != null) {
        userData1.IsDead = true;
        this.arena.CreateDamage(this.content, userData1.position);
        return false;
      }
      Border border = fixtureB.UserData as Border;
      if (border != null && userData1 != null)
        return false;
      
      return true;
    }
  }
}
