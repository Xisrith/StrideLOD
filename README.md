# StrideLOD
Working on a new LOD system for the Stride 3D game engine. Note, this repo is very volatile.

## Setup and Launching
Before launching you will want to change the location of the log file.
This can be done in code at FirstPersonShooter.Profiling.LoggingWriter
or in the editor under (Scene) TestScene > (Entity) Profiler > (Component) Logging Writer > (Property) Filename.
The log file will be created and populated automatically as a SQLite database file.

You can run the project from VS with the following command line options:

- `case:` - Which version of LOD to initialize, options are `lod_generic`, `lod_model`, and `static_x` where "x" is the desired number of triangles per sphere (20, 80, 320, 1080, 5120).
- `height:` - How many spheres to create in the Y direction
- `length:` - How many spheres to create in the Z direction
- `width:` - How many spheres to create in the X direction

Example arguments: `FirstPersonShooter.Windows.exe case:lod_generic height:3 lenght:10 width:10`

Upon launch, the scene will be populated from the given arguments. This behavior is controlled by the FirstPersonShooter.TestFactory class.
