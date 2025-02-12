$(document).ready(function myfunction() {
    const api = "https://localhost:44315/api/books";

    //inicialiando datatable
    let table = new $('#libros').DataTable({
        ajax: api,

        columns: [
            { data: 'id' },
            { data: 'title' },
            { data: 'description' },
            { data: 'pageCount', visible: false },
            { data: 'excerpt', visible: false },
            {
                data: 'publishDate',
                title: "Publicado",
                render: function (data, type, row) {
                    if (type === "display" || type === "filter") {
                        return new Date(data).toLocaleDateString('es-ES', {
                            year: 'numeric',
                            month: '2-digit',
                            day: '2-digit'
                        });
                    }
                    return data; // Mantener el formato original en la base de datos
                }
            },
            {
                "data": null,
                "name": "buttonColumn",
                "render": function (data, type, row) {
                    return '<div class="btn-group" role="group">' +
                        '<button type="button" class="btn btn-secondary dropdown-toggle" data-toggle="dropdown" aria-expanded="false" > ' +
                        'Opciones' +
                        '</button > ' +
                        '<div class="dropdown-menu" > ' +
                        '<a class="dropdown-item edit" href= "#"> Editar</a> ' +
                        '<a class="dropdown-item delete" href= "#"> Eliminar</a> ' +
                        '</div> ' +
                        '</div> ';
                }
            }
        ],
        "oLanguage": {  // Personalización de idioma
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 a 0 de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando..., por favor espere",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        }
    });

    //inicializando librerias

    //inicializando el datepicker
    $('#PublishDate').datepicker({
        weekStart: 1,
        daysOfWeekHighlighted: "6,0",
        autoclose: true,
        todayHighlight: true,
        format: "dd/mm/yyyy"
    });

    //validaciones para el modal
    $('#form-modal').validate({
        rules: {
            'Title': { required: true },
            'PageCount': { required: true },
            'Description': { required: true },
            'PublishDate': { required: true },
            'Excerpt': { required: true }
        },
        messages: {
            'Title': { required: 'Este campo es obligatorio' },
            'PageCount': { required: 'Este campo es obligatorio' },
            'Description': { required: 'Este campo es obligatorio' },
            'Excerpt': { required: 'Este campo es obligatorio' },
            'PublishDate': { required: 'Este campo es obligatorio' }
        },
        debug: true
    });


    $(document).on('click', '.edit', function myfunction() {
        //obteniendo valores de la tabla
        var index = $(this).parents("tr").index();
        const data = table.row(index).data();
        $('#modalLibroLabel').text('Editar libro: ' + data.title);
  
        //pasando valores
        $('#id').val(data.id);
        $('#title').val(data.title);
        $('#pageCount').val(data.pageCount);
        $('#description').val(data.description);
        $('#excerpt').val(data.excerpt);
        $('#PublishDate').datepicker("setDate", new Date(data.publishDate).toLocaleDateString('es-ES', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit'
        }));        

        $('#modalLibro').modal('show');
    });



    $(document).on('click', '.btn-nuevo', function myfunction() {
        $('#modalLibroLabel').text('Crear libro');  

        //limpiando valores
        $('#id').val('');
        $('#title').val('');
        $('#pageCount').val('');
        $('#description').val('');
        $('#excerpt').val('');
        $('#PublishDate').val('');


        $('#modalLibro').modal('show');
    });

    $(document).on('click', '.delete', function myfunction() {
        //obteniendo valores de la tabla
        var index = $(this).parents("tr").index();
        const data = table.row(index).data();

        Swal.fire({
            title: "Esta seguro que desea eliminar este libro?",
            showDenyButton: true,
            showCancelButton: true,
            confirmButtonText: "Eliminar",
        }).then((result) => {

            if (result.isConfirmed) {
                $.ajax({
                    url: api + "/" + data.id,
                    type: 'DELETE',
                    async: true,
                    success: function myfunction(response) {
                        Swal.fire("Eliminado!", "", "success");
                    },
                    error: function myfunction(xhr, status) {
                        alert(xhr.responseText);
                    }
                });

            } else if (result.isDenied) {
                Swal.fire("Cambios cancelados", "", "info");
            }
        });
    });

    $(document).on('click', '.btn-acept', function myfunction() {
        var $form = $('#form-modal');

        if ($form.valid()) {
            const id = $('#id').val();
            var test = $('#PublishDate').datepicker("getDate");
            const book = {
                Title: $('#title').val(),
                Description: $('#description').val(),
                PageCount: $('#pageCount').val(),
                Excerpt: $('#excerpt').val(),
                PublishDate: $('#PublishDate').datepicker("getDate")
            };


            Swal.fire({
                title: "Esta seguro que desea realizar esta acción?",
                showDenyButton: true,
                showCancelButton: true,
                confirmButtonText: "Aceptar",
            }).then((result) => {

                if (result.isConfirmed) {
                    $.ajax({
                        url: (id > 0 ? api + "/" + id : api),
                        type: (id > 0 ? 'PUT' : 'POST'),
                        data: JSON.stringify(book),
                        contentType: "application/json",
                        dataType: "json",
                        async: true,
                        success: function myfunction(response) {
                            Swal.fire("Datos salvados!", "", "success");
                            $('#modalLibro').modal('hide');
                        },
                        error: function myfunction(xhr, status) {
                            alert(xhr.responseText);
                        }
                    });

                } else if (result.isDenied) {
                    Swal.fire("Cambios cancelados", "", "info");
                }
            });
        }
    });
});