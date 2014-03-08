# ¿Por qué FluentMvcGrid? #

Porque según mi opinión, la grilla que incorpora de serie ASP.NET MVC tiene muchas carencias y hay que hacer muchos workarounds para conseguir hacer cosas que a priori cualquier grilla debería ofrecer de forma sencilla.

# ¿Otra grilla? #

Pues es cierto que no es muy original, hay muchas grillas y la gran mayoría serán mejoras que ésta (por no decir todas), pero me hacía ilusión crear un buen helper, publicarlo en github y hacerlo disponible en Nuget.

Además, si con esto puedo favorecer la reutilización de código, pues mejor que mejor :) 

# ¿Cuáles son sus características clave? #

Principalmente 2: Primero una API fluida que permite configurar la grilla y todos sus elementos internos. Segundo un uso extensivo de los Razor Templates que permiten intercalar código Razor donde aparezca un delegado genérico de tipo *Func<T, object>*. 

Para mí fue un grato descubrimiento los posts sobre este tema que escribió Eduard Tomàs en su blog, y a partir de ahí vino todo lo demás.
 
[http://geeks.ms/blogs/etomas/archive/2011/01/25/asp-net-mvc3-razor-templates.aspx](http://geeks.ms/blogs/etomas/archive/2011/01/25/asp-net-mvc3-razor-templates.aspx "ASP.NET MVC3: Razor Templates")
[http://geeks.ms/blogs/etomas/archive/2011/01/26/asp-net-mvc3-un-helper-repeater.aspx](http://geeks.ms/blogs/etomas/archive/2011/01/26/asp-net-mvc3-un-helper-repeater.aspx "ASP.NET MVC3: Un helper Repeater")

# ¿Cómo funciona? #

Los elementos de la grilla son:

- La grilla - Clase *FluentMvcGrid<T\>*
- Las columnas - Clase *FluentMvcGridColumn<T\>*
- Las celdas del pie - Clase *FluentMvcGridFooterColumn*
- La paginación - Clase *FluentMvcGridPagination*

## FluentMvcGrid ##

Lo primero e indispensable es tipar la grilla a un modelo que implemente *IEnumerable<T\>*

    Html.FluentMvcGrid(Model)

Algunos métodos de la grilla esperan recibir una cadena sin más...

- Id
- Class

Por ejemplo:

    .Id("table1")
    .Class("table table-striped table-hover table-bordered")

Otros métodos esperan recibir un Razor Template, es decir, un *Func<T, object>*

- RowClass

Recibiendo un Razor Template ahora podemos escribir código de 2 formas distintas, bien a través de código Razor embebido o con una lambda:

Con Razor embebido

    .RowClass(@<text>@(item.Age < 18 ? "info" : string.Empty)</text>)

Donde tendremos disponible automáticamente una variable *item* tipada a T

Con Lambda

    .RowClass(item => item.Age < 18 ? "info" : string.Empty)

De hecho, este ejemplo (el querer asignar una clase a la fila en función de alguna propiedad del elemento iterado) es el que provocó el nacimiento de FluentMvcGrid :)

Hay un tercer grupo de métodos que reciben *Func<dynamic, object>*. Esto es porque no se tiene un elemento actual sobre el que se está iterando pero igualmente se quiere poder escribir un Razor Template. Solución: declararlo *Func<dynamic, object>* y saber que en estos métodos *item* será null.

En este grupo encontramos:

- Eof
- HtmlBefore
- HtmlAfter

Si te fijas, en Eof no se puede pasar ningún elemento porque no hay ninguna fila por la que estemos iterando (es por ello que un acceso a *item* daría error, es null), igualmente pasa con HtmlBefore (da la oportunidad de inyectar código Html antes de la grilla) y HtmlAfter (esta vez se inyecta código después de la grilla)

    .Eof(@<p>No hay registros que mostrar</p>)
    .HtmlBefore(@<h1>Ejemplo FluentMvcGrid</h1>)
    .HtmlAfter(@<h4><a href="http://twitter.com/panicoenlaxbox">panicoenlaxbox</a></h4>)

Por otro lado, si no queremos Razor Template podemos escribir el anterior código como una lambda, pero recuerda de nuevo que el valor del parámetro es null.

    .Eof(o => "<p>No hay registros que mostrar</p>")

Un método interesante de la clase FluentMvcGrid (que también está disponible en FluentMvcGridColumn) es AddAttribute.

Con AddAttribute podemos agregar atributos a la grilla (o a sus columnas) con Razor Templates o lambdas. 

    .AddAttribute("style",o => "margin: 50px auto;")
    .AddAttribute("lang", @<text>es</text>)

La diferencia en AddAttribute entre FluentMvcGrid y FluentMvcGridColumn, es que la primera recibe un *Func<dynamic, object>* y la segunda un *Func<T, object>*. Es decir, con *FluentMvcGridColumn* tenemos un elemento *item* que es la fila actualmente iterada.

También cabe mencionar que AddAttribute reemplazará cualquier atributo asignado previamente con el que coincida, esto es que prevalecerá frente a los métodos Class o Id, claro está en el caso de agregar un atributo con la clave class o id.

## FluentMvcGridColumn ##

Una columna tiene los siguentes métodos directos (un tipo básico):

- HeaderText
- Sortable
- SortBy

Por defecto, una columna no es ordenable. En caso de hacerla ordenable, también habremos de facilitar el campo por el que se ordenará con Sort.

    .AddColumn(column => column.HeaderText("Nombre").Sortable(true).SortBy("FirstName")

Que la columna no sea ordenable por defecto responde a que ordenar no es gratis (hay que implementar la ordenación en código), luego en mi caso me he dado cuenta de que es más económico activar la ordenación de las columnas por las que se permite ordenar que no al contrario.

Otros métodos que habilitan la escritura de Razor templates o lambdas a nivel de columna son:

- Class
- Format
- AddAttribute

El método más importante es Format que es quien escribe el dato en la celda:

    .AddColumn(column => column.Format(item => item.FirstName)
    .HeaderText("Nombre")
    .Format(@<text>@item.FirstName</text>)
    .Class(item => item.FirstName == "Sergio" ? "text-center" : "")
    .AddAttribute("style", item => item.FirstName=="Sergio"? "font-weight: bold;": "")

El método HeaderClass (clase que se aplicará a la etiqueta th) es del tipo *Func<string\>*, luego lo utilizaremos de la siguiente forma:

    .HeaderClass(() => "text-center")

## FluentMvcGridFooterColumn ##

Podemos agregar cualquier número de celdas al pie de la tabla. 

Tanto el contenido de la celda como el número de celdas que agrupara (colspan) es cosa nuestra:

    .AddFooterColumn(footerColumn => footerColumn.ColSpan(3).Format(@<text>Pie</text>))

Si sólo agregamos una celda al pie, el valor de ColSpan será calculado automáticamente al número de columnas que tenga la grilla y podremos no especificarlo. Una pequeña ayuda, sólo eso.

## FluentMvcGridPagination ##

Además del helper de la grilla, la biblioteca incluye otros 2 helpers relativos a la paginación y que están basados en los [tipos de paginación que ofrece Bootstrap](http://getbootstrap.com/components/#pagination "tipos de paginación que ofrece Boostrap")

        public static HtmlString GetDefaultPagination(
            this HtmlHelper htmlHelper, 
            int pageIndex, 
            int totalCount,
            int pageSize, 
            PaginationSizing paginationSizing, 
            int numericLinksCount, 
            bool paginationInfo,
            object htmlAttributes,
			BootstrapVersion bootstrapVersion)

        public static HtmlString GetPagerPagination(
            this HtmlHelper htmlHelper, 
            int pageIndex, 
            int totalCount,
            int pageSize, 
            bool alignedLinks, 
            object htmlAttributes)

Estos helpers sirven a 2 propósitos, implementar la paginación en la grilla (a través del estilo de paginación por defecto de Boostrap) y también el poder ser utilizados de forma independiente. Por ejemplo, si en vez de una grilla estamos mostrando la información con cualquier otro recurso HTML (div, span, etc.) podemos paginar igualmente llamando directamente a estos helpers.

En la grilla tenemos disponible el método Pagination con los siguientes métodos:

- Enabled, por defecto true
- PageIndex, por defecto 1
- PageSize, por defecto 10
- TotalCount
- NumericLinksCount, por defecto 5
- Info, por defecto true
- Sizing, por defecto normal

En realidad, casi todos estos métodos se corresponden con los parámetros solicitados por el primero de los helpers de paginacion, GetDefaultPagination.

    .Pagination(pagination => pagination.PageIndex(1).TotalCount(50))

Lógicamente, los métodos PageIndex y TotalCount no pueden ir hardcodeados y tendremos que obtenerlos desde el modelo de la vista. Hay un ejemplo completo de paginación en el controlador Home.

# Atributos data #

El único marcado que emite de forma fija el helper de la grilla es:

- data-role="footer"
- data-role="pagination"

Estos 2 atributos (ambos en elementos tfoot) resultan útiles para localizar con un selector de jQuery la posición del pie de la tabla y la paginación.

# Compatibilidad con Boostrap 2 #

En principio la grilla estaba pensanda para funcionar con los estilos de Bootstrap 3, pero como hay aplicaciones que todavia utilizando la versión 2, se ha agregado el método Bootstrap a la clase FluentMvcGrid.

    .Bootstrap(BootstrapVersion.Bootstrap2)

Por ahora, este método sólo incide en el HTML generado para la paginación por defecto.

# Localización #

Aunque he sopesado la idea de incluir un fichero de recursos (resx) para la localización, finalmente he optado por una clase pública estática con campos públicos. De este modo se permite a quién utilice la librería cambiar el valor de estas variables en tiempo de ejecución.

    public static class Resources
    {
        public static string First = "First";
        public static string Next = "Next";
        public static string Previous = "Previous";
        public static string Last = "Last";
        public static string PaginationInfo = "Page {0} of {1}, {2} records";
    }