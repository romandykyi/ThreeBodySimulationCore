function renderThreeBodyAnimation3D(plotId, simulationResult) {
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

    // Initialize trail traces with empty arrays and mode 'lines'
    const body1Trail = {
        x: [],
        y: [],
        z: [],
        showlegend: false,
        mode: 'lines',
        type: 'scatter3d',
        name: 'Body 1 Trail',
        line: { color: '#0074D9', width: 2 }
    };

    const body2Trail = {
        x: [],
        y: [],
        z: [],
        showlegend: false,
        mode: 'lines',
        type: 'scatter3d',
        name: 'Body 2 Trail',
        line: { color: '#FF851B', width: 2 }
    };

    const body3Trail = {
        x: [],
        y: [],
        z: [],
        showlegend: false,
        mode: 'lines',
        type: 'scatter3d',
        name: 'Body 3 Trail',
        line: { color: '#2ECC40', width: 2 }
    };

    // Markers for bodies
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
        marker: {
            size: 6,
            color: '#888888'
        }
    };
    const centerOfMassTrail = {
        x: [],
        y: [],
        z: [],
        showlegend: false,
        mode: 'lines',
        type: 'scatter3d',
        name: 'Center of Mass Trail',
        line: { color: '#888888', width: 2, dash: 'dot' }  // dotted line style for COM trail
    };

    // Prepare frames for animation
    const animationFrames = frames.map((frame, index) => {
        // Trails accumulate all previous positions up to current frame
        const trailX1 = frames.slice(0, index + 1).map(f => f.body1.x);
        const trailY1 = frames.slice(0, index + 1).map(f => f.body1.y);
        const trailZ1 = frames.slice(0, index + 1).map(f => f.body1.z);

        const trailX2 = frames.slice(0, index + 1).map(f => f.body2.x);
        const trailY2 = frames.slice(0, index + 1).map(f => f.body2.y);
        const trailZ2 = frames.slice(0, index + 1).map(f => f.body2.z);

        const trailX3 = frames.slice(0, index + 1).map(f => f.body3.x);
        const trailY3 = frames.slice(0, index + 1).map(f => f.body3.y);
        const trailZ3 = frames.slice(0, index + 1).map(f => f.body3.z);

        const trailXCOM = frames.slice(0, index + 1).map(f => f.centerOfMass.x);
        const trailYCOM = frames.slice(0, index + 1).map(f => f.centerOfMass.y);
        const trailZCOM = frames.slice(0, index + 1).map(f => f.centerOfMass.z);

        return {
            name: index.toString(),
            data: [
                // Body 1 marker
                {
                    x: [frame.body1.x],
                    y: [frame.body1.y],
                    z: [frame.body1.z]
                },
                // Body 2 marker
                {
                    x: [frame.body2.x],
                    y: [frame.body2.y],
                    z: [frame.body2.z]
                },
                // Body 3 marker
                {
                    x: [frame.body3.x],
                    y: [frame.body3.y],
                    z: [frame.body3.z]
                },
                // Center of Mass marker
                {
                    x: [frame.centerOfMass.x],
                    y: [frame.centerOfMass.y],
                    z: [frame.centerOfMass.z]
                },
                // Body 1 trail line
                {
                    x: trailX1,
                    y: trailY1,
                    z: trailZ1
                },
                // Body 2 trail line
                {
                    x: trailX2,
                    y: trailY2,
                    z: trailZ2
                },
                // Body 3 trail line
                {
                    x: trailX3,
                    y: trailY3,
                    z: trailZ3
                },
                // Center of Mass trail line
                {
                    x: trailXCOM,
                    y: trailYCOM,
                    z: trailZCOM
                }
            ]
        };
    });

    const stepTime = 0.1;
    const sliderSteps = [];

    let nextStepTime = 0;
    for (let i = 0; i < frames.length; i++) {
        const frameTime = frames[i].time;

        if (frameTime >= nextStepTime) {
            sliderSteps.push({
                method: 'animate',
                label: frameTime.toFixed(1),
                args: [[i.toString()], {
                    mode: 'immediate',
                    frame: { duration: 0, redraw: true },
                    transition: { duration: 0 }
                }]
            });

            nextStepTime += stepTime;
        }
    }

    // Ensure the final frame is added if not already
    const lastFrameTime = frames[frames.length - 1].time;
    if (sliderSteps.length === 0 || parseFloat(sliderSteps[sliderSteps.length - 1].label) < lastFrameTime) {
        sliderSteps.push({
            method: 'animate',
            label: lastFrameTime.toFixed(1),
            args: [[(frames.length - 1).toString()], {
                mode: 'immediate',
                frame: { duration: 0, redraw: true },
                transition: { duration: 0 }
            }]
        });
    }

    const layout = {
        title: 'Three-Body Simulation',
        scene: {
            xaxis: { title: 'X', range: xRange },
            yaxis: { title: 'Y', range: yRange },
            zaxis: { title: 'Z', range: zRange },
            aspectmode: 'manual',
            aspectratio: { x: 1, y: 1, z: 1 }
        },
        updatemenus: [{
            type: 'buttons',
            showactive: false,
            y: 0,
            x: 1.05,
            xanchor: 'left',
            yanchor: 'bottom',
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
        sliders: [{
            pad: { t: 30 },
            currentvalue: {
                visible: true,
                prefix: 'Simulation Time: ',
                xanchor: 'right',
                font: { size: 14, color: '#666' }
            },
            steps: sliderSteps
        }],
        margin: { l: 0, r: 0, b: 0, t: 30 }
    };

    // Initial traces: bodies + center of mass + trails (empty trails initially)
    Plotly.newPlot(plotId, [
        body1Trace,
        body2Trace,
        body3Trace,
        centerOfMassTrace,
        body1Trail,
        body2Trail,
        body3Trail,
        centerOfMassTrail
    ], layout).then(() => {
        Plotly.addFrames(plotId, animationFrames);

        // Autoplay after initialization
        Plotly.animate(plotId, null, {
            fromcurrent: true,
            frame: { duration: interval, redraw: true },
            transition: { duration: 0 }
        });
    });
}
