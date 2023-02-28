// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.Camera
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using Microsoft.Xna.Framework;

namespace SpacewarMonoGame
{
  public class Camera
  {
    private Vector2 screenSize;
    private Vector2 screenOffset;
    private Vector2 viewportPosition;
    private Vector2 viewportSize;

    public Camera(Vector2 screenSize, Vector2 viewportPosition, Vector2 viewportSize)
    {
      this.screenSize = screenSize;
      this.screenOffset = new Vector2(0.0f, 0.0f);
      this.viewportPosition = viewportPosition;
      this.viewportSize = viewportSize;
    }

    public Camera(Vector2 screenSize, Vector2 screenOffset, Vector2 viewportPosition, Vector2 viewportSize)
    {
      this.screenSize = screenSize;
      this.screenOffset = screenOffset;
      this.viewportPosition = viewportPosition;
      this.viewportSize = viewportSize;
    }

    public void ToCamera(Vector2 position, Vector2 size, ref Rectangle destRect)
    {
      Vector2 vector2_1 = new Vector2((position.X - this.viewportPosition.X) * this.screenSize.X / this.viewportSize.X, (position.Y - this.viewportPosition.Y) * this.screenSize.Y / this.viewportSize.Y);
      Vector2 vector2_2 = new Vector2(size.X * this.screenSize.X / this.viewportSize.X, size.Y * this.screenSize.Y / this.viewportSize.Y);
      destRect.X = (int) ((double) this.screenOffset.X + (double) vector2_1.X - (double) vector2_2.X / 2.0);
      destRect.Y = (int) ((double) this.screenOffset.Y + (double) vector2_1.Y - (double) vector2_2.Y / 2.0);
      destRect.Width = (int) vector2_2.X;
      destRect.Height = (int) vector2_2.Y;
    }

    public void ToCamera(Vector2 position, ref Vector2 sdlPoint)
    {
      Vector2 vector2 = new Vector2((position.X - this.viewportPosition.X) * this.screenSize.X / this.viewportSize.X, (position.Y - this.viewportPosition.Y) * this.screenSize.Y / this.viewportSize.Y);
      sdlPoint.X = (float) (int) vector2.X;
      sdlPoint.Y = (float) (int) vector2.Y;
    }

    public void ToCameraPosition(Vector2 worldPosition, Vector2 worldSize, Vector2 textureSize, ref Vector2 drawPosition, ref Vector2 drawCenter, ref Vector2 drawScale)
    {
      Vector2 vector2_1 = new Vector2((worldPosition.X - this.viewportPosition.X) * this.screenSize.X / this.viewportSize.X, (worldPosition.Y - this.viewportPosition.Y) * this.screenSize.Y / this.viewportSize.Y);
      Vector2 vector2_2 = new Vector2(this.screenSize.X * worldSize.X / this.viewportSize.X, this.screenSize.Y * worldSize.Y / this.viewportSize.Y);
      drawPosition.X = this.screenOffset.X + vector2_1.X;
      drawPosition.Y = this.screenOffset.Y + vector2_1.Y;
      drawScale.X = vector2_2.X / textureSize.X;
      drawScale.Y = vector2_2.Y / textureSize.Y;
      drawCenter.X = textureSize.X / 2f;
      drawCenter.Y = textureSize.Y / 2f;
    }
  }
}
