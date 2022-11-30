namespace MandelbrotGL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Creates a new OpenGL window and runs it
            new Window(800, 600, "Mandlebrot").Run();
        }
    }
}