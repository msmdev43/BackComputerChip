namespace computerChip.DTOs.Requests.Pregunta
{
    public class PreguntaCreateRequest
    {
        public string TextoPregunta { get; set; } = string.Empty;
    }

    public class PreguntaResponderRequest
    {
        public string TextoRespuesta { get; set; } = string.Empty;
    }

    public class PreguntaUpdateRequest
    {
        public string? TextoPregunta { get; set; }
        public string? TextoRespuesta { get; set; }
    }
}