#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndRefillNextTurnEntries
{
    internal SkillProgramStepOutcome BeginEndRefillNextTurn(ProgramSkillFrame supplied) => _resolve().BeginEndRefillNextTurn(supplied);
    internal bool CanContinueEndRefillNextTurn(ProgramSkillFrame f) => _resolve().CanContinueEndRefillNextTurn(f);
    internal void CompleteEndRefillNextTurn(ProgramSkillFrame f) => _resolve().CompleteEndRefillNextTurn(f);
    internal void ConsumeEndRefillNextTurn(CharacterState current) => _resolve().ConsumeEndRefillNextTurn(current);
    internal bool DrainEndRefillNextTurnChildren(ProgramSkillFrame f) => _resolve().DrainEndRefillNextTurnChildren(f);
    internal IReadOnlyList<PromptChoice> EndRefillNextTurnChoices(ProgramSkillFrame f) => _resolve().EndRefillNextTurnChoices(f);
    internal double EndRefillNextTurnPublicScore(int owner) => _resolve().EndRefillNextTurnPublicScore(owner);
    internal double EndRefillPublicChoiceScore(int owner, int target) => _resolve().EndRefillPublicChoiceScore(owner, target);
    internal bool IsExactOwnAfterTurnEndedProgram(ProgramSkillFrame f) => _resolve().IsExactOwnAfterTurnEndedProgram(f);
    internal void PublishEndRefillNextTurnChoice(ProgramSkillFrame supplied) => _resolve().PublishEndRefillNextTurnChoice(supplied);
    internal void RequireEndRefillNextTurnReceipt(ProgramSkillFrame f) => _resolve().RequireEndRefillNextTurnReceipt(f);
    internal void ResolveEndRefillNextTurnChoice(PromptChoice choice) => _resolve().ResolveEndRefillNextTurnChoice(choice);
    internal bool ResumeEndRefillNextTurn(long id) => _resolve().ResumeEndRefillNextTurn(id);
    internal bool ReturnEndRefillNextTurnMovement(ProgramSkillFrame f) => _resolve().ReturnEndRefillNextTurnMovement(f);
    internal PromptChoice SelectAiEndRefillNextTurn(PendingDecision prompt, ProgramSkillFrame f) => _resolve().SelectAiEndRefillNextTurn(prompt, f);
}
