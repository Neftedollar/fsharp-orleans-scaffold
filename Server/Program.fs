// Learn more about F# at http://fsharp.org

open System
open FSharp.Control.Tasks.V2
open Microsoft.Extensions.Logging
open Orleans
open Orleans.Configuration
open Orleans.Hosting
// open Orleans.Configuration
// open Contracts.Say
open System.Reflection
open Grains.Say
let getSilo () = task {
  use codeGenLoggerFactory = new LoggerFactory();
  let t = typeof<HelloGrain>
  printfn "type %O" t
  let a = Assembly.GetAssembly(t)
  printfn "assembley %O" a
  let host =
     SiloHostBuilder()
      .UseLocalhostClustering()
      .Configure(fun (x:ClusterOptions) -> x.ClusterId <- "dev" ; x.ServiceId <- "OrleansBasic")
      .ConfigureApplicationParts(fun parts -> 
        parts.AddApplicationPart(a).WithReferences().WithCodeGeneration(codeGenLoggerFactory)
          |> ignore)
      .ConfigureLogging(fun (logging:ILoggingBuilder) -> logging.AddConsole() |> ignore)
      .Build()
  do! host.StartAsync()
  return host
}

let runMainAsync () = task {
  let! silo = getSilo()
  printfn "\n\n Press Enter to terminate...\n\n"
  Console.ReadLine() |> ignore
  do! silo.StopAsync()
}
  
  

[<EntryPoint>]
let main argv =
    runMainAsync ()
    |> Async.AwaitTask
    |> Async.RunSynchronously
    0 // return an integer exit code
