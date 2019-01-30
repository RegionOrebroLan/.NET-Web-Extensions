# .NET-Web-Extensions

Additions and extensions for ASP.NET.

[![NuGet](https://img.shields.io/nuget/v/RegionOrebroLan.Web.svg?label=NuGet)](https://www.nuget.org/packages/RegionOrebroLan.Web)

## 1. Paging/Pagination

### Source
- [RegionOrebroLan.Web.Paging.IPagination](/Source/Project/Paging/IPagination.cs)
- [RegionOrebroLan.Web.Paging.Pagination](/Source/Project/Paging/Pagination.cs)
- [RegionOrebroLan.Web.Paging.IPaginationFactory](/Source/Project/Paging/IPaginationFactory.cs)
- [RegionOrebroLan.Web.Paging.PaginationFactory](/Source/Project/Paging/PaginationFactory.cs)

### Example
- [SampleApplication.Controllers.ListController](/Source/Sample-application/Controllers/ListController.cs)
- [SampleApplication/Views/List/Index.cshtml](/Source/Sample-application/Views/List/Index.cshtml)
- [SampleApplication/Views/Shared/Pagination.cshtml](/Source/Sample-application/Views/Shared/Pagination.cshtml)

### Pagination.cshtml

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