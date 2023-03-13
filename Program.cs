// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Console.WriteLine("\nUse ⬆️ and ⬇️ arrow to navigate:");

ConsoleKeyInfo key;
int option = 1;
bool isSelected = false;
(int left, int top) = Console.GetCursorPosition();
string color = "✅            \u001b[32m";
string emptyString = "             ";
string url = "";
bool isManual = false;

while (!isSelected)
{
    Console.SetCursorPosition(left, top);
    Console.WriteLine($"{(option == 1 ? color : emptyString)}Restart 10.6:888 server\u001b[0m");
    Console.WriteLine($"{(option == 2 ? color : emptyString)}Restart 10.6:5555 server\u001b[0m");
    Console.WriteLine($"{(option == 3 ? color : emptyString)}Restart manual server\u001b[0m");

    key = Console.ReadKey(true);
    switch (key.Key)
    {
        case ConsoleKey.UpArrow:
            option--;
            option = option <= 0 ? 3 : option;
            isManual = false;
            break;

        case ConsoleKey.DownArrow:
            option++;
            option = option >= 4 ? 1 : option;
            isManual = false;
            break;
        case ConsoleKey.Enter:
            isSelected = true;
            isManual = true;
            break;
    }
}

switch (option)
{
    case 1:
        url = "192.168.10.6:888";
        break;
    case 2:
        url = "192.168.10.6:5555";
        break;
    case 3:
        Console.WriteLine("Enter the url to restart");
        url = Console.ReadLine() ?? "192.168.10.6:888";
        break;
}

RestartDomain restart = new RestartDomain(url, isManual);


Console.WriteLine("Initiating restart sequence sequence");
restart.InitiateDestructionSequence();
