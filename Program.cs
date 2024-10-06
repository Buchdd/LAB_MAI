using System;

class LAB_MAI
{
    static void Main(string[] args)
    {
        int[,] Analogs = new int[7, 3] { { 6, 9, 8 }, { 5, 8, 9 }, { 5, 9, 7 }, { 8, 5, 4 }, { 6, 8, 5 }, { 8, 6, 5 }, { 7, 9, 6 } };

        int[] weight = new int[7] { 9, 7, 8, 6, 5, 3, 5 };

        PrintMatrix(Analogs, "Матрица оценки трёх аналогов");
        PrintMatrix(weight, "Матрица весов");

        MAI(Analogs, weight);

        int[] newAnalog = new int[7] { 6, 7, 6, 7, 6, 4, 8 };
        PrintMatrix(newAnalog, "Новый аналог");

        int[,] unionAnalogs = unionMatrixAnalog(Analogs, newAnalog);
        PrintMatrix(unionAnalogs, "Объединенная матрица");

        MAIplus(unionAnalogs, weight);
    }

    static void MAI(int[,] Analog, int[] weight)
    {

    }

    static void MAIplus(int[,] Analog, int[] weight)
    {

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
                Console.Write($"{matrix[i, j],4} ");
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
            Console.Write($"{matrix[i],4} ");
        }
        Console.WriteLine();
    }
}