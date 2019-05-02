// Learn more about F# at http://fsharp.org

open Microsoft.Extensions.Logging
open Orleans
open Orleans.Configuration
open System
open System.Reflection
open Orleans
open Orleans.Configuration
open FSharp.Control.Tasks.V2
open Contracts.Say
open Orleans.Hosting


let doClientWork (client:IClusterClient) = task {
  let key = 0L
  let friend = client.GetGrain<IHello>(key)
  let! response = friend.SayHello "Roman"
  do Console.WriteLine("\n\n{0}\n\n", response)
}

let runMainAsync () = task {
  use client =
    ClientBuilder()
     .UseLocalhostClustering()
     .Configure(fun (x:ClusterOptions) -> x.ClusterId <- "dev";x.ServiceId <- "OrleansBasic" )
     .ConfigureLogging(fun (logging:ILoggingBuilder) -> logging.AddConsole() |> ignore)
     .ConfigureApplicationParts(fun parts -> 
        parts.AddApplicationPart(Assembly.GetExecutingAssembly()).WithReferences().WithCodeGeneration() |> ignore)
     .Build()
  do! client.Connect()
  Console.WriteLine("Client successfully connected to silo host \n");
  do! doClientWork client
  do Console.ReadKey() |> ignore
}


[<EntryPoint>]
let main argv =
    // printfn "Hello World from F#!"
    runMainAsync ()
    |> Async.AwaitTask
    |> Async.RunSynchronously
    0 // return an integer exit code
