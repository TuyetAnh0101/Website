window.drawRevenueChart = (data) => {
    console.log("Chart loaded"); // kiểm tra xem hàm chạy chưa

    const labels = data.map(x => new Date(x.date).toLocaleDateString());
    const orderData = data.map(x => x.orderRevenue);
    const tutorData = data.map(x => x.tutorRevenue);
    const totalRevenue = data.map(x => x.orderRevenue + x.tutorRevenue);

    const ctx = document.getElementById('revenueChart').getContext('2d');
    if (window.revenueChartInstance) {
        window.revenueChartInstance.destroy();
    }

    window.revenueChartInstance = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Đơn hàng',
                    data: orderData,
                    backgroundColor: '#007bff', // xanh dương đậm
                    barThickness: 20
                },
                {
                    label: 'Thuê gia sư',
                    data: tutorData,
                    backgroundColor: '#fd7e14', // cam đậm
                    barThickness: 20
                },
                {
                    label: 'Tổng doanh thu',
                    data: totalRevenue,
                    type: 'line',
                    borderColor: '#dc3545', // đỏ đậm
                    borderWidth: 2,
                    fill: false,
                    tension: 0.4,
                    pointRadius: 3
                }
            ]
        },
        options: {
            responsive: true,
            scales: {
                x: {
                    stacked: false
                },
                y: {
                    beginAtZero: true
                }
            }
        }
    });
};
