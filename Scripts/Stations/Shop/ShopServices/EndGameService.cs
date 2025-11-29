using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameService : IShopService
{
    private EndGame _endGame;

    public EndGameService(EndGame endGame)
    {
        _endGame = endGame;
    }

    public bool TryApply()
    {
        _endGame.gameObject.SetActive(true);
        _endGame.StartEndGame();
        return true;
    }
}
