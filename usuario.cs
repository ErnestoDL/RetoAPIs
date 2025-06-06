public class usuario
{
	public int ID_Usuario { get; set; }
	public string Nombre { get; set; }
	public string Usuario { get; set; }
	public string Contrasena { get; set; }
	public string Foto_de_Perfil { get; set; }
	public int ID_Cargo { get; set; }
	public DateTime Fecha_CuentaAgregada { get; set; }
	public string Idioma { get; set; }

	// Nuevos campos del ranking
	public int ID_LiderTienda { get; set; }
	public int Diamantes { get; set; }
}
