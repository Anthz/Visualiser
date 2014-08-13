//[VERTEX SHADER]
#version 330
 
in vec4 InVertex;
in vec3 InNormal;

out vec4 PassColor;

uniform vec4 InColor;
uniform mat4 ProjectionMatrix, ViewMatrix, ModelMatrix;

void main()
{
	vec3 Normal = InNormal;
    gl_Position = ProjectionMatrix * ViewMatrix * ModelMatrix * InVertex;
    PassColor = InColor;
}
