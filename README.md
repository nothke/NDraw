# NDraw
Runtime line drawing utility for Unity.

## Features:
1. Draws pixel perfect lines at runtime;
2. Very similar in usage to DrawGizmos and Debug.Draw- functions;
3. Can be called from anywhere in the code, and they will be automatically added
4. Can draw both in screenspace and worldspace through its Screen and World subclasses;
5. Simple code that can be easily extended;
6. Many available shapes that keep being added as I work on projects;
7. All of those shapes are also optional and can be freely removed from the project if not needed.

## How to use?
Using the functions follows a simple form: 

#### `Draw.<Where?>.<What?>()`

The Where? is either Screen or World. Screen will draw things in pixels on screen, while World will draw lines in worldspace in world units.

The What? is any set of available drawing functions, like those for drawing lines, circles, rects, cubes, etc.

It is very important to add a `Drawer` script to your main camera as the functions will not be drawn without it.

You can call your functions from anywhere and all the lines, shapes or polygons you called during one frame will be drawn at the end of the rendering frame.

To change the color, use one of the SetColor functions, for example `Draw.Screen.SetColor(Color);`. Note that each space has it's own color cache.

## Example
First, __add a Drawer component to your main camera__. This is very important and the functions will not work without it.

Then, in your script, __include `using NDraw;`__

Drawing a diagonal line from one corner of the screen to the other:

`Draw.Screen.Line(0, 0, Screen.width, Screen.height);`

Draw a ray in world-space from origin and up one meter:

`Draw.World.Ray(Vector3.zero, Vector3.up);`

See NDrawExample.cs for other examples.

## Optional components
The necessary components are __Drawer.cs__, which is the MonoBehaviour that draws the lines at the end of the frame, and __DrawCore.cs__, which contains the point and color caches used by the Drawer. Everything else is optional, for example if you just want to draw lines and rays you can keep the DrawLines.cs and remove all other scripts from the project.

## Extending
The whole library is a set of partial static classes and new drawing functions can be very easily added in separate files. But make sure to put `if (!Drawer.Exists) return;` on top of your methods since it may cause a memory leak if the Drawer doesn't exist in the scene. For examples, just take a look at Draw classes like `DrawShapes.cs`
