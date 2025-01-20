using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class WiFiConnection : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private string serverIP = "192.168.0.103"; // Dirección IP del ESP32
    private int serverPort = 502; // Puerto TCP o Modbus (ajusta según configuración)

    void Start()
    {
        ConnectToWiFiServer();
    }

    void ConnectToWiFiServer()
    {
        try
        {
            // Crear cliente TCP
            client = new TcpClient(serverIP, serverPort);
            Debug.Log("Conectado al servidor Wi-Fi en ESP32.");

            // Obtener el flujo de datos
            stream = client.GetStream();

            // Enviar un mensaje al servidor
            SendMessageToServer("Hola desde Unity");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error al conectar con el servidor: " + ex.Message);
        }
    }

    void SendMessageToServer(string message)
    {
        if (stream != null && stream.CanWrite)
        {
            // Convertir mensaje a bytes y enviarlo
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            stream.Write(messageBytes, 0, messageBytes.Length);
            Debug.Log("Mensaje enviado al servidor: " + message);
        }
    }

    void ReceiveMessageFromServer()
    {
        if (stream != null && stream.CanRead)
        {
            // Leer datos del servidor
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Debug.Log("Mensaje recibido del servidor: " + receivedMessage);
        }
    }

    void OnApplicationQuit()
    {
        // Cerrar conexión al salir
        if (stream != null)
        {
            stream.Close();
        }
        if (client != null)
        {
            client.Close();
        }
        Debug.Log("Conexión cerrada.");
    }
}
