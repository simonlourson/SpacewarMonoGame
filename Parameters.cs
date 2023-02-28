// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.Parameters
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace SpacewarMonoGame
{
  public abstract class Parameters : Dictionary<string, object>, IXmlSerializable
  {
    public static Random RNG = new Random();
    protected Type[] types;

    public virtual T GetItem<T>(string key)
    {
      return (T) this[key];
    }
    public Vector2[] GetItemVector2Array(string key) {
      var vectors = GetItem<Vector2Xml[]>(key);
      var returnVectors = new Vector2[vectors.Length];

      for (int i = 0; i < vectors.Length; i++)
        returnVectors[i] = vectors[i];

      return returnVectors;
    }

    public Color[] GetItemColorArray(string key) {
      var colors = GetItem<ColorXml[]>(key);
      var returnColors = new Color[colors.Length];

      for (int i = 0; i < colors.Length; i++)
        returnColors[i] = colors[i];

      return returnColors;
    }

    public XmlSchema GetSchema()
    {
      return (XmlSchema) null;
    }

    public abstract void InitTypes();

    public void ReadXml(XmlReader reader)
    {
      this.InitTypes();
      if (this.types == null)
        throw new Exception("The child class of Parameters must implement InitTypes()");
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (object), this.types);
      bool isEmptyElement = reader.IsEmptyElement;
      reader.Read();
      if (isEmptyElement)
        return;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        string attribute = reader.GetAttribute("key");
        reader.ReadStartElement("item");
        object obj = xmlSerializer.Deserialize(reader);
        this.Add(attribute, obj);
        reader.ReadEndElement();
        int content = (int) reader.MoveToContent();
      }
      reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
      this.InitTypes();
      if (this.types == null)
        throw new Exception("The child class of Parameters must implement InitTypes()");
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (object), this.types);
      foreach (string key in this.Keys)
      {
        writer.WriteStartElement("item");
        writer.WriteAttributeString("key", key);
        object o = this[key];
        xmlSerializer.Serialize(writer, o);
        writer.WriteEndElement();
      }
    }
  }
}
