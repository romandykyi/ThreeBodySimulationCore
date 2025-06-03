# Sample Initial Conditions

Here are some interesting initial conditions in a JSON format that can be pasted into the Blazor visualizer. 

### Set 1 (default Blazor visualizer parameters)

![newplot](https://github.com/user-attachments/assets/12ab7b5e-e0ab-4711-ad4c-e280494d9c04)

```json
{
  "Solver": 0,
  "G": 1,
  "StepSize": 0.0005,
  "SimulationTime": 3,
  "Body1": {
    "Position": {
      "X": -0.60288589811652,
      "Y": 0.059162128863347,
      "Z": 0
    },
    "Velocity": {
      "X": 0.122913546623784,
      "Y": 0.747443868604908,
      "Z": 0
    },
    "Mass": 1
  },
  "Body2": {
    "Position": {
      "X": 0.252709795391,
      "Y": 0.105825487222437,
      "Z": 0
    },
    "Velocity": {
      "X": -0.019325586404545,
      "Y": 1.369241993562101,
      "Z": 0
    },
    "Mass": 1
  },
  "Body3": {
    "Position": {
      "X": -0.355389016941814,
      "Y": 0.1038323764315145,
      "Z": 0
    },
    "Velocity": {
      "X": -0.103587960218793,
      "Y": -2.11668586216882,
      "Z": 0
    },
    "Mass": 1
  }
}
```

Source: https://observablehq.com/@rreusser/periodic-planar-three-body-orbits

### Set 2

![newplot (1)](https://github.com/user-attachments/assets/03022c0c-0147-407f-9f22-f33575476d7d)

```json
{
  "Solver": 0,
  "G": 1,
  "StepSize": 0.0005,
  "SimulationTime": 8,
  "Body1": {
    "Position": {
      "X": 0.517216786720872,
      "Y": 0.55610033157918,
      "Z": 0
    },
    "Velocity": {
      "X": 0.107632564012758,
      "Y": 0.681725256843756,
      "Z": 0
    },
    "Mass": 1
  },
  "Body2": {
    "Position": {
      "X": 0.002573889407142,
      "Y": 0.116484954113653,
      "Z": 0
    },
    "Velocity": {
      "X": -0.534918980283418,
      "Y": -0.854885322576851,
      "Z": 0
    },
    "Mass": 1
  },
  "Body3": {
    "Position": {
      "X": -0.20255534902211,
      "Y": -0.731794952123173,
      "Z": 0
    },
    "Velocity": {
      "X": 0.427286416269208,
      "Y": 0.173160065733631,
      "Z": 0
    },
    "Mass": 1
  }
}
```

Source: https://observablehq.com/@rreusser/periodic-planar-three-body-orbits

### Set 3

Notes: the center of mass moves along Z axis because the initial velocity of one of the bodies is (0, 0, 1).

![newplot (2)](https://github.com/user-attachments/assets/d16bc940-1f6f-44ca-9b2e-49b2c3a54916)

```json
{
  "Solver": 0,
  "G": 1,
  "StepSize": 0.0005,
  "SimulationTime": 15,
  "Body1": {
    "Position": {
      "X": 0,
      "Y": 0,
      "Z": 0
    },
    "Velocity": {
      "X": 0,
      "Y": 0,
      "Z": 0
    },
    "Mass": 1
  },
  "Body2": {
    "Position": {
      "X": -2,
      "Y": 0,
      "Z": 0
    },
    "Velocity": {
      "X": 0,
      "Y": 0,
      "Z": 0
    },
    "Mass": 1
  },
  "Body3": {
    "Position": {
      "X": -2,
      "Y": 2,
      "Z": 0
    },
    "Velocity": {
      "X": 0,
      "Y": 0,
      "Z": 1
    },
    "Mass": 1
  }
}
```
