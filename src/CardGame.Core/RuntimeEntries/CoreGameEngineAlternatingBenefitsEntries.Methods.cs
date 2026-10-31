#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlternatingBenefitsEntries
{
    internal int AlternatingNextDrawPhaseBonus(global::CardGame.Core.CharacterState player) => _resolve().AlternatingNextDrawPhaseBonus(player);
    internal int AlternatingOwnerTurnOrdinal(int owner) => _resolve().AlternatingOwnerTurnOrdinal(owner);
    internal void ApplyAlternatingChoiceBenefit(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ApplyAlternatingChoiceBenefit(frame, effect);
    internal void AssignAlternatingNextDrawPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseStartedEvent e) => _resolve().AssignAlternatingNextDrawPhase(e);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => _resolve().CompleteProgramEventHistory();
    internal global::CardGame.Core.AlternatingChoiceBenefitResolvedEvent? GetAlternatingChoicePredecessor(int owner, string skill, string instance, global::CardGame.Core.SkillProgramEffect effect) => _resolve().GetAlternatingChoicePredecessor(owner, skill, instance, effect);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramAlternatingChoiceStateSnapshot>? GetAlternatingChoiceStateSnapshot(global::CardGame.Core.CharacterState owner) => _resolve().GetAlternatingChoiceStateSnapshot(owner);
    internal global::CardGame.Core.SkillProgramStepOutcome ObtainDeckCardWithConsecutiveTarget(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ObtainDeckCardWithConsecutiveTarget(frame, targetSeat, effect);
}
