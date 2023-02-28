// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.SpacewarMonoGameParameters
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SpacewarMonoGame
{
  public class SpacewarMonoGameParameters : Parameters
  {
    public new static Random RNG = new Random();

    public SpacewarMonoGameParameters()
    {
      SpacewarMonoGameParameters.RNG = new Random();
    }

    public void ToXmlFile(string file)
    {
      TextWriter text = (TextWriter) File.CreateText(file);
      new XmlSerializer(this.GetType()).Serialize(text, (object) this);
      text.Close();
    }

    public static SpacewarMonoGameParameters FromFile(string file)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (SpacewarMonoGameParameters));
      FileStream fileStream = File.OpenRead(file);
      SpacewarMonoGameParameters monoGameParameters = (SpacewarMonoGameParameters) xmlSerializer.Deserialize((Stream) fileStream);
      fileStream.Close();
      monoGameParameters.Add("ARENA_TO_SCREEN_RATIO", (object) (float) ((double) monoGameParameters.GetItem<Vector2Xml>("SCREEN_SIZE").X / (double) monoGameParameters.GetItem<Vector2Xml>("ARENA_SIZE").X));
      return monoGameParameters;
    }

    public override T GetItem<T>(string key)
    {
      return (T) this[key];
    }

    public override void InitTypes()
    {
      this.types = new Type[]
      {
        typeof (List<BonusProbability>),
        typeof (Keys),
        typeof (Keys[]),
        typeof (bool),
        typeof (int),
        typeof (float),
        typeof (float[]),
        typeof (Vector2Xml),
        typeof (Vector2Xml[]),
        typeof (ColorXml),
        typeof (ColorXml[]),
        typeof (bool[]),
        typeof (string),
        typeof (string[]),
      };
    }
  }
}
