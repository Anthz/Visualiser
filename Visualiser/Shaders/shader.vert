#version 150 core
in Vector3 InVertex;
in Vector3 InColor;
out Vector3 PassColor;

void main(void)
{
	gl_Position = vec4(InVertex, 1.0);
	PassColor = InColor;
}  