#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTianYuOlEntries
{
    internal IReadOnlyList<ProgramDirectedDistanceGrantedEvent> ActiveDirectedDistanceGrants(int ownerSeat, int targetSeat) => _resolve().ActiveDirectedDistanceGrants(ownerSeat, targetSeat);
    internal RuleQueryEvaluation ApplyProgramDirectedDistance(CharacterState source, CharacterState target, RuleQueryEvaluation original) => _resolve().ApplyProgramDirectedDistance(source, target, original);
    internal bool CanSaodiAddTarget(CardUseFrame use, int targetSeat) => _resolve().CanSaodiAddTarget(use, targetSeat);
    internal void ResolveZhuitaoMarkChoice(PromptChoice selected) => _resolve().ResolveZhuitaoMarkChoice(selected);
    internal int[] SaodiBetweenSeats(int ownerSeat, int targetSeat) => _resolve().SaodiBetweenSeats(ownerSeat, targetSeat);
    internal SkillProgramStepOutcome SaodiProgramExpandTargets(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().SaodiProgramExpandTargets(frame, effect);
    internal PromptChoice SelectAiZhuitaoMarkChoice(PendingDecision decision) => _resolve().SelectAiZhuitaoMarkChoice(decision);
    internal SkillProgramStepOutcome ZhuitaoProgramMarkTarget(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ZhuitaoProgramMarkTarget(frame, effect);
    internal SkillProgramStepOutcome ZhuitaoProgramRevoke(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ZhuitaoProgramRevoke(frame, effect);
}
