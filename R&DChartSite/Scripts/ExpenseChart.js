var ctx = document.getElementById('expensechart').getContext('2d');

var barColors = [
    "#992d2d",
    "#a94242",
    "#c89c9c",
    "#dabdbd",
    "#eccfcf"
];

var expensechart = new Chart(ctx, {
    type: 'doughnut',
    data: {
        labels: ['Kırmızı', 'Mavi', 'Sarı', 'Yeşil', 'Turuncu'],
        datasets: [{
            data: [12, 19, 3, 5, 2],
            backgroundColor: barColors,
            borderColor: '#fff',
            borderWidth: 2
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                display: false
            }
        },
        cutout: '85%', 
    }
});
