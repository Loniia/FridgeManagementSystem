// reports-dashboard.js
// Fetches JSON endpoints from ReportsController and renders Chart.js charts and HTML tables.
// Now supports date range filtering and Export All PDF button.

(function () {
    // Helper: format number to currency string with 2 decimals
    function formatCurrency(num) {
        const n = Number(num || 0);
        return n.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    // Get selected date range from inputs
    function getDateRangeParams() {
        const from = document.getElementById('fromDate').value;
        const to = document.getElementById('toDate').value;
        let params = '';
        if (from) params += `from=${from}&`;
        if (to) params += `to=${to}&`;
        return params;
    }

    // ---------------------------
    // Render South Africa Map with Fridge Distribution
    // ---------------------------
    async function renderSouthAfricaMap() {
        try {
            const res = await fetch('/Reports/LocationMapData');
            const locationData = await res.json();

            // Hide loading indicator
            document.getElementById('mapLoading').style.display = 'none';

            const svgNS = "http://www.w3.org/2000/svg";
            const mapContainer = document.getElementById('southAfricaMap');

            // Clear previous map
            mapContainer.querySelector('svg')?.remove();

            const svg = document.createElementNS(svgNS, "svg");
            svg.setAttribute("width", "100%");
            svg.setAttribute("height", "100%");
            svg.setAttribute("viewBox", "0 0 800 600");

            // Simplified South Africa province coordinates (centers)
            const provinces = {
                'Gauteng': { x: 400, y: 300, color: '#3b7ddd' },
                'Western Cape': { x: 200, y: 450, color: '#1cbb8c' },
                'KwaZulu-Natal': { x: 450, y: 350, color: '#fcb92c' },
                'Eastern Cape': { x: 300, y: 450, color: '#dc3545' },
                'Free State': { x: 350, y: 250, color: '#6c757d' },
                'Limpopo': { x: 450, y: 200, color: '#17a2b8' },
                'Mpumalanga': { x: 500, y: 300, color: '#6610f2' },
                'North West': { x: 350, y: 200, color: '#e83e8c' },
                'Northern Cape': { x: 250, y: 300, color: '#fd7e14' }
            };

            // Create province areas
            Object.entries(provinces).forEach(([province, info]) => {
                const provinceData = locationData.find(d => d.Province === province);
                const fridgeCount = provinceData?.FridgeCount || 0;

                // Determine circle size based on fridge count
                let radius = 20;
                if (fridgeCount > 15) radius = 35;
                else if (fridgeCount > 5) radius = 28;
                else if (fridgeCount === 0) radius = 15;

                // Determine color intensity based on count
                let opacity = fridgeCount > 0 ? 0.8 : 0.3;
                let fillColor = fridgeCount > 0 ? info.color : '#cccccc';

                const circle = document.createElementNS(svgNS, "circle");
                circle.setAttribute("cx", info.x);
                circle.setAttribute("cy", info.y);
                circle.setAttribute("r", radius);
                circle.setAttribute("fill", fillColor);
                circle.setAttribute("opacity", opacity);
                circle.setAttribute("class", "province-area");
                circle.setAttribute("data-province", province);
                circle.setAttribute("data-count", fridgeCount);

                // Add hover effects
                circle.addEventListener('mouseenter', function (e) {
                    showMapTooltip(e, province, fridgeCount, provinceData);
                });
                circle.addEventListener('mouseleave', hideMapTooltip);

                svg.appendChild(circle);

                // Add province label
                const text = document.createElementNS(svgNS, "text");
                text.setAttribute("x", info.x);
                text.setAttribute("y", info.y + 4);
                text.setAttribute("text-anchor", "middle");
                text.setAttribute("fill", fridgeCount > 0 ? "white" : "#666666");
                text.setAttribute("font-size", "12");
                text.setAttribute("font-weight", "bold");
                text.textContent = province;
                svg.appendChild(text);

                // Add fridge count
                const countText = document.createElementNS(svgNS, "text");
                countText.setAttribute("x", info.x);
                countText.setAttribute("y", info.y + 20);
                countText.setAttribute("text-anchor", "middle");
                countText.setAttribute("fill", fridgeCount > 0 ? "white" : "#666666");
                countText.setAttribute("font-size", "10");
                countText.textContent = `${fridgeCount} fridges`;
                svg.appendChild(countText);
            });

            // Add city markers for locations with fridges
            locationData.forEach(provinceData => {
                provinceData.Locations.forEach(location => {
                    if (location.FridgeCount > 0) {
                        const province = provinces[provinceData.Province];
                        if (province) {
                            // Add some random variation to prevent overlapping
                            const variationX = (Math.random() - 0.5) * 50;
                            const variationY = (Math.random() - 0.5) * 50;

                            const marker = document.createElementNS(svgNS, "circle");
                            marker.setAttribute("cx", province.x + variationX);
                            marker.setAttribute("cy", province.y + variationY);
                            marker.setAttribute("r", 4);
                            marker.setAttribute("fill", "#ff6b6b");
                            marker.setAttribute("class", "location-marker");
                            marker.setAttribute("data-location", `${location.Address}, ${provinceData.City}`);
                            marker.setAttribute("data-fridges", location.FridgeCount);

                            marker.addEventListener('mouseenter', function (e) {
                                showLocationTooltip(e, location, provinceData.City);
                            });
                            marker.addEventListener('mouseleave', hideMapTooltip);

                            svg.appendChild(marker);
                        }
                    }
                });
            });

            mapContainer.appendChild(svg);

        } catch (error) {
            console.error('Error loading map data:', error);
            document.getElementById('mapLoading').innerHTML =
                '<p class="text-danger">Error loading map data. Please try again.</p>';
        }
    }

    // ---------------------------
    // Map Tooltip Functions
    // ---------------------------
    function showMapTooltip(event, province, count, provinceData) {
        let tooltip = document.getElementById('mapTooltip');
        if (!tooltip) {
            tooltip = document.createElement('div');
            tooltip.id = 'mapTooltip';
            tooltip.className = 'map-tooltip';
            document.body.appendChild(tooltip);
        }

        const cities = provinceData?.Locations?.map(l =>
            `${l.Address} (${l.FridgeCount} fridges)`
        ).join('<br>') || 'No fridges';

        tooltip.innerHTML = `
            <strong>${province}</strong><br>
            Total Fridges: ${count}<br>
            <small>${cities}</small>
        `;

        tooltip.style.left = (event.pageX + 10) + 'px';
        tooltip.style.top = (event.pageY - 10) + 'px';
        tooltip.style.display = 'block';
    }

    function showLocationTooltip(event, location, city) {
        let tooltip = document.getElementById('mapTooltip');
        if (!tooltip) {
            tooltip = document.createElement('div');
            tooltip.id = 'mapTooltip';
            tooltip.className = 'map-tooltip';
            document.body.appendChild(tooltip);
        }

        tooltip.innerHTML = `
            <strong>${location.Address}</strong><br>
            ${city}<br>
            Fridges: ${location.FridgeCount}<br>
            <small>Postal: ${location.PostalCode}</small>
        `;

        tooltip.style.left = (event.pageX + 10) + 'px';
        tooltip.style.top = (event.pageY - 10) + 'px';
        tooltip.style.display = 'block';
    }

    function hideMapTooltip() {
        const tooltip = document.getElementById('mapTooltip');
        if (tooltip) {
            tooltip.style.display = 'none';
        }
    }

    // ---------------------------
    // Render Monthly Sales chart
    // ---------------------------
    async function renderSalesChart() {
        const params = getDateRangeParams();
        const res = await fetch(`/Reports/MonthlySales?months=12&${params}`);
        const data = await res.json();

        const labels = data.map(d => {
            const dt = new Date(d.year || d.Year, (d.month || d.Month) - 1, 1);
            return dt.toLocaleString(undefined, { month: 'short', year: 'numeric' });
        });

        const values = data.map(d => d.totalAmount || d.TotalAmount || 0);
        const ctx = document.getElementById('salesChart').getContext('2d');

        new Chart(ctx, {
            type: 'line',
            data: { labels: labels, datasets: [{ label: 'Monthly Sales (R)', data: values, fill: false, tension: 0.2 }] },
            options: {
                responsive: true,
                plugins: {
                    tooltip: {
                        callbacks: { label: ctx => 'R ' + formatCurrency(ctx.parsed.y) }
                    }
                },
                scales: { y: { beginAtZero: true } }
            }
        });
    }

    // ---------------------------
    // Render Top Customers table
    // ---------------------------
    async function renderTopCustomers() {
        const params = getDateRangeParams();
        const res = await fetch(`/Reports/TopCustomers?top=10&${params}`);
        const data = await res.json();

        let html = '<table class="table table-sm">';
        html += '<thead><tr><th>#</th><th>Customer</th><th>Orders</th><th>Amount (R)</th></tr></thead><tbody>';

        data.forEach((row, idx) => {
            const name = row.customerName || row.CustomerName || 'Unknown';
            const orders = row.totalOrders || row.TotalOrders || 0;
            const amount = row.totalAmount || row.TotalAmount || 0;
            html += `<tr>
                        <td>${idx + 1}</td>
                        <td>${name}</td>
                        <td>${orders}</td>
                        <td>R ${formatCurrency(amount)}</td>
                     </tr>`;
        });

        html += '</tbody></table>';
        document.getElementById('topCustomersTable').innerHTML = html;
    }

    // ---------------------------
    // Render Top Faults chart
    // ---------------------------
    async function renderFaultsChart() {
        const params = getDateRangeParams();
        const res = await fetch(`/Reports/TopFaults?top=5&${params}`);
        const data = await res.json();

        const labels = data.map(d => d.faultType || d.FaultType || 'Unknown');
        const values = data.map(d => d.count || d.Count || 0);
        const ctx = document.getElementById('faultsChart').getContext('2d');

        new Chart(ctx, { type: 'bar', data: { labels: labels, datasets: [{ label: 'Fault Count', data: values }] }, options: { responsive: true } });
    }

    // ---------------------------
    // Render Maintenance summary (donut)
    // ---------------------------
    async function renderMaintenanceChart() {
        const params = getDateRangeParams();
        const res = await fetch(`/Reports/MaintenanceSummary?${params}`);
        const data = await res.json();

        const labels = data.map(d => d.status || d.Status || 'Unknown');
        const values = data.map(d => d.count || d.Count || 0);
        const ctx = document.getElementById('maintenanceChart').getContext('2d');

        new Chart(ctx, { type: 'doughnut', data: { labels: labels, datasets: [{ data: values }] }, options: { responsive: true } });
    }

    // ---------------------------
    // Inventory by model (pie)
    // ---------------------------
    async function renderInventoryChart() {
        const params = getDateRangeParams();
        const res = await fetch(`/Reports/InventoryByModel?${params}`);
        const data = await res.json();

        const labels = data.map(d => d.model || d.Model || 'Unknown');
        const values = data.map(d => d.count || d.Count || 0);
        const ctx = document.getElementById('inventoryChart').getContext('2d');

        new Chart(ctx, { type: 'pie', data: { labels: labels, datasets: [{ data: values }] }, options: { responsive: true } });
    }

    // ---------------------------
    // Location distribution (bar)
    // ---------------------------
    async function renderLocationChart() {
        const params = getDateRangeParams();
        const res = await fetch(`/Reports/LocationDistribution?${params}`);
        const data = await res.json();

        const labels = data.map(d => d.province || d.Province || 'Unknown');
        const values = data.map(d => d.count || d.Count || 0);
        const ctx = document.getElementById('locationChart').getContext('2d');

        new Chart(ctx, { type: 'bar', data: { labels: labels, datasets: [{ label: 'Fridges', data: values }] }, options: { responsive: true } });
    }

    // ---------------------------
    // Supplier performance table
    // ---------------------------
    async function renderSupplierTable() {
        const params = getDateRangeParams();
        const res = await fetch(`/Reports/SupplierPerformance?top=10&${params}`);
        const data = await res.json();

        let html = '<table class="table table-sm">';
        html += '<thead><tr><th>#</th><th>Supplier</th><th>Orders</th><th>Amount (R)</th></tr></thead><tbody>';

        data.forEach((row, idx) => {
            const name = row.supplierName || row.SupplierName || 'Unknown';
            const orders = row.ordersCount || row.OrdersCount || 0;
            const amount = row.totalAmount || row.TotalAmount || 0;
            html += `<tr>
                        <td>${idx + 1}</td>
                        <td>${name}</td>
                        <td>${orders}</td>
                        <td>R ${formatCurrency(amount)}</td>
                     </tr>`;
        });

        html += '</tbody></table>';
        document.getElementById('supplierTable').innerHTML = html;
    }

    // ---------------------------
    // Contracts table (placeholder)
    // ---------------------------
    async function renderContractsTable() {
        const res = await fetch('/Reports/ContractsActive');
        const data = await res.json();

        if (!data || data.length === 0) {
            document.getElementById('contractsTable').innerHTML = '<p class="text-muted">No contract data available.</p>';
            return;
        }

        let html = '<table class="table table-sm"><thead><tr><th>#</th><th>Customer</th><th>Start</th><th>End</th></tr></thead><tbody>';
        data.forEach((c, idx) => {
            const start = c.startDate ? new Date(c.startDate).toLocaleDateString() : '';
            const end = c.endDate ? new Date(c.endDate).toLocaleDateString() : '';
            html += `<tr><td>${idx + 1}</td><td>${c.customerName || c.CustomerName}</td><td>${start}</td><td>${end}</td></tr>`;
        });
        html += '</tbody></table>';
        document.getElementById('contractsTable').innerHTML = html;
    }

    // ---------------------------
    // Re-render all charts/tables
    // ---------------------------
    function renderAll() {
        renderSalesChart();
        renderTopCustomers();
        renderFaultsChart();
        renderMaintenanceChart();
        renderInventoryChart();
        renderLocationChart();
        renderSupplierTable();
        renderContractsTable();
        renderSouthAfricaMap(); // Add this line
    }

    // ---------------------------
    // Event: Apply date filter
    // ---------------------------
    document.getElementById('filterBtn').addEventListener('click', function () {
        renderAll();
    });

    // ---------------------------
    // Event: Export All to PDF
    // ---------------------------
    document.getElementById('exportPdfBtn').addEventListener('click', function () {
        const params = getDateRangeParams();
        window.open(`/Reports/ExportPdf?${params}`, '_blank');
    });

    // ---------------------------
    // Initialize on DOM ready
    // ---------------------------
    document.addEventListener('DOMContentLoaded', function () {
        renderAll();
    });

})();