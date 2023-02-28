using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace SpacewarMonoGame;

public struct Vector2Xml
{
  [XmlAttribute]
  public float X;

  [XmlAttribute]
  public float Y;

  public static implicit operator Vector2 (Vector2Xml v)
  {
    return v.ToVector();
  }

  public Vector2 ToVector()
  {
    return new Vector2(X, Y);
  }
}