window.drawRevenueChart = (data) => {
    console.log("Chart loaded");

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
                    backgroundColor: 'rgba(0, 123, 255, 0.9)',
                    borderRadius: 5,
                    // 👇 bỏ barThickness, dùng barPercentage
                    barPercentage: 0.6,
                    categoryPercentage: 0.6
                },
                {
                    label: 'Thuê gia sư',
                    data: tutorData,
                    backgroundColor: 'rgba(253, 126, 20, 0.9)',
                    borderRadius: 5,
                    barPercentage: 0.6,
                    categoryPercentage: 0.6
                },
                {
                    label: 'Tổng doanh thu',
                    data: totalRevenue,
                    type: 'line',
                    borderColor: '#28a745',
                    borderWidth: 3,
                    fill: false,
                    tension: 0.4,
                    pointRadius: 5,
                    pointBackgroundColor: '#28a745',
                    pointBorderColor: '#28a745',
                    pointHoverRadius: 7
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                    labels: {
                        font: {
                            size: 14,
                            weight: 'bold'
                        }
                    }
                },
                tooltip: {
                    backgroundColor: '#343a40',
                    titleFont: { size: 14 },
                    bodyFont: { size: 13 },
                    footerFont: { weight: 'bold' }
                }
            },
            scales: {
                x: {
                    stacked: false,
                    grid: {
                        display: false
                    },
                    ticks: {
                        font: {
                            size: 13
                        }
                    }
                },
                y: {
                    beginAtZero: true,
                    grid: {
                        color: '#dee2e6'
                    },
                    ticks: {
                        font: {
                            size: 13
                        }
                    }
                }
            }
        }
    });
};
