#pragma checksum "C:\Users\Admin\Desktop\S_HCM_API_ASP_NET_CORE_AND_JWT_NC_B1\WebApplication2\Views\FPTApis\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4d0027c67b0425f0ea22dd95b6a8271b626db487"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_FPTApis_Index), @"mvc.1.0.view", @"/Views/FPTApis/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4d0027c67b0425f0ea22dd95b6a8271b626db487", @"/Views/FPTApis/Index.cshtml")]
    #nullable restore
    public class Views_FPTApis_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<WebApplication2.Models.fpt_login>>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Admin\Desktop\S_HCM_API_ASP_NET_CORE_AND_JWT_NC_B1\WebApplication2\Views\FPTApis\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Index</h1>\r\n\r\n<p>\r\n    <a asp-action=\"Create\">Create New</a>\r\n</p>\r\n<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
#nullable restore
#line 16 "C:\Users\Admin\Desktop\S_HCM_API_ASP_NET_CORE_AND_JWT_NC_B1\WebApplication2\Views\FPTApis\Index.cshtml"
           Write(Html.DisplayNameFor(model => model.password));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
#nullable restore
#line 22 "C:\Users\Admin\Desktop\S_HCM_API_ASP_NET_CORE_AND_JWT_NC_B1\WebApplication2\Views\FPTApis\Index.cshtml"
 foreach (var item in Model) {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr>\r\n            <td>\r\n                ");
#nullable restore
#line 25 "C:\Users\Admin\Desktop\S_HCM_API_ASP_NET_CORE_AND_JWT_NC_B1\WebApplication2\Views\FPTApis\Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.password));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                <a asp-action=\"Edit\"");
            BeginWriteAttribute("asp-route-id", " asp-route-id=\"", 585, "\"", 614, 1);
#nullable restore
#line 28 "C:\Users\Admin\Desktop\S_HCM_API_ASP_NET_CORE_AND_JWT_NC_B1\WebApplication2\Views\FPTApis\Index.cshtml"
WriteAttributeValue("", 600, item.username, 600, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Edit</a> |\r\n                <a asp-action=\"Details\"");
            BeginWriteAttribute("asp-route-id", " asp-route-id=\"", 667, "\"", 696, 1);
#nullable restore
#line 29 "C:\Users\Admin\Desktop\S_HCM_API_ASP_NET_CORE_AND_JWT_NC_B1\WebApplication2\Views\FPTApis\Index.cshtml"
WriteAttributeValue("", 682, item.username, 682, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Details</a> |\r\n                <a asp-action=\"Delete\"");
            BeginWriteAttribute("asp-route-id", " asp-route-id=\"", 751, "\"", 780, 1);
#nullable restore
#line 30 "C:\Users\Admin\Desktop\S_HCM_API_ASP_NET_CORE_AND_JWT_NC_B1\WebApplication2\Views\FPTApis\Index.cshtml"
WriteAttributeValue("", 766, item.username, 766, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Delete</a>\r\n            </td>\r\n        </tr>\r\n");
#nullable restore
#line 33 "C:\Users\Admin\Desktop\S_HCM_API_ASP_NET_CORE_AND_JWT_NC_B1\WebApplication2\Views\FPTApis\Index.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<WebApplication2.Models.fpt_login>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
