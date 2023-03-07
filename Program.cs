// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
RestartDomain restart = new RestartDomain();


Console.WriteLine("Initiating destruction sequence");
restart.InitiateDestructionSequence();

// await Task.Delay(21000);

Console.WriteLine("Restarting the domain");
restart.FinalBlow();

// await Task.Delay(60000);

restart.Dispose();