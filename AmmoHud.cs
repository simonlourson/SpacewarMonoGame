// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.AmmoHud
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace SpacewarMonoGame
{
  public class AmmoHud
  {
    private BonusType bonusType;
    private int playerIndex;
    private Vector2 position;
    private bool inverted;
    private Color color;
    private Dictionary<BonusType, int> bonusAmmo;
    private int bonusIndex;
    private bool previousChangeWeaponPositive;
    private bool previousChangeWeaponNegative;

    public BonusType BonusType
    {
      get
      {
        return this.bonusType;
      }
    }

    public int RemainingAmmo
    {
      get
      {
        return this.bonusAmmo[this.bonusType];
      }
    }

    public AmmoHud(Vector2 position, bool inverted, Color color, int playerIndex)
    {
      this.position = position;
      this.inverted = inverted;
      this.color = color;
      this.playerIndex = playerIndex;
      this.Reset();
    }

    public void Update(ContentHolder content, float seconds)
    {
      if (!this.previousChangeWeaponPositive && Keyboard.GetState().IsKeyDown(content.Parameters.GetItem<Keys[]>("KEY_ACTION_CHANGE_POSITIVE")[this.playerIndex]))
        this.ChangeWeapon(1);
      if (!this.previousChangeWeaponNegative && Keyboard.GetState().IsKeyDown(content.Parameters.GetItem<Keys[]>("KEY_ACTION_CHANGE_NEGATIVE")[this.playerIndex]))
        this.ChangeWeapon(-1);
      this.previousChangeWeaponPositive = Keyboard.GetState().IsKeyDown(content.Parameters.GetItem<Keys[]>("KEY_ACTION_CHANGE_POSITIVE")[this.playerIndex]);
      if (Keyboard.GetState().IsKeyDown(content.Parameters.GetItem<Keys[]>("KEY_ACTION_CHANGE_NEGATIVE")[this.playerIndex]))
        this.previousChangeWeaponNegative = true;
      else
        this.previousChangeWeaponNegative = false;
    }

    public void Reset()
    {
      this.bonusAmmo = new Dictionary<BonusType, int>();
      this.UpdateHud();
    }

    private void ChangeWeapon(int delta)
    {
      this.UpdateHud();
      if (this.bonusAmmo.Count <= 1)
        return;
      this.bonusIndex += delta;
      this.UpdateHud();
    }

    public void ChangeAmmo(BonusType bonusType, int delta)
    {
      if (!this.bonusAmmo.Keys.Contains<BonusType>(bonusType))
        this.bonusAmmo.Add(bonusType, 0);
      this.bonusAmmo[bonusType] += delta;
      if (this.bonusAmmo[bonusType] >= 0)
        return;
      this.bonusAmmo[bonusType] = 0;
    }

    public void SetAmmo(BonusType bonusType)
    {
      while (this.bonusType != bonusType)
        this.ChangeWeapon(1);
    }

    private void UpdateHud()
    {
      List<BonusType> bonusTypeList = new List<BonusType>();
      foreach (BonusType key in this.bonusAmmo.Keys)
      {
        if (this.bonusAmmo[key] == 0)
          bonusTypeList.Add(key);
      }
      foreach (BonusType key in bonusTypeList)
        this.bonusAmmo.Remove(key);
      if (this.bonusAmmo.Count == 0)
      {
        this.bonusType = BonusType.NONE;
      }
      else
      {
        if (this.bonusIndex >= this.bonusAmmo.Keys.Count)
          this.bonusIndex = 0;
        else if (this.bonusIndex < 0)
          this.bonusIndex = this.bonusAmmo.Keys.Count - 1;
        this.bonusType = this.bonusAmmo.Keys.ToList<BonusType>()[this.bonusIndex];
      }
    }

    public void Draw(SpriteBatch spriteBatch, ContentHolder content)
    {
      if (this.BonusType == BonusType.NONE)
        return;
      this.BonusType.ToSprite();
      string sprite = this.BonusType.ToSprite();
      string name = sprite + "_contour";
      SpriteInfo spriteInfo1 = content.GetSpriteInfo(sprite);
      int num1 = 20;
      int num2 = (int) content.Parameters.GetItem<Vector2Xml>("STATUS_BAR_SIZE").Y * 2;
      Vector2 position1 = this.position;
      Vector2 vector2 = content.GetFont(content.Parameters.GetItem<string>("FONT_AMMO")).MeasureString(this.RemainingAmmo.ToString());
      float scale = (float) num2 / vector2.Y;
      Vector2 position2;
      if (this.inverted)
      {
        position1 -= new Vector2((float) spriteInfo1.Width, 0.0f);
        position2 = position1 - new Vector2((float) num1 + vector2.X * scale, 0.0f);
      }
      else
        position2 = position1 + new Vector2((float) (spriteInfo1.Width + num1), 0.0f);
      Rectangle destinationRectangle;
      destinationRectangle.X = (int) position1.X;
      destinationRectangle.Y = (int) position1.Y;
      destinationRectangle.Width = num2;
      destinationRectangle.Height = num2;
      SpriteInfo spriteInfo2 = content.GetSpriteInfo(sprite);
      SpriteInfo spriteInfo3 = content.GetSpriteInfo(name);
      Texture2D texture = content.GetTexture(spriteInfo2.TextureName);
      spriteBatch.Draw(texture, destinationRectangle, new Rectangle?(spriteInfo2.SourceRectangle), this.color, 0.0f, Vector2.Zero, SpriteEffects.None, content.Parameters.GetItem<float>("DEPTH_HUD"));
      spriteBatch.Draw(texture, destinationRectangle, new Rectangle?(spriteInfo3.SourceRectangle), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, content.Parameters.GetItem<float>("DEPTH_HUD") + 1f / 1000f);
      spriteBatch.DrawString(content.GetFont(content.Parameters.GetItem<string>("FONT_AMMO")), this.RemainingAmmo.ToString(), position2, this.color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, content.Parameters.GetItem<float>("DEPTH_HUD"));
    }
  }
}
