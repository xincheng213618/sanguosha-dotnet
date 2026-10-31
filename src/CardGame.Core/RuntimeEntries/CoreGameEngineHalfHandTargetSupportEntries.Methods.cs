#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHalfHandTargetSupportEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<int> HalfHandActualTrickTargets(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().HalfHandActualTrickTargets(use, targets);
    internal global::System.Collections.Generic.Dictionary<string, string> HalfHandSupportParameters(global::CardGame.Core.ProgramSkillFrame f, string option) => _resolve().HalfHandSupportParameters(f, option);
    internal bool HalfHandSupportPaymentMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ActualUseTargetWindowFrame parent, bool requirePaid) => _resolve().HalfHandSupportPaymentMatches(f, parent, requirePaid);
    internal bool HasHalfHandSelfTargetSupport(int target) => _resolve().HasHalfHandSelfTargetSupport(target);
    internal bool IsHalfHandImplicitSelfTarget(global::CardGame.Core.CardUseFrame use, int target) => _resolve().IsHalfHandImplicitSelfTarget(use, target);
    internal bool IsHalfHandSupportCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().IsHalfHandSupportCandidate(candidate);
    internal global::CardGame.Core.SkillProgramStepOutcome OfferHalfHandSupport(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().OfferHalfHandSupport(frame, effect);
    internal void ResolveHalfHandSupportChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveHalfHandSupportChoice(choice);
    internal global::CardGame.Core.PromptChoice SelectAiHalfHandSupport(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiHalfHandSupport(decision, frame);
}
