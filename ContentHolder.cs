// Decompiled with JetBrains decompiler
// Type: SpacewarMonoGame.ContentHolder
// Assembly: SpacewarMonoGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE911453-265D-4743-9A13-C4D7C2C7456D
// Assembly location: E:\SpacewarNetwork\SpacewarMonoGame.exe

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SpacewarMonoGame
{
  public class ContentHolder
  {
    private Dictionary<string, Texture2D> contentTextures;
    private Dictionary<string, IntPtr> contentSounds;
    private Dictionary<string, SpriteInfo> contentSpriteInfos;
    private Dictionary<string, SpriteFont> contentFonts;
    private Dictionary<string, ParticuleStateList> contentParticleStateLists;
    [XmlIgnore]
    public ParticleEngine ParticulEngine;
    [XmlIgnore]
    public Parameters Parameters;
    public List<SpriteInfo> SpriteInfos;
    public List<ParticuleStateList> ParticleInfos;
    public List<SoundInfo> SoundInfos;
    public List<string> FontInfos;

    public ContentHolder()
    {
      this.contentTextures = new Dictionary<string, Texture2D>();
      this.contentSpriteInfos = new Dictionary<string, SpriteInfo>();
      this.contentFonts = new Dictionary<string, SpriteFont>();
      this.contentParticleStateLists = new Dictionary<string, ParticuleStateList>();
      this.contentSounds = new Dictionary<string, IntPtr>();
      this.ParticulEngine = new ParticleEngine();
    }

    public void InitFromXml()
    {
      foreach (SpriteInfo spriteInfo in this.SpriteInfos)
      {
        spriteInfo.CreateRectangle();
        this.contentSpriteInfos.Add(spriteInfo.Name, spriteInfo);
      }
      foreach (ParticuleStateList particleInfo in this.ParticleInfos)
        this.contentParticleStateLists.Add(particleInfo.Name, particleInfo);
    }

    public void InitTextures(GraphicsDevice graphicsDevice)
    {
      foreach (string name in this.SpriteInfos.Select<SpriteInfo, string>((Func<SpriteInfo, string>) (s => s.TextureName)).Distinct<string>())
        this.LoadTexture(graphicsDevice, name);
    }

    public void InitSounds()
    {
      foreach (SoundInfo soundInfo in this.SoundInfos)
        this.LoadSound(soundInfo.Name, soundInfo.SoundFile);
    }

    public void InitFonts(ContentManager content)
    {
      foreach (string fontInfo in this.FontInfos)
        this.LoadFont(content, fontInfo);
    }

    public void LoadSound(string name, string soundFile)
    {
    }

    public void LoadTexture(GraphicsDevice graphicsDevice, string name)
    {
      Texture2D texture2D = Texture2D.FromFile(graphicsDevice, name);
      this.contentTextures.Add(name, texture2D);
    }

    public void LoadFont(ContentManager content, string fontName)
    {
      SpriteFont spriteFont = content.Load<SpriteFont>(fontName);
      this.contentFonts.Add(fontName, spriteFont);
    }

    public Texture2D GetTexture(string name)
    {
      return this.contentTextures[name];
    }

    public SpriteInfo GetSpriteInfo(string name)
    {
      return this.contentSpriteInfos[name];
    }

    public SpriteFont GetFont(string name)
    {
      return this.contentFonts[name];
    }

    public IntPtr GetSound(string name)
    {
      return this.contentSounds[name];
    }

    public ParticuleStateList GetParticuleStateList(string name)
    {
      return this.contentParticleStateLists[name];
    }

    public void Dispose()
    {
      foreach (GraphicsResource graphicsResource in this.contentTextures.Values)
        graphicsResource.Dispose();
    }

    public void ToXmlFile(string file)
    {
      Type[] extraTypes = new Type[]
      {
        typeof (SpriteInfo)
      };
      TextWriter text = (TextWriter) File.CreateText(file);
      new XmlSerializer(this.GetType(), extraTypes).Serialize(text, (object) this);
      text.Close();
    }

    public static ContentHolder FromFile(string file)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (ContentHolder), new Type[]
      {
        typeof (SpriteInfo)
      });
      FileStream fileStream = File.OpenRead(file);
      ContentHolder contentHolder = (ContentHolder) xmlSerializer.Deserialize((Stream) fileStream);
      fileStream.Close();
      return contentHolder;
    }
  }
}
