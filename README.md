# Three-body Problem Simulation

A cross-platform simulation and visualization of the classic Newtonian three-body problem, implemented in .NET.

## Overview

### Project Structure

The entire project consists of three main parts:

1. `ThreeBodySimulation` - the core project that targets .NET Standard 2.1. It contains the main simulation logic, ODE solvers and data structures. The .NET Standard 2.1 had been chosen to support Unity, but I didn't use this engine anyway.
2. `ThreeBodySimulation.Blazor` - the interactive three-body problem visualizer that uses Blazor and the [Plotly](https://plotly.com/javascript/) library.
3. `ThreeBodySimulationPlotDrawer` - an F# console project that asks users to enter the initial conditions and then draws the simulation plots using [Plotly.NET](https://plotly.net/). Allows export/import to JSON. It's generally faster then the Blazor visualizer, but doesn't support the actual visualization (because the library doesn't support this).

### Underlying Math

The program solves the three-body problem using a system of Newtonian equations of motion for vector positions ([source](https://en.wikipedia.org/wiki/Three-body_problem)).

![image](https://github.com/user-attachments/assets/158ba9fb-1801-413b-9e07-e12f004c7e0c)

This system is solved as a system of 18 first order scalar differential equations:

```
{
  // First body
  vx1,
  vy1,
  vz1,
  // Second body
  vx2,
  vy2,
  vz2,
  // Third body
  vx3,
  vy3,
  vz3, 
  
  // First body
  AxisMotionEquation(x1, x2, x3, d12, d13, m2, m3, g),
  AxisMotionEquation(y1, y2, y3, d12, d13, m2, m3, g),
  AxisMotionEquation(z1, z2, z3, d12, d13, m2, m3, g),
  
  // Second body
  AxisMotionEquation(x2, x3, x1, d23, d12, m3, m1, g),
  AxisMotionEquation(y2, y3, y1, d23, d12, m3, m1, g),
  AxisMotionEquation(z2, z3, z1, d23, d12, m3, m1, g),
  
  // Third body
  AxisMotionEquation(x3, x1, x2, d13, d23, m1, m2, g),
  AxisMotionEquation(y3, y1, y2, d13, d23, m1, m2, g),
  AxisMotionEquation(z3, z1, z2, d13, d23, m1, m2, g)
}

```

Where `d{i}{j}` is a distance between the body `i`th and `j`th, `v{x/y/z}{i}` is the `i`th body's velocity, `{x/y/z}{i}` is the `i`th body's position. 

The `AxisMotionEquation` is defined as so (`Cube(x)` is a function that returns `x*x*x`):

```
AxisMotionEquation(r1, r2, r3, d12, d13, m2, m3, g) => -g * m2 * (r1 - r2) / Cube(d12) - g * m3 * (r1 - r3) / Cube(d13);
```

The program has two fixed-step 4th order solvers implemented: Yoshida and Runge-Kutta.

## Sample Initial Conditions

Get the sample initial conditions [here](https://github.com/romandykyi/ThreeBodySimulationCore/blob/master/SampleInitialConditions.md).

## Local Setup Steps

### Initial

1. Make sure that you have [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (or later) installed.
2. Clone the repository: `git clone https://github.com/romandykyi/ThreeBodySimulationCore.git`.

### Running the Blazor Demo

1. Go to the `/ThreeBodySimulation.Blazor` folder (`cd ThreeBodySimulation.Blazor` from the root project directory).
2. Run this command: `dotnet run --configuration Release`.
3. Open http://localhost:5249 (or https://localhost:7150) in your browser.

### Running the Console Plot Drawer

1. Go to the `./ThreeBodySimulationPlotDrawer` folder (`cd ThreeBodySimulationPlotDrawer` from the root project directory).
2. Run this command: `dotnet run --configuration Release`.

## Using the Simulator in Your Code

If you're interested in writing your own visualization or in using the simulation in general, at first you need to link the `ThreeBodySimulation` project. For example, in Unity you can compile the `ThreeBodySimulation` project and include the `ThreeBodySimulation.dll` in the `Plugins` folder.

Here are some code snippets that may help you getting started:

### C# Version

```csharp
using ThreeBodySimulation.Data;
using ThreeBodySimulation.Simulation;
using ThreeBodySimulation.Simulation.Solvers;

// Step-size of the ODE solver
const double stepSize = 0.0001;

// Initialize the solver
IBodiesSolver solver = new RK4Solver() { Step = stepSize };
// Alternatively:
//IBodiesSolver solver = new Yoshida4Solver() { Step = stepSize };

// Define the parameters:
const double StartTime = 0.0; // The start time of the simulation
const double EndTime = 5.0; // The end time of the simulation
const double G = 1.0; // Gravitational constant

// First body parameters:
Body body1 = new()
{
    Position = new BodyPosition(5, 0, 0),
    Velocity = BodyPosition.Zero,
    Mass = 1
};
// Second body parameters:
Body body2 = new()
{
    Position = new BodyPosition(0, 0, -5),
    Velocity = BodyPosition.Zero,
    Mass = 1
};
// Third body parameters:
Body body3 = new()
{
    Position = new BodyPosition(0, 0, 0),
    Velocity = BodyPosition.Zero,
    Mass = 2
};

// Initialize the simulator
BodiesSimulator simulator = new(body1, body2, body3, solver, G);

// Iterates each simulation state (useful for visualizations).
// The Simulate method uses lazy loading and states are not preserved
// unless you store them
foreach (SimulationState state in simulator.Simulate(StartTime, EndTime))
{
    // Do something with the state (e.g. print the first body position)
    var pos1 = state.Body1Position;
    Console.WriteLine($"{state.SimulationTime}: {pos1.X} {pos1.Y} {pos1.Z}");
}

// Saves all states in the array (useful for analysis)
List<SimulationState> states = simulator.Simulate(StartTime, EndTime).ToList();
```

### F# Version

```fsharp
open ThreeBodySimulation.Data
open ThreeBodySimulation.Simulation
open ThreeBodySimulation.Simulation.Solvers

// Step-size of the ODE solver
let stepSize = 0.0001

// Initialize the solver
let solver = RK4Solver(Step = stepSize)
    // or use: Yoshida4Solver(Step = stepSize)

// Define the parameters
let startTime = 0.0
let endTime = 5.0
let G = 1.0

// First body parameters
let body1 = Body(
    Position = BodyPosition(5.0, 0.0, 0.0),
    Velocity = BodyPosition.Zero,
    Mass = 1.0
)

// Second body parameters
let body2 = Body(
    Position = BodyPosition(0.0, 0.0, -5.0),
    Velocity = BodyPosition.Zero,
    Mass = 1.0
)

// Third body parameters
let body3 = Body(
    Position = BodyPosition(0.0, 0.0, 0.0),
    Velocity = BodyPosition.Zero,
    Mass = 2.0
)

// Initialize the simulator
let simulator = BodiesSimulator(body1, body2, body3, solver, G)

// Iterate each simulation state (useful for visualizations)
for state in simulator.Simulate(startTime, endTime) do
    let pos1 = state.Body1Position
    printfn "%f: %f %f %f" state.SimulationTime pos1.X pos1.Y pos1.Z

// Saves all states to a list (useful for analysis)
let states = simulator.Simulate(startTime, endTime) |> Seq.toList
```
