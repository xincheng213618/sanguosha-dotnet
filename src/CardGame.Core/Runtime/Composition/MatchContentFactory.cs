namespace CardGame.Core;

/// <summary>Projects registered content into deterministic match definitions and physical cards.</summary>
internal static class MatchContentFactory
{
    internal static IReadOnlyList<GeneralDefinition> CreateGeneralPool(
        ContentRegistry contentRegistry,
        ContentModeDefinition mode)
    {
        var ids = mode.GeneralPoolIds ?? contentRegistry.Generals.Keys
            .Where(contentRegistry.IsGeneralPlayable)
            .OrderBy(id => id, StringComparer.Ordinal)
            .ToArray();
        return ids.Select(id =>
            {
                var definition = contentRegistry.Generals.TryGetValue(id, out var general)
                    ? general
                    : throw new InvalidOperationException(
                        $"Mode '{mode.Id}' references unknown general '{id}'.");
                var skills = definition.SkillIds
                    .Select(contentRegistry.GetSkill)
                    .ToArray();
                return new GeneralDefinition(
                    definition.Id,
                    definition.Name,
                    definition.PortraitKey,
                    skills.Select(RuntimeSkills.RuntimeRules_Core_NationalSkills.ToRuntimeSkillDefinition).ToArray(),
                    definition.FactionId,
                    definition.BaseHp,
                    definition.Gender) { InitialHp = definition.InitialHp };
            })
            .ToArray();
    }

    internal static ContentDeckRecipe ResolveDeck(
        ContentRegistry contentRegistry,
        string? deckId)
    {
        var requestedId = deckId ?? "standard:basic-demo";
        if (!contentRegistry.Decks.TryGetValue(requestedId, out var definition))
        {
            throw new InvalidOperationException(
                $"The supplied content registry does not contain deck '{requestedId}'.");
        }

        var hasCountRecipe = definition.Cards.Count > 0;
        var hasPhysicalRecipe = definition.PhysicalCards is { Count: > 0 };
        if (definition.InitialHandSize < 0 || definition.DrawPerTurn < 0 ||
            hasCountRecipe == hasPhysicalRecipe)
        {
            throw new InvalidOperationException("The supplied deck recipe is invalid.");
        }

        return definition;
    }

    internal static IReadOnlyList<Card> CreateDeck(
        ContentRegistry contentRegistry,
        ContentDeckRecipe definition)
    {
        if (definition.PhysicalCards is { Count: > 0 } physicalCards)
        {
            return physicalCards.Select((entry, index) =>
            {
                var cardDefinition = contentRegistry.GetCard(entry.CardDefinitionId);
                if (cardDefinition.LegacyKind is not { } kind)
                {
                    throw new InvalidOperationException(
                        $"Card '{cardDefinition.Id}' has no legacy runtime projection for this engine.");
                }

                return new Card(index + 1, kind, entry.Suit, entry.Rank);
            }).ToArray();
        }

        var cards = new List<Card>(definition.Cards.Sum(entry => entry.Count));
        var id = 1;
        foreach (var entry in definition.Cards)
        {
            if (entry.Count <= 0)
            {
                throw new InvalidOperationException(
                    $"Deck recipe '{definition.Id}' contains a non-positive count.");
            }

            var cardDefinition = contentRegistry.GetCard(entry.CardDefinitionId);
            if (cardDefinition.LegacyKind is not { } kind)
            {
                throw new InvalidOperationException(
                    $"Card '{cardDefinition.Id}' has no legacy runtime projection for this engine.");
            }

            for (var copy = 0; copy < entry.Count; copy++)
            {
                var suit = (Suit)((id - 1) % 4);
                var rank = ((id - 1) % 13) + 1;
                cards.Add(new Card(id++, kind, suit, rank));
            }
        }

        return cards;
    }

}
