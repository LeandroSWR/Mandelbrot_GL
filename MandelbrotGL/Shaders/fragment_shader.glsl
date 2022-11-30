#version 330 core
#extension GL_ARB_gpu_shader_fp64: enable
#extension GL_NV_gpu_shader_fp64: enable

uniform double windowAspect;

uniform vec2 center;
uniform float scale;
uniform int maxIterations;

uniform double centerX;
uniform double centerY;

varying vec2 pixelPosition;

out vec4 fragColor;

vec4 mapColor(float64_t mcol) {
    return vec4(0.5 + 0.5*cos(2.7+float(mcol)*30.0 + vec3(1.0,0.5,0.0)),1.0);
}

f64vec2 complexMult(f64vec2 a, f64vec2 b) {
	return f64vec2(a.x*b.x - a.y*b.y, a.x*b.y + a.y*b.x);
}

float64_t iterateMandelbrot(f64vec2 coord)
{
	f64vec2 testPoint = f64vec2(0,0);

	for (int i = 0; i < maxIterations; i++){
		testPoint = complexMult(testPoint,testPoint) + coord;
        float64_t ndot = dot(testPoint,testPoint);
		if (ndot > 7.0) {
            float64_t sl = float64_t(i) - log2(log2(float(ndot)))+4.0;
			return sl*.0025;
		}
	}
	return 0.0;
}

void main()
{
	f64vec2 fragment = f64vec2(
		windowAspect * pixelPosition.x * scale + centerX,
		pixelPosition.y * scale + centerY
	);

	fragColor = mapColor(iterateMandelbrot(fragment));
} 