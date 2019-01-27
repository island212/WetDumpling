using System;
using System.Collections.Generic;
using System.Linq;
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

    public IList<CardActionData> GetCards(int amount, bool allowRemove = true)
    {
        var output = new List<CardActionData>();

        if (amount > cards.Count)
            amount = cards.Count;

        if (amount <= 0)
            throw new IndexOutOfRangeException("Deck is empty");

        for (int i = 0; i < amount; i++)
        {
            var card = cards.First();
            output.Add(card);

            if (allowRemove)
                cards.Remove(card);
        }

        return output;
    }

    public IList<CardActionData> AddCards(IEnumerable<CardActionData> newCards)
    {
        foreach (var card in newCards)
        {
            fullDeck.Add(card);
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