#version 330 core
out vec4 FragColor;
in vec3 ourPosition;

uniform vec3 red_color;
uniform vec3 green_color;
uniform vec3 blue_color;

vec3 editColor(vec3 position) {
    if ((position.y + 0.5f) > (2.0f / 3.0f)) return red_color;
    else if ((position.y + 0.5f) > (1.0f / 3.0f)) return green_color;
    
    return blue_color;
}

void main()
{
    FragColor = vec4(editColor(ourPosition), 0.0f);
}