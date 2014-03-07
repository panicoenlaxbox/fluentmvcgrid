# fluentmvcgrid #
Grilla de ASP.NET MVC con una API fluida

Basada en Razor Templates
[http://geeks.ms/blogs/etomas/archive/2011/01/25/asp-net-mvc3-razor-templates.aspx](http://geeks.ms/blogs/etomas/archive/2011/01/25/asp-net-mvc3-razor-templates.aspx "ASP.NET MVC3 Razor Templates")
[http://geeks.ms/blogs/etomas/archive/2011/01/26/asp-net-mvc3-un-helper-repeater.aspx](http://geeks.ms/blogs/etomas/archive/2011/01/26/asp-net-mvc3-un-helper-repeater.aspx "ASP.NET MVC3 Un helper repeater")

La ordenación y paginación respetan el mismo comportamiento que la grilla de serie de ASP.NET MVC, WebGrid

Agrega características de paginación, pie de tabla, etc

Los estilos de la grilla están basados en Bootstrap 3.1

Un ejemplo de la grilla sería el siguiente:

    @(Html.FluentMvcGrid(Model)
    	.Id("table1")
    	.Class("table table-striped table-hover table-bordered")
    	.RowClass(item =item.Age < 18 ? "info" : string.Empty)
    	.Eof(@<p>No hay registros que mostrar</p>)
    	.HtmlBefore(@<h1>FluentMvcGrid</h1>)
    	.HtmlAfter(@<h4>panicoenlaxbox</h4>)
    	.AddAttribute("style",o ="margin: 50px auto;" )
    	.AddAttribute("lang", @<text>es</text>)
    	.AddColumn(column =column.Format(item =item.FirstName).HeaderText("Nombre").Sortable(true).SortBy("FirstName").Class(item ="lead"))
    	.AddColumn(column =column.Format(item =item.LastName).HeaderText("Apellidos").Sortable(false)
    	.AddAttribute("style", item =item.LastName=="León"?"font-weight: bold;":string.Empty ))
    	.AddColumn(column =column.Format(@<text>@item.Age</text>).HeaderText("Edad").HeaderClass(() ="text-right"))
    	.AddFooterColumn(footerColumn =footerColumn.ColSpan(3).Format(@<text>Your text goes here...</text>))
    	.Pagination(pagination =>pagination.Enabled(true).PageSize(5).PageIndex(1).TotalCount(50)))

Queda pendiente agregar pruebas unitarias.
