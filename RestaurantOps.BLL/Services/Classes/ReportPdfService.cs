using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RestaurantOps.DAL.Repositories.Interfaces;


namespace RestaurantOps.BLL.Services.Classes
{
    public class ReportPdfService
    {
        private readonly IOrderRepository _orderRepository;

        public ReportPdfService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public IDocument CreateSalesReportDocument()
        {
            var orders = _orderRepository.GetAllWithDetails().ToList();

            var today = DateTime.UtcNow.Date;
            var thisMonth = new DateTime(today.Year, today.Month, 1);

            var todayOrders = orders.Where(o => o.Date.Date == today).ToList();
            var monthlyOrders = orders.Where(o => o.Date >= thisMonth).ToList();

            var todayRevenue = todayOrders.Sum(o => o.OrderItems.Sum(i => i.Quantity * i.Price));
            var monthlyRevenue = monthlyOrders.Sum(o => o.OrderItems.Sum(i => i.Quantity * i.Price));

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header()
                        .Column(col =>
                        {
                            col.Item().Text("RestaurantOps").FontSize(24).SemiBold().FontColor(Colors.Blue.Medium);
                            col.Item().Text("Sales Report").FontSize(16);
                            col.Item().Text($"Generated at: {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC").FontSize(9);
                        });

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(col =>
                        {
                            col.Spacing(20);

                            col.Item().Text("Summary").FontSize(14).SemiBold();

                            col.Item()
                                .Grid(grid =>
                                {
                                    grid.Columns(2);

                                    grid.Item()
                                        .Column(c =>
                                        {
                                            c.Item().Text($"Total Orders: {orders.Count}");
                                            c.Item().Text($"Today Orders: {todayOrders.Count}");
                                            c.Item().Text($"Monthly Orders: {monthlyOrders.Count}");
                                        });

                                    grid.Item()
                                        .Column(c =>
                                        {
                                            c.Item().Text($"Today Revenue: {todayRevenue:N2}");
                                            c.Item().Text($"Monthly Revenue: {monthlyRevenue:N2}");
                                        });
                                });

                            col.Item().Text("Orders Details").FontSize(14).SemiBold();

                            col.Item()
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(cols =>
                                    {
                                        cols.ConstantColumn(50);
                                        cols.ConstantColumn(80);
                                        cols.RelativeColumn();
                                        cols.ConstantColumn(80);
                                        cols.ConstantColumn(80);
                                    });

                                    table.Header(h =>
                                    {
                                        h.Cell().Text("ID").SemiBold();
                                        h.Cell().Text("Date").SemiBold();
                                        h.Cell().Text("Customer").SemiBold();
                                        h.Cell().AlignRight().Text("Status").SemiBold();
                                        h.Cell().AlignRight().Text("Total").SemiBold();
                                    });

                                    foreach (var o in orders)
                                    {
                                        var total = o.OrderItems.Sum(i => i.Quantity * i.Price);
                                        var statusText = o.OrderStatusEnum.ToString();

                                        table.Cell().Text(o.Id.ToString());
                                        table.Cell().Text(o.Date.ToString("yyyy-MM-dd"));
                                        table.Cell().Text(o.Customer != null ? o.Customer.Name : "N/A");
                                        table.Cell().AlignRight().Text(statusText);
                                        table.Cell().AlignRight().Text(total.ToString("N2"));
                                    }
                                });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                        });
                });
            });
        }
    }
}
