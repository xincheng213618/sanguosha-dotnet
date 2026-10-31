#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashUseOrdinalsEntries
{
    internal int CountSlashUsePrefix(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, int start, int end, int expectedActorSeat, bool elemental, global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent? current) => _resolve().CountSlashUsePrefix(history, start, end, expectedActorSeat, elemental, current);
    internal bool MatchesSlashUseOrdinalModifier(global::CardGame.Core.SkillProgramDamageModifier modifier, global::CardGame.Core.GameEngine.IDamageAttempt? attack, int actorSeat, global::CardGame.Core.CardKind effectiveKind) => _resolve().MatchesSlashUseOrdinalModifier(modifier, attack, actorSeat, effectiveKind);
    internal global::CardGame.Core.CardKind SlashUseKindAtPrefix(global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent fact, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, int end, global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent? current) => _resolve().SlashUseKindAtPrefix(fact, history, end, current);
    internal global::CardGame.Core.CardKind SlashUseKindAtPrefix2(global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent fact, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, int end, global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent? current, global::CardGame.Core.CardKind original) => _resolve().SlashUseKindAtPrefix(fact, history, end, current, original);
}
