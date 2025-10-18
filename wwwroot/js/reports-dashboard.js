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
