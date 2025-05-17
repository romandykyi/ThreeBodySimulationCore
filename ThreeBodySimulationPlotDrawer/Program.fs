open PlotDrawer
open Plotly.NET

open ThreeBodySimulation.Data
open ThreeBodySimulation.Simulation
open ThreeBodySimulation.Simulation.Solvers

open System
open System.Globalization

let rec promptNumber () =
    match Double.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture) with
    | true, value -> value
    | false, _ -> promptNumber()

let rec promptPositiveNumber () =
    let result = promptNumber()
    if result > 0 then
        result
    else
        promptPositiveNumber()

let rec promptThreeNumbers () =
    let input = Console.ReadLine()
    let parts = input.Split([| ' ' |], StringSplitOptions.RemoveEmptyEntries)
    
    if parts.Length = 3 then
        match 
            Double.TryParse(parts.[0], CultureInfo.InvariantCulture), 
            Double.TryParse(parts.[1], CultureInfo.InvariantCulture), 
            Double.TryParse(parts.[2], CultureInfo.InvariantCulture) 
        with
        | (true, a), (true, b), (true, c) -> BodyPosition(a, b, c)
        | _ -> promptThreeNumbers ()
    else
        promptThreeNumbers ()

let rec promptSolver () : IFixedStepBodiesSolver =
    let input = Console.ReadLine()

    match input with
    | "yoshida4" -> Yoshida4Solver()
    | "rk4" -> RK4Solver()
    | _ -> promptSolver()

let rec promptBody (index: int) =
    printfn ""
    printfn $"Body #{index}"
    printf $"Enter position (x y z) for body #{index}: "
    let pos = promptThreeNumbers()
    printf $"Enter velocity (vx vy vz) for body #{index}: "
    let vel = promptThreeNumbers()
    printf $"Enter mass for body #{index}: "
    let mass = promptPositiveNumber()
    Body(pos, vel, mass)

let rec promptSimulation () =
    printf "Enter solver (yoshida4/rk4): "
    let solver = promptSolver()

    printf "Enter gravitational constant G: "
    let G = promptPositiveNumber()

    let body1 = promptBody 1
    let body2 = promptBody 2
    let body3 = promptBody 3
    printfn ""

    printf "Enter simulation step size: "
    let step = promptPositiveNumber()
    solver.Step <- step
    
    printfn ""

    BodiesSimulator(body1, body2, body3, solver, G)

let sim = promptSimulation()

printf "Enter simulation time: "
let simTime = promptPositiveNumber()
printfn ""

let options = { defaultSimPlotOptions with showProgress = true }
let chart = plotSim sim 0.0 simTime options
printfn ""
printfn "Plotting..."

chart |> Chart.show
printfn "Done"