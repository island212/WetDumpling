using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using UnityEditor;

public class CharacterLane : MonoBehaviour
{
    [SerializeField] private List<CharacterComponent> characters = null;
    public List<Transform> spawnPositions = null;

    private void Awake()
    {
        var spawns = transform.GetComponentsInChildren<Transform>()
                              .Where(r => r.CompareTag("Enemy"))
                              .ToArray();
        if (spawns.Length > 0)
        {
            spawnPositions = new List<Transform>(spawns);
        }

        var components = transform.GetComponentsInChildren<Transform>()
                                  .Where(r => r.CompareTag("Entity"))
                                  .ToArray();
        if (components.Length > 0)
        {
            characters.Add(components[0].GetComponent<CharacterComponent>());
        }
    }

    public IEnumerable<CardAction> GetTurnActions()
    {
        var cardActions = new List<CardAction>();
        foreach (var character in characters)
        {
            bool isPlayer = character.IsPlayer;
            var cards = isPlayer ? GetPlayerCards(character) : GetEnemyCards(character);

            cardActions.AddRange(cards.Select(cardActionData => new CardAction
            {
                Data = cardActionData,
                Source = character
            }));
        }

        return cardActions.Shuffle();
    }

    private static IList<CardActionData> GetPlayerCards(CharacterComponent player) =>
        player.Deck.Shuffle().GetCards(player.characterData.speed);

    private static IList<CardActionData> GetEnemyCards(CharacterComponent enemy) =>
        enemy.Deck
             .Shuffle()
             .GetCards(enemy.characterData.speed, false);

    public void ExecuteAction(CardData data)
    {
        int pushIndex = data.push;

        if (pushIndex > 0)
        {
            var charToPush = characters[0];
            var nextChar = characters[pushIndex];
            characters[pushIndex] = charToPush;
            characters[0] = nextChar;
            UpdateLaneState();
        }

        characters[0].ExecuteAction(data);
    }

    public bool IsGameOver()
    {
        UpdateLaneState();
        return characters.Count <= 0;
    }

    private void UpdateLaneState()
    {
        var charsToRemove = new List<CharacterComponent>();

        foreach (var character in characters)
        {
            if (character.IsDead)
                charsToRemove.Add(character);
        }

        foreach (var character in charsToRemove)
        {
            characters.Remove(character);
            Destroy(character.gameObject);
        }

        if (spawnPositions.Count > 0)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                iTween.MoveTo(characters[i].gameObject, spawnPositions[i].position, 1.0f);
            }
        }
    }

    public void AddCharacter(GameObject character, int index)
    {
        var newCharacter = Instantiate(character, spawnPositions[index].position, Quaternion.identity);
        characters.Add(newCharacter.GetComponent<CharacterComponent>());
    }

    public CharacterComponent getPlayer()
    {
        foreach (CharacterComponent i in characters)
        {
            if (i.tag == "Entity")
            {
                return i;
            }
        }

        return null;
    }


}