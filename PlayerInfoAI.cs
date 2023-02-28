// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.PlayerInfoAI
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace SpacewarMonoGame
{
  public class PlayerInfoAI
  {
    public float MoonRadial { get; set; }

    public float MoonDistance { get; set; }

    public float PlayerRadial { get; set; }

    public float PlayerDistance { get; set; }

    public float MyEnergy { get; set; }

    public float MyLife { get; set; }

    public float EnemyEnergy { get; set; }

    public float EnemyLife { get; set; }

    public bool RotationNegative { get; set; }

    public bool RotationPositive { get; set; }

    public bool Engine { get; set; }

    public bool Bullet { get; set; }

    public void Update(ContentHolder content, Arena arena, PlayerShip playerShip)
    {
      this.MyLife = playerShip.GetLife() / content.Parameters.GetItem<float>("LIFE_MAX");
      this.MyEnergy = playerShip.GetEnergy() / content.Parameters.GetItem<float>("ENERGY_MAX");
      Vector2 vector2 = content.Parameters.GetItem<Vector2Xml>("PLANETOID_POSITION") - playerShip.position;
      this.MoonRadial = (float) (((double) (MathHelper.ToDegrees((float) Math.Atan2((double) vector2.Y, (double) vector2.X)) + 90f) - (double) playerShip.rotation) % 360.0 / 180.0);
      if ((double) this.MoonRadial > 1.0)
        this.MoonRadial -= 2f;
      if ((double) this.MoonRadial < -1.0)
        this.MoonRadial += 2f;
      this.MoonDistance = vector2.Length() / 100f;
      PlayerShip playerShip1 = arena.ArenaObjects.OfType<PlayerShip>().Where<PlayerShip>((Func<PlayerShip, bool>) (p => p != playerShip)).First<PlayerShip>();
      vector2 = playerShip1.position - playerShip.position;
      this.PlayerRadial = (float) (((double) (MathHelper.ToDegrees((float) Math.Atan2((double) vector2.Y, (double) vector2.X)) + 90f) - (double) playerShip.rotation) % 360.0 / 180.0);
      if ((double) this.PlayerRadial > 1.0)
        this.PlayerRadial -= 2f;
      if ((double) this.PlayerRadial < -1.0)
        this.PlayerRadial += 2f;
      this.PlayerDistance = vector2.Length() / 200f;
      this.EnemyLife = playerShip1.GetLife() / content.Parameters.GetItem<float>("LIFE_MAX");
      this.EnemyEnergy = playerShip1.GetEnergy() / content.Parameters.GetItem<float>("ENERGY_MAX");
    }

    public override string ToString()
    {
      string str1 = " PlayerRadial: ";
      float num = this.PlayerRadial;
      string str2 = num.ToString("0.00");
      string str3 = " PlayerDistance: ";
      num = this.PlayerDistance;
      string str4 = num.ToString("0.00");
      return str1 + str2 + str3 + str4;
    }
  }
}
