#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramAttackRangeAidEntries
{
    internal void PublishProgramAttackRangeAid(ProgramSkillFrame frame, CardMoveReason reason) => _resolve().PublishProgramAttackRangeAid(frame, reason);
    internal SkillProgramStepOutcome RequestProgramAttackRangeAid(ProgramSkillFrame frame, CardMoveReason reason) => _resolve().RequestProgramAttackRangeAid(frame, reason);
    internal void ResolveProgramAttackRangeAid(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected) => _resolve().ResolveProgramAttackRangeAid(frame, effect, selected);
    internal PromptChoice SelectAiProgramAttackRangeAid(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiProgramAttackRangeAid(decision, frame);
}
