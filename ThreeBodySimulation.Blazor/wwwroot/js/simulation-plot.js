function test(data) {
	TESTER = document.getElementById('plot');
	console.log(data);
	Plotly.newPlot(TESTER, [{
		x: data.x,
		y: data.y
	}], {
		margin: { t: 0 }
	});
}