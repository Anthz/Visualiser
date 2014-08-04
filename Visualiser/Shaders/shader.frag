//[FRAGMENT SHADER]
#version 330
 
smooth in vec4 PassColor;
 
out vec4 FragColor;

void main()
{
	FragColor = PassColor;
}
