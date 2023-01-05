# .NET-Web-Extensions

Additions and extensions for .NET web-applications, ASP.NET and ASP.NET Core.

[![NuGet](https://img.shields.io/nuget/v/RegionOrebroLan.Web.svg?label=NuGet)](https://www.nuget.org/packages/RegionOrebroLan.Web)

## 1 Development

### 1.1 Signing

Drop the "StrongName.snk" file in the repository-root. The file should not be included in source control.

## 2 Features

### 2.1 Paging/Pagination

#### 2.1.1 Source
- [RegionOrebroLan.Web.Paging.IPagination](/Source/Project/Paging/IPagination.cs)
- [RegionOrebroLan.Web.Paging.Pagination](/Source/Project/Paging/Pagination.cs)
- [RegionOrebroLan.Web.Paging.IPaginationFactory](/Source/Project/Paging/IPaginationFactory.cs)
- [RegionOrebroLan.Web.Paging.PaginationFactory](/Source/Project/Paging/PaginationFactory.cs)

#### 2.1.2 Example
- [Application.Controllers.ListController](/Samples/Application/Controllers/ListController.cs)
- [Application/Views/List/Index.cshtml](/Samples/Application/Views/List/Index.cshtml)
- [Application/Views/Shared/Pagination.cshtml](/Samples/Application/Views/Shared/Pagination.cshtml)

#### 2.1.3 Pagination.cshtml

    @model IPagination
    @if(Model != null && Model.Pages.Any())
    {
	    <ul class="pagination">
		    <!-- First -->
		    @if(Model.FirstPageUrl == null)
		    {
			    <li class="page-item disabled">
				    <span class="page-link">&#x21E4;</span>
			    </li>
		    }
		    else
		    {
			    <li class="page-item">
				    <a class="page-link" href="@Model.FirstPageUrl.PathAndQueryAndFragment()" title="First">&#x21E4;</a>
			    </li>
		    }
		    <!-- Previous -->
		    @if(Model.PreviousPageUrl == null)
		    {
			    <li class="page-item disabled">
				    <span class="page-link">&laquo;</span>
			    </li>
		    }
		    else
		    {
			    <li class="page-item">
				    <a class="page-link" href="@Model.PreviousPageUrl.PathAndQueryAndFragment()" title="Previous">&laquo;</a>
			    </li>
		    }
		    @if(Model.Pages.Count() < Model.TotalNumberOfPages)
		    {
			    <!-- Previous group -->
			    if(Model.PreviousPageGroupUrl == null)
			    {
				    <li class="page-item disabled">
					    <span class="page-link">...</span>
				    </li>
			    }
			    else
			    {
				    <li class="page-item">
					    <a class="page-link" href="@Model.PreviousPageGroupUrl.PathAndQueryAndFragment()" title="Previous group">...</a>
				    </li>
			    }
		    }
		    <!-- Pages -->
		    @foreach(var page in Model.Pages)
		    {
			    if(page.Selected)
			    {
				    <li class="page-item active">
					    <a class="page-link" href="@(page.Url.PathAndQueryAndFragment())">@(page.Index)</a>
				    </li>
			    }
			    else
			    {
				    <li class="page-item">
					    <a class="page-link" href="@(page.Url.PathAndQueryAndFragment())">@(page.Index)</a>
				    </li>
			    }
		    }
		    @if(Model.Pages.Count() < Model.TotalNumberOfPages)
		    {
			    <!-- Next group -->
			    if(Model.NextPageGroupUrl == null)
			    {
				    <li class="page-item disabled">
					    <span class="page-link">...</span>
				    </li>
			    }
			    else
			    {
				    <li class="page-item">
					    <a class="page-link" href="@Model.NextPageGroupUrl.PathAndQueryAndFragment()" title="Next group">...</a>
				    </li>
			    }
		    }
		    <!-- Next -->
		    @if(Model.NextPageUrl == null)
		    {
			    <li class="page-item disabled">
				    <span class="page-link">&raquo;</span>
			    </li>
		    }
		    else
		    {
			    <li class="page-item">
				    <a class="page-link" href="@Model.NextPageUrl.PathAndQueryAndFragment()" title="Next">&raquo;</a>
			    </li>
		    }
		    <!-- Last -->
		    @if(Model.LastPageUrl == null)
		    {
			    <li class="page-item disabled">
				    <span class="page-link">&#x21E5;</span>
			    </li>
		    }
		    else
		    {
			    <li class="page-item">
				    <a class="page-link" href="@Model.LastPageUrl.PathAndQueryAndFragment()" title="Last">&#x21E5;</a>
			    </li>
		    }
	    </ul>
    }

### 2.2 Recaptcha

- [reCAPTCHA v3](https://developers.google.com/recaptcha/docs/v3/)
- [reCAPTCHA demo](https://recaptcha-demo.appspot.com/)

#### 2.2.1 Source

- [RegionOrebroLan.Web.Security.Captcha](/Source/Project/Security/Captcha/)

#### 2.2.2 Example

- [Application.Business.Web.Mvc.Filters.ValidateRecaptchaTokenFilter](/Samples/Application/Business/Web/Mvc/Filters/ValidateRecaptchaTokenFilter.cs)
- [Application.Business.Web.Mvc.ValidateRecaptchaTokenAttribute](/Samples/Application/Business/Web/Mvc/ValidateRecaptchaTokenAttribute.cs)
- [Application.Controllers.RecaptchaController](/Samples/Application/Controllers/RecaptchaController.cs)
- [Application/Views/Recaptcha/Form.cshtml](/Samples/Application/Views/Recaptcha/Form.cshtml)
- [Application/Views/Recaptcha/Index.cshtml](/Samples/Application/Views/Recaptcha/Index.cshtml)
- [Application/Views/Shared/_Layout.cshtml](/Samples/Application/Views/Shared/_Layout.cshtml#L47)