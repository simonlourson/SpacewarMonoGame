// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.ParticuleState
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace SpacewarMonoGame
{
  public class ParticuleState
  {
    [XmlAttribute("targetTime")]
    public float targetTime;
    [XmlAttribute("targetSizeRatio")]
    public float targetSizeRatio;
    public ColorXml targetColor;

    public ParticuleState Clone()
    {
      return new ParticuleState()
      {
        targetTime = this.targetTime,
        targetSizeRatio = this.targetSizeRatio,
        targetColor = this.targetColor
      };
    }
  }
}
