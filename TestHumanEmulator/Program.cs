// See https://aka.ms/new-console-template for more information
using XHE;

Console.WriteLine("Hello, World!");

ProcessorBase processorBase = new FavbetProcessor("127.0.0.1:7011");

processorBase.ProcessBet();
