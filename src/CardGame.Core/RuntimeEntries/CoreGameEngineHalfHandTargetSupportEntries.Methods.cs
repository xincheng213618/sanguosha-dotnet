#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHalfHandTargetSupportEntries
{
    internal IReadOnlyList<int> HalfHandActualTrickTargets(CardUseFrame use, IReadOnlyList<int> targets) => _resolve().HalfHandActualTrickTargets(use, targets);
    internal Dictionary<string, string> HalfHandSupportParameters(ProgramSkillFrame f, string option) => _resolve().HalfHandSupportParameters(f, option);
    internal bool HalfHandSupportPaymentMatches(ProgramSkillFrame f, ActualUseTargetWindowFrame parent, bool requirePaid) => _resolve().HalfHandSupportPaymentMatches(f, parent, requirePaid);
    internal bool HasHalfHandSelfTargetSupport(int target) => _resolve().HasHalfHandSelfTargetSupport(target);
    internal bool IsHalfHandImplicitSelfTarget(CardUseFrame use, int target) => _resolve().IsHalfHandImplicitSelfTarget(use, target);
    internal bool IsHalfHandSupportCandidate(ProgramTriggerCandidate candidate) => _resolve().IsHalfHandSupportCandidate(candidate);
    internal SkillProgramStepOutcome OfferHalfHandSupport(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().OfferHalfHandSupport(frame, effect);
    internal void ResolveHalfHandSupportChoice(PromptChoice choice) => _resolve().ResolveHalfHandSupportChoice(choice);
    internal PromptChoice SelectAiHalfHandSupport(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiHalfHandSupport(decision, frame);
}
