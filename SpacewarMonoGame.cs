using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpacewarMonoGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpacewarMonoGameParameters _parameters;
    private ContentHolder _contentHolder;
    private Camera _camera;
    private Arena _arena;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
      this._parameters = SpacewarMonoGameParameters.FromFile(Environment.CurrentDirectory + "/Content/config.xml");
      this._contentHolder = ContentHolder.FromFile(Environment.CurrentDirectory + "/Content/content.xml");
      this._contentHolder.Parameters = (Parameters) this._parameters;
      string str1 = "size of window: ";
      Rectangle clientBounds = this.Window.ClientBounds;
      string str2 = clientBounds.Width.ToString();
      string str3 = "x";
      clientBounds = this.Window.ClientBounds;
      string str4 = clientBounds.Height.ToString();
      Console.WriteLine(str1 + str2 + str3 + str4);
      this._graphics.IsFullScreen = this._parameters.GetItem<bool>("FULLSCREEN");
      Vector2 vector2 = this._parameters.GetItem<Vector2Xml>("SCREEN_SIZE");
      if ((double) this.Window.ClientBounds.Width < (double) vector2.X || (double) this.Window.ClientBounds.Height < (double) vector2.Y)
      {
        Console.WriteLine("resizing to: " + vector2.X.ToString() + "x" + vector2.Y.ToString());
        this._graphics.PreferredBackBufferWidth = (int) vector2.X;
        this._graphics.PreferredBackBufferHeight = (int) vector2.Y;
        this._graphics.ApplyChanges();
      }

      base.Initialize();
    }

    protected override void LoadContent()
    {
      _spriteBatch = new SpriteBatch(GraphicsDevice);
      this._spriteBatch = new SpriteBatch(this.GraphicsDevice);
      this._contentHolder.InitTextures(this.GraphicsDevice);
      this._contentHolder.InitFonts(this.Content);
      this._contentHolder.InitFromXml();
      this._camera = new Camera(this._parameters.GetItem<Vector2Xml>("SCREEN_SIZE"), Vector2.Zero, this._parameters.GetItem<Vector2Xml>("ARENA_SIZE"));
      this._arena = new Arena(this._contentHolder);
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      // if (Keyboard.GetState().IsKeyDown(Keys.H))
      //   this.Content.Load<SoundEffect>("bullet").Play();

      float seconds = (float) ((double) gameTime.ElapsedGameTime.Ticks * (1.0 / 1000.0) / 10000.0);
      this._contentHolder.ParticulEngine.Update(this._contentHolder, seconds);
      this._arena.Update(this._contentHolder, seconds);

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Red);

      this._spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, (SamplerState) null, (DepthStencilState) null, (RasterizerState) null, (Effect) null, new Matrix?());
      Vector2 vector2 = this._contentHolder.Parameters.GetItem<Vector2Xml>("SCREEN_SIZE");
      SpriteInfo spriteInfo = this._contentHolder.GetSpriteInfo("background");
      this._spriteBatch.Draw(this._contentHolder.GetTexture(spriteInfo.TextureName), new Rectangle(0, 0, (int) vector2.X, (int) vector2.Y), new Rectangle?(spriteInfo.SourceRectangle), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, this._contentHolder.Parameters.GetItem<float>("DEPTH_BACKGROUND"));
      this._contentHolder.ParticulEngine.Draw(this._spriteBatch, this._contentHolder, this._camera, BlendState.NonPremultiplied);
      this._arena.Draw(this._spriteBatch, this._contentHolder, this._camera);
      this._spriteBatch.End();
      this._spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Additive, (SamplerState) null, (DepthStencilState) null, (RasterizerState) null, (Effect) null, new Matrix?());
      this._contentHolder.ParticulEngine.Draw(this._spriteBatch, this._contentHolder, this._camera, BlendState.Additive);
      this._spriteBatch.End();

      base.Draw(gameTime);
    }
}
