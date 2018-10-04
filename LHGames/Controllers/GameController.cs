using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using LHGames;
using LHGames.DataStructures;

namespace StarterProject.Web.Api.Controllers
{
    [Route("/")]
    public class GameController : Controller
    {
        static Bot playerBot = new Bot();

        [HttpPost]
        public string Index([FromForm]string data)
        {
            if (data == null)
            {
                return "";
            }

            GameInfo gameInfo = JsonConvert.DeserializeObject<GameInfo>(data);
            var map = new Map(gameInfo.CustomSerializedMap, gameInfo.xMin, gameInfo.yMin);

            playerBot.BeforeTurn(gameInfo.Player);
            var playerAction = playerBot.ExecuteTurn(map, gameInfo.OtherPlayers.Select(p => p as IPlayer).ToList());

            playerBot.AfterTurn();
            return playerAction;
        }
    }
}
