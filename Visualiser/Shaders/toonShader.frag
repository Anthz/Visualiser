//[FRAGMENT SHADER]
#version 330
 
in vec4 PassColor;
 
out vec4 FragColor;

varying float intensity;

void main()
{
	vec4 color = PassColor;
	if(intensity > 0.95)
		color = PassColor;
	else if(intensity > 0.5)
		color = vec4(color.x * 0.75, color.y * 0.75, color.z * 0.75, color.w);
	else if(intensity > 0.25)
		color = vec4(color.x * 0.5, color.y * 0.5, color.z * 0.5, color.w);
	else
		color = vec4(color.x * 0.25, color.y * 0.25, color.z * 0.25, color.w);

	FragColor = color;
}
