//[VERTEX SHADER]
#version 330

in vec2 InVertex;
in vec2 InTexCoords;

out vec2 TexCoords;
out vec4 PassColor;

uniform vec4 InColor;
uniform mat4 ProjectionMatrix;

void main()
{
	gl_Position = ProjectionMatrix * vec4(InVertex, 1.0, 1.0);
	TexCoords = InTexCoords;
	PassColor = InColor;
}