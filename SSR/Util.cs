using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OpenCvSharp;

namespace SSR;

public enum StateID {
    MENU,
    GAME,
    SCORE,
    CREDITS,
    OPTIONS,
    MISC
}

public class DependencyContainer {
    public bool paused = false;
    public bool state_changed = false;
    public bool screen_changed = false;
    public Color bg_colour;

    public int screen_width;
    public int screen_height;

    public Texture2D placeholder;

    private SpriteFont font;
    
    // engine stuff?
    public Game _gamePointer;
    public SpriteBatch _SpriteBatch;

    private DateTime timePressed = DateTime.Now;

    public DependencyContainer(Game gamePointer, SpriteBatch spriteBatchPointer) {
        this._gamePointer = gamePointer;
        this._SpriteBatch = spriteBatchPointer;
        this.font = _gamePointer.Content.Load<SpriteFont>("SSRFont");
        placeholder = StaticUtil.genPlaceholderTexture2D(_gamePointer.GraphicsDevice);
        
        updateScreenSizeInfo();
    }
    
    
    public void updateInputBuffer() {
        timePressed = DateTime.Now;
    }
    public bool hasInputBufferElapsed(float seconds = 0.1f) {
        return DateTime.Now > timePressed.AddSeconds(0.05);
    }
    
    public void updateScreenSizeInfo() {
        screen_width = _gamePointer.GraphicsDevice.PresentationParameters.BackBufferWidth;
        screen_height = _gamePointer.GraphicsDevice.PresentationParameters.BackBufferHeight;
    }

    public Vector2 CentreVector() {
        int x = (int) Math.Floor((double)  (screen_width / 2));
        int y = (int) Math.Floor((double) (screen_height / 2));
        return new Vector2(x, y);
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