using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SSR;

public class Pen {
    private const float MAX_RADIAN = 6.28f;
    private const float Pi = 3.14f;

    private GamePadDPad free_dpad = new GamePadDPad(
        ButtonState.Released, ButtonState.Released,
        ButtonState.Released, ButtonState.Released);

    private DependencyContainer _deps;

    private Texture2D down_texture;
    private Texture2D up_texture;
    private Texture2D current_texture;

    private Vector2 _position;
    private Vector2 _origin_offset;
    private float _angle = 0f;

    private float _rotation_speed = 0.05f;

    private Vector2 vector_speed = Vector2.Zero;

    private const int MAX_SPEED = 10;
    private const int MIN_SPEED = 1;
    private float _velocity = 0.7f;


    public bool isDown = true;

    private Canvas _canvas;

    public Pen(DependencyContainer _dependencies, Vector2 canvas_position, int canvas_width = 800,
        int canvas_height = 600) {
        _deps = _dependencies;
        
        down_texture = _deps.loadTexture2D("pen_down");
        up_texture = _deps.loadTexture2D("pen_up");
        current_texture = down_texture;

        _origin_offset = new Vector2(up_texture.Width / 2, up_texture.Height / 2);

        _canvas = new Canvas(_dependencies, canvas_position, canvas_width, canvas_height);
        _position = _dependencies.CentreVector() + _canvas.centre + new Vector2(50,50);
        Trace.Listeners.Add(new ConsoleTraceListener());
    }

    private bool RotatePressed(GamePadDPad dpad) {
        return dpad.Left == ButtonState.Pressed || dpad.Right == ButtonState.Pressed;
    }

    private bool buttonsPressed(GamePadState gamePadState) {
        if (gamePadState.DPad != free_dpad) {
            return true;
        }

        if (gamePadState.Buttons.A == ButtonState.Pressed || gamePadState.Buttons.A == ButtonState.Pressed ||
            gamePadState.Buttons.Start == ButtonState.Pressed) {
            return true;
        }

        return gamePadState.Triggers.Right < 0.5f;
    }

    public int reset(string image) {

        int aaaa = StaticUtil.compareimages(image, _canvas.canvas);
        _canvas.resetCanvas();
        _position = _deps.CentreVector() + _canvas.centre + new Vector2(50,50);

        return aaaa;
    }

    public void Update(GameTime gameTime, GamePadState gamePadState) {
        _deps.updateInputBuffer();

        if (gamePadState.IsButtonDown(Buttons.RightShoulder) && _deps.hasInputBufferElapsed()) {
            isDown = !isDown;
            _deps.updateInputBuffer();
        }
        
        if (isDown) {
            if (gamePadState.IsButtonDown(Buttons.B)) {
                _velocity -= 0.5f;
            }

            if (gamePadState.IsButtonDown(Buttons.A)) {
                _velocity += 0.5f;
            }

            GamePadDPad dPad = gamePadState.DPad;

            float temp_angle = 0;
            if (dPad.Left == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.A)) {
                temp_angle -= _rotation_speed;
            }

            if (dPad.Right == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.D)) {
                temp_angle += _rotation_speed;
            }


            _angle += temp_angle;
            if (_angle > MAX_RADIAN) {
                _angle -= MAX_RADIAN;
            }
            else if (_angle < 0) {
                _angle = MAX_RADIAN - _angle;
            }

            Vector2 ExtendedVector = new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle));

            ExtendedVector.X *= _velocity;
            ExtendedVector.Y *= _velocity;
            //Trace.WriteLine("\nExtendedVector: " + ExtendedVector);

            if (dPad.Up == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.W)) {
                _position -= ExtendedVector;
            }

            if (dPad.Down == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.S)) {
                _position += ExtendedVector;
            }

            if (_position.X < _canvas.screen_position.X - _origin_offset.X ) {
                _position.X = _canvas.screen_position.X - _origin_offset.X;
            }
                
            if(_position.X > _canvas.screen_position.X + _canvas.getCanvas().Width - _origin_offset.X) {
                _position.X = _canvas.getCanvas().Width + _canvas.screen_position.X - _origin_offset.X;
            }

            if (_position.Y < _canvas.screen_position.Y - _origin_offset.Y) {
                _position.Y = _canvas.screen_position.Y - _origin_offset.Y;
            }
            if (_position.Y > _canvas.screen_position.Y + _canvas.getCanvas().Height- _origin_offset.Y ) {
                _position.Y = _canvas.screen_position.Y + _canvas.getCanvas().Height- _origin_offset.Y;
            }

            current_texture = down_texture;
            _canvas.updateCanvas(_position + _origin_offset);
        }
        else {
            current_texture = up_texture;
        }
    }

    public double toDegrees() {
        return _angle * (180 / Pi);
    }

    public float returnAngle() {
        return _angle;
    }

    public void Draw() {
        _canvas.drawCanvas();
        var rotate_angle = _angle + 1.5f * Pi;
        try {
            _deps._SpriteBatch.Draw(current_texture, _position + _origin_offset, null, Color.White,
                rotate_angle,
                _origin_offset, Vector2.One, SpriteEffects.None, 0f);
        }
        catch (Exception e) {
            Console.WriteLine(e);
        }
    }
}