open PlotDrawer
open Plotly.NET

open ThreeBodySimulation.Data
open ThreeBodySimulation.Simulation
open ThreeBodySimulation.Simulation.Solvers

let G = 1

let body1Pos = BodyPosition(4.5, 5, -3)
let body1Vel = BodyPosition(0, 0, 0)
let body1Mass = 10

let body2Pos = BodyPosition(-3, 0, 3)
let body2Vel = BodyPosition(0, 0, 0)
let body2Mass = 20

let body3Pos = BodyPosition(-6, -5, -5)
let body3Vel = BodyPosition(0, 0, 0)
let body3Mass = 30

let startTime = 0
let endTime = 100

let step = 0.0001

let body1 = Body(body1Pos, body1Vel, body1Mass)
let body2 = Body(body2Pos, body2Vel, body2Mass)
let body3 = Body(body3Pos, body3Vel, body3Mass)

let solver = Yoshida4BodiesSolver() 
solver.Step <- step

let simulator = BodiesSimulator(body1, body2, body3, solver, G)

let chart = plotSim simulator startTime endTime defaultSimPlotOptions

chart |> Chart.show