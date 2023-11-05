using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SSR; 

public class Canvas {
    private const int BRUSH_DEFAULT = 5;
    private Texture2D canvas;
    private Color[] data;
    private int data_size;
    private DependencyContainer _dependencyBox;

    public Vector2 screen_position;

    private int brush_size = 5;

    public Vector2 centre;

    public Canvas(DependencyContainer _dependencyContainer, Vector2 screenPosition,int canvas_width = 800, int canvas_height = 600) {
        _dependencyBox = _dependencyContainer;

        data_size = canvas_width * canvas_height;
        centre = new Vector2(canvas_width / 2, canvas_height / 2);
        data = new Color[data_size];
        canvas = new Texture2D(_dependencyContainer._gamePointer.GraphicsDevice, canvas_width, canvas_height);
        for (int i = 0; i < data_size; i++) {
            data[i] = Color.White;
        }
        canvas.SetData(data);

        screen_position = screenPosition;
    }


    public void resetCanvas() {
        saveCanvas();
        clearCanvas();
    }
    
   /* public string findFilename(string filename = "picture.png") {
        Random random = new Random();
        if(!File.Exists(filename))
            return filename;
        else {
            filename = random.Next(10000) + ".png";
        }
        
    }*/

    public void saveCanvas() {
        Random rand = new Random();
        string filename = rand.Next(100000) + ".png";
        string path = @"C:\Users\Milica\Documents\Coding\Hacknotts\sketcher-sketch-revolution\SSR\drawings\";
        //filename = findFilename(filename);
        try {Stream stream = File.Create(path + filename);
            canvas.SaveAsPng(stream, canvas.Width,canvas.Height);
            stream.Dispose(); }
        catch (Exception e) {
            Trace.WriteLine(e);
        }
        
    }

    public void clearCanvas() {
        for (int i = 0; i < data_size; i++) {
            data[i] = Color.White;
        }
        canvas.SetData(data);
    }
    
    public void setBrushSize(int size) {
        brush_size = size;
    }

    public void resetBrushSize() {
        brush_size = BRUSH_DEFAULT;
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

        for (int i = 0; i < brush_size; i++) {
            for (int j = 0; j < brush_size; j++) {
                if(lx+i < canvas.Width && ly+j < canvas.Height && lx+i > 0 && ly+j > 0) {
                    int xx = (int)(lx + i);
                    int yy = (int)(ly + j);
                    int pixel = xx + yy * canvas.Width;
                    updateColourAtIndex(pixel);
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