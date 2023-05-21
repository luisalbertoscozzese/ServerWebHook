using System.Collections.Specialized;
using System.Reflection.Metadata;
using System.Web;

public class ConnectionMiddleware
{
    private readonly RequestDelegate _next;

        public ConnectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

        var request = context.Request; // Obtener la instancia de HttpRequestMessage
        var queryString = request.QueryString.ToString(); // Obtener la URL del solicitante

        string localHost= string.Equals(context.Connection.RemoteIpAddress?.ToString() ?? string.Empty, "127.0.0.1", StringComparison.OrdinalIgnoreCase)==true? "localhost": context?.Connection?.RemoteIpAddress?.ToString()?? string.Empty;


        // Construir la URL de destino usando la dirección IP del cliente
        var urlDestino = $"http://{localHost}:{context.Connection.LocalPort }/endpoint";

        NameValueCollection queryParameters = HttpUtility.ParseQueryString(queryString);
        string clientUrl = queryParameters.Get("clientUrl");
        Uri clientUri = new Uri(clientUrl);


        // Invocar el siguiente middleware en la cadena
        await _next.Invoke(context);
        }

    }
