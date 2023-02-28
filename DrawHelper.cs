// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.DrawHelper
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacewarMonoGame
{
  public class DrawHelper
  {
    public static void Draw(ContentHolder content, SpriteBatch spriteBatch, Camera camera, string sprite, Color color, Vector2 position, Vector2 size, float rotation, float depth)
    {
      SpriteInfo spriteInfo = content.GetSpriteInfo(sprite);
      Texture2D texture = content.GetTexture(spriteInfo.TextureName);
      Rectangle sourceRectangle = spriteInfo.SourceRectangle;
      Vector2 textureSize = new Vector2((float) sourceRectangle.Width, (float) sourceRectangle.Height);
      Vector2 drawPosition = new Vector2();
      Vector2 drawCenter = new Vector2();
      Vector2 drawScale = new Vector2();
      camera.ToCameraPosition(position, size, textureSize, ref drawPosition, ref drawCenter, ref drawScale);
      spriteBatch.Draw(texture, drawPosition, new Rectangle?(sourceRectangle), color, MathHelper.ToRadians(rotation), drawCenter, drawScale, SpriteEffects.None, depth);
    }
  }
}
