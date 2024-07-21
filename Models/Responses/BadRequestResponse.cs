

namespace API_CRUDTareas.Models.Responses
{
    /// <summary>
    /// Clase que representa una respuesta de solicitud incorrecta (400 Bad Request).
    /// </summary>
    public class BadRequestResponse
    {
        /// <summary>
        /// Lista de mensajes de error.
        /// </summary>
        public IList<string> MessageList { get; set; } = new List<string>();
        /// <summary>
        /// Fuente del error.
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// Descripción de la excepción.
        /// </summary>
        public string Exception { get; set; }
        /// <summary>
        /// Identificador del error.
        /// </summary>
        public string ErrorId { get; set; }
        /// <summary>
        /// Mensaje de soporte para el error.
        /// </summary>
        public string SupportMessage { get; set; }
        /// <summary>
        /// Código de estado HTTP.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BadRequestResponse"/> con una lista de mensajes.
        /// </summary>
        /// <param name="messageList">Lista de mensajes de error.</param>
        public BadRequestResponse(IList<string> messageList)
        {
            this.MessageList = messageList;
            this.StatusCode = StatusCodes.Status400BadRequest;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BadRequestResponse"/> con una excepción.
        /// </summary>
        /// <param name="ex">La excepción que causó el error.</param>
        public BadRequestResponse(Exception ex)
        {
            this.Source = ex.Source;
            this.Exception = ex.Message;
            this.ErrorId = ex.Message;
            this.SupportMessage = ex.HelpLink;
            this.StatusCode = StatusCodes.Status400BadRequest;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BadRequestResponse"/> con una excepción y una lista de mensajes.
        /// </summary>
        /// <param name="ex">La excepción que causó el error.</param>
        /// <param name="messageList">Lista de mensajes de error.</param>
        public BadRequestResponse(Exception ex, IList<string> messageList)
        {
            this.MessageList = messageList;
            this.Source = ex.Source;
            this.Exception = ex.Message;
            this.ErrorId = ex.Message;
            this.SupportMessage = ex.HelpLink;
            this.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
