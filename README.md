# Zwerg - Distance Field Editor

by movAX13h (filip.sound@gmail.com)

http://blog.thrill-project.com/zwerg-distance-field-editor/

![ScreenShot](http://blog.thrill-project.com/wp-content/uploads/2014/10/screenshot.png)

Feel free to fork and modify.

## Changelog

- **1.4** - 2016-06-05
  - renamed nodes.xml to functions.xml (format extended), contains defines, helpers and all code nodes
  - nodes can have a 'comment' attribute now which is displayed in the properties panel when the node is selected
  - function tags can have a 'requires' attribute now (name of other node or helper) to maintain code dependencies
  - Zwerg writes only required functions to the shader now (preparation for minify features)
  - renamed raymarcher.fs to shader.fs, moved optional code to functions.xml
  - integrated most operators of the [Mercury SDF Library](http://mercury.sexy/hg_sdf/)
  - **PLEASE NOTE: Scene files created with earlier versions won't load because SDF function definitions have changed! The scene file format itself has not changed.**
  
- **1.3** - 2014-06-15: external nodes.xml (contains all available nodes from scene view context menu), simplified internal tree structure and code cleanup, fixed: node order
- **1.2** - 2014-06-14: fixed mouse sensitivity issues
- **1.1** - 2014-06-10: mouse click selection of df objects + minor fixes

## Compiling
- C# .NET4.5, VisualStudio 12 (2013) solution
- OpenGL via OpenTK (find dlls in bin/Debug)

## Download
- Binary (v1.4) from author server: http://thrill-project.com/archiv/coding/Zwerg14.zip (760kb)

## Features
- distance functions (primitives)
- distance operations
- domain operations
- scene view
- realtime preview/camera/light
- properties panel
- select model on mouse click (experimental)
- delete node (del key and context menu)
- save/load scene to/from file (xml)

## To do
- better scene treeview
- export shader code (remove all Zwerg specific code)
- minimize output shader code (http://www.pouet.net/prod.php?which=55176)
- animation
- materials

## Thanks
- Shadertoy community, esp. iq
- demoscene community (http://www.pouet.net)
