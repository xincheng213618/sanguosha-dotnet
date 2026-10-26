using CardGame.Core;

namespace CardGame.Modes.Standard;

/// <summary>Standard and activity equipment recipes; the Core resolver only assembles the selected content.</summary>
public static class StandardModeEquipmentProfiles
{
    public static ModeEquipmentRecipe CreateRecipe(ContentModeDefinition mode, ContentRegistry registry,
        GameOptions? options = null, IEnumerable<CardKind>? additionalKinds = null)
    {
        ArgumentNullException.ThrowIfNull(mode);
        ArgumentNullException.ThrowIfNull(registry);
        var required = new HashSet<CardKind>(additionalKinds ?? []);
        if (mode.HulaoActivity is not null) required.UnionWith(BuiltInEquipmentContent.Hulao.Definitions.Select(definition => definition.Kind));
        if (mode.ShenwuActivity is not null) required.UnionWith(BuiltInEquipmentContent.Shenwu.Definitions.Select(definition => definition.Kind));
        if (mode.GuanduActivity is not null) required.UnionWith(BuiltInEquipmentContent.Guandu.Definitions.Select(definition => definition.Kind));
        var deckId = options?.DeckId ?? mode.DeckId;
        if (deckId is not null && registry.Decks.TryGetValue(deckId, out var deck))
            foreach (var id in deck.Cards.Select(card => card.CardDefinitionId)
                .Concat((deck.PhysicalCards ?? []).Select(card => card.CardDefinitionId)))
                if (registry.Cards[id].LegacyKind is { } kind) required.Add(kind);
        if (options?.ShanheBattle is { } setup)
        {
            required.UnionWith(setup.StartingCards.Concat(setup.StartingEquipment).Select(card => card.Kind));
            foreach (var member in setup.AllyRoster.Concat(setup.EnemyRoster))
                required.UnionWith((member.StartingHand ?? []).Concat(member.StartingCards ?? [])
                    .Concat(member.StartingEquipment ?? []).Select(card => card.Kind));
        }
        var wushuang = mode.Id == WushuangMode.ModeId;
        if (wushuang)
            required.UnionWith(WushuangCatalogue.Cards.Where(card => card.IsAvailableInShop && card.IsEquipment && card.Kind is not null)
                .Select(card => card.Kind!.Value));
        return new(required, wushuang ? [] : [BuiltInEquipmentContent.WushuangPurchases.Id]);
    }

    public static EquipmentContentRegistry CreateForMatch(ContentModeDefinition mode, ContentRegistry registry,
        IEnumerable<CardKind>? additionalKinds = null, IEnumerable<EquipmentContentPackage>? additionalPackages = null,
        GameOptions? options = null) =>
        BuiltInEquipmentContent.CreateForMatch(CreateRecipe(mode, registry, options, additionalKinds), registry, additionalPackages);
}

internal sealed partial class StandardModeSetupFeature : IMatchModeEquipmentProfile
{
    private ModeEquipmentRecipe? _equipmentRecipe;
    public ModeEquipmentRecipe EquipmentRecipe => _equipmentRecipe ??=
        StandardModeEquipmentProfiles.CreateRecipe(_mode, _registry, _options);
}
