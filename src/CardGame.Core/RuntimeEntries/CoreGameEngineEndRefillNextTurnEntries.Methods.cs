#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndRefillNextTurnEntries
{
    internal void AssertRegisteredEndRefillNextTurnReceipt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertRegisteredEndRefillNextTurnReceipt(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginEndRefillNextTurn(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginEndRefillNextTurn(supplied);
    internal bool CanContinueEndRefillNextTurn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueEndRefillNextTurn(f);
    internal void CompleteEndRefillNextTurn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteEndRefillNextTurn(f);
    internal void ConsumeEndRefillNextTurn(global::CardGame.Core.CharacterState current) => _resolve().ConsumeEndRefillNextTurn(current);
    internal bool DrainEndRefillNextTurnChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainEndRefillNextTurnChildren(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> EndRefillNextTurnChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().EndRefillNextTurnChoices(f);
    internal double EndRefillNextTurnPublicScore(int owner) => _resolve().EndRefillNextTurnPublicScore(owner);
    internal double EndRefillPublicChoiceScore(int owner, int target) => _resolve().EndRefillPublicChoiceScore(owner, target);
    internal bool IsEndRefillNextTurnMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsEndRefillNextTurnMovement(f, e, p);
    internal bool IsExactOwnAfterTurnEndedProgram(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsExactOwnAfterTurnEndedProgram(f);
    internal void PublishEndRefillNextTurnChoice(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().PublishEndRefillNextTurnChoice(supplied);
    internal void RequireEndRefillNextTurnReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireEndRefillNextTurnReceipt(f);
    internal void ResolveEndRefillNextTurnChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveEndRefillNextTurnChoice(choice);
    internal bool ResumeEndRefillNextTurn(long id) => _resolve().ResumeEndRefillNextTurn(id);
    internal bool ReturnEndRefillNextTurnMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnEndRefillNextTurnMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiEndRefillNextTurn(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiEndRefillNextTurn(prompt, f);
}
