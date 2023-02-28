// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.ParticleEmiter
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacewarMonoGame
{
  public class ParticleEmiter
  {
    public ParticuleStateList particuleStates;
    private float emitFrequency;
    private float lastEmitTime;
    public float depth;
    public string spriteName;
    public Vector2 size;
    public BlendState blendMode;
    private ParticleEngine particleEngine;

    public ParticleEmiter(ContentHolder content, ParticleEngine particleEngine, ParticuleStateList particuleStates, float emitFrequency)
    {
      this.particleEngine = particleEngine;
      this.particuleStates = particuleStates;
      this.emitFrequency = emitFrequency;
      this.spriteName = "particule";
      this.depth = content.Parameters.GetItem<float>("DEPTH_DEFAULT");
      this.blendMode = BlendState.NonPremultiplied;
      particleEngine.AddParticuleEmiter(this);
    }

    public void Update(float seconds)
    {
      this.lastEmitTime += seconds;
    }

    public void Emit(ContentHolder content, Vector2 position, Vector2 velocity)
    {
      if ((double) this.lastEmitTime <= (double) this.emitFrequency)
        return;
      this.lastEmitTime = 0.0f;
      this.particleEngine.AddParticule(new Particule(content, this.particuleStates)
      {
        depth = this.depth,
        size = this.size,
        position = position,
        blendMode = this.blendMode,
        velocity = velocity,
        spriteName = this.spriteName
      });
    }
  }
}
