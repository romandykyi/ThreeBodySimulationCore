open PlotDrawer
open Plotly.NET

open ThreeBodySimulation.Data
open ThreeBodySimulation.Simulation
open ThreeBodySimulation.Simulation.Solvers

let G = 1

let body1Pos = BodyPosition(0.9700436, -0.24308753, 0)
let body1Vel = BodyPosition(0.466203685, 0.43236573, 2)
let body1Mass = 10

let body2Pos = BodyPosition(-0.9700436, 0.24308753, 0)
let body2Vel = BodyPosition(0.466203685, 0.43236573, 0)
let body2Mass = 10

let body3Pos = BodyPosition(0, 0, 0)
let body3Vel = BodyPosition(-0.93240737, -0.86473146, 0)
let body3Mass = 10

let startTime = 0
let endTime = 1.5

let step = 0.00001

let body1 = Body(body1Pos, body1Vel, body1Mass)
let body2 = Body(body2Pos, body2Vel, body2Mass)
let body3 = Body(body3Pos, body3Vel, body3Mass)

let solver = Yoshida4Solver() 
solver.Step <- step

let simulator = BodiesSimulator(body1, body2, body3, solver, G)

let options = { defaultSimPlotOptions with showCenterOfMass = true }
let chart = plotSim simulator startTime endTime options

chart |> Chart.show