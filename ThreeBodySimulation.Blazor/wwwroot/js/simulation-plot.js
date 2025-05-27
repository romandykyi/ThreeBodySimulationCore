function renderThreeBodyAnimation3D(simulationResult) {
    const frames = simulationResult.simulationFrames;
    const interval = simulationResult.interval * 1000; // seconds -> ms

    // Calculate bounds
    const allX = [], allY = [], allZ = [];

    frames.forEach(f => {
        [f.body1, f.body2, f.body3].forEach(b => {
            allX.push(b.x);
            allY.push(b.y);
            allZ.push(b.z);
        });
    });

    const min = arr => Math.min(...arr);
    const max = arr => Math.max(...arr);

    const margin = 1;
    const xRange = [min(allX) - margin, max(allX) + margin];
    const yRange = [min(allY) - margin, max(allY) + margin];
    const zRange = [min(allZ) - margin, max(allZ) + margin];

    const body1Trace = {
        x: [],
        y: [],
        z: [],
        mode: 'markers',
        type: 'scatter3d',
        name: 'Body 1',
        marker: { size: 5, color: '#0074D9' }
    };

    const body2Trace = {
        x: [],
        y: [],
        z: [],
        mode: 'markers',
        type: 'scatter3d',
        name: 'Body 2',
        marker: { size: 5, color: '#FF851B' }
    };

    const body3Trace = {
        x: [],
        y: [],
        z: [],
        mode: 'markers',
        type: 'scatter3d',
        name: 'Body 3',
        marker: { size: 5, color: '#2ECC40' }
    };

    const centerOfMassTrace = {
        x: [],
        y: [],
        z: [],
        mode: 'markers',
        type: 'scatter3d',
        name: 'Center of Mass',
        marker: { size: 3, color: '#888888' }
    };

    const animationFrames = frames.map((frame, index) => {
        return {
            name: index.toString(),
            data: [
                {
                    x: [frame.body1.x],
                    y: [frame.body1.y],
                    z: [frame.body1.z]
                },
                {
                    x: [frame.body2.x],
                    y: [frame.body2.y],
                    z: [frame.body2.z]
                },
                {
                    x: [frame.body3.x],
                    y: [frame.body3.y],
                    z: [frame.body3.z]
                },
                {
                    x: [frame.centerOfMass.x],
                    y: [frame.centerOfMass.y],
                    z: [frame.centerOfMass.z]
                }
            ]
        };
    });

    const layout = {
        title: 'Three-Body Simulation',
        scene: {
            xaxis: { title: 'X', range: xRange },
            yaxis: { title: 'Y', range: yRange },
            zaxis: { title: 'Z', range: zRange },
            aspectmode: 'manual',
            aspectratio: {
                x: 1,
                y: 1,
                z: 1
            }
        },
        updatemenus: [{
            type: 'buttons',
            showactive: false,
            buttons: [
                {
                    label: 'Play',
                    method: 'animate',
                    args: [null, {
                        fromcurrent: true,
                        frame: { duration: interval, redraw: true },
                        transition: { duration: 0 }
                    }]
                },
                {
                    label: 'Pause',
                    method: 'animate',
                    args: [[null], {
                        mode: 'immediate',
                        frame: { duration: 0, redraw: false },
                        transition: { duration: 0 }
                    }]
                }
            ]
        }],
        margin: { l: 0, r: 0, b: 0, t: 30 }
    };

    Plotly.newPlot('three-body-plot', [body1Trace, body2Trace, body3Trace, centerOfMassTrace], layout)
        .then(() => {
            Plotly.addFrames('three-body-plot', animationFrames);
        });
}