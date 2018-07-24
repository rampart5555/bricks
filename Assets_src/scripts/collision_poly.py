import math
"""
- {x: -0.47552827, y: 0.15450849}
- {x: -0.2938926, y: -0.40450853}
- {x: 0.29389268, y: -0.40450847}
- {x: 0.47552824, y: 0.15450856}
- {x: 0.47552824, y: 0.15450856}
- {x: 0.47552824, y: 0.15450856}
- {x: 0.47552824, y: 0.15450856}
"""      
start_angle=60.0
end_angle=120.0
steps=9
pas=(end_angle-start_angle)/steps
for i in range(1,steps):
    angle=math.pi/180.0*(start_angle+i*pas)
    x=math.cos(angle)
    y=math.sin(angle)
    print "- {{x: {0:8f}, y: {1:8f}}}".format(x,y)

