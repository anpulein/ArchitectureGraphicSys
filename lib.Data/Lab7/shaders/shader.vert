#version 330 core
// the position variable has attribute position 0

layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec2 vTexCoord;
layout (location = 2) in vec3 vNormal;

out vec3 position;
out vec3 normal;
out vec2 texCoord;

uniform mat4 projection;
uniform mat4 modelViewMatrix;

void main()
{
    position = vec3(modelViewMatrix * vec4(vPosition, 1));
    normal = vec3(modelViewMatrix * vec4 (vNormal, 0));
    texCoord = vec2(vTexCoord.s, vTexCoord.t);

    gl_Position = projection * modelViewMatrix * vec4(vPosition, 1.0f);
}