// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.Particule
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SpacewarMonoGame
{
  public class Particule
  {
    public float particuleTime;
    public Color currentColor;
    public float currentSizeRatio;
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    public AccelerationMode accelerationMode;
    public Vector2 size;
    public float rotation;
    public BlendState blendMode;
    public string spriteName;
    public float depth;
    public List<ParticuleState> particuleStates;

    public bool IsDead { get; set; }

    public Particule(ContentHolder content, ParticuleStateList particuleStates)
    {
      this.IsDead = false;
      this.particuleStates = particuleStates.ParticuleStates;
    }

    public void Update(ContentHolder content, float seconds)
    {
      ParticuleState particuleState1 = (ParticuleState) null;
      ParticuleState particuleState2 = (ParticuleState) null;
      foreach (ParticuleState particuleState3 in this.particuleStates)
      {
        if ((double) this.particuleTime >= (double) particuleState3.targetTime)
          particuleState1 = particuleState3;
      }
      foreach (ParticuleState particuleState3 in this.particuleStates)
      {
        if ((double) this.particuleTime < (double) particuleState3.targetTime)
        {
          particuleState2 = particuleState3;
          break;
        }
      }
      if (particuleState2 == null)
        particuleState2 = this.particuleStates.Last<ParticuleState>();
      float num = (float) (((double) this.particuleTime - (double) particuleState1.targetTime) / ((double) particuleState2.targetTime - (double) particuleState1.targetTime));
      this.currentColor.A = (byte) ((double) particuleState1.targetColor.A * (1.0 - (double) num) + (double) particuleState2.targetColor.A * (double) num);
      this.currentColor.R = (byte) ((double) particuleState1.targetColor.R * (1.0 - (double) num) + (double) particuleState2.targetColor.R * (double) num);
      this.currentColor.G = (byte) ((double) particuleState1.targetColor.G * (1.0 - (double) num) + (double) particuleState2.targetColor.G * (double) num);
      this.currentColor.B = (byte) ((double) particuleState1.targetColor.B * (1.0 - (double) num) + (double) particuleState2.targetColor.B * (double) num);
      this.currentSizeRatio = (float) ((double) particuleState1.targetSizeRatio * (1.0 - (double) num) + (double) particuleState2.targetSizeRatio * (double) num);
      this.position += this.velocity * seconds;
      if (this.accelerationMode == AccelerationMode.ABSOLUTE)
        this.velocity += this.acceleration * seconds;
      else if (this.accelerationMode == AccelerationMode.RELATIVE)
        this.velocity *= Vector2.One - (Vector2.One - this.acceleration) * seconds;
      this.particuleTime += seconds;
      if ((double) this.particuleTime <= (double) this.particuleStates.Last<ParticuleState>().targetTime)
        return;
      this.IsDead = true;
    }

    public void Draw(ContentHolder content, SpriteBatch spriteBatch, Camera camera)
    {
      if ((double) this.size.X == 0.0)
        this.size = new Vector2((float) content.GetSpriteInfo(this.spriteName).Width, (float) content.GetSpriteInfo(this.spriteName).Height) / content.Parameters.GetItem<float>("ARENA_TO_SCREEN_RATIO");
      DrawHelper.Draw(content, spriteBatch, camera, this.spriteName, this.currentColor, this.position, this.size * this.currentSizeRatio, this.rotation, this.depth);
    }
  }
}
