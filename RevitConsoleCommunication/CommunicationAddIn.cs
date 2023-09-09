using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitConsoleCommunication
{
    [Transaction(TransactionMode.Manual)]
    public class CommunicationAddIn : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            Communicate communicate = new Communicate();
            IntPtr windowHandle = commandData.Application.MainWindowHandle;
            System.Windows.Interop.WindowInteropHelper helper =
                new System.Windows.Interop.WindowInteropHelper(communicate);
            helper.Owner = windowHandle;
            // show center of revit window
            communicate.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            communicate.ShowDialog();
            return Result.Succeeded;
        }
    }
}