using UnityEngine;
using System;
using System.IO.Ports;                                                                            //AQUIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// V19: Física dos freios diferenciada. O freio dianteiro aplica uma
/// força brusca, enquanto o freio traseiro aumenta o atrito (drag) para
/// simular um deslize mais suave.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações da Bicicleta")]
    public float maxSpeed = 20f;
    public float pedalForce = 50f;
    public float frontBrakeForce = 100f;
    [Tooltip("Atrito (arrasto) padrão da bicicleta quando não está a travar.")]
    public float defaultDrag = 0.5f;
    [Tooltip("Atrito (arrasto) aplicado ao usar o freio traseiro para simular um deslize.")]
    public float rearBrakeDrag = 5f;
    public Transform frontWheel;

    [Header("Configurações da Câmara")]
    public Transform playerCam; // Arraste o objeto da sua Câmara Principal aqui
    public float sensX = 100f;
    public float sensY = 100f;
    [Tooltip("Limite máximo de giro do corpo da bicicleta em graus (para cada lado).")]
    public float maxTurnLimit = 90f;
    [Tooltip("Tempo de suavização da rotação em segundos. Menor = mais rápido.")]
    public float steeringSmoothTime = 0.08f; // Reduzido para uma resposta mais rápida
    [Tooltip("Número de leituras a serem usadas no filtro de mediana. Menor = mais rápido, mas pode reintroduzir tremor.")]
    public int filterSampleSize = 5; // Reduzido drasticamente para diminuir o atraso
    [Tooltip("Zona morta do guidão para evitar tremores no centro (valor de 0 a 1).")]
    public float steeringDeadZone = 0.05f;
    [Tooltip("Marque para inverter a direção do guidão (esquerda/direita).")]
    public bool invertSteering = true;

    [Header("Ativação")]
    [Tooltip("Está ativado")]
    public bool Activated;


    [Header("Configurações do Arduino")]
    public string portName = "COM5";
    public int baudRate = 9600;
    [Tooltip("A bicicleta acelera quando o valor do Hall for MENOR que este número.")]
    public int pedalThreshold = 100;
    [Tooltip("O freio é ativado quando o valor do potenciômetro for MAIOR que este número.")]
    public int brakeThreshold = 100;

    // --- Variáveis de Física e Rotação ---
    private Rigidbody rb;
    float rotationX, rotationY;
    private float rotationYVelocity; // Variável para o SmoothDamp

    // --- Variáveis do Filtro de Mediana ---
    private Queue<float> guidaoReadings;

    // --- Variáveis do Arduino ---
    private SerialPort stream;                                                        //AQUIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII
    private bool isArduinoConnected = false;
    private int hallValue = 999;
    private int frontBrakeValue = 0;
    private int rearBrakeValue = 0;

    void Start()
    {
        Activated = true;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (filterSampleSize % 2 == 0) filterSampleSize++;

        guidaoReadings = new Queue<float>();
        for (int i = 0; i < filterSampleSize; i++)
        {
            guidaoReadings.Enqueue(512f);
        }

        ConnectToArduino();
    }

    void Update()
    {
        if (isArduinoConnected)
        {
            try
            {
                string dataString = stream.ReadLine();
                ParseArduinoData(dataString);
            }
            catch (TimeoutException) { /* Ignora */ }
            catch (Exception e)
            {
                Debug.LogWarning("Erro ao ler do Arduino: " + e.Message);
                isArduinoConnected = false;
            }
        }

        float targetRotationY;

        if (isArduinoConnected)
        {
            // Etapa 1: Pega o valor mediano das últimas leituras para obter um sinal estável
            float[] readingsArray = guidaoReadings.ToArray();
            Array.Sort(readingsArray);
            float medianGuidaoValue = readingsArray[filterSampleSize / 2];

            float steerInput = (medianGuidaoValue - 512f) / 512f;

            if (invertSteering) steerInput *= -1;

            if (Mathf.Abs(steerInput) < steeringDeadZone)
            {
                steerInput = 0f;
            }

            // Define o ângulo de rotação alvo com base no input
            targetRotationY = steerInput * maxTurnLimit;
        }
        else
        {
            // Usa o rato para definir a rotação alvo
            float steerInput = Input.GetAxisRaw("Mouse X");
            if (invertSteering) steerInput *= -1;
            targetRotationY = rotationY + steerInput * Time.deltaTime * sensX; // Acumula o movimento do rato
            targetRotationY = Mathf.Clamp(targetRotationY, -maxTurnLimit, maxTurnLimit);
        }

        // Suaviza a rotação horizontal (esquerda/direita)
        rotationY = Mathf.SmoothDamp(rotationY, targetRotationY, ref rotationYVelocity, steeringSmoothTime);

        // A lógica de rotação vertical (para cima/baixo) continua a mesma
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // A câmara (filho) controla a rotação para cima e para baixo no seu eixo local
        if (Activated)
        {
            if (playerCam != null)
            {
                playerCam.localRotation = Quaternion.Euler(rotationX, 0, 0);
            }
            else
            {
                //Debug.LogError("A variável 'playerCam' não foi atribuída no Inspector!");
            }
        }

        // A roda dianteira reage visualmente à direção
        if (frontWheel != null)
        {
            float targetSteer = (rotationY / maxTurnLimit) * 45f;
            Quaternion targetRotation = Quaternion.Euler(0, targetSteer, 0);
            frontWheel.localRotation = Quaternion.Slerp(frontWheel.localRotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    void FixedUpdate()
    {
        // --- APLICA A ROTAÇÃO E MOVIMENTO ---

        // 1. O corpo da bicicleta (pai) controla a rotação para os lados
        rb.MoveRotation(Quaternion.Euler(0, rotationY, 0));

        // 2. Aplica as forças de movimento e freio
        if (Activated)
        {
            if (Input.GetKey(KeyCode.W) || hallValue < pedalThreshold)
            {
                rb.AddForce(transform.forward * pedalForce, ForceMode.Force);
            }
        }

        // Lógica de Freio Revisada
        bool isBrakingFront = Input.GetMouseButton(0) || frontBrakeValue > brakeThreshold;
        // O freio traseiro só é considerado se o dianteiro não estiver ativo
        bool isBrakingRear = !isBrakingFront && (Input.GetMouseButton(1) || rearBrakeValue > brakeThreshold);

        if (isBrakingFront)
        {
            // Freio dianteiro: força brusca contra o movimento.
            rb.AddForce(-rb.linearVelocity.normalized * frontBrakeForce, ForceMode.Force);
            rb.linearDamping = defaultDrag; // Usa o atrito padrão
        }
        else if (isBrakingRear)
        {
            // Freio traseiro: aumenta o atrito (drag) para um deslize suave.
            rb.linearDamping = rearBrakeDrag;
        }
        else
        {
            // Nenhum freio: aplica o atrito padrão.
            rb.linearDamping = defaultDrag;
        }

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }
    }

    #region Arduino Communication

    void ConnectToArduino()
    {
        try
        {
            stream = new SerialPort(portName, baudRate);
            stream.ReadTimeout = 20;
            stream.Open();
            isArduinoConnected = true;
            Debug.Log($"<color=green>Arduino conectado com sucesso na porta {portName}</color>");
        }
        catch (Exception e)
        {
            isArduinoConnected = false;
            Debug.LogWarning($"Falha ao conectar ao Arduino: {e.Message}. Usando apenas teclado/rato.");
        }
    }

    void ParseArduinoData(string data)
    {
        string[] pairs = data.Split(';');
        foreach (var pair in pairs)
        {
            if (string.IsNullOrEmpty(pair)) continue;
            string[] keyValue = pair.Split(':');
            if (keyValue.Length == 2)
            {
                string key = keyValue[0].Trim();
                int.TryParse(keyValue[1], out int value);

                switch (key)
                {
                    case "guidao":
                        guidaoReadings.Enqueue(value);
                        if (guidaoReadings.Count > filterSampleSize)
                        {
                            guidaoReadings.Dequeue();
                        }
                        break;
                    case "freioE":
                        frontBrakeValue = value;
                        break;
                    case "freioD":
                        rearBrakeValue = value;
                        break;
                    case "hall":
                        hallValue = value;
                        break;
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        if (stream != null && stream.IsOpen) { stream.Close(); }
    }

    #endregion
}