using System.Drawing;

static double[,] CreateDiagonallyDominantMatrix(int n = 20, double q = 0.8)
{ 
        var matrix = new double[n, n];
        var random = new Random();

        for (int i = 0; i < n; i++)
        {
            double s = 0;
            for (int j = 0; j < n; j++)
            {
                if (i != j)
                {
                    matrix[i, j] = random.Next(-100, 100);
                    s += Math.Abs(matrix[i, j]);
                }
            }
            matrix[i, i] = random.Next(s / q >= 0 ? (int)(s / q + 1) : (int)(-s / q - 1), 10000);
        }

        return matrix;
}
static double[] MultiplyMatrixVector(double[,] matrix, double[] vector)
{
    int rows = matrix.GetLength(0);
    int cols = matrix.GetLength(1);
    var result = new double[rows];

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            result[i] += matrix[i, j] * vector[j];
        }
    }

    return result;
}
static double Norm(double[] vector)
{
    double sum = 0;
    foreach (var value in vector)
    {
        sum += value * value;
    }
    return Math.Sqrt(sum);
}
static double[] SubtractVectors(double[] a, double[] b)
{
    int length = a.Length;
    double[] result = new double[length];

    for (int i = 0; i < length; i++)
    {
        result[i] = a[i] - b[i];
    }
    return result;
}

static double[] JacobiMethod(double[,] A, double[] b, double tolerance, out int arithmeticOperations)
{
    int n = b.Length;
    double[] x0 = new double[n];
    double[] x = new double[n];
    arithmeticOperations = 0;

    for (int iteration = 0; iteration < 1000; iteration++)
    {
        Array.Copy(x0, x, n);
        for (int i = 0; i < n; i++)
        {
            double sum = 0;
            for (int j = 0; j < n; j++)
            {
                if (i != j)
                {
                    sum += A[i, j] * x[j];
                    arithmeticOperations++; 
                }
            }
            x0[i] = (b[i] - sum) / A[i, i]; 
            arithmeticOperations++; 
        }

        // Проверка на сходимость
        if (Norm(SubtractVectors(x0, x)) <= tolerance)
        {
            break;
        }
    }
    return x0;
}
static void Print2DArray(double[,] array)
{
    int rows = array.GetLength(0);
    int cols = array.GetLength(1);

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            Console.Write($"{array[i, j],6:F2} "); 
        }
        Console.WriteLine();
    }
}

int size = 20;
double[,] A = CreateDiagonallyDominantMatrix(size, 0.5);
Console.WriteLine("Matrix");
Print2DArray(A);
var xVec = Enumerable.Range(1, 20).Select(i => (double)i).ToArray();
double[] b = MultiplyMatrixVector(A, xVec);
Console.WriteLine("Vector");
foreach (double x in b)
{
    Console.WriteLine(x);
}
double[] solution = JacobiMethod(A, b, 1e-2, out int arithmeticOperations);

Console.WriteLine("Найденное решение:");
foreach (var item in solution)
{
    Console.WriteLine(item);
}


Console.WriteLine("Точное решение:");
foreach (var item in xVec)
{
    Console.WriteLine(item);
}



double result = 0;

for (int i = 0; i < solution.Length; i++)
{
    result += Math.Abs(solution[i] - xVec[i]);
}
Console.WriteLine("дельта между решениями :" + result);

Console.WriteLine($"Количество выполненных арифметических операций: {arithmeticOperations}");

/* Для системы из n уравнений (или nn переменных) процесс может быть оценен следующим образом:
На первом шаге нужно будет провести 
n−1
n−1 операций для первого уравнения, затем 
n−2
n−2 для второго и так далее, что в сумме даёт:
Количество операций≈n(n−1)/2
 (для приведения к треугольному виду)

Таким образом, общее количество операций для метода Гаусса можно оценить как:
O(n^3)
 */
int theoreticalOperations = (size * (size * (size + 1))/2); // Оценка порядка O(n^3)
Console.WriteLine($"Теоретическое количество операций для метода Гаусса: {theoreticalOperations}");

