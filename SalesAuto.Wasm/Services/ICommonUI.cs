using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public interface ICommonUI
    {
        Task BusyDialog(DialogService dialogService, string message);
    }
}
