Tools:
Blender 2.79b
Unity 2017.4.2f2
In blender
- CTRL+1 to switch to back view
- If the mesh has Armature ensure that bones has roll of 0 (in edit mode)
- Bone orientation head->tail on +Z axis
- For meshes without armature, in unity -> reset transform and rotate on Y axe with 180 degree
- For meshes with armature, in unity-> reset rotation transform for all bones and set rotation for Armature on Z axe with -90 degree
- Save as blender file in Prefabs

Exporting FBX
- CTRL+1 to switch to back view
- If the mesh has Armature ensure that bones has roll of 0 (in edit mode)
- Bone orientation head->tail on +Z axis
- In the main section of FBX menu check button in the right of Scale 
- Apply scale should be FBX all
- Axis 
    Forward : -Z Forward
     Up:     Y Yp


