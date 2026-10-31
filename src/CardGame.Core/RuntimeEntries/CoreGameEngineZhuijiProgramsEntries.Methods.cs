#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineZhuijiProgramsEntries
{
    internal SkillProgramStepOutcome ArmZhuijiActualPlay(ProgramSkillFrame input) => _resolve().ArmZhuijiActualPlay(input);
    internal ZhuijiPlayPhaseKey? CaptureZhuijiEndingPhase(CharacterState owner) => _resolve().CaptureZhuijiEndingPhase(owner);
    internal ProgramSkillWindowContext CreateZhuijiEndingContext(ProgramLifecycleTriggerWindowFrame f, ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CreateZhuijiEndingContext(f, c, context);
    internal bool HasIssuedZhuijiDebtCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().HasIssuedZhuijiDebtCandidate(c, w);
    internal IEnumerable<ProgramTriggerCandidate> IssuedZhuijiEndingCandidates(CharacterState owner) => _resolve().IssuedZhuijiEndingCandidates(owner);
    internal ZhuijiPlayGrant? MatchingZhuijiGrant(ProgramTriggerCandidate c) => _resolve().MatchingZhuijiGrant(c);
    internal ZhuijiPlayGrant? MatchingZhuijiGrant2(ProgramTriggerCandidate c, ZhuijiPlayPhaseKey phase) => _resolve().MatchingZhuijiGrant(c, phase);
    internal void ObserveZhuijiDebtTermination(IGameEvent payload) => _resolve().ObserveZhuijiDebtTermination(payload);
    internal void PublishZhuijiDiscardPrompt(ProgramSkillFrame f) => _resolve().PublishZhuijiDiscardPrompt(f);
    internal void ResolveZhuijiDiscardChoice(PromptChoice choice) => _resolve().ResolveZhuijiDiscardChoice(choice);
    internal bool ResumeZhuijiDebt(long id) => _resolve().ResumeZhuijiDebt(id);
    internal SkillProgramStepOutcome SettleZhuijiActualPlayDebt(ProgramSkillFrame input) => _resolve().SettleZhuijiActualPlayDebt(input);
    internal IEnumerable<ZhuijiPlayGrant> UnsettledZhuijiGrants() => _resolve().UnsettledZhuijiGrants();
    internal bool ValidZhuijiIssued(ProgramSkillFrame f) => _resolve().ValidZhuijiIssued(f);
}
