#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashUseOrdinalsEntries
{
    internal int CountSlashUsePrefix(IReadOnlyList<IGameEvent> history, int start, int end, int expectedActorSeat, bool elemental, CurrentTurnCardUseKindsRecordedEvent? current) => _resolve().CountSlashUsePrefix(history, start, end, expectedActorSeat, elemental, current);
    internal bool MatchesSlashUseOrdinalModifier(SkillProgramDamageModifier modifier, GameEngine.IDamageAttempt? attack, int actorSeat, CardKind effectiveKind) => _resolve().MatchesSlashUseOrdinalModifier(modifier, attack, actorSeat, effectiveKind);
    internal CardKind SlashUseKindAtPrefix(CurrentTurnCardUseKindsRecordedEvent fact, IReadOnlyList<IGameEvent> history, int end, CurrentTurnCardUseKindsRecordedEvent? current) => _resolve().SlashUseKindAtPrefix(fact, history, end, current);
    internal CardKind SlashUseKindAtPrefix2(CurrentTurnCardUseKindsRecordedEvent fact, IReadOnlyList<IGameEvent> history, int end, CurrentTurnCardUseKindsRecordedEvent? current, CardKind original) => _resolve().SlashUseKindAtPrefix(fact, history, end, current, original);
}
