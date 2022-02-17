using ParallelProject.Main;
using System.Diagnostics;

public class Program {

    /*-----------------------------------------------------------------------------------------------*/
    /*          Guillermo Adrián Alonso Arámbula - Matrícula: 1812367 - Carrera: ITS                 */
    /*-----------------------------------------------------------------------------------------------*/
    /* Sistemas Distribuidos y Paralelos Sabatino (N4) -  M.I.I Carlos Adrián Pérez Cortez - EJ2022  */
    /*-----------------------------------------------------------------------------------------------*/
    /* Programa que realiza la generación de enteros que son almacenados en un archivo de texto, así */
    /* mismo son almacenados en una lista y realiza la suma de cada uno de sus elementos (Ejecución  */
    /* secuencial y con hilos).                                                                      */
    /*-----------------------------------------------------------------------------------------------*/

    static Random rd = new Random();
    static readonly String address = "NumerosGenerados.txt";

    // Variables de ejecución secuencial.

    static int resultadoSecuencial = 0;
    static int resultadoHilos = 0;
    static long tiempoSecuencial = 0;

    // MÉTODO PRINCIPAL
    public static void Main(string[] args)
    {
        int numHilos = rd.Next(6, 12);
        NumberGenerator(rd.Next(1500, 3000) * numHilos); // GENERADOR DE NÚMEROS A ARCHIVO TXT.

        Thread[] thread = new Thread[numHilos];             // ARREGLOS DE CAPTURA DE TIEMPOS, INSTANCIA Y RESULTADOS PARA EJECUCIÓN DE HILOS.
        SumLogic[] sLog = new SumLogic[numHilos];
        Stopwatch[] stArray = new Stopwatch[numHilos];
        long[] stCounter = new long[numHilos];

        List<int> intList = NumberSource(); // OBTENCIÓN DEL ARREGLO DE NÚMEROS BASADOS EN EL ARCHIVO TXT.
        Console.WriteLine("-----------------------------------------------------------------------------------------------");

        Stopwatch sw = Stopwatch.StartNew(); // CONTEO DE EJECUCIÓN SECUENCIAL
        for (int i = 0; i < intList.Count; i++) { 
            resultadoSecuencial = resultadoSecuencial + intList[i];
            if (i == intList.Count - 1) {
                Console.WriteLine("\n -- El resultado secuencial total es igual a: " + resultadoSecuencial);
            }
        }
        sw.Stop();
        tiempoSecuencial = sw.ElapsedTicks * 100; // TIEMPO MEDIDO DE EJECUCIÓN SECUENCIAL
        Console.WriteLine(" -- El tiempo de ejecución secuencial es igual a: " + tiempoSecuencial + " nanosegundos.\n");
        Console.WriteLine("-----------------------------------------------------------------------------------------------");

        Parallel.For(0, numHilos, i => // BUCLE PARALELO QUE INICIA LOS HILOS DE EJECUCIÓN
        {      
            thread[i] = new Thread((ThreadStart) =>
            {
                
                int limInf = (intList.Count / numHilos) * i;
                int limSup = (intList.Count / numHilos) * (i + 1);

                sLog[i] = new SumLogic(i,0,limInf,limSup);
                
                stArray[i] = Stopwatch.StartNew(); // EJECUCIÓN DE TIEMPO DE OBTENCIÓN DE SUMA CON HILOS
                for (int n = sLog[i].limInf; n < sLog[i].limSup; n++)
                {
                    sLog[i].result += intList[n];
                }
                stArray[i].Stop();
                stCounter[i] = stArray[i].ElapsedTicks * 100;

                resultadoHilos += sLog[i].result;
            });
            thread[i].Start();      
        });

        // IMPRESIÓN DE DATOS EN CONSOLA

        for (int i = 0; i < numHilos; i++) {
            Console.WriteLine("-----------------------------------------------------------------------------------------------");
            Console.WriteLine(" -- Hilo de ejecución [" + (i + 1) + "] tuvo un resultado de: " + sLog[i].result + ".");
            Console.WriteLine(" -- Tiempo de ejecución de hilo [" + (i + 1) + "] con: " + stCounter[i] + " nanosegundos.\n");
            Console.WriteLine("\n Limite Inferior -- " + (intList.Count / numHilos) * i);
            Console.WriteLine(" Limite Superior -- " + (intList.Count / numHilos) * (i + 1) + "\n");
            Console.WriteLine("-----------------------------------------------------------------------------------------------");
        }


        Console.WriteLine(" -- Resultado ejecución hilos: " + resultadoHilos);
    }

    public static void NumberGenerator(int limEnteros) { // FUNCIÓN QUE GENERA NUMEROS ALEATORIOS Y QUE LOS GUARDA EN UN ARCHIVO DE TEXTO.

        List<int> intSample = new List<int>();
        StreamWriter sw = new StreamWriter(address);

        for (int i = 0; i < limEnteros; i++)
        {
            intSample.Add(rd.Next(0, 1000));
            sw.WriteLine(intSample[i]);
        }
        sw.Close();

    }

    public static List<int> NumberSource() { // FUNCIÓN QUE OBTIENE LOS NUMEROS DEL ARCHIVO DE TEXTO Y QUE LOS COLLECIONA EN UNA LISTA DE ENTEROS.
        List<int> intSample = new List<int>();

        try
        {
            StreamReader sr = new StreamReader(address);

            while (!sr.EndOfStream) {
                String lineSample = sr.ReadLine();
                intSample.Add(Int32.Parse(lineSample));            
            }
            sr.Close();
        }
        catch (Exception e) {
            Console.WriteLine("No se pudo obtener la lista de enteros solicitada.\nExcepción: " + e.StackTrace);
        }

        return intSample;
    }
}

