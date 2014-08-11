//[VERTEX SHADER]
#version 330

in vec4 InVertex;
in vec3 InNormal;
in vec4 InColor;

out vec4 PassColor;

varying float intensity;

uniform vec3 lightDir;
uniform mat4 ProjectionMatrix, ViewMatrix, ModelMatrix;

void main()
{
    gl_Position = ProjectionMatrix * ViewMatrix * ModelMatrix * InVertex;
    PassColor = InColor;
	intensity = dot(lightDir, InNormal);
}
