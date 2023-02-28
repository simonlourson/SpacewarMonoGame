// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.IUpdateableDrawable
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace SpacewarMonoGame
{
  public interface IUpdateableDrawable
  {
    void Update(ContentHolder content, Arena arena, float seconds);

    void Draw(ContentHolder content, SpriteBatch spriteBatch, Camera camera);

    bool IsDead { get; set; }
  }
}
