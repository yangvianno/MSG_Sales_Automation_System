using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public class CommonUI: ICommonUI
    {
        public async Task BusyDialog(DialogService dialogService, string message)
        {
            await dialogService.OpenAsync("", ds =>
            {
                RenderFragment content = b =>
                {
                    b.OpenElement(0, "div");
                    b.AddAttribute(1, "class", "row");

                    b.OpenElement(2, "div");
                    b.AddAttribute(3, "class", "col-md-12");

                    b.AddContent(4, message);
                    b.OpenElement(5, "div");
                    b.AddAttribute(6, "class", "spinner-grow text-success");
                    b.AddAttribute(7, "role", "status");
                    b.CloseElement();
                    b.CloseElement();
                    b.CloseElement();
                };
                return content;
            }, new DialogOptions() { ShowTitle = false, Style = "min-height:auto;min-width:auto;width:auto" });
        }

    }
}
