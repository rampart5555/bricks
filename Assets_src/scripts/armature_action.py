import bpy
for obj in bpy.data.objects:
    obj.select=False
 
arm_obj = bpy.data.objects["Armature"]
arm_obj.select=True

frames=[
(0.0, (0.0, 0.0, 0.0)),
(15.0, (0.0, 0.5, 0.0)),
(45.0, (-0.5, 0.5, -3.0)),
(60.0, (-0.5, 0.0, -3.0))]

data_path = 'pose.bones["%s"].location' % "paddle_slot_2"
action = bpy.data.actions.new(name="paddle_slot_1_move")
arm_obj.animation_data.action = action
for axis_i in range(3):
    curve = action.fcurves.new(data_path=data_path, index=axis_i)
    keyframe_points = curve.keyframe_points
    num_frames = len(frames)
    keyframe_points.add(num_frames)
    for frame_i in range(num_frames):
        keyframe_points[frame_i].co = (frames[frame_i][0], frames[frame_i][1][axis_i])
for cu in action.fcurves:
    for bez in cu.keyframe_points:
        bez.interpolation = 'LINEAR'

