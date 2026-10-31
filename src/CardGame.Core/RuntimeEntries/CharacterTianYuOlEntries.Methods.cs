#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTianYuOlEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramDirectedDistanceGrantedEvent> ActiveDirectedDistanceGrants(int ownerSeat, int targetSeat) => _resolve().ActiveDirectedDistanceGrants(ownerSeat, targetSeat);
    internal global::CardGame.Core.RuleQueryEvaluation ApplyProgramDirectedDistance(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.RuleQueryEvaluation original) => _resolve().ApplyProgramDirectedDistance(source, target, original);
    internal bool CanSaodiAddTarget(global::CardGame.Core.CardUseFrame use, int targetSeat) => _resolve().CanSaodiAddTarget(use, targetSeat);
    internal void ResolveZhuitaoMarkChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveZhuitaoMarkChoice(selected);
    internal int[] SaodiBetweenSeats(int ownerSeat, int targetSeat) => _resolve().SaodiBetweenSeats(ownerSeat, targetSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome SaodiProgramExpandTargets(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().SaodiProgramExpandTargets(frame, effect);
    internal global::CardGame.Core.PromptChoice SelectAiZhuitaoMarkChoice(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiZhuitaoMarkChoice(decision);
    internal global::CardGame.Core.SkillProgramStepOutcome ZhuitaoProgramMarkTarget(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ZhuitaoProgramMarkTarget(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome ZhuitaoProgramRevoke(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ZhuitaoProgramRevoke(frame, effect);
}
