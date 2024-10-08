class LAB_MAI
{
    static void Main(string[] args)
    {
        int[,] Analogs = new int[7, 3] { { 6, 9, 8 }, { 5, 8, 9 }, { 5, 9, 7 }, { 8, 5, 4 }, { 6, 8, 5 }, { 8, 6, 5 }, { 7, 9, 6 } };

        int[] weight = new int[7] { 9, 7, 8, 6, 5, 3, 5 };

        PrintMatrix(Analogs, "Матрица оценки трёх аналогов");
        PrintMatrix(weight, "Матрица весов");

        int[] newAnalog = new int[7] { 6, 7, 6, 7, 6, 4, 8 };
        PrintMatrix(newAnalog, "Новый аналог");

        int[,] unionAnalogs = unionMatrixAnalog(Analogs, newAnalog);
        PrintMatrix(unionAnalogs, "Объединенная матрица");

        MAI(Analogs, weight, true);
        Console.WriteLine("\n\n\n");
        MAI(unionAnalogs, weight, true);

        Console.WriteLine("\n\n\n");
        Console.WriteLine("МАИ+");
        Console.WriteLine("\n\n\n");
        MAIplus(Analogs, weight);
        MAIplus(unionAnalogs, weight);
    }

    static double[,] CreatePCC(int[] weight)
    {
        double[,] PCC = new double[weight.Length, weight.Length];
        for (int i = 0; i < PCC.GetLength(0); i++)
        {
            for (int j = 0; j < PCC.GetLength(0); j++)
            {
                PCC[i, j] = (double)weight[i] / weight[j];
            }
        }
        return PCC;
    }

    static double[] CreateMiddleGeom(int[] weight, double[,] PCC)
    {
        double[] middleGeom = new double[weight.Length];
        for (int i = 0; i < middleGeom.GetLength(0); i++)
        {
            double middle = 1;
            for (int j = 0; j < middleGeom.GetLength(0); j++)
            {
                middle *= PCC[i, j];
            }
            middleGeom[i] = Math.Pow(middle, 1.0 / 7.0);
        }
        return middleGeom;
    }
    static double CreateSummGeom(int[] weight, double[] middleGeom)
    {
        double summGeom = 0;
        for (int i = 0; i < middleGeom.GetLength(0); i++)
        {
            summGeom += middleGeom[i];
        }
        return summGeom;
    }
    static double[] CreateNorm(int[] weight, double[] middleGeom, double summGeom)
    {
        double[] norm = new double[weight.Length];
        for (int i = 0; i < norm.GetLength(0); i++)
        {
            norm[i] = middleGeom[i] / summGeom;
        }
        return norm;
    }
    static double CreateLmax(int[] weight, double[,] PCC, double[] norm)
    {
        double Lmax = 0;
        for (int i = 0; i < PCC.GetLength(0); i++)
        {
            double summ = 0;
            for (int j = 0; j < PCC.GetLength(0); j++)
            {
                summ += PCC[j, i];
            }
            Lmax += summ * norm[i];
        }
        return Lmax;
    }

    static void MAI(int[,] Analog, int[] weight, Boolean print)
    {
        //Console.WriteLine(weight.Length);
        double[,] PCC = CreatePCC(weight);
        double[] middleGeom = CreateMiddleGeom(weight,PCC);
        double summGeom = CreateSummGeom(weight,middleGeom);
        double[] norm = CreateNorm(weight, middleGeom, summGeom);
        double Lmax = CreateLmax(weight, PCC, norm);

        if (print)
        {
            PrintMatrix(PCC, "PCC");
            PrintMatrix(middleGeom, "middleGeom");
            Console.WriteLine($"Сумма ср.геометр.");
            PrintMatrix(norm, "norm");
            Console.WriteLine($"Lmax: {Lmax}");
        }           

        List< ReleatedMAIMatrix > tempMatrix = new List< ReleatedMAIMatrix >();

        for (int i = 0; i < Analog.GetLength(0); i++)
        {
            tempMatrix.Add(new ReleatedMAIMatrix(Analog, i));
            if (print)
            {
                Console.WriteLine();
                Console.WriteLine($"C{i + 1}");
                PrintMatrix(tempMatrix[i].PCC, "PCC");
                PrintMatrix(tempMatrix[i].middleGeom, "middleGeom");
                Console.WriteLine("Сумма средних геом: " + tempMatrix[i].summGeom);
                PrintMatrix(tempMatrix[i].norm, "norm");
                Console.WriteLine("Lmax: " + tempMatrix[i].Lmax);
            }
        }

        double[] result = new double[Analog.GetLength(1)];
        for (int i = 0;i < Analog.GetLength(1); i++)
        {
            result[i] = 0;
            for (int j = 0;j < Analog.GetLength(0); j++)
            {
                result[i] += norm[j] * tempMatrix[j].norm[i];
            }
        }
        if (print)
            PrintMatrix(result, "Результат");
    }

    public class ReleatedMAIMatrix
    {
        public double[,] PCC;
        public double[] middleGeom;
        public double summGeom = 0;
        public double[] norm;
        public double Lmax = 0;

        public ReleatedMAIMatrix(int[,] Analog, int i)
        {
            PCC = new double[Analog.GetLength(1),Analog.GetLength(1)];
            for(int j = 0;j < Analog.GetLength(1); j++)
            {
                for(int k = 0; k < Analog.GetLength(1); k++)
                {
                    PCC[j,k] = (double) Analog[i,j] / Analog[i,k];
                }
            }
            //middle
            middleGeom = new double[Analog.GetLength(1)];
            for (int k = 0; k < middleGeom.GetLength(0); k++)
            {
                double middle = 1;
                for (int j = 0; j < middleGeom.GetLength(0); j++)
                {
                    middle *= PCC[k, j];
                }
                middleGeom[k] = Math.Pow(middle, 1.0 / Analog.GetLength(1));
            }
            //summGeom
            for (int k = 0; k < middleGeom.GetLength(0); k++)
            {
                summGeom += middleGeom[k];
            }
            //norm
            norm = new double[Analog.GetLength(1)];
            for (int k = 0; k < norm.GetLength(0); k++)
            {
                norm[k] = middleGeom[k] / summGeom;
            }
            //Lmax
            for (int k = 0; k < PCC.GetLength(0); k++)
            {
                double summ = 0;
                for (int j = 0; j < PCC.GetLength(0); j++)
                {
                    summ += PCC[j, k];
                }
                Lmax += summ * norm[k];
            }
        }
    }

    static void MAIplus(int[,] Analog, int[] weight)
    {
        int countAl = Analog.GetLength(1);
        int countCr = Analog.GetLength(0);

        double[,] PCC = CreatePCC(weight);
        double[] middleGeom = CreateMiddleGeom(weight, PCC);
        double summGeom = CreateSummGeom(weight, middleGeom);
        double[] norm = CreateNorm(weight, middleGeom, summGeom);
        double Lmax = CreateLmax(weight, PCC, norm);

        List<ReleatedMAIMatrix> tempMatrix = new List<ReleatedMAIMatrix>();

        for (int i = 0; i < countCr; i++)
        {
            tempMatrix.Add(new ReleatedMAIMatrix(Analog, i));
        }

        double[,] summaryTable = new double[countAl, countCr];

        for(int i = 0; i < countCr; i++)
        {
            for(int j = 0; j < countAl; j++)
            {
                summaryTable[j, i] = tempMatrix[i].norm[j];
            }
        }
        PrintMatrix(summaryTable, "Смежная таблица");

        double[,,] Bhatch = new double[countCr, countAl, countAl * 2];
        for(int i = 0;i < countCr; i++)
        {
            for (int j = 0;j < countAl; j++)
            {
                for( int k = 0; k < countAl*2; k++)
                {
                    if(k%2 == 0)
                    {
                        Bhatch[i, j, k] = summaryTable[j, i];
                        
                    }
                    else
                    {
                        Bhatch[i, j, k] = summaryTable[k / 2, i];
                    }
                }
            }
        }
        PrintMatrix(Bhatch, "Матрицы B'");


        double[,,] Bstar = new double[countCr, countAl, countAl * 2];
        for (int i = 0; i < countCr; i++)
        {
            for (int j = 0; j < countAl; j++)
            {
                for (int k = 0; k < countAl * 2; k++)
                {
                    if (k % 2 == 0)
                    {
                        Bstar[i, j, k] = Bhatch[i,j,k]/(Bhatch[i,j,k]+Bhatch[i,j,k+1]);

                    }
                    else
                    {
                        Bstar[i, j, k] = Bhatch[i, j, k] / (Bhatch[i, j, k] + Bhatch[i, j, k - 1]);
                    }
                }
            }
        }
        PrintMatrix(Bstar, "Матрицы B*  ");

        double[,] B = new double[countAl, countAl * 2];
        for (int i = 0; i < countAl; i++)
        {
            for (int j = 0; j < countAl*2; j++)
            {
                B[i, j] = 0;
                for (int k = 0; k < countCr; k++)
                {
                    B[i,j] += norm[k] * Bstar[k,i,j];
                }
            }
        }
        PrintMatrix(B, "Матрицы B");

        double[] resultW = new double[countAl];
        double summResultW = 0;
        double[] normRsultW = new double[countAl];
        for (int i = 0;i < countAl; i++)
        {
            resultW[i] = 0;
            for(int j = 0;j < countAl*2; j+=2)
            {
                resultW[i] += B[i, j];
            }
            summResultW += resultW[i];
        }
        for(int i = 0; i < countAl; i++)
        {
            normRsultW[i] = resultW[i]/summResultW;
        }
        PrintMatrix(resultW, "ResultW");
        Console.WriteLine(summResultW);
        PrintMatrix(normRsultW, "NormResultW");
    }

    static int[,] unionMatrixAnalog(int[,] Analogs, int[] newAnalog)
    {
        int[,] result = new int[Analogs.GetLength(0),Analogs.GetLength(1)+1];
        for(int i = 0; i < Analogs.GetLength(0); i++)
        {
            for(int j = 0; j < Analogs.GetLength(1) + 1; j++)
            {
                if (j == Analogs.GetLength(1))
                    result[i, j] = newAnalog[i];
                else
                    result[i, j] = Analogs[i, j];
            }
        }
        return result;
    }

    static void PrintMatrix(int[,] matrix, string message)
    {
        int rows = matrix.GetLength(0);
        int columns = matrix.GetLength(1);

        Console.WriteLine($"{message}:");

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Форматируем вывод для аккуратного отображения
                Console.Write($"{matrix[i, j],6} ");
            }
            Console.WriteLine(); // Переход на новую строку после строки матрицы
            
        }
    }
    static void PrintMatrix(int[] matrix, string message)
    {
        int rows = matrix.GetLength(0);

        Console.WriteLine($"{message}:");

        for (int i = 0; i < rows; i++)
        {
            Console.Write($"{matrix[i],6} ");
        }
        Console.WriteLine();
    }
    static void PrintMatrix(double[,] matrix, string message)
    {
        int rows = matrix.GetLength(0);
        int columns = matrix.GetLength(1);

        Console.WriteLine($"{message}:");

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Форматируем вывод для аккуратного отображения
                Console.Write($"{matrix[i, j].ToString("F4"),6} ");
            }
            Console.WriteLine(); // Переход на новую строку после строки матрицы

        }
    }
    static void PrintMatrix(double[] matrix, string message)
    {
        int rows = matrix.GetLength(0);

        Console.WriteLine($"{message}:");

        for (int i = 0; i < rows; i++)
        {
            Console.Write($"{matrix[i].ToString("F4"),6} ");
        }
        Console.WriteLine();
    }
    static void PrintMatrix(double[,,] matrix, string message)
    {
        int table = matrix.GetLength(0);
        int rows = matrix.GetLength(1);
        int column = matrix.GetLength(2);

        Console.WriteLine($"{message}:");

        for (int i = 0; i < table; i++)
        {
            Console.WriteLine($"B{i}:");
            for (int j = 0;j < rows; j++)
            {
                for(int k = 0; k < column; k++)
                {
                    Console.Write($"{matrix[i,j,k].ToString("F4"),6} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
