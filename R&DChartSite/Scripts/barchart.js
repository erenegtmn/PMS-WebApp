z
    const table = document.getElementById('tbl');

    const days = [];
    const hours = [];

    for (let i = 1; i < table.rows.length - 1; i++) {
        const row = table.rows[i];
        const day = row.cells[0].textContent;
        const hour = row.cells[1].textContent;
        const date = new Date(day);
        const formattedDay = date.toLocaleString('tr-TR', { month: 'short', day: 'numeric' });
        days.push(formattedDay);
        hours.push(hour);
    }
    var xValues = days;
    var yValues = hours;
    new Chart("myChart", {
        type: 'bar',
        data: {

            labels: xValues,
            datasets: [{
                label: 'Saat',
                data: yValues,
                backgroundColor: '#992d2d',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true,
                    max: 24,
                    ticks: {
                        stepSize: 1
                    }
                }
            }
        }
    });

