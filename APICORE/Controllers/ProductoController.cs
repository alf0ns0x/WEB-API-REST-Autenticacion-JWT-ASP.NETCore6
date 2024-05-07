using APICORE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace APICORE.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly string cadenaSQL;
        public ProductoController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_lista_productos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                Nombre = rd["Nombre"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Categoria"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"])
                            });
                        }
                    }
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message, response = lista });
            }
        }

        [HttpGet]
        [Route("Obtener")]
        public IActionResult Obtener(int idProducto)
        {
            List<Producto> lista = new List<Producto>();
            Producto oproducto = new Producto();

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_lista_productos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                Nombre = rd["Nombre"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Categoria"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"])
                            });
                        }
                    }
                    oproducto = lista.Where(e => e.IdProducto == idProducto).FirstOrDefault();

                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = oproducto });
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message, response = oproducto });
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_guardar_producto", conexion);
                    cmd.Parameters.AddWithValue("CodigoBarra", objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("Nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("Marca", objeto.Marca);
                    cmd.Parameters.AddWithValue("Categoria", objeto.Categoria);
                    cmd.Parameters.AddWithValue("Precio", objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "agregado" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message });
            }
        }

        [HttpPost]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_editar_producto", conexion);
                    cmd.Parameters.AddWithValue("IdProducto", objeto.IdProducto);
                    cmd.Parameters.AddWithValue("CodigoBarra", objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("Nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("Marca", objeto.Marca);
                    cmd.Parameters.AddWithValue("Categoria", objeto.Categoria);
                    cmd.Parameters.AddWithValue("Precio", objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Editado" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message});
            }
        }

        [HttpPost]
        [Route("Eliminar")]
        public IActionResult Eliminar(int IdProducto)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_eliminar_producto", conexion);
                    cmd.Parameters.AddWithValue("IdProducto", IdProducto);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Eliminado" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message});
            }
        }
    }
}
