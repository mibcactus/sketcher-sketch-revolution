using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SSR;

public class GameState : State {
    private Color text_colour = Color.Aqua;
    private Pen _pen;

    // the size of the images and canvas
    private Vector2 _pic_res = new Vector2(300, 490);
    private Vector2 _canvas_pos = Vector2.Zero;
    
    //private bool timer_on = false;

    // used for storing the images shown to the player
    private List<Texture2D> _images = new List<Texture2D>();
    private int _image_index = 0;

    private int score = 0;

    public GameState(DependencyContainer deps) : base(deps) {
        _canvas_pos.X = _deps.screen_width - 320;
        _canvas_pos.Y = 20;
        _pen = new Pen(_deps, _canvas_pos, 300, 490);
    }

    public override void LoadContent() {
        _backgroundTxt = _deps.loadTexture2D("game_bg");
        
        _images.Add(_deps.loadTexture2D("image1"));
        _images.Add(_deps.loadTexture2D("image2"));
        _images.Add(_deps.loadTexture2D("image3"));
    }

    public override void Update(GameTime gameTime) {
        _gamePadState = GamePad.GetState(PlayerIndex.Four);

        if (_gamePadState.IsButtonDown(Buttons.LeftShoulder)) {
            _deps.SetState("menu");
        }

        if (_deps.hasTimerpassed(180)) {
            score += _pen.reset("image"+ (_image_index + 1));
            if (_image_index == 3) {
                _image_index = 0;
            }
            else {
                _image_index++;
            }
            
            _deps.timeSinceRoundStarted = DateTime.Now;
        }

        _pen.Update(gameTime, _gamePadState);
        
    }

    public override void Draw(GameTime gameTime) {
        _deps._SpriteBatch.Draw(_backgroundTxt, Vector2.Zero, Color.White);
        

        Vector2 image_pos = new Vector2(
            image_pos.X = _canvas_pos.X - _pic_res.X - 20,
            image_pos.Y = _canvas_pos.Y);
        _deps._SpriteBatch.Draw(_images[_image_index], image_pos, Color.White);
        
        _pen.Draw();
        
        Vector2 pos = new Vector2(10, 10);
        string srr = (DateTime.Now.Second - _deps.timeSinceRoundStarted.Second).ToString();
        string time_str = "00:" + srr;
        string scr = "Score: " + score;

        _deps._SpriteBatch.DrawString(_deps.big_font, srr, pos, text_colour);
        pos.Y += 30;
        _deps._SpriteBatch.DrawString(_deps.big_font, scr, pos, text_colour);
    }
}