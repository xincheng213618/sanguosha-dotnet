#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoZhenMobileShiRuntime
{
    internal bool IsShiCaoZPrivateProcessing(int id) => HasShiCaoZ && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.ShiCaoZhen is { Operation: SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengPlace } r && r.Pool.Contains(id));
    [ProgramViewContributor(ProgramViewContribution.InvariantProcessing, 49900)]
    [ProgramViewContributor(ProgramViewContribution.PublicProcessing, 49900)]
    internal IReadOnlyList<Card> WithoutShiCaoZPrivateProcessing(IReadOnlyList<Card> cards) => !HasShiCaoZ ? cards : Array.AsReadOnly(cards.Where(c => !IsShiCaoZPrivateProcessing(c.Id)).ToArray());
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 49900)]
    internal IReadOnlyList<CardSnapshot> ShiCaoZPrivateCards(int viewer) => !HasShiCaoZ ? [] : Array.AsReadOnly(_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.ShiCaoZhen is { Operation: SkillProgramEffectOp.ResolveMobileShiCaoZhenSifengPlace }).SelectMany(f => f.ShiCaoZhen!.Pool).Where(IsShiCaoZPrivateProcessing).Concat(LiveShiCaoZMaterials().Where(m => m.Source.OwnerSeat == viewer).Select(m => m.CardId)).Distinct().Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray());
    [ProgramViewContributor(ProgramViewContribution.NamedAmbushPiles, 49900)]
    internal IReadOnlyList<NamedAmbushPileSnapshot>? IncludeShiCaoZPileViews(IReadOnlyList<NamedAmbushPileSnapshot>? ordinary, int holder, int viewer, bool revealAll)
    {
        if (!HasShiCaoZ)
            return ordinary;
        var piles = LiveShiCaoZMaterials().Where(m => m.Holder == holder).GroupBy(m => m.Source.OwnerSeat).Select(group => new NamedAmbushPileSnapshot(ShiCaoZSifeng, "伺锋", group.Count(), revealAll || group.Key == viewer ? Array.AsReadOnly(group.OrderBy(m => m.PlacementId).Select(m => new NamedAmbushPrivateEntrySnapshot(_coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId)), m.Kind)).ToArray()) : null)).ToArray();
        return piles.Length == 0 ? ordinary : Array.AsReadOnly((ordinary ?? []).Concat(piles).ToArray());
    }
}
