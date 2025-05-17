module PlotDrawer

open ThreeBodySimulation.Simulation
open ThreeBodySimulation.Simulation.Utils
open Plotly.NET

type SimPlotOptions = {
    showProgress : bool

    showCenterOfMass : bool
    showLastPosition : bool
    visualizationStep : double

    body1Color : Color
    body2Color : Color
    body3Color : Color
    
    minMarkerSize : int
    maxMarkerSize : int

    centerOfMassColor : Color
    centerOfMassMarkerSize : int
}

let defaultSimPlotOptions = {
    showProgress = false

    showCenterOfMass = true
    showLastPosition = true
    visualizationStep = 0.01

    body1Color = Color.fromHex "#0074D9"
    body2Color = Color.fromHex "#FF851B"
    body3Color = Color.fromHex "#2ECC40"

    minMarkerSize = 7
    maxMarkerSize = 15

    centerOfMassMarkerSize = 8
    centerOfMassColor = Color.fromHex "#888888"
}

let interpolateMarkerSize mass minMass maxMass minSize maxSize =
    if minMass = maxMass then (minSize + maxSize) / 2
    else
        let t = (mass - minMass) / (maxMass - minMass)
        int (round ((1.0 - t) * float minSize + t * float maxSize))

let createBodyBubble name pos size color mass =
    Chart.Bubble3D(
        xyz = [pos],
        sizes = [size],
        Name = $"{name} (mass = {mass})",
        MarkerColor = color,
        ShowLegend = false
    )

let plotSim (simulator : BodiesSimulator) startTime endTime (options : SimPlotOptions) = 
    let body1Positions = ResizeArray()
    let body2Positions = ResizeArray()
    let body3Positions = ResizeArray()
    let centersOfMass = ResizeArray()

    let mutable prevTime = -infinity

    for state in simulator.Simulate(startTime, endTime) do
        let timeStep = state.SimulationTime - prevTime
        if options.visualizationStep <= 0.0 || timeStep >= options.visualizationStep then
            let body1Pos = (state.Body1Position.X, state.Body1Position.Y, state.Body1Position.Z)
            let body2Pos = (state.Body2Position.X, state.Body2Position.Y, state.Body2Position.Z)
            let body3Pos = (state.Body3Position.X, state.Body3Position.Y, state.Body3Position.Z)

            body1Positions.Add body1Pos
            body2Positions.Add body2Pos
            body3Positions.Add body3Pos

            let com = SimulationUtils.CalculateCenterOfMass(
                simulator.Body1, 
                simulator.Body2, 
                simulator.Body3)  
            centersOfMass.Add (com.X, com.Y, com.Z)

            prevTime <- state.SimulationTime

            if options.showProgress then
                printf "\r%.2f/%.2f" state.SimulationTime endTime

                
    if options.showProgress then
         printfn "\r%.2f/%.2f" endTime endTime

    let n = body1Positions.Count
    let m1, m2, m3 = simulator.Body1.Mass, simulator.Body2.Mass, simulator.Body3.Mass
    let minMass = min (min m1 m2) m3
    let maxMass = max (max m1 m2) m3

    let size1 = interpolateMarkerSize m1 minMass maxMass options.minMarkerSize options.maxMarkerSize
    let size2 = interpolateMarkerSize m2 minMass maxMass options.minMarkerSize options.maxMarkerSize
    let size3 = interpolateMarkerSize m3 minMass maxMass options.minMarkerSize options.maxMarkerSize

    Chart.combine [
        if options.showCenterOfMass then 
            let lastCenterOfMass = centersOfMass[n - 1]

            yield Chart.Line3D(
                xyz = centersOfMass, 
                Name="Center of Mass", 
                LineColor = options.centerOfMassColor
                ) |> Chart.withLineStyle(Dash = StyleParam.DrawingStyle.Dot)

            if options.showLastPosition then
                yield Chart.Bubble3D(
                    xyz = [lastCenterOfMass],
                    sizes = [options.centerOfMassMarkerSize],
                    Name = "Center of Mass",
                    MarkerColor = options.centerOfMassColor,
                    ShowLegend = false)

        yield Chart.Line3D(xyz = body1Positions, Name = "Body 1",  LineColor = options.body1Color)
        yield Chart.Line3D(xyz = body2Positions, Name = "Body 2",  LineColor = options.body2Color)
        yield Chart.Line3D(xyz = body3Positions, Name = "Body 3",  LineColor = options.body3Color)

        if options.showLastPosition then
            yield createBodyBubble "Body 1" body1Positions[n - 1] size1 options.body1Color m1
            yield createBodyBubble "Body 2" body2Positions[n - 1] size2 options.body2Color m2
            yield createBodyBubble "Body 3" body3Positions[n - 1] size3 options.body3Color m3
    ]
