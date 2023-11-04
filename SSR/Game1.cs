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

    private Color bgcolour = Color.CornflowerBlue;

    private State _state;

    private List<State> _state_list = new List<State>();
    
    public Game1() {
        
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Trace.Listeners.Add(new ConsoleTraceListener());
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        
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
    
    
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        _dependencyBox = new DependencyContainer(this, _spriteBatch);
        _dependencyBox.updateScreenSizeInfo();
        
        _state_list.Add(new MiscState(_dependencyBox));



        foreach (var state in _state_list) {
            state.LoadContent();
        }
        
        _state = _state_list[0];


        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        _state.Update(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(bgcolour);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        _state.Draw(gameTime);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
