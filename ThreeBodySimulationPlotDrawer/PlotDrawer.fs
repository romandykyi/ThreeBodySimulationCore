module PlotDrawer

open ThreeBodySimulation.Simulation
open Plotly.NET

type SimPlotOptions = {
    drawCenterOfMass : bool
}

let defaultSimPlotOptions = {
    drawCenterOfMass = false
}

let plotSim (simulator : BodiesSimulator) startTime endTime (options : SimPlotOptions) = 
    let body1Positions = ResizeArray()
    let body2Positions = ResizeArray()
    let body3Positions = ResizeArray()

    for state in simulator.Simulate(startTime, endTime) do
        body1Positions.Add (state.Body1Position.X, state.Body1Position.Y, state.Body1Position.Z)
        body2Positions.Add (state.Body2Position.X, state.Body2Position.Y, state.Body2Position.Z)
        body3Positions.Add (state.Body3Position.X, state.Body3Position.Y, state.Body3Position.Z)

    Chart.combine [
        // TODO: implement center of mass drawing
        if options.drawCenterOfMass then 
            yield Chart.Line3D(xyz = [], Name="Center of Mass")
        
        yield Chart.Line3D(xyz = body1Positions, Name="Body 1")
        yield Chart.Line3D(xyz = body2Positions, Name="Body 2")
        yield Chart.Line3D(xyz = body3Positions, Name="Body 3")
    ]