﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System.Linq;

public class Deck
{
    private IList<CardData> cards;
    private IList<CardData> fullDeck;

    public Deck(IList<CardData> cards)
    {
        this.cards = cards;
        this.fullDeck = cards;
    }

    public IList<CardData> Shuffle()
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = fullDeck.Count;
        cards = fullDeck;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            var card = cards[k];
            cards[k] = cards[n];
            cards[n] = card;
        }

        return cards;
    }

    public IList<CardData> GetCards(int amount)
    {
        var output = new List<CardData>();

        for (int i = 0; i < amount; i++)
        {
            output.Add(cards.ElementAt(i));
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
