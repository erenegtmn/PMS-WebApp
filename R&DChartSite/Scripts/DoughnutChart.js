$(document).ready(function () {
    const table = document.getElementById('users');

    const projects = [];
    const users = [];

    for (let i = 0; i < table.rows.length; i++) {
        const row = table.rows[i];
        const project = row.cells[0].textContent;
        const user = row.cells[1].textContent;
        projects.push(project);
        users.push(user);
    }

    var xValues = projects;
    var yValues = users;
    var barColors = [
        "#a328ad",
        "#d11098",
        "#f2167e",
        "#ff3962",
        "#ff5f46",
        "#ff8329",
        "#ffa600"
    ];
    var barColors2 = [
        "#D2CDEA",
        "#73628A",
        "#8D849A",
        '#313D5A',
        "#3D485A",
        "#183642",
        "#1F3D54",
        "#0B191E",
        "#111F23"
    ];
    if (barColors.length < projects.length) {
        var remainingColors = projects.length - barColors.length;
        for (let i = 0; i < remainingColors; i++) {
            var randomColor = generateRandomColors();
            barColors.push(randomColor);
        }
    }

    new Chart("doughnutchart", {
        type: 'doughnut',
        data: {
            labels: xValues,
            datasets: [{
                label: 'Çalışan Sayısı',
                data: yValues,
                backgroundColor: barColors,
                borderWidth: 1
            }]
        },
        options: {
            maintainAspectRatio: false,
            aspectRatio: 1,
        }
    });
});

function generateRandomColors() {
    const randomColor = "#" +
        Math.floor(Math.random() * 16777215).toString(16);
    return randomColor;
}
