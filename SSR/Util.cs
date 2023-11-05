using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
    public bool state_changed = false;
    public bool screen_changed = false;

    public int screen_width;
    public int screen_height;

    public Texture2D placeholder;

    private SpriteFont font;
    public SpriteFont big_font;
    
    // engine stuff?
    public Game _gamePointer;
    public SpriteBatch _SpriteBatch;

    private DateTime timePressed = DateTime.Now;
    public DateTime timeSinceRoundStarted = DateTime.Now;
    
    public State _state;
    public string _new_state;

    //private List<State> _state_list = new List<State>();
    public Dictionary<string, State> _state_dict = new Dictionary<string, State>();

    public DependencyContainer(Game gamePointer, SpriteBatch spriteBatchPointer) {
        _gamePointer = gamePointer;
        _SpriteBatch = spriteBatchPointer;
        font = _gamePointer.Content.Load<SpriteFont>("SSRFont");
        big_font = _gamePointer.Content.Load<SpriteFont>("BigFont");
        placeholder = StaticUtil.genPlaceholderTexture2D(_gamePointer.GraphicsDevice);
        
        updateScreenSizeInfo();
    }
    
    public bool SetState(string new_state) {
        if(_state_dict.ContainsKey(new_state)) {
            _new_state = new_state;
            state_changed = true;
            return true;
        }
        return false;
    }

    public void updateState() {
        _state = _state_dict[_new_state];
        state_changed = false;
        if (_new_state == "game") {
            timeSinceRoundStarted = DateTime.Now;
        }
    }

    public void resetTimer() {
        timeSinceRoundStarted = DateTime.Now;
    }

    public int secs() {
        return timeSinceRoundStarted.Second;
    }
    
    public void updateInputBuffer() {
        timePressed = DateTime.Now;
    }
    public bool hasInputBufferElapsed(float seconds = 0.05f) {
        return DateTime.Now > timePressed.AddSeconds(seconds);
    }

    public bool hasTimerpassed(float seconds = 30) {
        return DateTime.Now > timeSinceRoundStarted.AddSeconds(30);
    }

    public DateTime getTimer() {
        return timeSinceRoundStarted;
    }
    
    public void updateScreenSizeInfo() {
        screen_width = 960;
        screen_height = 540;
    }

    public Vector2 CentreVector() {
        int x = 960 / 2;
        int y = 540 / 2;
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