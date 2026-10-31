#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuRuntimeRuntime
{
    internal bool GuanOpeningUseExists(int owner, bool red) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GuanOpeningUseEvent>().Any(e => e.OwnerSeat == owner && e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && (red ? e.IsRed : e.IsBasic));
    internal bool GuanFirstBasicAvailable(CharacterState owner, CardKind kind) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, "mou-ol:duoshou") && CardUseCategoryCatalog.Get(kind) == CardUseCategories.Basic && !GuanOpeningUseExists(owner.Seat, false);
    internal bool GuanFirstRedAvailable(CharacterState owner, Suit? suit) => suit is Suit.Heart or Suit.Diamond && _coreNationalSkillsOperations.HasRuntimeSkill(owner, "mou-ol:duoshou") && !GuanOpeningUseExists(owner.Seat, true);
    internal bool GuanFirstRedDistance(CharacterState owner, Card card, long? useId = null) => useId is { } id ? _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.GuanFirstRedDistance == true : GuanFirstRedAvailable(owner, _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card));
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 220)]
    internal bool DistancePhysicalSlash220GuanFirstRedDistance(ProgramSlashDistanceQuery q) => GuanFirstRedDistance(q.Actor, q.PhysicalCard!, q.ExistingUseFrameId);
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 40)]
    internal bool DistanceVirtualSlash40GuanFirstRedAvailable(ProgramSlashDistanceQuery q) => GuanFirstRedAvailable(q.Actor, q.PhysicalSuit);
}
