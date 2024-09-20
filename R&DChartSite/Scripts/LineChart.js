document.addEventListener('DOMContentLoaded', function () {
    const table = document.getElementById('hours');

    const projects = [];
    const hours = [];

    for (let i = 0; i < table.rows.length; i++) {
        const row = table.rows[i];
        const project = row.cells[0].textContent;
        const hour = row.cells[1].textContent;
        projects.push(project);
        hours.push(hour);

    }


    var xValues = projects;
    var yValues = hours;
    var maxHour = Math.max(...hours);

    new Chart("linechart", {
        type: 'line',
        data: {
            labels: xValues,
            datasets: [{
                label: 'Saat',
                data: yValues,
                backgroundColor: '#992d2d',
                borderWidth: 3,
            }]
        },
        options: {
            maintainAspectRatio: false,
            aspectRatio: 0.8,
            scales: {
                x: {
                    ticks: {
                        autoSkip: false,
                        maxRotation: 0,
                        minRotation: 0,
                        padding: 10,
                    }
                },
                y: {
                    beginAtZero: true,
                    max: Math.ceil((maxHour + 30) / 10) * 10,
                    ticks: {
                        stepSize: 10
                    }
                }
            },
        }
    });
});
