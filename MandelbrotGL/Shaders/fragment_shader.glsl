#version 330 core

uniform float windowAspect;

uniform vec2 center;
uniform float scale;
uniform int maxIterations;

varying vec2 pixelPosition;

out vec4 fragColor;

vec4 mapColor(float mcol) {
    return vec4(0.5 + 0.5*cos(2.7+mcol*30.0 + vec3(0.0,.6,1.0)),1.0);
}

vec2 complexMult(vec2 a, vec2 b) {
	return vec2(a.x*b.x - a.y*b.y, a.x*b.y + a.y*b.x);
}

float iterateMandelbrot(vec2 coord)
{
	vec2 testPoint = vec2(0,0);

	for (int i = 0; i < maxIterations; i++){
		testPoint = complexMult(testPoint,testPoint) + coord;
        float ndot = dot(testPoint,testPoint);
		if (ndot > 7.0) {
            float sl = float(i) - log2(log2(ndot))+4.0;
			return sl*.0025;
		}
	}
	return 0.0;
}

void main()
{
	vec2 fragment = vec2(
		windowAspect * pixelPosition.x * scale + center.x,
		pixelPosition.y * scale + center.y
	);

	const vec2 zoomP = vec2(-.7451544,.1853);

	vec4 outs = vec4(0.0);

	fragColor = mapColor(iterateMandelbrot(fragment));
} 