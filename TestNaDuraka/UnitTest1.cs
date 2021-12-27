using System.Collections.Generic;
using DurackServer.Controller;
using DurackServer.Model;
using DurackServer.Model.DataType;
using DurackServer.Model.Game;
using DurackServer.networking.PlayerIO;
using DurackServer.networking.Session;
using NUnit.Framework;

public class Tests
{
    private Controller _controller;
    [SetUp]
    public void Setup()
    {

        _controller = new Controller();
    }

    [Test]
    public void Test1()
    {

        var player1 = new OnlinePlayer();
        var player2 = new OnlinePlayer();
        _controller.AddPlayer(player1);
        _controller.AddPlayer(player2);
        _controller.StartGameRound(null, new Command
        {
            PlayerId = 0,
            Code = CommandCodes.ThrowCards,
            Cards = new List<CardType> {player1.hand[0]}
        });
    }
}