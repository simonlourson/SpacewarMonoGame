// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.ParticuleStateList
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using System.Collections.Generic;
using System.Xml.Serialization;

namespace SpacewarMonoGame
{
  public class ParticuleStateList
  {
    [XmlAttribute("name")]
    public string Name;
    public List<ParticuleState> ParticuleStates;

    public ParticuleStateList()
    {
      this.ParticuleStates = new List<ParticuleState>();
    }

    public ParticuleStateList Clone()
    {
      ParticuleStateList particuleStateList = new ParticuleStateList();
      foreach (ParticuleState particuleState in this.ParticuleStates)
        particuleStateList.ParticuleStates.Add(particuleState.Clone());
      particuleStateList.Name = this.Name;
      return particuleStateList;
    }
  }
}
