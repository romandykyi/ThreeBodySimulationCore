function renderThreeBodyAnimation3D(positions) {
    const totalFrames = positions[0].x.length;

    const getColor = (id) => {
        if (id === 'com') return 'black';
        const colors = ['red', 'green', 'blue'];
        return colors[parseInt(id.replace('body', '')) - 1] || 'gray';
    };

    // Initial plot data (only current position + trail)
    const traces = positions.map((body, i) => ({
        type: 'scatter3d',
        mode: 'lines+markers',
        name: body.id,
        x: [body.x[0]],
        y: [body.y[0]],
        z: [body.z[0]],
        line: {
            width: body.id === 'com' ? 2 : 4,
            color: getColor(body.id)
        },
        marker: {
            size: body.id === 'com' ? 4 : 6,
            color: getColor(body.id)
        }
    }));

    // Generate animation frames
    const frames = Array.from({ length: totalFrames }, (_, i) => ({
        name: i.toString(),
        data: positions.map(body => ({
            x: body.x.slice(0, i + 1),
            y: body.y.slice(0, i + 1),
            z: body.z.slice(0, i + 1)
        }))
    }));

    const layout = {
        title: 'Three-Body Problem Animation (3D)',
        scene: {
            xaxis: { title: 'X' },
            yaxis: { title: 'Y' },
            zaxis: { title: 'Z' }
        },
        margin: { l: 0, r: 0, t: 30, b: 0 },
        showlegend: true,
        updatemenus: [{
            type: 'buttons',
            showactive: false,
            y: 1.1,
            x: 0,
            xanchor: 'left',
            buttons: [
                {
                    label: 'Play',
                    method: 'animate',
                    args: [null, {
                        frame: { duration: 30, redraw: true },
                        transition: { duration: 0 },
                        fromcurrent: true
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
        }]
    };

    Plotly.newPlot('threeBodyPlot', traces, layout, { frames });
}