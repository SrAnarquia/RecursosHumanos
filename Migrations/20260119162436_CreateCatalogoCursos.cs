using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecursosHumanos.Migrations
{
    /// <inheritdoc />
    public partial class CreateCatalogoCursos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Reclutamiento");

            migrationBuilder.EnsureSchema(
                name: "Usuario");

            migrationBuilder.CreateTable(
                name: "Base",
                schema: "Reclutamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Base = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    FechaCreación = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Base__3214EC074BC0B040", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogoNivels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogoNivels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Datos_Reclutamiento_Reflejo",
                schema: "Reclutamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Telefono = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Comentarios = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    Empresa = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Base = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Fuente = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Estatus = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Posicion = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Sexo = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Reclutador = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Datos_Re__3214EC07155A07DC", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresa",
                schema: "Reclutamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Empresa = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    FechaCreación = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Empresa__3214EC0716581833", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estatus",
                schema: "Reclutamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estatus = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    FechaCreación = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Estatus__3214EC072F975721", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fuente",
                schema: "Reclutamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fuente = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    FechaCreación = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Fuente__3214EC0784036524", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posicion",
                schema: "Reclutamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Posicion = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    FechaCreación = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Posicion__3214EC07E21C48C7", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reclutador",
                schema: "Reclutamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    FechaCreación = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reclutad__3214EC0739300128", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sexo",
                schema: "Reclutamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sexo = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    FechaCreación = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sexo__3214EC072F6FFB1B", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoCursos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCursos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioEspejo",
                schema: "Usuario",
                columns: table => new
                {
                    ID = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Usuario = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Contraseña = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    es_administrador = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    nombre = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    correo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                schema: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Usuario = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Contraseña = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    es_administrador = table.Column<bool>(type: "bit", nullable: false),
                    nombre = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    correo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__usuarios__3214EC0764D14EAE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Datos_Reclutamiento",
                schema: "Reclutamiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Telefono = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Comentarios = table.Column<string>(type: "varchar(2555)", unicode: false, maxLength: 2555, nullable: false),
                    FechaCreación = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IdEmpresa = table.Column<int>(type: "int", nullable: true),
                    IdBase = table.Column<int>(type: "int", nullable: true),
                    IdFuente = table.Column<int>(type: "int", nullable: true),
                    IdEstatus = table.Column<int>(type: "int", nullable: true),
                    IdPosicion = table.Column<int>(type: "int", nullable: true),
                    IdSexo = table.Column<int>(type: "int", nullable: true),
                    IdReclutador = table.Column<int>(type: "int", nullable: true),
                    FechaContacto = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Datos_Re__3214EC07A4DE686C", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Base_Nombre",
                        column: x => x.IdBase,
                        principalSchema: "Reclutamiento",
                        principalTable: "Base",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Empresa_Nombre",
                        column: x => x.IdEmpresa,
                        principalSchema: "Reclutamiento",
                        principalTable: "Empresa",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Estatus_Nombre",
                        column: x => x.IdEstatus,
                        principalSchema: "Reclutamiento",
                        principalTable: "Estatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fuente_Nombre",
                        column: x => x.IdFuente,
                        principalSchema: "Reclutamiento",
                        principalTable: "Fuente",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Posicion_Nombre",
                        column: x => x.IdPosicion,
                        principalSchema: "Reclutamiento",
                        principalTable: "Posicion",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reclutador_Nombre",
                        column: x => x.IdReclutador,
                        principalSchema: "Reclutamiento",
                        principalTable: "Reclutador",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sexo_Nombre",
                        column: x => x.IdSexo,
                        principalSchema: "Reclutamiento",
                        principalTable: "Sexo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CatalogoCursos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdDepartamento = table.Column<int>(type: "int", nullable: true),
                    IdTipoCurso = table.Column<int>(type: "int", nullable: true),
                    IdNivel = table.Column<int>(type: "int", nullable: true),
                    Diploma = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFinalizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdDepartamentoNavigationId = table.Column<int>(type: "int", nullable: true),
                    IdNivelNavigationId = table.Column<int>(type: "int", nullable: true),
                    IdTipoCursoNavigationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogoCursos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogoCursos_CatalogoNivels_IdNivelNavigationId",
                        column: x => x.IdNivelNavigationId,
                        principalTable: "CatalogoNivels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogoCursos_Departamentos_IdDepartamentoNavigationId",
                        column: x => x.IdDepartamentoNavigationId,
                        principalTable: "Departamentos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogoCursos_TipoCursos_IdTipoCursoNavigationId",
                        column: x => x.IdTipoCursoNavigationId,
                        principalTable: "TipoCursos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogoCursos_IdDepartamentoNavigationId",
                table: "CatalogoCursos",
                column: "IdDepartamentoNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogoCursos_IdNivelNavigationId",
                table: "CatalogoCursos",
                column: "IdNivelNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogoCursos_IdTipoCursoNavigationId",
                table: "CatalogoCursos",
                column: "IdTipoCursoNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_Datos_Reclutamiento_IdBase",
                schema: "Reclutamiento",
                table: "Datos_Reclutamiento",
                column: "IdBase");

            migrationBuilder.CreateIndex(
                name: "IX_Datos_Reclutamiento_IdEmpresa",
                schema: "Reclutamiento",
                table: "Datos_Reclutamiento",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Datos_Reclutamiento_IdEstatus",
                schema: "Reclutamiento",
                table: "Datos_Reclutamiento",
                column: "IdEstatus");

            migrationBuilder.CreateIndex(
                name: "IX_Datos_Reclutamiento_IdFuente",
                schema: "Reclutamiento",
                table: "Datos_Reclutamiento",
                column: "IdFuente");

            migrationBuilder.CreateIndex(
                name: "IX_Datos_Reclutamiento_IdPosicion",
                schema: "Reclutamiento",
                table: "Datos_Reclutamiento",
                column: "IdPosicion");

            migrationBuilder.CreateIndex(
                name: "IX_Datos_Reclutamiento_IdReclutador",
                schema: "Reclutamiento",
                table: "Datos_Reclutamiento",
                column: "IdReclutador");

            migrationBuilder.CreateIndex(
                name: "IX_Datos_Reclutamiento_IdSexo",
                schema: "Reclutamiento",
                table: "Datos_Reclutamiento",
                column: "IdSexo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogoCursos");

            migrationBuilder.DropTable(
                name: "Datos_Reclutamiento",
                schema: "Reclutamiento");

            migrationBuilder.DropTable(
                name: "Datos_Reclutamiento_Reflejo",
                schema: "Reclutamiento");

            migrationBuilder.DropTable(
                name: "UsuarioEspejo",
                schema: "Usuario");

            migrationBuilder.DropTable(
                name: "usuarios",
                schema: "Usuario");

            migrationBuilder.DropTable(
                name: "CatalogoNivels");

            migrationBuilder.DropTable(
                name: "Departamentos");

            migrationBuilder.DropTable(
                name: "TipoCursos");

            migrationBuilder.DropTable(
                name: "Base",
                schema: "Reclutamiento");

            migrationBuilder.DropTable(
                name: "Empresa",
                schema: "Reclutamiento");

            migrationBuilder.DropTable(
                name: "Estatus",
                schema: "Reclutamiento");

            migrationBuilder.DropTable(
                name: "Fuente",
                schema: "Reclutamiento");

            migrationBuilder.DropTable(
                name: "Posicion",
                schema: "Reclutamiento");

            migrationBuilder.DropTable(
                name: "Reclutador",
                schema: "Reclutamiento");

            migrationBuilder.DropTable(
                name: "Sexo",
                schema: "Reclutamiento");
        }
    }
}
