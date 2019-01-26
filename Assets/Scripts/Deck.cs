using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Linq;
using UnityEngine.SocialPlatforms;
using Utils;

public class Deck
{
    private IList<CardData> cards;
    private readonly IList<CardData> fullDeck;

    public Deck(IEnumerable<CardData> cards)
    {
        fullDeck = new List<CardData>(cards);
        this.cards = fullDeck;
    }

    public Deck Shuffle()
    {
        cards = fullDeck;
        cards.Shuffle();
        return this;
    }

    public IList<CardData> GetCards(int amount)
    {
        return GetCards(amount, true);
    }

    public IList<CardData> GetCards(int amount, bool allowRemove)
    {
        var output = new List<CardData>();

        if (amount > cards.Count)
            amount = cards.Count;

        for (int i = 0; i < amount; i++)
        {
            var card = cards.ElementAt(i);
            output.Add(card);
            if(allowRemove)
                cards.RemoveAt(i);
        }

        return output;
    }

    public IList<CardData> AddCards(IList<CardData> newCards)
    {
        foreach (var card in newCards)
        {
            fullDeck.Insert(cards.Count, card);
        }

        return fullDeck;
    }

    public IList<CardData> GetFullDeck()
    {
        return fullDeck;
    }

    public int Count()
    {
        return cards.Count;
    }
}
