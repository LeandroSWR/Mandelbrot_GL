using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MandelbrotGL
{
    internal class Window : GameWindow
    {
        private int vBufferH;
        private int vArrayH;

        private int shaderProgram;

        private float scale = 2.0f;
        private Vector2 center = new Vector2(-3.0f / 4.0f, 0.0f);
        private int maxIterations = 3000;

        public Window() : base(GameWindowSettings.Default, new NativeWindowSettings())
        {
            this.CenterWindow(new Vector2i(1280, 720));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);

            base.OnResize(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            // Zoom in/out
            scale *= (1.0f - e.OffsetY * 0.05f);

            base.OnMouseWheel(e);
        }

        
        protected unsafe override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (MouseState.IsButtonDown(MouseButton.Middle))
            {
                // Hide the mouse cursor
                GLFW.SetInputMode(WindowPtr, CursorStateAttribute.Cursor, CursorModeValue.CursorHidden);

                // Set the translations speed when moving arround
                var translationSpeed = 0.003f * scale;

                center.X -= e.DeltaX * translationSpeed;
                center.Y += e.DeltaY * translationSpeed;
            }
            else
            {
                // Show the mouse cursor
                GLFW.SetInputMode(WindowPtr, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
            }

            base.OnMouseMove(e);
        }

        protected override void OnLoad()
        {
            GL.ClearColor(new Color4(0.3f, 0.4f, 0.5f, 1.0f));

            float[] vertex = new float[]
            {
            -1.0f, 1.0f, 0.0f,
            -1.0f, -1.0f, 0.0f,
            1.0f, 1f, 0.0f,
            1.0f,  1.0f, 0.0f,
            1.0f, -1.0f, 0.0f,
            -1.0f, -1.0f, 0.0f
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

            float windowAspect = (float)ClientSize.X / (float)ClientSize.Y;

            GL.UseProgram(shaderProgram);
            GL.Uniform1(GL.GetUniformLocation(shaderProgram, "windowAspect"), windowAspect);
            GL.Uniform2(GL.GetUniformLocation(shaderProgram, "center"), center);
            GL.Uniform1(GL.GetUniformLocation(shaderProgram, "scale"), scale);
            GL.Uniform1(GL.GetUniformLocation(shaderProgram, "maxIterations"), maxIterations);

            // Bind Vertex Array Object
            GL.BindVertexArray(vArrayH);
            // Draw
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            this.Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
