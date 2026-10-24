using CardGame.Core;

namespace CardGame.Modes.Standard;

internal sealed partial class StandardModeSetupFeature
{
    public IReadOnlyList<Card> PrepareDeck(IReadOnlyList<Card> original) => TransformDeck(original, rollHulaoEquipment: true);
    public IReadOnlyList<Card> ProjectDeckCardNames(IReadOnlyList<Card> original) => TransformDeck(original, rollHulaoEquipment: false);
    private IReadOnlyList<Card> TransformDeck(IReadOnlyList<Card> original, bool rollHulaoEquipment)
    {
        // Keep the former engine pipeline order, including Hulao's two random operations.
        IReadOnlyList<Card> cards = rollHulaoEquipment ? PrepareHulaoDeck(original) : _mode.HulaoActivity is null ? original :
            Array.AsReadOnly(original.Select(card => card.Kind is CardKind.FiveGrains or CardKind.PeachGarden
                ? card with { Kind = CardKind.AllianceFeast } : card).ToArray());
        if (_mode.ShenwuActivity is not null) cards = Array.AsReadOnly(cards
            .Where(card => card.Kind is not (CardKind.Indulgence or CardKind.SupplyShortage))
            .Select(card => card.Kind switch
            {
                CardKind.DrawTwo => card with { Kind = CardKind.SowBeans },
                CardKind.BorrowedSword => card with { Kind = CardKind.TransplantWood }, _ => card
            }).ToArray());
        if (_mode.HellActivity is not null) cards = Array.AsReadOnly(cards.Where(card => card.Kind != CardKind.WoodenOx).ToArray());
        if (_mode.WenheActivity is not null) cards = Array.AsReadOnly(cards.Where(card => card.Kind != CardKind.Indulgence)
            .Select(card => card with { Kind = card.Kind switch
            {
                CardKind.PeachGarden or CardKind.WoodenOx => CardKind.StarShift,
                CardKind.Nullification => card.Suit is Suit.Spade or Suit.Club ? CardKind.BeamSwap : CardKind.HandRedistribution,
                CardKind.SupplyShortage => CardKind.WenheChaos, _ => card.Kind
            } }).ToArray());
        if (_mode.DragonBoatActivity is not null) cards = Array.AsReadOnly(cards
            .Where(card => card.Kind is not (CardKind.Indulgence or CardKind.SupplyShortage or CardKind.Lightning))
            .Select(card => card with { Kind = card.Kind switch
            {
                CardKind.Peach => CardKind.Zong, CardKind.Alcohol => CardKind.RealgarWine,
                CardKind.DrawTwo => CardKind.SameBoat, CardKind.FiveGrains or CardKind.PeachGarden => CardKind.StriveUpstream,
                CardKind.BarbarianAssault or CardKind.ArrowBarrage => CardKind.AgainstCurrent, _ => card.Kind
            } }).ToArray());
        if (_mode.GuanduActivity is not null) cards = Array.AsReadOnly(cards.Where(card => card.Kind != CardKind.WoodenOx)
            .Select(card => card with { Kind = card.Kind switch
            {
                CardKind.Alcohol => CardKind.Intoxication, CardKind.FiveGrains => CardKind.StoreGrain,
                CardKind.PeachGarden or CardKind.Lightning => CardKind.Reinforcements,
                CardKind.BarbarianAssault => CardKind.UnitedEffort, _ => card.Kind
            } }).ToArray());
        return cards;
    }

