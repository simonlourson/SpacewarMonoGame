// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.BonusTypeExtensions
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

namespace SpacewarMonoGame
{
  public static class BonusTypeExtensions
  {
    public static string ToSprite(this BonusType me)
    {
      switch (me)
      {
        case BonusType.LIFE:
          return "bonus_life";
        case BonusType.ENERGY:
          return "bonus_energy";
        case BonusType.TRIPE_BULLET:
          return "bonus_tbullet";
        case BonusType.HEAT_SEEKING:
          return "bonus_hseeking";
        case BonusType.BOMB:
          return "bonus_bomb";
        case BonusType.PLASMA:
          return "bonus_plasma";
        case BonusType.SHIELD:
          return "bonus_shield";
        case BonusType.LASER:
          return "bonus_laser";
        default:
          return "particle";
      }
    }
  }
}
