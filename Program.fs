open System
open System.Collections
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Todos.Http
open Todos
open TodoInMemory
open TodoMongoDB
open MongoDB.Driver

let routes =
    choose [
        TodoHttp.handlers]

let configureApp (app : IApplicationBuilder) =
    app.UseGiraffe routes

let configureServices (services : IServiceCollection) =
    let mongo = MongoClient ("mongodb://localhost:27017/")
    let db = mongo.GetDatabase "todos"

    services.AddGiraffe() |> ignore
    services.AddTodoMongoDB(db.GetCollection<Todo>("todos")) |> ignore

[<EntryPoint>]
let main _ =
    WebHostBuilder()
        .UseKestrel()
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .Build()
        .Run()
    0