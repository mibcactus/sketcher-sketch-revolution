using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using OpenCvSharp;
using OpenCvSharp.Text;



namespace SSR;

public class Game1 : Game {
    private DependencyContainer _deps;
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private RenderTarget2D _nativeRenderTarget;
    private Rectangle _actualScreenRectangle;
    
    public const int screenWidth = 960;
    public const int screenHeight = 540;
    private int scaleSize = 2;
    
    private Color bgcolour = Color.CornflowerBlue;

    

    public Game1() {
        
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = screenWidth * scaleSize;
        _graphics.PreferredBackBufferHeight = screenHeight * scaleSize;
        //_graphics.IsFullScreen = true;
        _graphics.ToggleFullScreen();
        
        _actualScreenRectangle = new Rectangle(0, 0, screenWidth * scaleSize, screenHeight * scaleSize);

        //int scaleSize = _graphics.GraphicsDevice.DisplayMode.Width / screenWidth;
        
       // _actualScreenRectangle = new Rectangle(0, 0, screenWidth * scaleSize, screenHeight * scaleSize);
        
        _graphics.ApplyChanges();
        
        Trace.Listeners.Add(new ConsoleTraceListener());
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    
    
    protected override void Initialize() {
        _nativeRenderTarget = new RenderTarget2D(GraphicsDevice, screenWidth, screenHeight);
        //open cv test stuff
        // open an image, convert to greyscale, create new texture 2D out of it

        base.Initialize();
        
    }

    private void debugTestOpenCVSharpInLoadContent() {
        string cvtest_name = @"C:\\Users\\Milica\\Documents\\Coding\\Hacknotts\\sketcher-sketch-revolution\\SSR\resources\test.png";
        bool fe = File.Exists(cvtest_name);
        Trace.WriteLine(fe ? "Image exists" : "Image doesnt exist");

        using (var imag = new Mat(cvtest_name)) {
            if(!imag.Empty()) {
                Trace.WriteLine("<- Size of Mat created successfully", imag.Size().ToString());
                using (var grey_image = imag.CvtColor(ColorConversionCodes.BGR2GRAY)) {
                    grey_image.SaveImage(@"C:\\Users\\Milica\\Documents\\Coding\\Hacknotts\\sketcher-sketch-revolution\\SSR\resources/example_convert.png");
                }

                bgcolour = Color.Red;
            }
            else {
                Trace.WriteLine("Test image not loaded!");
            }
        }
    }
    
    
    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        _deps = new DependencyContainer(this, _spriteBatch);
        _deps.updateScreenSizeInfo();
        

        
        _deps._state_dict.Add("menu", new MenuState(_deps));
        _deps._state_dict.Add("misc", new MiscState(_deps));
        _deps._state_dict.Add("game", new GameState(_deps));
        
        foreach (var item in _deps._state_dict) {
            item.Value.LoadContent();
        }

        _deps._new_state = "menu";
        _deps.updateState();
        //_deps._state = _deps._state_dict["menu"];


        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (_deps.screen_changed) {
            _deps.updateScreenSizeInfo();
            _deps.screen_changed = false;
        }

        if (_deps.state_changed) {
            _deps.updateState();
            _deps.state_changed = false;
        }
        
        _deps._state.Update(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.SetRenderTarget(_nativeRenderTarget);
        GraphicsDevice.Clear(bgcolour);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _deps._state.Draw(gameTime);
        _spriteBatch.End();
        
        base.Draw(gameTime);
        
        GraphicsDevice.SetRenderTarget(null);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_nativeRenderTarget, _actualScreenRectangle, Color.White);
        _spriteBatch.End();
    }
}
