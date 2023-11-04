using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OpenCvSharp;

namespace SSR;

public enum StateID {
    MENU,
    GAME,
    TEST,
    MISC
}

public class DependencyContainer {
    public bool paused;
    public bool state_changed;
    public Color bg_colour;

    public int screen_width;
    public int screen_height;

    public Texture2D placeholder;
    private int placeholder_width = 100;
    private int placeholder_height = 100;

    private SpriteFont font;
    
    // engine stuff?
    public Game _gamePointer;
    public SpriteBatch _SpriteBatch;

    //private GraphicsDeviceManager _GraphicsDeviceManager;

    public DependencyContainer(Game gamePointer, SpriteBatch spriteBatchPointer) {
        this._gamePointer = gamePointer;
        this._SpriteBatch = spriteBatchPointer;
        this.font = _gamePointer.Content.Load<SpriteFont>("SSRFont");
    }

    public void updateScreenSizeInfo() {
        screen_width = _gamePointer.GraphicsDevice.PresentationParameters.BackBufferWidth;
        screen_height = _gamePointer.GraphicsDevice.PresentationParameters.BackBufferHeight;
    }

    public SpriteFont Font() {
        return font;
    }

    public Texture2D loadTexture2D(string filename, int error_width = 100, int error_height = 100) {
        Texture2D _txt;
        try {
            _txt = _gamePointer.Content.Load<Texture2D>(filename);
        }
        catch (ContentLoadException e) {
            Console.WriteLine(e);
            _txt = StaticUtil.genPlaceholderTexture2D(_gamePointer.GraphicsDevice,error_width, error_height);
        }
        
        return _txt;
    }

}