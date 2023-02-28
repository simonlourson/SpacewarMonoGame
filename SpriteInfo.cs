// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.SpriteInfo
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace SpacewarMonoGame
{
  public class SpriteInfo
  {
    [XmlAttribute("name")]
    public string Name;
    [XmlIgnore]
    public Rectangle SourceRectangle;
    [XmlAttribute("x")]
    public int X;
    [XmlAttribute("y")]
    public int Y;
    [XmlAttribute("width")]
    public int Width;
    [XmlAttribute("height")]
    public int Height;
    [XmlAttribute("texture")]
    public string TextureName;

    public virtual void CreateRectangle()
    {
      this.SourceRectangle.X = this.X;
      this.SourceRectangle.Y = this.Y;
      this.SourceRectangle.Width = this.Width;
      this.SourceRectangle.Height = this.Height;
    }
  }
}
