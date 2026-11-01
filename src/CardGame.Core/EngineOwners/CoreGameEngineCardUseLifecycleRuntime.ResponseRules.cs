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
            audience: rule.Audience));
    }

    internal void IssueResponseRule(long frameId, ResponseRule rule)
    {
        var frame = _resolutionStackOperations._resolutionStack.Single(f => f.Id == frameId);
        if (frame is ProgramSkillFrame { CardAttack.ProgramUseInstructionIndex: { } instruction })
            rule = rule with { InstructionIndex = instruction };
        else if (frame is not CardUseFrame)
            throw new InvalidOperationException("A response rule requires its owning use or issued direct-card instruction.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { ResponseRules = [.. frame.ResponseRules ?? [], rule] });
    }

    internal void IssueNoResponsePolicy(IssuedCardNoResponse policy)
    {
        var use = LifecycleCardUse(policy.ParentFrameId) ?? throw new InvalidOperationException("A no-response policy requires its owning native use.");
        var alreadyNotified = use.ResponseRules?.Any(rule => rule.Forbid && rule.Audience == ResponseRuleAudience.All && rule.Source is not null) == true;
        IssueResponseRule(use.Id, new(forbid: true, audience: ResponseRuleAudience.All, source: policy.Source));
        if (!alreadyNotified)
            _responseRuleEvents.AdvanceEventRulesAndQueueFact(new IssuedCardNoResponseEvent(policy));
    }

    internal void RemoveResponseRulesIssuedBy(long frameId, CardConversionSource source)
    {
        var frame = _resolutionStackOperations._resolutionStack.Single(frame => frame.Id == frameId);
        if (frame.ResponseRules is { } rules)
            _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { ResponseRules = rules.Where(rule => rule.Source != source).ToArray() });
    }

    internal NativeResponseQuery? CreateResponseQuery(long incomingFrameId, int opponentSeat, int responderSeat,
        CardKind incoming, CardKind required, IReadOnlyList<int> targets, int? providerSeat = null, int counterspellDepth = 0) =>
        _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == incomingFrameId) switch
        {
            CardUseFrame use => new(incomingFrameId, null, use.SourceSeat, opponentSeat, responderSeat, providerSeat ?? responderSeat, incoming, required, targets, counterspellDepth),
            ProgramSkillFrame { CardAttack: { ProgramUseInstructionIndex: { } instruction } attack } =>
                new(incomingFrameId, instruction, attack.CardUserSeat, opponentSeat, responderSeat, providerSeat ?? responderSeat, incoming, required, targets, counterspellDepth),
            _ => null
        };

    internal ResponseDecision GetResponseDecision(NativeResponseQuery query)
    {
        if (query.CounterspellDepth != 0 ||
            _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == query.IncomingFrameId) is not { ResponseRules: { } rules } owner)
            return ResponseDecision.Default;
        var kind = owner switch { CardUseFrame use => use.Action?.EffectiveKind ?? use.CardKind, ProgramSkillFrame program => program.CardAttack?.EffectiveCardKind, _ => null };
        var minimum = 1;
        var forbidden = false;
        List<string>? names = null;
        ResponseRuleExpressionWorld? world = null;
        foreach (var rule in rules)
        {
            if (rule.InstructionIndex != query.InstructionIndex || rule.Audience == ResponseRuleAudience.Targets && !query.TargetSeats.Contains(query.ResponderSeat) ||
                rule.TargetSeat is { } target && target != query.ResponderSeat ||
                rule.ActorSeat is { } actor && actor != query.ActorSeat || rule.OpponentSeat is { } opponent && opponent != query.OpponentSeat ||
                rule.MatchUseKind && kind != query.IncomingKind ||
                rule.IncomingKinds.Count != 0 && !ProgramCardKindMatch.Contains(rule.IncomingKinds, query.IncomingKind) ||
                rule.RequiredKinds.Count != 0 && !ProgramCardKindMatch.Contains(rule.RequiredKinds, query.RequiredKind) ||
                rule.While is { } condition && ProgramExpressions.EvaluateBool(condition, world ??= new(query,
                    _coreMatchParticipantsPlayersAccess._players, _cardZonesOperations._cardZones, _coreHealthQueriesOperations)) != true)
                continue;
            minimum = Math.Max(minimum, rule.Minimum);
            forbidden |= rule.Forbid;
            if (rule.SkillName is { } name && !(names ??= []).Contains(name, StringComparer.Ordinal)) names.Add(name);
        }
        return new(minimum, forbidden, names is null ? [] : Array.AsReadOnly(names.ToArray()));
    }

    internal bool HasUnconditionalResponseProhibition(long frameId)
    {
        var owner = _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == frameId);
        if (owner is not { ResponseRules: { } rules }) return false;
        var instruction = owner is ProgramSkillFrame { CardAttack.ProgramUseInstructionIndex: { } index } ? index : (int?)null;
        var actor = owner switch { CardUseFrame use => use.SourceSeat, ProgramSkillFrame program => program.CardAttack?.CardUserSeat, _ => null };
        return actor is { } currentActor && rules.Any(rule => rule.InstructionIndex == instruction &&
            rule.Audience == ResponseRuleAudience.All && rule.Forbid && rule.TargetSeat is null &&
            (rule.ActorSeat is null || rule.ActorSeat == currentActor) && rule.OpponentSeat is null &&
            rule.IncomingKinds.Count == 0 && rule.RequiredKinds.Count == 0 && rule.While is null && !rule.MatchUseKind);
    }
}
