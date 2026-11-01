#nullable enable

namespace CardGame.Core.EngineOwners;

internal sealed partial class CoreGameEngineCardUseLifecycleRuntime
{
    internal void IssueResponseRule(long frameId, ResponseRule rule)
    {
        var frame = _resolutionStackOperations._resolutionStack.Single(f => f.Id == frameId);
        if (frame is ProgramSkillFrame { CardAttack.ProgramUseInstructionIndex: { } instruction })
            rule = rule with { InstructionIndex = instruction };
        else if (frame is not CardUseFrame)
            throw new InvalidOperationException("A response rule requires its owning use or issued direct-card instruction.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { ResponseRules = [.. frame.ResponseRules ?? [], rule] });
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
        if (query.CounterspellDepth != 0 || !query.TargetSeats.Contains(query.ResponderSeat) ||
            _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == query.IncomingFrameId) is not { ResponseRules: { } rules } owner)
            return ResponseDecision.Default;
        var kind = owner switch { CardUseFrame use => use.Action?.EffectiveKind ?? use.CardKind, ProgramSkillFrame program => program.CardAttack?.EffectiveCardKind, _ => null };
        var minimum = 1;
        var forbidden = false;
        List<string>? names = null;
        ResponseRuleExpressionWorld? world = null;
        foreach (var rule in rules)
        {
            if (rule.InstructionIndex != query.InstructionIndex || rule.TargetSeat is { } target && target != query.ResponderSeat ||
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
}
