using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SSR; 

public class Canvas {
    private Texture2D canvas;
    private Color[] data;
    private int data_size;
    private DependencyContainer _dependencyBox;

    private Vector2 screen_position;

    //private Rectangle brush;

    private Rectangle? brush;

    public Canvas(DependencyContainer _dependencyContainer, Vector2 screenPosition,int canvas_width = 800, int canvas_height = 600) {
        _dependencyBox = _dependencyContainer;

        data_size = canvas_width * canvas_height;
        data = new Color[data_size];
        canvas = new Texture2D(_dependencyContainer._gamePointer.GraphicsDevice, canvas_width, canvas_height);
        for (int i = 0; i < data_size; i++) {
            data[i] = Color.White;
        }
        canvas.SetData(data);

        screen_position = screenPosition;
        brush = new Rectangle(5, 5, 5, 5);
    }

    public Texture2D getCanvas() {
        return canvas;
    }

 
    public Vector2 localisePosition(Vector2 _target) {
        return _target - screen_position;
    }

    public void updateCanvas(Vector2 position) {
        Vector2 target_location = localisePosition(position);
        
        float lx = target_location.X;
        float ly = target_location.Y;
        /*if (lx < 0 || lx > canvas.Width || ly < 0 || ly > canvas.Height) {
            return;
        }*/

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                
                if(lx+i < canvas.Width && ly+j < canvas.Height 
                   && lx+i > 0 && ly+j > 0) {
                    int aaax = (int)(lx + i);
                    int aaay = (int)(ly + j);
                    int aaa = aaax + aaay * canvas.Width;
                    updateColourAtIndex(aaa);
                }
                
            }
        }  
        
        canvas.SetData(data);
        
    }
    
    public void updateColourAtIndex(int index) {
        data[index] = Color.Black;
    }

    public void drawCanvas() {
        _dependencyBox._SpriteBatch.Draw(canvas, screen_position, Color.White);
    }
    
}