module Ubictionary.LanguageServer.Program

open Ubictionary.LanguageServer.LanguageServer
open Serilog
open System

let setupLogging = 
    Log.Logger <- LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.FromLogContext()
        .WriteTo.File("log.txt")
        .CreateLogger();

let private startWithConsole = 
    setupAndStartLanguageServer (Console.OpenStandardInput()) (Console.OpenStandardOutput())

let startAndWaitForExit = async {
    let! server = startWithConsole
    do! server.WaitForExit |> Async.AwaitTask
    Log.Logger.Information "Server exited."
}

[<EntryPoint>]
let main argv =
    setupLogging
    startAndWaitForExit
    |> Async.RunSynchronously
    0 // return an integer exit code