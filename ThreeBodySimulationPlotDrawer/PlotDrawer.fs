module PlotDrawer

open ThreeBodySimulation.Simulation
open ThreeBodySimulation.Simulation.Utils
open Plotly.NET
open Plotly.NET.StyleParam

type SimPlotOptions = {
    drawCenterOfMass : bool
    showLastPosition : bool
    visualizationStep : double
}

let defaultSimPlotOptions = {
    drawCenterOfMass = false
    showLastPosition = true
    visualizationStep = 0.01
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

    Chart.combine [
        if options.drawCenterOfMass then 
            yield Chart.Line3D(
                xyz = centersOfMass, 
                Name="Center of Mass", 
                LineColor = Color.fromHex("#888888")
                ) |> 
                Chart.withLineStyle(Dash = StyleParam.DrawingStyle.Dot)
        
        yield Chart.Line3D(xyz = body1Positions, Name="Body 1")
        yield Chart.Line3D(xyz = body2Positions, Name="Body 2")
        yield Chart.Line3D(xyz = body3Positions, Name="Body 3")
    ]