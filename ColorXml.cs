using System;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace SpacewarMonoGame;

public struct ColorXml
{
  [XmlAttribute]
  public int R;

  [XmlAttribute]
  public int G;

  [XmlAttribute]
  public int B;

  [XmlAttribute]
  public int A;

  public static implicit operator Color (ColorXml c)
  {
    return c.ToColor();
  }

  public Color ToColor()
  {
    return new Color(R, G, B, A);
  }

  public override string ToString()
  {
    return String.Format("A:{0}, R:{1}, G:{2}, B:{3}", A, R, G, B);
  }
}