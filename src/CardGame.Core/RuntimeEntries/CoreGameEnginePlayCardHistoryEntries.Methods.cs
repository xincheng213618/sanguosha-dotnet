#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlayCardHistoryEntries
{
    internal void AddPhaseLimitedBasicCardActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playableCards, int? selectedPhysicalCardId = default) => _resolve().AddPhaseLimitedBasicCardActions(actions, actor, playableCards, selectedPhysicalCardId);
    internal Card ApplyProgramUseAppearance(CharacterState owner, Card card, CardConversionSource? source) => _resolve().ApplyProgramUseAppearance(owner, card, source);
    internal bool CanUsePhaseLimitedViewAs(IndexedSkillProgramInstance instance, SkillProgramViewAs rule, CharacterState owner) => _resolve().CanUsePhaseLimitedViewAs(instance, rule, owner);
    internal Suit? CaptureUsedCardSuit(int actorSeat, IReadOnlyList<int> physicalIds, Card card) => _resolve().CaptureUsedCardSuit(actorSeat, physicalIds, card);
    internal void ConsumeProgramViewAsUsage(IReadOnlyList<CardConversionSource> sources) => _resolve().ConsumeProgramViewAsUsage(sources);
    internal int DamageInstancesTakenThisTurn(int ownerSeat) => _resolve().DamageInstancesTakenThisTurn(ownerSeat);
    internal IEnumerable<IGameEvent> EventsSinceLastBoundary(Func<IGameEvent, bool> boundary) => _resolve().EventsSinceLastBoundary(boundary);
    internal bool MatchesPreviousPlayCard(CardActionContext action) => _resolve().MatchesPreviousPlayCard(action);
    internal bool PhaseConversionUsageUnavailable(ProgramConfiguredViewAsQuery query) => _resolve().PhaseConversionUsageUnavailable(query);
    internal CardActionContext? PreviousPlayCard(int actorSeat, long beforeActionId = 9223372036854775807L) => _resolve().PreviousPlayCard(actorSeat, beforeActionId);
    internal bool PreviousPlayCardIsBasic(CardActionContext action) => _resolve().PreviousPlayCardIsBasic(action);
    internal bool? PreviousPlayCardIsBasicFor(int actorSeat) => _resolve().PreviousPlayCardIsBasicFor(actorSeat);
    internal Suit? PreviousPlaySuitForInheritance(int actorSeat) => _resolve().PreviousPlaySuitForInheritance(actorSeat);
    internal SkillProgramViewAs? ViewAsRule(CardConversionSource source) => _resolve().ViewAsRule(source);
    internal string ViewAsUsageId(CardConversionSource source, SkillProgramViewAs rule) => _resolve().ViewAsUsageId(source, rule);
    internal bool TracksPlayCardHistory { get => _resolve().TracksPlayCardHistory; }
}
