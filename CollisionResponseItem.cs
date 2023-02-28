// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.CollisionResponseItem
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using System;

namespace SpacewarMonoGame
{
  public class CollisionResponseItem
  {
    public Type T1 { get; set; }

    public Type T2 { get; set; }

    public bool SimpleResponse { get; set; }

    public CollisionResponse ComplexResponse { get; set; }
  }
}
