// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.StatusBar
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacewarMonoGame
{
  public class StatusBar
  {
    public float value;
    public string prefix;
    public Vector2 position;
    public Vector2 fullSize;
    public bool inverted;

    public StatusBar(Parameters parameters, Vector2 position, string prefix, bool inverted)
    {
      this.prefix = prefix;
      this.position = position;
      this.fullSize = parameters.GetItem<Vector2Xml>("STATUS_BAR_SIZE");
      this.inverted = inverted;
    }

    public void Draw(SpriteBatch spriteBatch, ContentHolder content)
    {
      string name1 = this.prefix + "_left";
      string name2 = this.prefix + "_center";
      string name3 = this.prefix + "_right";
      SpriteInfo spriteInfo1 = content.GetSpriteInfo(name1);
      SpriteInfo spriteInfo2 = content.GetSpriteInfo(name3);
      Rectangle destinationRectangle1;
      destinationRectangle1.X = (int) this.position.X;
      destinationRectangle1.Y = (int) this.position.Y;
      destinationRectangle1.Width = spriteInfo1.Width;
      destinationRectangle1.Height = (int) this.fullSize.Y;
      Rectangle destinationRectangle2;
      destinationRectangle2.X = destinationRectangle1.X + destinationRectangle1.Width;
      destinationRectangle2.Y = destinationRectangle1.Y;
      destinationRectangle2.Width = (int) ((double) this.fullSize.X * (double) this.value);
      destinationRectangle2.Height = (int) this.fullSize.Y;
      Rectangle destinationRectangle3;
      destinationRectangle3.X = destinationRectangle2.X + destinationRectangle2.Width;
      destinationRectangle3.Y = destinationRectangle1.Y;
      destinationRectangle3.Width = spriteInfo2.Width;
      destinationRectangle3.Height = (int) this.fullSize.Y;
      if (this.inverted)
      {
        destinationRectangle1.X += (int) ((double) this.fullSize.X - (double) destinationRectangle2.Width);
        destinationRectangle2.X += (int) ((double) this.fullSize.X - (double) destinationRectangle2.Width);
        destinationRectangle3.X += (int) ((double) this.fullSize.X - (double) destinationRectangle2.Width);
      }
      SpriteInfo spriteInfo3 = content.GetSpriteInfo(name1);
      SpriteInfo spriteInfo4 = content.GetSpriteInfo(name2);
      SpriteInfo spriteInfo5 = content.GetSpriteInfo(name3);
      Texture2D texture1 = content.GetTexture(spriteInfo3.TextureName);
      Texture2D texture2 = content.GetTexture(spriteInfo3.TextureName);
      Texture2D texture3 = content.GetTexture(spriteInfo3.TextureName);
      spriteBatch.Draw(texture1, destinationRectangle1, new Rectangle?(spriteInfo3.SourceRectangle), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, content.Parameters.GetItem<float>("DEPTH_HUD"));
      spriteBatch.Draw(texture2, destinationRectangle2, new Rectangle?(spriteInfo4.SourceRectangle), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, content.Parameters.GetItem<float>("DEPTH_HUD"));
      spriteBatch.Draw(texture3, destinationRectangle3, new Rectangle?(spriteInfo5.SourceRectangle), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, content.Parameters.GetItem<float>("DEPTH_HUD"));
    }
  }
}
