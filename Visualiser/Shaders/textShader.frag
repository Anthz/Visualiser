//[FRAGMENT SHADER]
#version 330

uniform sampler2D Texture;

in vec2 TexCoords;
in vec4 PassColor;

out vec4 FragColor;

void main()
{
	vec4 TexColor = texture2D(Texture, TexCoords);

	if(TexColor == vec4(0.0f, 0.0f, 0.0f, 1.0f))
	{
		TexColor = vec4(0.0f, 0.0f, 0.0f, 0.0f);
	}

	FragColor = TexColor * PassColor;
}