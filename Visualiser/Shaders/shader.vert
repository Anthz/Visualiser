//[VERTEX SHADER]
#version 330
 
in vec4 InVertex;
in vec3 InNormal;
in vec4 InColor;

out vec4 PassColor;

uniform mat4 CameraMatrix, ModelMatrix;

void main()
{
	vec3 Normal = InNormal;
    gl_Position = CameraMatrix * ModelMatrix * InVertex;
    PassColor = InColor;
}
