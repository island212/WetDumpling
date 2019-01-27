using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

public class Deck
{
    private IList<CardActionData> cards;
    public IList<CardActionData> FullDeck { get; }

    public int Count => cards.Count;
    public bool IsEmpty => cards.Count <= 0;

    public Deck(IEnumerable<CardActionData> cards)
    {
        FullDeck = new List<CardActionData>(cards);
        this.cards = FullDeck;
    }

    public Deck Shuffle()
    {
        cards = FullDeck;
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
            FullDeck.Add(card);
        }

        return FullDeck;
    }    
}