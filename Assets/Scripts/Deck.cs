using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Linq;
using UnityEngine.SocialPlatforms;
using Utils;

public class Deck
{
    private IList<CardActionData> cards;
    private readonly IList<CardActionData> fullDeck;

    public Deck(IEnumerable<CardActionData> cards)
    {
        fullDeck = new List<CardActionData>(cards);
        this.cards = fullDeck;
    }

    public Deck Shuffle()
    {
        cards = fullDeck;
        cards.Shuffle();
        return this;
    }

    public IList<CardActionData> GetCards(int amount)
    {
        return GetCards(amount, true);
    }

    public IList<CardActionData> GetCards(int amount, bool allowRemove)
    {
        var output = new List<CardActionData>();

        if (amount > cards.Count)
            amount = cards.Count;

        for (int i = 0; i < amount; i++)
        {
            var card = cards.First();
            output.Add(card);
            if(allowRemove)
                cards.Remove(card);
        }

        return output;
    }

    public IList<CardActionData> AddCards(IList<CardActionData> newCards)
    {
        foreach (var card in newCards)
        {
            fullDeck.Insert(cards.Count, card);
        }

        return fullDeck;
    }

    public IList<CardActionData> GetFullDeck()
    {
        return fullDeck;
    }

    public int Count()
    {
        return cards.Count;
    }
}
