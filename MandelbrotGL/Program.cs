using OpenTK.Windowing.Desktop;

namespace MandelbrotGL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Creates a new OpenGL window and runs it
            Window window = new Window();
            window.Run();
        }
    }
}