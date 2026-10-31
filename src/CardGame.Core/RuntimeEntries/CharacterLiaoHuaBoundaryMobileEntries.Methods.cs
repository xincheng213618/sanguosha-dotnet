#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiaoHuaBoundaryMobileEntries
{
    internal void AnswerBoundaryLiaoHuaDiscardSlash(PromptChoice selected) => _resolve().AnswerBoundaryLiaoHuaDiscardSlash(selected);
    internal SkillProgramStepOutcome BeginBoundaryLiaoHuaDiscardSlash(ProgramSkillFrame supplied) => _resolve().BeginBoundaryLiaoHuaDiscardSlash(supplied);
    internal IReadOnlyList<PromptChoice> BoundaryLiaoHuaDiscardChoices(ProgramSkillFrame frame) => _resolve().BoundaryLiaoHuaDiscardChoices(frame);
    internal PromptChoice ChooseBoundaryLiaoHuaDiscard(PendingDecision prompt, ProgramSkillFrame frame) => _resolve().ChooseBoundaryLiaoHuaDiscard(prompt, frame);
    internal void PublishBoundaryLiaoHuaDiscardSlash(ProgramSkillFrame frame) => _resolve().PublishBoundaryLiaoHuaDiscardSlash(frame);
    internal bool ResumeBoundaryLiaoHuaDiscard(long id) => _resolve().ResumeBoundaryLiaoHuaDiscard(id);
    internal bool ReturnBoundaryLiaoHuaDiscardMovement(ProgramSkillFrame frame) => _resolve().ReturnBoundaryLiaoHuaDiscardMovement(frame);
    internal bool ValidBoundaryLiaoHuaDiscardSlash(ProgramSkillFrame frame) => _resolve().ValidBoundaryLiaoHuaDiscardSlash(frame);
}
