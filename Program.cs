// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using RestartAutomation;
using RestartAutomation.Modules;
Global.infoString = "";
// Application code should start here.


IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

(int initialLeft, int initialTop) = Console.GetCursorPosition();


Console.WriteLine("Hello, World!");

Console.WriteLine("\nUse ⬆ and ⬇ arrow to navigate:");
ConsoleKeyInfo key;
int option = 1, moduleOption = 1, action = 1;
bool isDomainSelected = false, isModuleSelected = false, isManual = false, isActionSelected = false;
(int left, int top) = Console.GetCursorPosition();
string color = "✅            \u001b[32m";
string emptyString = "             ";
string url = "";
LoginCredentials loginCredentials = new LoginCredentials();


while (!isActionSelected)
{
    Console.SetCursorPosition(left, top);
    Console.WriteLine($"{(action == 1 ? color : emptyString)}Only restart the domain\u001b[0m");
    Console.WriteLine($"{(action == 2 ? color : emptyString)}Publish file and restart domain\u001b[0m");
    Console.WriteLine($"{(action == 3 ? color : emptyString)}exit\u001b[0m");

    key = Console.ReadKey(true);
    switch (key.Key)
    {
        case ConsoleKey.UpArrow:
            action--;
            action = action <= 0 ? 3 : action;
            break;

        case ConsoleKey.DownArrow:
            action++;
            action = action >= 4 ? 1 : action;
            break;

        case ConsoleKey.Enter:
            isActionSelected = true;
            Console.Clear();
            break;
    }
}

if (action == 3) Environment.Exit(1);

if (action == 2) ModuleSelection();

RestartSequence();






void RestartSequence()
{
    Console.Clear();

    Console.SetCursorPosition(initialLeft, initialTop);
    Console.WriteLine("Select Domain to restart");

    (left, top) = Console.GetCursorPosition();

    while (!isDomainSelected)
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
                isDomainSelected = true;
                isManual = true;
                break;
        }
    }



    switch (option)
    {
        case 1:
            url = "192.168.10.6:888";
            loginCredentials = config.GetRequiredSection("Credentials").GetRequiredSection("DEV").Get<LoginCredentials>()!;
            break;
        case 2:
            url = "192.168.10.6:5555";
            loginCredentials = config.GetRequiredSection("Credentials").GetRequiredSection("QA").Get<LoginCredentials>()!;
            break;
        case 3:
            Console.WriteLine("Enter the url to restart");
            url = Console.ReadLine() ?? "192.168.10.6:888";

            Console.WriteLine($"Enter the usernme for {url}");
            loginCredentials.UserName = Console.ReadLine() ?? "";

            Console.WriteLine($"Enter the password for {url}");
            loginCredentials.Password = Console.ReadLine() ?? "";

            break;
    }
    Console.Clear();
    Global.infoString += $"\nSelected Domain : {url}";
    Global.infoString += $"\nInitiating restart sequence sequence";
    Console.WriteLine(Global.infoString);
    // Console.WriteLine(loginCredentials.UserName);
    RestartDomain restart = new RestartDomain(url, isManual, loginCredentials);
    Console.WriteLine();
    restart.InitiateDestructionSequence();
}


void ModuleSelection()
{
    Console.Clear();
    Console.SetCursorPosition(initialLeft, initialTop);
    Console.WriteLine("Select Module");
    PathInfo pathInfo = config.GetRequiredSection("PathInfo").Get<PathInfo>() ?? new();

    (left, top) = Console.GetCursorPosition();
    while (!isModuleSelected)
    {
        Console.SetCursorPosition(left, top);

        int i = 1;
        foreach (var item in pathInfo.ModuleInfos)
        {
            Console.WriteLine($"{(moduleOption == i ? color : emptyString)}{item.Name}\u001b[0m");
            i++;

        }
        Console.WriteLine($"{(moduleOption == i ? color : emptyString)}Cancle Module update and go to restart\u001b[0m");



        key = Console.ReadKey(true);
        switch (key.Key)
        {
            case ConsoleKey.UpArrow:
                moduleOption--;
                moduleOption = moduleOption <= 0 ? pathInfo.ModuleInfos.Count() + 1 : moduleOption;
                break;

            case ConsoleKey.DownArrow:
                moduleOption++;
                moduleOption = moduleOption > pathInfo.ModuleInfos.Count() + 1 ? 1 : moduleOption;
                break;

            case ConsoleKey.Enter:
                isModuleSelected = true;
                Console.Clear();
                break;
        }
    }

    Console.WriteLine("\n");

    if (moduleOption != pathInfo.ModuleInfos.Count() + 1)
    {

        Console.SetCursorPosition(initialLeft, initialTop);
        Console.WriteLine("Select Area");

        (left, top) = Console.GetCursorPosition();

        ModuleInfo selectedModel = pathInfo.ModuleInfos.Where(x => x.Option == moduleOption).FirstOrDefault() ?? new();

        isModuleSelected = false;
        moduleOption = 1;
        while (!isModuleSelected)
        {
            Console.SetCursorPosition(left, top);
            int i = 1;
            foreach (var item in selectedModel.Areas)
            {
                Console.WriteLine($"{(moduleOption == i ? color : emptyString)}{item}\u001b[0m");
                i++;

            }

            key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    moduleOption--;
                    moduleOption = moduleOption <= 0 ? selectedModel.Areas.Count() : moduleOption;
                    break;

                case ConsoleKey.DownArrow:
                    moduleOption++;
                    moduleOption = moduleOption > selectedModel.Areas.Count() ? 1 : moduleOption;
                    break;

                case ConsoleKey.Enter:
                    isModuleSelected = true;
                    Console.Clear();
                    break;
            }
        }

        string sourcePathString = $"{pathInfo.Source}\\{selectedModel.Name}\\Nimble.Web\\Areas\\{selectedModel.Areas[moduleOption - 1]}";
        string destinationPathString = $"{pathInfo.Destination}\\{selectedModel.Areas[moduleOption - 1]}";
        Global.infoString += $"\nSource path: {sourcePathString}";
        Global.infoString += $"\nDestination path: {destinationPathString}";
        Console.WriteLine(Global.infoString);
        new CopyFiles(sourcePathString, destinationPathString, pathInfo.FilesToCopy).Start();
    }




}

public static class Global
{
    public static string infoString;
}