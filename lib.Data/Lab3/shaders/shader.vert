#version 330 core
// the position variable has attribute position 0
layout (location = 0) in vec3 aPosition;

out vec3 ourColor;

uniform vec3 position;
uniform vec3 aColor;
uniform mat4 projection;
uniform mat4 model;
uniform mat4 view;
uniform mat4 transform;

void main()
{
    vec4 pos = vec4(position.x + aPosition.x, position.y + aPosition.y, position.z + aPosition.z, 1.0);
    gl_Position =  pos * transform * model * view * projection;
    ourColor = aColor;
}