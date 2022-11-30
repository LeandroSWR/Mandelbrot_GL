#version 330 core

uniform float windowAspect;

uniform vec2 center;
uniform float scale;
uniform int maxIterations;
uniform sampler2D colorTexture;

varying vec2 pixelPosition;

out vec4 fragColor;

int runMandelbrotIterations(vec2 c)
{
	vec2 z = c;

	int n;

	for (n = 0; n < maxIterations; ++n)
	{
		z = vec2(z.x * z.x - z.y * z.y, 2.0 * z.x * z.y) + c; // Mandelbrot Equation

		if (length(z) > 2.0)
		{
			break; // If it's higher than 2 then it's outsize.
		}
	}

	return n;
}

void main()
{
	vec2 c = vec2(
		windowAspect * pixelPosition.x * scale + center.x,
		pixelPosition.y * scale + center.y
	);

	int n = runMandelbrotIterations(c);

	if (n == maxIterations)
	{
		// When we hit max iterations we consider the pixel to be inside so
		// we give it a black color
		fragColor = vec4(0.0, 0.0, 0.0, 0.0);
	}
	else
	{
		// When Z is outside the parameters give the pixel a color from the given texture
		// Chose the color based of the number of iterations performed to get to a value higher than 2
		fragColor = texture(colorTexture, vec2(n / float(maxIterations), 0.0));
	}
} 