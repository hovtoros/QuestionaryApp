using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace QuestionaryApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var templateJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "questionaryTemplate.json");
            string jsonString;
            using (var reader = new StreamReader(templateJsonPath))
            {
                jsonString = reader.ReadToEnd();
            }

            var questionsList = JsonConvert.DeserializeObject<List<Questionary>>(jsonString);
            int questionCounter = 1;
            int rightAnswers = 0;
            foreach (var question in questionsList)
            {
                Console.WriteLine($"Question {questionCounter++}: {question.Question}");
                int answersCount = 1;
                foreach (var possibleAnswer in question.PossibleAnswers)
                {
                    Console.WriteLine($"Answer {answersCount++}: {possibleAnswer}");
                }

                var isUserInputValid = false;
                string userInput = String.Empty;
                while (isUserInputValid != true)
                {
                    userInput = Console.ReadLine();
                    isUserInputValid = UserInputValidator(userInput, questionsList.Count);
                    if(!isUserInputValid)
                        Console.WriteLine("Please enter a valid number.");
                }

                var userInputNumbered = Int32.Parse(userInput);
                if (question.RightAnswer.Equals(question.PossibleAnswers[userInputNumbered - 1]))
                    rightAnswers++;

                Console.WriteLine("\n");
            }

            Console.WriteLine($"Results: right answers - {rightAnswers}, wrong answers - {questionsList.Count - rightAnswers}");
        }
        private static bool UserInputValidator(string input, int upperBound)
        {
            bool isInputInteger = Int32.TryParse(input, out var resultedInteger);
            if (isInputInteger && resultedInteger <= upperBound && resultedInteger >= 1)
                return true;
            return false;
        }
    }
}
