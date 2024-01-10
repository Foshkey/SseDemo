# Server-Sent Events Demo

This is a very bare-bones project demonstrating server-sent events. When the page is loaded, a request is sent to the `/sse` endpoint which responds with an event stream type. Every time a message is entered, it is posted to the server and the server sends it to the event stream.

## Running the Demo

Either load in vscode and hit F5 (Run & Debug) or run the command `dotnet run` and navigate to the URL listed in the logs (typically `http://localhost:XXXX`)

## Limitations Observed

Seems like we need to keep the connection open by looping with a `Task.Delay`. Without that bit of code, the context gets disposed and the event stream can't be written to anymore. There is code included to remove the context when it disconnects, though this is largely untested.