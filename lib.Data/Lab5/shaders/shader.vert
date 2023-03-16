#version 330 core
// the position variable has attribute position 0
layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec3 vNormal;
layout (location = 2) in vec2 vTexCoord;

out vec4 ourColor;

//uniform vec3 position;
uniform vec4 aColor;
uniform mat4 projection;
uniform mat4 modelViewMatrix;

void main()
{
    gl_Position =  vec4(vPosition, 1.0f) * modelViewMatrix * projection;
    ourColor = aColor;
}