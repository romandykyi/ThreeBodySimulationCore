open PlotDrawer
open Preset

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

let rec promptSolverMethod () : IFixedStepBodiesSolver =
    let input = Console.ReadLine()

    match input with
    | "yoshida4" -> Yoshida4Solver()
    | "rk4" -> RK4Solver()
    | _ -> promptSolverMethod()

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

let promptSolver () = 
    printf "Enter solver (yoshida4/rk4): "
    let solver = promptSolverMethod()

    printf "Enter simulation step size: "
    let step = promptPositiveNumber()
    solver.Step <- step

    solver

let printVector (v : BodyPosition) =
    printfn $"{v.X} {v.Y} {v.Z}"
    
let printBody (index, body : Body) = 
    printfn ""
    printfn $"Body #{index}"

    printf("Position: ")
    printVector body.Position
    printf("Velocity: ")
    printVector body.Velocity
    printf("Mass: ")
    printfn $"{body.Mass}"

let printSimulation (sim : BodiesSimulator) =
    printfn $"Gravitational constant (G): {sim.G}"

    printBody(1, sim.Body1)
    printBody(2, sim.Body2)
    printBody(3, sim.Body3)
    printfn ""

let rec promptSimulation solver =
    printf "Input preset path (leave empty to enter data manually): "
    let inputPath = Console.ReadLine()
    
    if String.IsNullOrWhiteSpace inputPath then
        printfn ""
        printf "Enter gravitational constant (G): "
        let G = promptPositiveNumber()

        let body1 = promptBody 1
        let body2 = promptBody 2
        let body3 = promptBody 3
        printfn ""
        printfn ""

        BodiesSimulator(body1, body2, body3, solver, G)
    else
        match loadPreset inputPath with
        | Ok preset -> 
            printfn "Preset loaded successfully:"
            let simulator = BodiesSimulator(preset.Body1, preset.Body2, preset.Body3, solver, preset.G)
            printSimulation simulator
            simulator
        | Error msg -> 
            fprintfn stderr $"{msg}"
            promptSimulation solver

let copyBody (body : Body) = Body(body.Position, body.Velocity, body.Mass)

let simToPreset (sim : BodiesSimulator) = {
    Body1 = copyBody(sim.Body1)
    Body2 = copyBody(sim.Body2)
    Body3 = copyBody(sim.Body3)
    G = sim.G
}

let rec promptPresetSave preset =
    printf "Output preset path (leave empty to not save): "
    let outputPath = Console.ReadLine()
    
    if not(String.IsNullOrWhiteSpace outputPath) then
        match savePreset outputPath preset with
        | Ok () -> printfn "Preset saved successfully."
        | Error msg -> 
            printfn "Error saving preset: %s" msg
            promptPresetSave preset

let solver = promptSolver()
printfn ""

let sim = promptSimulation solver
printfn ""

let preset = simToPreset sim

printf "Enter simulation time: "
let simTime = promptPositiveNumber()
printfn ""

let options = { defaultSimPlotOptions with showProgress = true }
let chart = plotSim sim 0.0 simTime options
printfn ""
printfn "Plotting..."

chart |> Chart.show
printfn "Done"
printfn ""

promptPresetSave preset
