using DurackServer.Controller;
using DurackServer.networking;
var controller = new Controller();
var server = new Server(controller);
server.Start();