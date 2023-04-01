using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleksMethod
{
    internal class SimplexMethod
    {
        public static void startSimplexMethod()
        {
            double[,] matrix=initMatrix();
            Console.WriteLine("Исходная матрица:");
            showMatrix(matrix);
            int rows = matrix.GetUpperBound(0) + 1; //увеличиваем матрицу для предотвращения исключений
            int columns = matrix.Length / rows;
            double max;
            int i = 1;
            do
            {
                max = findMaxEl(matrix, rows, columns);
                if (max > 0)
                {
                    methodGayssa(matrix, max, rows, columns);
                    Console.WriteLine("Промежуточный результат №{0}",i);
                    showMatrix(matrix);
                    i++;
                } //условие решения задачи, зацикливаем метод гаусса
            } while (max > 0);
            Console.WriteLine();
            Console.WriteLine("Результат:");
            showMatrix(matrix);
            
        }
        static double[,] initMatrix() //инициализация
        {
            double[,] matrix =  { { 1, -1, 1, 0, 2 }, 
                { 2, 1, 0, 1, 6 }, 
                { 3, 2, 0, 0, 0 } };
            return matrix;
            //Console.WriteLine("Введите количество строк матрицы (с учетом последней строки функции)");
            //int rows = Convert.ToInt32(Console.ReadLine());
            //Console.WriteLine("Введите количество столбцов матрицы (с учетом столбца коэффициентов)");
            //int columns = Convert.ToInt32(Console.ReadLine());
            //double[,] matrix = new double[rows, columns];
            //for (int i = 0; i < rows; i++)
            //{
            //    for (int j = 0; j < columns; j++)
            //    {
            //        Console.WriteLine("Введите элемент с индексами [{0},{1}]", i + 1, j + 1);
            //        matrix[i, j] = Convert.ToDouble(Console.ReadLine());
            //    }
            //}
        }
        static void showMatrix(double[,] matrix) //вывод на экран
        {
            int rows = matrix.GetUpperBound(0) + 1;
            int columns = matrix.Length / rows;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(matrix[i,j]+" ");
                }
                Console.WriteLine();
            }
        }
        static double findMaxEl(double[,] matrix, int rows, int columns) //поиск максимального элемента в последней строке
        {
            double max = 0;
            for (int i = rows - 1; i < rows; i++) //ничего не увеличится, учитываем только строку с функцией
            {
                for (int j = 0; j < columns; j++)
                {
                    if (max < matrix[i, j]) max = matrix[i, j];
                }
            }
            return max;
        }
        static int findColumn(double[,] matrix, double max, int rows, int columns) //поиск стобаца
        {
            int findColumn = 0;
            for (int i = rows - 1; i < rows; i++) //ничего не увеличится, учитываем только строку с функцией
            {
                for (int j = 0; j < columns; j++)
                {
                    if (max == matrix[i, j]) 
                    {
                        findColumn = j; //нашли индекс
                        break; 
                    }
                }
            }
            return findColumn;
        }
        static int findRow(double[,] matrix, int column, int rows, int columns) //поиск строки
        {
            double[] divided = new double[rows]; //матрица для разделенных элементов в строке коэффициентов
            for (int i = 0; i < rows; i++) //цикл только по строкам
            {
                //условие для предотвращения лишних операций и неверных подсчетов
                if (matrix[i, column] != 0 && ((matrix[i, column] > 0 && matrix[i, columns - 1] > 0) || ((matrix[i, column] < 0 && matrix[i, columns - 1] < 0))))
                { 
                    divided[i] = matrix[i, columns - 1] / matrix[i, column]; //делим элемент столбца коэффициентов на элемент из нужной строки
                }
                else divided[i] = 99; //для того, чтобы избежать нулевых элементов, так как они не должны учитываться далее
            }
            int row = Array.IndexOf(divided, divided.Min()); //поиск индекса минимального элемента в уже разделенном столбце коэффициентов
            return row;
        }
        static double[,] divideOnFoundedEl(double[,] matrix, int row, int column, int rows, int columns) //делим найденные элементы
        {
            double temp = matrix[row, column]; //найденный элемент, на который делим
            for (int i = row; i <= row; i++) //цикл только для одной найденной строки
            {
                for (int j = 0; j < columns; j++) //цикл по столбцам, строка не меняется
                {
                    matrix[i, j] = matrix[i, j] / temp; //делим
                }
            }
            return matrix; //матрица с одной разделенной строчкой (получение значения=1 нужного нам элемента)
        }
        static void divideAllColumns(double[,] matrix, int row, int column, int rows, int columns) //делим все столбцы на соответствующий элемент выделенной строки
        {
            for (int i = 0; i < rows; i++)
            {
                if (i == row) continue; //пропускаем строку с ведущим элементом, её не учитываем
                int temp = 0; //вспомогательная переменная для определения нужды умножения на противоположное число
                if (matrix[i, column] >= 0) temp = -1;
                else temp = 1;
                double Numb = matrix[i, column]; //выбранное число из строки
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = matrix[row, j] * temp * Numb + matrix[i, j];
                }
            }
        }
        static void methodGayssa(double[,] matrix, double max, int rows, int columns) //метод гаусса без цикла
        {
            int column = findColumn(matrix, max, rows, columns); //найденный столбец
            int row = findRow(matrix, column, rows, columns); //найденная строка
            divideOnFoundedEl(matrix, row, column, rows, columns); //делим строку на нужный элемент
            divideAllColumns(matrix, row, column, rows, columns); //делим столбцы
        }
        
    }
}
