using AvaloniaSix.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Printing;

namespace AvaloniaSix.Services;

public class PrinterService
{
    public ObservableCollection<PrinterDetailsViewModel> AvailablePrinters()
    {
        var printerDetails = new ObservableCollection<PrinterDetailsViewModel>();

        // Windows 6.1+
        if (OperatingSystem.IsWindowsVersionAtLeast(6, 1))
        {
            printerDetails = GetWindowsPrinters();
        }

        return printerDetails;
    }

    public ObservableCollection<PrinterDetailsViewModel> GetWindowsPrinters()
    {
        if (!OperatingSystem.IsWindowsVersionAtLeast(6, 1))
        {
            return [];
        }

        var result = new ObservableCollection<PrinterDetailsViewModel>();
        var printDoc = new PrintDocument();
        for (var i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
        {
            var printerName = PrinterSettings.InstalledPrinters[i];
            printDoc.PrinterSettings.PrinterName = printerName;
            if (!printDoc.PrinterSettings.IsValid)
                continue;

            var detail = new PrinterDetailsViewModel()
            {
                Id = i.ToString(),
                Name = printerName,
                PaperSizes = new() { "(Default)" },
                SourceTrays = new() { "(Default)" }
            };
            foreach (PaperSize paperSize in printDoc.PrinterSettings.PaperSizes)
            {
                detail.PaperSizes.Add(paperSize.PaperName);
            }
            foreach (PaperSource sourceTray in printDoc.PrinterSettings.PaperSources)
            {
                detail.SourceTrays.Add(sourceTray.SourceName);
            }

            result.Add(detail);
        }
        return result;

    }
}
