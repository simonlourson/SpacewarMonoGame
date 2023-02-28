// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.PhysicsObject
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacewarMonoGame
{
  public class PhysicsObject : IUpdateableDrawable
  {
    public Body physicsBody;
    public Vector2 position;
    public Vector2 size;
    public string sprite;
    public float rotation;
    public float depth;
    protected Color color;
    protected Arena arena;

    public bool IsDead { get; set; }

    public PhysicsObject(ContentHolder content, Arena arena)
    {
      this.arena = arena;
      this.depth = content.Parameters.GetItem<float>("DEPTH_DEFAULT");
      this.color = Color.White;
    }

    public virtual void Update(ContentHolder content, Arena arena, float seconds)
    {
      this.position = this.physicsBody.GetWorldPoint(Vector2.Zero);
    }

    public virtual void Draw(ContentHolder content, SpriteBatch spriteBatch, Camera camera)
    {
      SpriteInfo spriteInfo = content.GetSpriteInfo(this.sprite);
      Texture2D texture = content.GetTexture(spriteInfo.TextureName);
      Rectangle sourceRectangle = spriteInfo.SourceRectangle;
      Vector2 textureSize = new Vector2((float) sourceRectangle.Width, (float) sourceRectangle.Height);
      Vector2 drawPosition = new Vector2();
      Vector2 drawCenter = new Vector2();
      Vector2 drawScale = new Vector2();
      camera.ToCameraPosition(this.position, this.size, textureSize, ref drawPosition, ref drawCenter, ref drawScale);
      spriteBatch.Draw(texture, drawPosition, new Rectangle?(sourceRectangle), this.color, MathHelper.ToRadians(this.rotation), drawCenter, drawScale, SpriteEffects.None, this.depth);
    }

    public virtual void ChangeColor(Color color)
    {
      this.color = color;
    }
  }
}
