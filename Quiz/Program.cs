using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;

class Quiz
{
    private Dictionary<string, string> credentials;
    private Dictionary<string, string[]> questions;

    public Quiz()
    {
        credentials = LoadCredentials("C:\\Users\\Sami\\source\\repos\\Quiz\\Quiz\\credentials.txt");
        questions = LoadQuestions("C:\\Users\\Sami\\source\\repos\\Quiz\\Quiz\\questions.txt");
    }

    public void Start()
    {
        
        Console.WriteLine("Services:");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Registration");
        Console.WriteLine("3. Admin Login");
        int userChoice = int.Parse(Console.ReadLine());

        string username;
        string password;
        string conf_password;

        switch (userChoice)
        {
            case 1:
                Console.WriteLine("Login:");
                Console.Write("Username: ");
                username = Console.ReadLine();
                Console.Write("Password: ");
                password = Console.ReadLine();

                if (!credentials.ContainsKey(username) || credentials[username] != password)
                {
                    Console.WriteLine("Invalid username or password.");
                    return;
                }

                Console.WriteLine($"Welcome, {username}!");

                List<string> selectedQuestions = SelectRandomQuestions(10);

                int totalMarks = QuizUser(selectedQuestions);

                Console.WriteLine($"Total marks: {totalMarks}/10");

                RecordAttemptHistory(username, totalMarks);
                break;
            case 2:
                Console.WriteLine("Register New User");
                Console.WriteLine("Username: ");
                username= Console.ReadLine();
                Console.WriteLine("Password: ");
                password = Console.ReadLine();
                Console.WriteLine("Enter your password again: ");
                conf_password = Console.ReadLine();

                if(password == conf_password)
                {
                    Console.WriteLine("Registration Successful");
                    RegisterNewUser(username, password);
                }
                else
                {
                    Console.WriteLine("Passwords do not match");
                }
                break;
            case 3:
                Console.WriteLine();
                break;
            default:
                Console.WriteLine("Invalid Input");
                break;
        }
        

        
    }

    private Dictionary<string, string> LoadCredentials(string filePath)
    {
        Dictionary<string, string> credentials = new Dictionary<string, string>();
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            credentials.Add(parts[0], parts[1]);
        }
        return credentials;
    }

    private Dictionary<string, string[]> LoadQuestions(string filePath)
    {
        Dictionary<string, string[]> questions = new Dictionary<string, string[]>();
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split('|');
            string question = parts[0];
            string[] options = parts.Skip(1).ToArray();
            questions.Add(question, options);
        }
        return questions;
    }

    private List<string> SelectRandomQuestions(int count)
    {
        Random random = new Random();
        return questions.Keys.OrderBy(x => random.Next()).Take(count).ToList();
    }

    private int QuizUser(List<string> selectedQuestions)
    {
        int totalMarks = 0;
        foreach (string question in selectedQuestions)
        {
            Console.WriteLine(question);

            string[] options = questions[question];
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{(char)('A' + i)}. {options[i]}");
            }
            Console.Write("Your answer: ");
            string answer = Console.ReadLine().ToUpper();
            if (answer.Length == 1 && answer[0] >= 'A' && answer[0] < 'A' + options.Length)
            {
                if (options[answer[0] - 'A'] == "correct")
                {
                    totalMarks++;
                }
            }
        }
        return totalMarks;
    }

    private void RecordAttemptHistory(string username, int totalMarks)
    {
        string historyFilePath = "C:\\Users\\Sami\\source\\repos\\Quiz\\Quiz\\history.txt";
        string history = $"{DateTime.Now},{username},{totalMarks}";
        File.AppendAllText(historyFilePath, history + Environment.NewLine);
    }

    private void RegisterNewUser(string username, string password)
    {
        string filepath = "C:\\Users\\Sami\\source\\repos\\Quiz\\Quiz\\credentials.txt";
        string newUser = $"{username},{password}";
        File.AppendAllText(filepath, newUser + Environment.NewLine);
    }
}

class Program
{
    static void Main()
    {
        Quiz quiz = new Quiz();
        quiz.Start();
    }
}
