namespace Script.Controlador.Principal.Modulo1.SubcontenidoSonidos.modelo
{
    public class NivelSonido:Item

    {
    public string ResourceSonido { get; private set; }
    public string Respuesta { get; private set; }

    public NivelSonido(string resourceSonido, string respuesta)
    {
        ResourceSonido = resourceSonido;
        Respuesta = respuesta;
    }


    }
}