    private IReadOnlyList<Card> PrepareHulaoDeck(IReadOnlyList<Card> original)
    {
        if (_mode.HulaoActivity is null) return original;
        var candidates = new List<CardKind> { CardKind.WushuangHalberd, CardKind.PurpleGoldCrown, CardKind.RedCottonRobe, CardKind.LionBelt };
        _random.Shuffle(candidates);
        var selected = candidates.Take(_random.Next(4) + 1).ToHashSet();
        var cards = original.Select(card => card.Kind switch
        {
            CardKind.FiveGrains or CardKind.PeachGarden => card with { Kind = CardKind.AllianceFeast },
            CardKind.FangtianHalberd when selected.Contains(CardKind.WushuangHalberd) => card with { Kind = CardKind.WushuangHalberd },
            CardKind.BaguaFormation when selected.Contains(CardKind.LionBelt) => card with { Kind = CardKind.LionBelt }, _ => card
        }).ToList();
        var id = cards.Select(card => card.Id).DefaultIfEmpty(0).Max();
        if (selected.Contains(CardKind.PurpleGoldCrown)) cards.Add(new(++id, CardKind.PurpleGoldCrown, Suit.Club, 1));
        if (selected.Contains(CardKind.RedCottonRobe)) cards.Add(new(++id, CardKind.RedCottonRobe, Suit.Diamond, 1));
        return Array.AsReadOnly(cards.ToArray());
    }

    public ModeSelectedEquipmentPlan? PrepareSelectedEquipment(IReadOnlyList<Card> drawPile, IReadOnlyList<ModeSeatSetupFacts> seats)
    {
        if (_mode.ShenwuActivity is not { } activity) return null;
        var selected = activity.PlayerSeatOffsets.Select(RelativeSeat).Select(seat => seats.Single(p => p.Seat == seat))
            .Where(player => activity.GodPoolIds.Contains(player.GeneralId!, StringComparer.Ordinal))
            .Select(player => (Player: player, Equipment: ShenwuGodEquipment(_registry.Generals[player.GeneralId!].CharacterId ??
                ShenwuLegacyGodFamily(player.GeneralId!)))).Where(entry => entry.Equipment is not null).ToArray();
        var replacements = selected.Where(entry => entry.Equipment!.Value.Replaces is not null)
            .ToDictionary(entry => entry.Equipment!.Value.Replaces!.Value, entry => entry.Equipment!.Value.Kind);
        var cards = drawPile.Select(card => replacements.TryGetValue(card.Kind, out var kind) ? card with { Kind = kind } : card).ToList();
        var nextId = cards.Select(card => card.Id).DefaultIfEmpty().Max();
        foreach (var (_, equipment) in selected.Where(entry => entry.Equipment!.Value.Replaces is null))
            cards.Add(new(++nextId, equipment!.Value.Kind, Suit.Spade, 1));
        var placements = selected.Select(entry => new ModeInitialEquipmentPlacement(entry.Player.Seat,
            cards.FirstOrDefault(card => card.Kind == entry.Equipment!.Value.Kind)?.Id ??
                throw new InvalidOperationException("A chosen god's original dedicated equipment entity is missing from the military deck."))).ToArray();
        return new(cards, placements, [new ShenwuEquipmentInitializedEvent(selected.Length)]);
    }
    private static (CardKind Kind, CardKind? Replaces)? ShenwuGodEquipment(string? family) => family switch
    {
        "character:guan-yu" => (CardKind.GhostDragonCrescentBlade, CardKind.QinglongCrescentBlade),
        "character:lu-meng" => (CardKind.NationalStyleRobe, null),
        "character:zhou-yu" => (CardKind.CrimsonSoulZither, CardKind.ZhuqueFan),
        "character:zhuge-liang" => (CardKind.QimenEightTrigrams, CardKind.BaguaFormation),
        "character:cao-cao" => (CardKind.DustlessGoldenSpear, CardKind.DefensiveHorse),
        "character:lu-bu" => (CardKind.ShuraInfernoHalberd, CardKind.FangtianHalberd),
        "character:sima-yi" => (CardKind.DelusionCrown, null),
        "character:zhao-yun" => (CardKind.ScarletBloodSword, CardKind.QinggangSword), _ => null
    };
    private static string? ShenwuLegacyGodFamily(string id) => id switch
    {
        "classic:shen-guan-yu" => "character:guan-yu", "classic:shen-lu-meng" => "character:lu-meng",
        "classic:shen-zhou-yu" => "character:zhou-yu", "classic:shen-cao-cao" => "character:cao-cao",
        "classic:shen-lu-bu" => "character:lu-bu", "classic:shen-sima-yi" => "character:sima-yi", _ => null
    };
}
