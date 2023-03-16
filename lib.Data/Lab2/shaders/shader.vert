#version 330 core
// the position variable has attribute position 0
layout (location = 0) in vec3 aPosition;
// the color variable has attribute position 1
layout (location = 1) in vec3 aColor;

// Output a position to the fragment shader
out vec3 ourPosition;

uniform vec3 offset;

void main()
{
    gl_Position = vec4(aPosition + offset, 1.0);
    ourPosition = aPosition;
}