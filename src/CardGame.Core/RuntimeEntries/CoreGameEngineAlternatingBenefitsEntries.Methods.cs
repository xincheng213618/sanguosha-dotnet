#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlternatingBenefitsEntries
{
    internal int AlternatingNextDrawPhaseBonus(CharacterState player) => _resolve().AlternatingNextDrawPhaseBonus(player);
    internal int AlternatingOwnerTurnOrdinal(int owner) => _resolve().AlternatingOwnerTurnOrdinal(owner);
    internal void ApplyAlternatingChoiceBenefit(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ApplyAlternatingChoiceBenefit(frame, effect);
    internal void AssignAlternatingNextDrawPhase(ActualDiscardRecoveryPhaseStartedEvent e) => _resolve().AssignAlternatingNextDrawPhase(e);
    internal IEnumerable<IGameEvent> CompleteProgramEventHistory() => _resolve().CompleteProgramEventHistory();
    internal AlternatingChoiceBenefitResolvedEvent? GetAlternatingChoicePredecessor(int owner, string skill, string instance, SkillProgramEffect effect) => _resolve().GetAlternatingChoicePredecessor(owner, skill, instance, effect);
    internal IReadOnlyList<ProgramAlternatingChoiceStateSnapshot>? GetAlternatingChoiceStateSnapshot(CharacterState owner) => _resolve().GetAlternatingChoiceStateSnapshot(owner);
    internal SkillProgramStepOutcome ObtainDeckCardWithConsecutiveTarget(ProgramSkillFrame frame, int targetSeat, SkillProgramEffect effect) => _resolve().ObtainDeckCardWithConsecutiveTarget(frame, targetSeat, effect);
}
