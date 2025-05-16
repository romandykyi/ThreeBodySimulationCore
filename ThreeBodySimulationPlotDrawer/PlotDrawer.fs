module PlotDrawer

open ThreeBodySimulation.Simulation
open Plotly.NET

type SimPlotOptions = {
    drawCenterOfMass : bool
    visualizationStep : double
}

let defaultSimPlotOptions = {
    drawCenterOfMass = false
    visualizationStep = 0.01
}

let plotSim (simulator : BodiesSimulator) startTime endTime (options : SimPlotOptions) = 
    let body1Positions = ResizeArray()
    let body2Positions = ResizeArray()
    let body3Positions = ResizeArray()

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

            prevTime <- state.SimulationTime

    Chart.combine [
        // TODO: implement center of mass drawing
        if options.drawCenterOfMass then 
            yield Chart.Line3D(xyz = [], Name="Center of Mass")
        
        yield Chart.Line3D(xyz = body1Positions, Name="Body 1")
        yield Chart.Line3D(xyz = body2Positions, Name="Body 2")
        yield Chart.Line3D(xyz = body3Positions, Name="Body 3")
    ]