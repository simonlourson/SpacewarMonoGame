// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.FrequencyChecker
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

namespace SpacewarMonoGame
{
  public class FrequencyChecker
  {
    private float emitFrequency;
    private float lastEmitTime;

    public FrequencyChecker(float emitFrequency)
    {
      this.emitFrequency = emitFrequency;
    }

    public void Update(float seconds)
    {
      this.lastEmitTime += seconds;
    }

    public bool CheckFrequency()
    {
      if ((double) this.lastEmitTime <= (double) this.emitFrequency)
        return false;
      this.lastEmitTime = 0.0f;
      return true;
    }
  }
}
