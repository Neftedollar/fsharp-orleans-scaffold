namespace Grains
open Microsoft.Extensions.Logging
open FSharp.Control.Tasks.V2
open Contracts.Say
module Say =
  type HelloGrain (log:ILogger<HelloGrain>) =
    inherit Orleans.Grain()
    interface IHello with
      member this.SayHello greeting = task {
          do log.LogInformation(sprintf "SayHello message received: greeting = '%s'" greeting)
          return sprintf "Client said '%s', so HelloGrain says: Hello!" greeting
      }
