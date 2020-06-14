using System;
using System.Collections.Generic;

namespace FibonacciMethod
{
	class Program
	{
		const int countOfSteps = 20;
		const int nowFunction = 1;
		private delegate double FunctionDelegate(double x);

		private static List<Function> functions = new List<Function>()
		{
			new Function(1, 3, (x) => Math.Abs(x) + Math.Abs(x + 1) - 1),
			new Function(-1.5d, 0, (x) => x / (Math.Pow(x, 2) + 1))
		};

		private static void Main(string[] args)
		{
			// Epsilon
			double[] accuracies = new double[] { 0.1d, 0.01d, 0.0001d };
			Console.WriteLine($"Count of steps = {countOfSteps}.");
			foreach (double accuracy in accuracies)
			{

				// Eng: Calculate length of interval
				// Ru: Считаем длину интервала
				double intervalLength = functions[nowFunction].EndInterval - functions[nowFunction].StartInterval;

				// Eng: Interval length divide accuracy
				// Ru: Длина интервала деленная на точность т.е. эпсилон
				double intervalLengthDivideAccuracy = intervalLength / accuracy;

				Console.WriteLine($"Accuracy (epsilon) = {accuracy}.");
				Console.WriteLine($"{"k",-4} {"aK",-15} {"bK",-15} {"lambdaK",-15} {"nyuK",-15} {"funcLambda",-15} {"funcNyu",-15}");

				// Eng: Step 2
				// Ru: Шаг 2
				int countOfIteration = countOfSteps;

				// Eng: Step 4
				// Ru: Шаг 4
				while (intervalLengthDivideAccuracy <= getFibonacciNumber(countOfIteration + 2) && intervalLengthDivideAccuracy >= getFibonacciNumber(countOfIteration + 1))
				{
					countOfIteration++;
				}

				// Eng: Step 5
				// Ru: Шаг 5
				double lambdaK = functions[nowFunction].StartInterval + (getFibonacciNumber(countOfIteration) / getFibonacciNumber(countOfIteration + 2) * intervalLength);
				double nyuK = functions[nowFunction].StartInterval + (getFibonacciNumber(countOfIteration + 1) / getFibonacciNumber(countOfIteration + 2) * intervalLength);

				// Eng: Step 6
				// Ru: Шаг 6
				double aK, bK;
				if (functions[nowFunction].FunctionDelegate(lambdaK) <= functions[nowFunction].FunctionDelegate(nyuK))
				{
					aK = functions[nowFunction].StartInterval;
					bK = nyuK;
				}
				else
				{
					aK = lambdaK;
					bK = functions[nowFunction].EndInterval;
				}

				// Eng: Step 7
				// Ru: Шаг 7
				int k = 2;

				double funcLambda = functions[nowFunction].FunctionDelegate(lambdaK);
				double funcNyu = functions[nowFunction].FunctionDelegate(nyuK);
				while (true)
				{

					// Eng: Step 8
					// Ru: Шаг 8
					if (funcLambda <= funcNyu)
					{
						lambdaK = aK + (getFibonacciNumber(countOfIteration - k + 1) / getFibonacciNumber(countOfIteration + 2) * intervalLength);

						// Eng: Step 9
						// Ru: Шаг 9
						nyuK = lambdaK;
						funcNyu = funcLambda;
					}
					else
					{
						aK = lambdaK;
						lambdaK = nyuK;
						funcLambda = funcNyu;

						// Eng: Step 10
						// Ru: Шаг 10
						nyuK = aK + (getFibonacciNumber(countOfIteration - k + 2) / getFibonacciNumber(countOfIteration + 2) * intervalLength);
					}

					// Eng: Step 11
					// Ru: Шаг 11
					if (funcLambda <= funcNyu)
					{
						bK = nyuK;
					}
					else
					{
						aK = lambdaK;
					}

					Console.Write($"{k,-4} {Math.Round(aK, 4),-15} {Math.Round(bK, 4),-15} {Math.Round(lambdaK, 4),-15} {Math.Round(nyuK, 4),-15}");
					Console.WriteLine($" {Math.Round(funcLambda, 4),-15} {Math.Round(funcNyu, 4),-15}");

					// Eng: Step 12
					// Ru: Шаг 12
					if (k >= countOfIteration)
					{
						break;
					}
					k++;
				}

				// Eng: Uncertainty interval
				// Ru: Интервал неопределенности
				Console.WriteLine($"\nUncertainty interval [{aK}, {bK}].");
				// Eng: Optimal value of x
				// Ru: Оптимальное значение
				Console.WriteLine($"Optimal value of x = {(aK + bK) / 2}.\n");
			}

			Console.ReadKey();

		}

		// Eng: Using Binet's formula to avoid stack overflow
		// Ru: Использую формулу Бинета что бы избежать переполнения стэка
		// https://en.wikipedia.org/wiki/Fibonacci_number#Closed-form_expression
		private static double phi = (1 + Math.Sqrt(5)) / 2;
		private static double getFibonacciNumber(int step)
		{
			return (Math.Pow(phi, step) - Math.Pow(-phi, -step)) / (2 * phi - 1);
		}

		private sealed class Function
		{
			public readonly double EndInterval;
			public readonly double StartInterval;
			public readonly FunctionDelegate FunctionDelegate;

			public Function(double startInterval, double endInterval, FunctionDelegate functionDelegate)
			{
				this.StartInterval = startInterval;
				this.EndInterval = endInterval;
				this.FunctionDelegate = functionDelegate;
			}
		}
	}
}
