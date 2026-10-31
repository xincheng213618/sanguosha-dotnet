#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineZhuijiProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome ArmZhuijiActualPlay(global::CardGame.Core.ProgramSkillFrame input) => _resolve().ArmZhuijiActualPlay(input);
    internal global::CardGame.Core.ZhuijiPlayPhaseKey? CaptureZhuijiEndingPhase(global::CardGame.Core.CharacterState owner) => _resolve().CaptureZhuijiEndingPhase(owner);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateZhuijiEndingContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame f, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CreateZhuijiEndingContext(f, c, context);
    internal bool HasIssuedZhuijiDebtCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().HasIssuedZhuijiDebtCandidate(c, w);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedZhuijiEndingCandidates(global::CardGame.Core.CharacterState owner) => _resolve().IssuedZhuijiEndingCandidates(owner);
    internal global::CardGame.Core.ZhuijiPlayGrant? MatchingZhuijiGrant(global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().MatchingZhuijiGrant(c);
    internal global::CardGame.Core.ZhuijiPlayGrant? MatchingZhuijiGrant2(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ZhuijiPlayPhaseKey phase) => _resolve().MatchingZhuijiGrant(c, phase);
    internal void ObserveZhuijiDebtTermination(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveZhuijiDebtTermination(payload);
    internal void PublishZhuijiDiscardPrompt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishZhuijiDiscardPrompt(f);
    internal void ResolveZhuijiDiscardChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveZhuijiDiscardChoice(choice);
    internal bool ResumeZhuijiDebt(long id) => _resolve().ResumeZhuijiDebt(id);
    internal global::CardGame.Core.SkillProgramStepOutcome SettleZhuijiActualPlayDebt(global::CardGame.Core.ProgramSkillFrame input) => _resolve().SettleZhuijiActualPlayDebt(input);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ZhuijiPlayGrant> UnsettledZhuijiGrants() => _resolve().UnsettledZhuijiGrants();
    internal bool ValidZhuijiIssued(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidZhuijiIssued(f);
}
