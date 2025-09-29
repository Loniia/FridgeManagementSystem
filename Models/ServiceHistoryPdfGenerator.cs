using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using FridgeManagementSystem.Models;

namespace FridgeManagementSystem.Models
{
    public class ServiceHistoryPdfGenerator : IDocument
    {
        private readonly List<MaintenanceVisit> _visits;
        private readonly Fridge _fridge;

        public ServiceHistoryPdfGenerator(List<MaintenanceVisit> visits, Fridge fridge)
        {
            _visits = visits;
            _fridge = fridge;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);

                // HEADER
                page.Header()
                    .Row(row =>
                    {
                        row.ConstantItem(60).Image("wwwroot/images/logo.jpg"); // Update path to your logo
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignRight().Text("Dynamic Ice Inc").FontSize(16).Bold();
                            col.Item().AlignRight().Text("Service History Report").FontSize(12);
                        });
                    });

                // CONTENT
                page.Content().Column(content =>
                {
                    content.Item().PaddingBottom(10).Text($"Fridge Model: {_fridge.Model}").FontSize(12).Bold();
                    content.Item().PaddingBottom(10).Text($"Customer: {_fridge.Customer?.FullName}").FontSize(12);
                    content.Item().PaddingBottom(20).Text($"Address: {_fridge.Customer?.Location?.Address}").FontSize(12);

                    if (!_visits.Any())
                    {
                        content.Item().Text("No service history found for this fridge.").Italic();
                    }
                    else
                    {
                        foreach (var visit in _visits)
                        {
                            content.Item().PaddingVertical(10).BorderBottom(1).Text(
                                $"Visit on {visit.ScheduledDate:dd/MM/yyyy} at {visit.ScheduledTime:hh\\:mm}"
                            ).FontSize(12).Bold();

                            content.Item().Text($"Status: {visit.Status}").FontSize(11);

                            // Visit Notes
                            if (!string.IsNullOrEmpty(visit.VisitNotes))
                            {
                                content.Item().PaddingTop(5).Text("Visit Notes:").Bold();
                                content.Item().Text(visit.VisitNotes).FontSize(11).Italic();
                            }

                            // Checklist
                            content.Item().PaddingTop(8).Text("Checklist:").Bold().FontSize(11);
                            if (visit.MaintenanceChecklist != null)
                            {
                                content.Item().Text($"• Temperature Status: {visit.MaintenanceChecklist.TemperatureStatus}");
                                content.Item().Text($"• Condenser Coils Cleaned: {(visit.MaintenanceChecklist.CondenserCoilsCleaned ? "Yes" : "No")}");
                                content.Item().Text($"• Coolant Level: {visit.MaintenanceChecklist.CoolantLevel}");
                                content.Item().Text($"• Door Seal Condition: {visit.MaintenanceChecklist.DoorSealCondition}");
                                content.Item().Text($"• Lighting Status: {visit.MaintenanceChecklist.LightingStatus}");
                                content.Item().Text($"• Power Cable Condition: {visit.MaintenanceChecklist.PowerCableCondition}");
                            }
                            else
                            {
                                content.Item().Text("No checklist completed for this visit.").Italic();
                            }

                            // Components
                            content.Item().PaddingTop(8).Text("Components Used:").Bold().FontSize(11);
                            if (visit.ComponentUsed != null && visit.ComponentUsed.Any())
                            {
                                foreach (var c in visit.ComponentUsed)
                                {
                                    content.Item().Text($"• {c.ComponentName} - Qty: {c.Quantity} - Condition: {c.Condition}");
                                }
                            }
                            else
                            {
                                content.Item().Text("No components recorded.").Italic();
                            }

                            // Faults
                            content.Item().PaddingTop(8).Text("Fault Reports:").Bold().FontSize(11);
                            if (visit.FaultReport != null && visit.FaultReport.Any())
                            {
                                foreach (var f in visit.FaultReport)
                                {
                                    content.Item().Text($"• {f.ReportDate:dd/MM/yyyy} - {f.FaultType} - {f.FaultDescription}");
                                }
                            }
                            else
                            {
                                content.Item().Text("No faults reported for this visit.").Italic();
                            }
                        }
                    }
                });

                // FOOTER
                page.Footer().Element(footer =>
                {
                    footer.AlignCenter().Text(x =>
                    {
                        x.Span("Generated by Dynamic Ice Inc - ").FontSize(10);
                        x.CurrentPageNumber().FontSize(10);
                        x.Span(" / ").FontSize(10);
                    });
                });



            });
        }
    }
}
