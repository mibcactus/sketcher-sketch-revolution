using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using OpenCvSharp;
using OpenCvSharp.Text;



namespace SSR;

public class Game1 : Game {
    private DependencyContainer _dependencyBox;
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private RenderTarget2D _nativeRenderTarget;
    private Rectangle _actualScreenRectangle;
    
    private const int screenWidth = 960;
    private const int screenHeight = 540;
    private int scaleSize = 2;
    
    private Color bgcolour = Color.CornflowerBlue;

    private State _state;

    //private List<State> _state_list = new List<State>();
    private Dictionary<string, State> _state_dict = new Dictionary<string, State>();

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
        
        _dependencyBox = new DependencyContainer(this, _spriteBatch);
        _dependencyBox.updateScreenSizeInfo();
        

        
        //_state_dict.Add("menu", new MenuState(_dependencyBox));
        _state_dict.Add("misc", new MiscState(_dependencyBox));
        
        foreach (var item in _state_dict) {
            item.Value.LoadContent();
        }
        _state = _state_dict["misc"];


        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (_dependencyBox.screen_changed) {
            _dependencyBox.updateScreenSizeInfo();
            _dependencyBox.screen_changed = false;
        }
        
        _state.Update(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.SetRenderTarget(_nativeRenderTarget);
        GraphicsDevice.Clear(bgcolour);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _state.Draw(gameTime);
        _spriteBatch.End();
        
        base.Draw(gameTime);
        
        GraphicsDevice.SetRenderTarget(null);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_nativeRenderTarget, _actualScreenRectangle, Color.White);
        _spriteBatch.End();
    }
}
