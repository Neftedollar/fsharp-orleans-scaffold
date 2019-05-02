namespace Contracts
open System.Threading.Tasks
module Say =

  type IHello =
    inherit Orleans.IGrainWithIntegerKey
    abstract member SayHello: string -> Task<string>