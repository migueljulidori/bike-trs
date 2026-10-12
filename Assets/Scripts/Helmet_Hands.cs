using NativeWebSocket;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

// Classe para deserializar o JSON recebido do Python
[System.Serializable]
public class DetectionData
{
    public string gesture;
    public bool helmet;
}

public class Helmet_Hands : MonoBehaviour
{
    [Header("Status da Conexão")]
    public string serverStatus = "Desconectado";

    [Header("Dados Recebidos em Tempo Real")]
    public string currentGesture = "nenhum";
    public bool isHelmetDetected = false;

    private NativeWebSocket.WebSocket websocket;
    private Queue<DetectionData> dataQueue = new Queue<DetectionData>();

    void Start()
    {
        string serverUrl = "ws://localhost:8765";
        websocket = new NativeWebSocket.WebSocket(serverUrl);

        websocket.OnOpen += () =>
        {
            serverStatus = "Conectado a " + serverUrl;
            Debug.Log("Conexão com o servidor Python aberta!");
        };

        websocket.OnError += (e) =>
        {
            serverStatus = "Erro na conexão! Verifique se o servidor Python está rodando.";
            Debug.LogError("Erro na conexão WebSocket: " + e);
        };

        websocket.OnClose += (e) =>
        {
            serverStatus = "Desconectado";
            Debug.Log("Conexão com o servidor fechada!");
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            DetectionData data = JsonUtility.FromJson<DetectionData>(message);

            // Usa uma fila para garantir que os dados sejam processados na thread principal do Unity
            lock (dataQueue)
            {
                dataQueue.Enqueue(data);
            }
        };

        Debug.Log("--- INICIANDO TENTATIVA DE CONEXÃO ---");
        websocket.Connect();
    }

    void Update()
    {
        if (websocket != null)
        {
            // A cada 120 frames (aprox. 2 segundos), exibe o estado atual da conexão
            if (Time.frameCount % 120 == 0)
            {
                Debug.Log($"Verificando status... Estado do WebSocket: {websocket.State}");
            }

            websocket.DispatchMessageQueue();
        }

        lock (dataQueue)
        {
            if (dataQueue.Count > 0)
            {
                DetectionData data = dataQueue.Dequeue();
                currentGesture = data.gesture;
                isHelmetDetected = data.helmet;

                // Exibe os dados recebidos no console
                LogReceivedData(data);
            }
        }
    }

    private void LogReceivedData(DetectionData data)
    {
        // 1. Escreve o status do capacete no console
        if (data.helmet)
        {
            Debug.Log("<color=green>Capacete Detectado</color>");
        }
        else
        {
            Debug.Log("<color=red>AVISO: Sem Capacete</color>");
        }

        // 2. Escreve o gesto de sinalização no console
        switch (data.gesture)
        {
            case "direita":
                Debug.Log("<color=yellow>Sinal de Direita ativado.</color>");
                break;
            case "esquerda":
                Debug.Log("<color=yellow>Sinal de Esquerda ativado.</color>");
                break;
                // Nenhuma mensagem para "nenhum" para não poluir o console
        }
    }

    private async void OnApplicationQuit()
    {
        if (websocket != null && websocket.State == NativeWebSocket.WebSocketState.Open)
        {
            await websocket.Close();
        }
    }
}