#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiaoHuaBoundaryMobileEntries
{
    internal void AnswerBoundaryLiaoHuaDiscardSlash(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerBoundaryLiaoHuaDiscardSlash(selected);
    internal void AssertBoundaryLiaoHuaDiscard(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertBoundaryLiaoHuaDiscard(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryLiaoHuaDiscardSlash(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginBoundaryLiaoHuaDiscardSlash(supplied);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryLiaoHuaDiscardChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundaryLiaoHuaDiscardChoices(frame);
    internal global::CardGame.Core.PromptChoice ChooseBoundaryLiaoHuaDiscard(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ChooseBoundaryLiaoHuaDiscard(prompt, frame);
    internal bool IsBoundaryLiaoHuaDiscardMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsBoundaryLiaoHuaDiscardMovement(frame, effect, pending);
    internal void PublishBoundaryLiaoHuaDiscardSlash(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishBoundaryLiaoHuaDiscardSlash(frame);
    internal bool ResumeBoundaryLiaoHuaDiscard(long id) => _resolve().ResumeBoundaryLiaoHuaDiscard(id);
    internal bool ReturnBoundaryLiaoHuaDiscardMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnBoundaryLiaoHuaDiscardMovement(frame);
    internal bool ValidBoundaryLiaoHuaDiscardSlash(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidBoundaryLiaoHuaDiscardSlash(frame);
}
