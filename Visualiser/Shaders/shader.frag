#version 150 core
in Vector3 PassColor;
out vec4 OutColor;

void main(void)
{
	OutColor = vec4(PassColor, 1.0);  
}