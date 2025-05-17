module PlotDrawer

open ThreeBodySimulation.Simulation
open ThreeBodySimulation.Simulation.Utils
open Plotly.NET
open Plotly.NET.StyleParam

type SimPlotOptions = {
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

    let n = body1Positions.Count


    Chart.combine [
        if options.showCenterOfMass then 
            let lastCenterOfMass = centersOfMass[n - 1]

            yield Chart.Line3D(
                xyz = centersOfMass, 
                Name="Center of Mass", 
                LineColor = options.centerOfMassColor
                ) |> 
                Chart.withLineStyle(Dash = StyleParam.DrawingStyle.Dot)

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
            let body1Pos = body1Positions[n - 1]
            let body2Pos = body2Positions[n - 1]
            let body3Pos = body3Positions[n - 1]
            
            // Adapt marker size to the mass
            let m1 = simulator.Body1.Mass
            let m2 = simulator.Body2.Mass
            let m3 = simulator.Body3.Mass

            let minMass = min (min m1 m2) m3
            let maxMass = max (max m1 m2) m3

            let t1 = (m1 - minMass) / (maxMass - minMass)
            let t2 = (m2 - minMass) / (maxMass - minMass)
            let t3 = (m3 - minMass) / (maxMass - minMass)

            let minMarkerSize = double options.minMarkerSize
            let maxMarkerSize = double options.maxMarkerSize
            let markerSize1 = int(round((1.0 - t1) * minMarkerSize + t1 * maxMarkerSize))
            let markerSize2 = int(round((1.0 - t2) * minMarkerSize + t2 * maxMarkerSize))
            let markerSize3 = int(round((1.0 - t3) * minMarkerSize + t3 * maxMarkerSize))

            yield Chart.Bubble3D(
                xyz = [body1Pos],
                sizes = [markerSize1],
                Name = $"Body 1 (mass = {m1})",
                MarkerColor = options.body1Color,
                ShowLegend = false
            )

            yield Chart.Bubble3D(
                xyz = [body2Pos],
                sizes = [markerSize2],
                Name = $"Body 2 (mass = {m2})",
                MarkerColor = options.body2Color,
                ShowLegend = false
            )

            yield Chart.Bubble3D(
                xyz = [body3Pos],
                sizes = [markerSize3],
                Name = $"Body 3 (mass = {m3})",
                MarkerColor = options.body3Color,
                ShowLegend = false
            )
    ]