// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.BonusProbability
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using System.Xml.Serialization;

namespace SpacewarMonoGame
{
  public class BonusProbability
  {
    [XmlAttribute("bonusType")]
    public BonusType BonusType;
    [XmlAttribute("bonusFrequency")]
    public float BonusFrequency;
    [XmlAttribute("bonusNbAmmo")]
    public int BonusNbAmmo;
  }
}
