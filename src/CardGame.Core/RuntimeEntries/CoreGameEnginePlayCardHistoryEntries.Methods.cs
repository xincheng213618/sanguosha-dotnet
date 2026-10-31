#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlayCardHistoryEntries
{
    internal void AddPhaseLimitedBasicCardActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> playableCards, int? selectedPhysicalCardId = default) => _resolve().AddPhaseLimitedBasicCardActions(actions, actor, playableCards, selectedPhysicalCardId);
    internal global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source) => _resolve().ApplyProgramUseAppearance(owner, card, source);
    internal bool CanUsePhaseLimitedViewAs(global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule, global::CardGame.Core.CharacterState owner) => _resolve().CanUsePhaseLimitedViewAs(instance, rule, owner);
    internal global::CardGame.Core.Suit? CaptureUsedCardSuit(int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> physicalIds, global::CardGame.Core.Card card) => _resolve().CaptureUsedCardSuit(actorSeat, physicalIds, card);
    internal void ConsumeProgramViewAsUsage(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> sources) => _resolve().ConsumeProgramViewAsUsage(sources);
    internal int DamageInstancesTakenThisTurn(int ownerSeat) => _resolve().DamageInstancesTakenThisTurn(ownerSeat);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> EventsSinceLastBoundary(global::System.Func<global::CardGame.Core.IGameEvent, bool> boundary) => _resolve().EventsSinceLastBoundary(boundary);
    internal bool MatchesPreviousPlayCard(global::CardGame.Core.CardActionContext action) => _resolve().MatchesPreviousPlayCard(action);
    internal bool PhaseConversionUsageUnavailable(global::CardGame.Core.ProgramConfiguredViewAsQuery query) => _resolve().PhaseConversionUsageUnavailable(query);
    internal global::CardGame.Core.CardActionContext? PreviousPlayCard(int actorSeat, long beforeActionId = 9223372036854775807L) => _resolve().PreviousPlayCard(actorSeat, beforeActionId);
    internal bool PreviousPlayCardIsBasic(global::CardGame.Core.CardActionContext action) => _resolve().PreviousPlayCardIsBasic(action);
    internal bool? PreviousPlayCardIsBasicFor(int actorSeat) => _resolve().PreviousPlayCardIsBasicFor(actorSeat);
    internal global::CardGame.Core.Suit? PreviousPlaySuitForInheritance(int actorSeat) => _resolve().PreviousPlaySuitForInheritance(actorSeat);
    internal global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => _resolve().ViewAsRule(source);
    internal string ViewAsUsageId(global::CardGame.Core.CardConversionSource source, global::CardGame.Core.SkillProgramViewAs rule) => _resolve().ViewAsUsageId(source, rule);
    internal bool TracksPlayCardHistory { get => _resolve().TracksPlayCardHistory; }
}
