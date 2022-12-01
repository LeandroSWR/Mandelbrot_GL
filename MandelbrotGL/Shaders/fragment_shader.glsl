#version 330 core
#extension GL_NV_gpu_shader_fp64: enable

uniform double windowAspect;

uniform float scale;
uniform int maxIterations;
uniform double centerX;
uniform double centerY;

varying vec2 pixelPosition;

out vec4 fragColor;

vec4 mapColor(float mcol)
{
    return vec4(0.5 + 0.5*cos(2.5+mcol*30.0 + vec3(1.0,0.5,0.0)),1.0);
}

// Mandelbrot Equation
f64vec2 complexMult(f64vec2 a, f64vec2 b)
{
	return f64vec2(a.x*b.x - a.y*b.y, a.x*b.y + a.y*b.x);
}

float iterateMandelbrot(f64vec2 coord)
{
	f64vec2 currentPoint = f64vec2(0,0);

	for (int i = 0; i < maxIterations; i++)
	{
		// Calculate the mandelbrot equation
		currentPoint = complexMult(currentPoint,currentPoint) + coord;

		// Calculate the dot of the vec2 over it self to check if the current pixel is outside of the set
        double ndot = dot(currentPoint,currentPoint);

		// If it's outside of the set
		if (ndot > 7.0)
		{
			// Smooth values for better color representation later
            float64_t sl = float64_t(i) - log2(log2(float(ndot)))+4.0;

			return float(sl * 0.0025);
		}
	}

	// If it's inside the set return 0.0 (black)
	return 0.0;
}

void main()
{
	f64vec2 fragment = f64vec2(
		windowAspect * pixelPosition.x * scale + centerX,
		pixelPosition.y * scale + centerY
	);

	;
	fragColor = mapColor(iterateMandelbrot(fragment));
} 