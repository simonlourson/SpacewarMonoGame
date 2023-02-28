// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.ParticleEngine
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpacewarMonoGame
{
  public class ParticleEngine
  {
    private List<Particule> particules;
    private List<ParticleEmiter> particulEmiters;

    public ParticleEngine()
    {
      this.particules = new List<Particule>();
      this.particulEmiters = new List<ParticleEmiter>();
    }

    public void AddParticule(Particule p)
    {
      this.particules.Add(p);
    }

    public void AddParticuleEmiter(ParticleEmiter pE)
    {
      this.particulEmiters.Add(pE);
    }

    public void Update(ContentHolder content, float seconds)
    {
      foreach (Particule particule in this.particules)
        particule.Update(content, seconds);
      foreach (ParticleEmiter particulEmiter in this.particulEmiters)
        particulEmiter.Update(seconds);
      this.particules.RemoveAll((Predicate<Particule>) (p => p.IsDead));
    }

    public void Draw(SpriteBatch spriteBatch, ContentHolder content, Camera camera, BlendState blendMode)
    {
      foreach (Particule particule in this.particules.Where<Particule>((Func<Particule, bool>) (p => p.blendMode == blendMode)))
        particule.Draw(content, spriteBatch, camera);
    }
  }
}
