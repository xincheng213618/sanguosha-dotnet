#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramAttackRangeAidEntries
{
    internal void PublishProgramAttackRangeAid(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMoveReason reason) => _resolve().PublishProgramAttackRangeAid(frame, reason);
    internal global::CardGame.Core.SkillProgramStepOutcome RequestProgramAttackRangeAid(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMoveReason reason) => _resolve().RequestProgramAttackRangeAid(frame, reason);
    internal void ResolveProgramAttackRangeAid(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramAttackRangeAid(frame, effect, selected);
    internal global::CardGame.Core.PromptChoice SelectAiProgramAttackRangeAid(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiProgramAttackRangeAid(decision, frame);
}
