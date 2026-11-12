#nullable enable

namespace CardGame.Core.EngineOwners;

internal sealed partial class CoreGameEngineCardUseLifecycleRuntime : ICurrentResponseRuleProgramHost
{
    void ICurrentResponseRuleProgramHost.IssueCurrentResponseRule(ProgramSkillFrame frame, ProgramCurrentResponseRule declaration)
    {
        if (frame.WindowContext is not { CardUse: { } context } window ||
            LifecycleCardUse(context.ParentCardUseFrameId) is not { Action: { Type: CardActionType.Use } action } use || action.ActionId != context.CardActionId)
            throw new InvalidOperationException("An issued response rule requires the original accepted card use.");
        int? Resolve(ProgramParticipantReference? reference) => reference?.Kind switch
        {
            null => null, ProgramParticipantRef.Owner => frame.OwnerSeat, ProgramParticipantRef.EventSource => context.ActorSeat,
            ProgramParticipantRef.EventTarget => window.TargetSeat ?? throw new InvalidOperationException("A response rule lost its frozen current target."),
            _ => throw new InvalidOperationException("Unsupported issued response participant.")
        };
        var rule = declaration.Rule;
        IssueResponseRule(use.Id, new(rule.Minimum, rule.Forbid, Resolve(declaration.ResponderRef), Resolve(declaration.ActorRef),
            Resolve(declaration.OpponentRef), rule.IncomingKinds, rule.RequiredKinds, rule.While, rule.MatchUseKind,
            audience: rule.Audience, excludeActor: rule.ExcludeActor));
    }

    internal void IssueResponseRule(long frameId, ResponseRule rule)
        => _responseRules.IssueResponseRule(frameId, rule);

    internal void IssueNoResponsePolicy(IssuedCardNoResponse policy)
    {
        var use = LifecycleCardUse(policy.ParentFrameId) ?? throw new InvalidOperationException("A no-response policy requires its owning native use.");
        var alreadyNotified = use.ResponseRules?.Any(rule => rule.IsWholeUseNoResponsePolicy) == true;
        IssueResponseRule(use.Id, new(forbid: true, audience: ResponseRuleAudience.All, source: policy.Source)
            { IsWholeUseNoResponsePolicy = true });
        if (!alreadyNotified)
            _responseRuleEvents.AdvanceEventRulesAndQueueFact(new IssuedCardNoResponseEvent(policy));
    }

    internal void RemoveResponseRulesIssuedBy(long frameId, CardConversionSource source)
        => _responseRules.RemoveResponseRulesIssuedBy(frameId, source);

    internal NativeResponseQuery? CreateResponseQuery(long incomingFrameId, int opponentSeat, int responderSeat,
        CardKind incoming, CardKind required, IReadOnlyList<int> targets, int? providerSeat = null, int counterspellDepth = 0) =>
        _responseRules.CreateResponseQuery(incomingFrameId, opponentSeat, responderSeat, incoming, required, targets, providerSeat, counterspellDepth);

    internal ResponseDecision GetResponseDecision(NativeResponseQuery query)
        => _responseRules.GetResponseDecision(query);

    internal bool HasUnconditionalResponseProhibition(long frameId)
        => _responseRules.HasUnconditionalResponseProhibition(frameId);
}
