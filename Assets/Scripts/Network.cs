/*
using UnityEngine;//Unity
using System.Net.Sockets;//Libreria que nos permite usar Sockets
using System;//Lo necesitamos para usar las interfaces Actions
using System.Text;//Lo necesitamos para decodificar los bytes provenientes del servidor
using System.Threading.Tasks;

public class Network : MonoBehaviour
{
	TcpClient client = new TcpClient();//Instancia de nuestro client TCP
	NetworkStream stream;//Lo usamos para leer y escribir en el servidor

	const string IP = "192.168.0.159";//Direccion IP del servidor(al principio sera la ip de tu pc)
	const int PORT = 8080;//Puerto en el cual esta isRunning el servidor
	const double memory = 5e+6;//Significa 5mbs en bytes
	const int limitTimeConnection = 5000;//Tiempo limite de conexion en milisegundos

	public byte[] data = new byte[(int)memory];//Donde almacenamos lo que viene del servidor
	public bool isRunning = false;//Para saber si el client esta isRunning

	public string id = "";
	public bool isReading = false;
	public bool isWriting = false;
	public bool searchingGame = false;
	public bool isConnected = false;

	private void Start()
	{
		//Intentamos conectarnos al servidor
		Connect((bool res) =>
		{
			if (res == true)
			{
				isConnected = true;
				stream = client.GetStream();//Obtenemos la instancia del stream de la conexion
				isRunning = true;
			}
			else
			{
				Debug.LogError("NO SE PUDO CONECTAR");
			}
		});
	}
	
	/// <summary>
	/// Este metodo hace algo dependiendo el command que le pasen
	/// <paramref name="command"/> Comando a ejecutar
	/// </summary>
	public void ReadCommand(string command)
	{
		if (command == "conectado")
		{
			Debug.Log("CONECTADO AL SERVIDOR");
		}

		if (command.StartsWith("id:"))
		{
			id = command.Replace("id: ", "");
			Debug.Log("ID RECIBIDO");
			WriteCommand("BUSCAR_PARTIDA");
		}
	}
	
	/// <summary>
	/// Este metodo se ejecuta cada vez que se termina de leer algo proveniente del servidor
	/// </summary>
	void FinishReading(IAsyncResult arr)
	{
		isReading = false;
		int t = stream.EndRead(arr);
		string mensaje = Encoding.UTF8.GetString(data, 0, t);
		ReadCommand(mensaje);
	}
	
	/// <summary>
	/// Este metodo manda un mensaje al servidor
	/// <paramref name="command"/> Comando a escribir
	/// </summary>
	void WriteCommand(string command)
	{
		if (command == "BUSCAR_PARTIDA") searchingGame = true;
		isWriting = true;
		stream.BeginWrite(Encoding.UTF8.GetBytes(command), 0, command.Length, new AsyncCallback(FinishWriting), stream);
	}


	/// <summary>
	/// Este metodo se ejecuta cada vez que se termina de escribir en el servidor
	/// </summary>
	void FinishWriting(IAsyncResult arr)
	{
		isWriting = false;
		stream.EndWrite(arr);
	}

	private void Update()
	{

		if (isRunning == true)
		{
			if (stream.DataAvailable)//Asi sabemos si el servidor ha enviado algo
			{
				isReading = true;
				stream.BeginRead(data, 0, data.Length, new AsyncCallback(FinishReading), stream);
			}
		}
	}
	
	/// <summary>
	/// Este metodo intenta conectarse al servidor durante un tiempo limite ya definido <see cref="limitTimeConnection"/>
	/// <paramref name="callback"/> se ejecuta despues de haber intentado conectarse al servidor y devuelve un bool dependiendo de si se conecto o no
	/// </summary>
	private void Connect(Action<bool> callback)
	{
		bool result = client.ConnectAsync(IP, PORT).Wait(limitTimeConnection);
		callback(result);
	}

	private void OnApplicationQuit()
	{
		isRunning = false;
	}
}
*/