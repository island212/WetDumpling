using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using UnityEngine.SocialPlatforms;
using Utils;

public class Deck
{
    private IList<CardData> cards;
    private readonly IList<CardData> fullDeck;

    public Deck(IList<CardData> cards)
    {
        this.cards = cards;
        fullDeck = cards;
    }

    public IList<CardData> Shuffle()
    {
        cards = fullDeck;
        return cards.Shuffle();
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
            output.Add(cards.ElementAt(i));
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
}
