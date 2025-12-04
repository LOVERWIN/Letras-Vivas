using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TensorFlowLite;
using TMPro;


public class RedNeuronalABC : MonoBehaviour
{
    [SerializeField, FilePopup("*.tflite")] string fileName = "alfabeto_model.tflite";
    [SerializeField] TextMeshProUGUI outputTextView;
    [SerializeField] ComputeShader compute = null;
    
    private string[] clases = new string[]
    {
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
        "K", "L", "M", "N", "Ñ", "O", "P", "Q", "R", "S",
        "T", "U", "V", "W", "X", "Y", "Z",
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j",
        "k", "l", "m", "n", "ñ", "o", "p", "q", "r", "s",
        "t", "u", "v", "w", "x", "y", "z"
    };
    Interpreter interpreter;

    bool isProcessing = false;
    float[,] inputs = new float[120, 120];
    float[] outputs = new float[54];
    ComputeBuffer inputBuffer;

    System.Text.StringBuilder sb = new System.Text.StringBuilder();

    void Start()
    {
        var options = new InterpreterOptions()
        {
            threads = 2,
        };
        interpreter = new Interpreter(FileUtil.LoadFile(fileName), options);
        interpreter.ResizeInputTensor(0, new int[] { 1, 120, 120, 1 });
        interpreter.AllocateTensors();

        inputBuffer = new ComputeBuffer(120 * 120, sizeof(float));
        var outputShape = interpreter.GetOutputTensorInfo(0).shape;
        Debug.Log("Output shape: " + string.Join(",", outputShape));

    }

    void OnDestroy()
    {
        interpreter?.Dispose();
        inputBuffer?.Dispose();
    }

    public void OnDrawTexture(RenderTexture texture)
    {
        if (!isProcessing)
        {
            Invoke(texture);
        }
    }

    void Invoke(RenderTexture texture)
    {
        isProcessing = true;

        compute.SetTexture(0, "InputTexture", texture);
        compute.SetBuffer(0, "OutputTensor", inputBuffer);
        compute.Dispatch(0, 120 / 4, 120 / 4, 1);
        inputBuffer.GetData(inputs);

        float startTime = Time.realtimeSinceStartup;
        interpreter.SetInputTensorData(0, inputs);
        interpreter.Invoke();
        interpreter.GetOutputTensorData(0, outputs);
        float duration = Time.realtimeSinceStartup - startTime;

        sb.Clear();
        //sb.AppendLine($"Process time: {duration: 0.00000} sec");
        //sb.AppendLine("---");

        float maxValor = float.MinValue;
        int maxIndice = -1; // Aquí guardamos en qué índice está
        for (int i = 0; i < outputs.Length; i++)
        {
            
            if (outputs[i] > maxValor)
            {
                maxValor = outputs[i]; // Guardamos el más grande
                maxIndice = i; // Guardamos el índice
            }
        }
        
       
// Ahora usamos el arreglo de clases para obtener la letra
        string letraDetectada = clases[maxIndice];

        sb.AppendLine($"{letraDetectada}");
        //sb.AppendLine($"Confianza: {maxValor:0.00}");
        outputTextView.text = sb.ToString();
        Debug.Log("porvcentaje de inferencia: "+ maxValor);
        

        isProcessing = false;
    }
}
