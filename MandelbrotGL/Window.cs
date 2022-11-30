using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace MandelbrotGL
{
internal class Window : GameWindow
{
    private int vBufferH;
    private int vArrayH;

    private int shaderProgram;

    public Window() : base(GameWindowSettings.Default, new NativeWindowSettings())
    {
        this.CenterWindow(new Vector2i(1280, 720));
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, e.Width, e.Height);

        base.OnResize(e);
    }

    protected override void OnLoad()
    {
        GL.ClearColor(new Color4(0.3f, 0.4f, 0.5f, 1.0f));

        float[] vertex = new float[]
        {
            -0.5f, -0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            0.0f,  0.5f, 0.0f
        };

        // Generate and bind the vertex buffer
        vBufferH = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vBufferH);
        GL.BufferData(BufferTarget.ArrayBuffer, vertex.Length * sizeof(float), vertex, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        vArrayH = GL.GenVertexArray();
        GL.BindVertexArray(vArrayH);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vBufferH);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindVertexArray(0);

        shaderProgram = ShaderFactory.CreateProgram("Shaders/vertex_shader.glsl", "Shaders/fragment_shader.glsl");

        base.OnLoad();
    }

    protected override void OnUnload()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.DeleteBuffer(vBufferH);

        GL.UseProgram(0);
        GL.DeleteProgram(shaderProgram);

        base.OnUnload();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.UseProgram(shaderProgram);
        GL.BindVertexArray(vArrayH);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        this.Context.SwapBuffers();
        base.OnRenderFrame(args);
    }
}
}
