namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class AppliedResponseRulesRuntime(IAppliedResponseRelationStore store, IProgramRuntimeFrames frames,
    IProgramTurnQueries turn, IProgramRuleLifetimes lifetimes, IMatchParticipantStateAccess participants,
    ICardZoneQueries cards, RuntimeEntries.CoreGameEngineHealthQueriesEntries health,
    INativeCardUseAdmission admission) : IAppliedResponseRules
{
    public AppliedResponseRelation Grant(ProgramSkillFrame supplied, string relationId, int actorSeat, int? responderSeat,
        AppliedResponseRelationSpec spec, int issuanceSlot = 0)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        ArgumentNullException.ThrowIfNull(spec);
        if (string.IsNullOrWhiteSpace(relationId) || issuanceSlot < 0 || !Valid(actorSeat) ||
            responderSeat is { } target && !Valid(target) || turn.TurnNumber <= 0 ||
            responderSeat is null && spec.Until.Any(end => end.Participant == SkillStateSubject.Target))
            throw new InvalidOperationException("An applied response relation requires its original instruction and declared participants.");
        var source = new CardConversionSource(frame.SkillId, frame.TriggerId ?? frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId);
        var index = frame.InstructionIndex - 1;
        var previous = store.Relations.SingleOrDefault(relation => relation.ProducerFrameId == frame.Id &&
            relation.InstructionIndex == index && relation.IssuanceSlot == issuanceSlot && relation.RelationId == relationId);
        if (previous is not null)
        {
            if (previous.Source != source || previous.GameplayHash != frame.GameplayHash || previous.ActorSeat != actorSeat ||
                previous.ResponderSeat != responderSeat || !SameSpec(previous.Spec, spec))
                throw new InvalidOperationException("An issued response relation cannot change on reentry.");
            return previous;
        }
        var lifetime = spec.Until.Count == 0 ? null : lifetimes.CreateApplicationLifetime(frame.OwnerSeat, responderSeat ?? actorSeat,
            spec.Until, new(frame.OwnerSeat, responderSeat ?? actorSeat, IssuedTurnNumber: turn.TurnNumber));
        if (spec.Until.Count != 0 && lifetime is null)
            throw new InvalidOperationException("An applied response relation requires a valid remaining lifetime.");
        return store.Add(new(0, relationId, frame.Id, index, issuanceSlot, source, frame.GameplayHash, actorSeat,
            responderSeat, turn.TurnNumber, spec, lifetime));
    }

    public AppliedResponseRelation? GetMatchingRelation(string relationId, int actorSeat, int responderSeat,
        CardKind incomingKind, bool? effectiveIsRed) => store.Relations.LastOrDefault(relation =>
            relation.RelationId == relationId && MatchesRelation(relation, actorSeat, responderSeat, incomingKind, effectiveIsRed));

    private bool Valid(int seat) => seat >= 0 && seat < participants.Players.Count;
    private bool MatchesRelation(AppliedResponseRelation relation, int actor, int responder, CardKind kind, bool? red, bool applyExcludeActor = true) =>
        relation.ActorSeat == actor && (relation.ResponderSeat is null || relation.ResponderSeat == responder) &&
        (relation.Spec.Rule.TargetSeat is null || relation.Spec.Rule.TargetSeat == responder) &&
        (!applyExcludeActor || !relation.Spec.Rule.ExcludeActor || actor != responder) &&
        (relation.Spec.RemainingExpiry != AppliedResponseRemainingExpiry.IssuedTurnNumberChanges || relation.IssuedTurnNumber == turn.TurnNumber) &&
        (!relation.Spec.ActorMustBeTurnOwner || actor == turn.CurrentSeat && relation.IssuedTurnNumber == turn.TurnNumber) &&
        (relation.Spec.Match.Kind switch
        {
            ResponseRelationMatchKind.Any => true,
            ResponseRelationMatchKind.ExactCardName => CardCatalog.Get(relation.Spec.Match.ExactNameKind!.Value).DisplayName == CardCatalog.Get(kind).DisplayName,
            ResponseRelationMatchKind.EffectiveColor => relation.Spec.Match.IsRed is { } expected && red == expected,
            _ => false
        });

    private IReadOnlyList<ResponseRule> CaptureRules(int actor, CardKind kind, bool? red, AppliedResponseCaptureKind capture,
        int provider, CardActionContext? action)
    {
        List<ResponseRule>? rules = null;
        foreach (var relation in store.Relations)
        {
            var spec = relation.Spec;
            if ((spec.CaptureKinds & capture) == 0 ||
                spec.SelfProvidedCounterspellOnly && capture == AppliedResponseCaptureKind.Counterspell &&
                    (provider != actor || action?.ResponderSeat != actor || action.RequesterSeat is not null)) continue;
            if (relation.ResponderSeat is null && !spec.RequireLivingParticipantsAtCapture)
            {
                if (MatchesRelation(relation, actor, spec.Rule.TargetSeat ?? actor, kind, red, applyExcludeActor: false))
                    (rules ??= []).Add(IssuedRule(relation, spec.Rule.TargetSeat));
                continue;
            }
            // Capture-time life conditions never refresh after issuance.
            foreach (var responder in relation.ResponderSeat is { } target ? new[] { target } : Enumerable.Range(0, participants.Players.Count))
            {
                if (!MatchesRelation(relation, actor, responder, kind, red, applyExcludeActor: false) ||
                    spec.RequireLivingParticipantsAtCapture && (!participants.At(actor).IsAlive || !participants.At(responder).IsAlive)) continue;
                (rules ??= []).Add(IssuedRule(relation, responder));
            }
        }
        return rules is null ? [] : Array.AsReadOnly(rules.ToArray());
    }
    private static ResponseRule IssuedRule(AppliedResponseRelation relation, int? responder)
    {
        var rule = relation.Spec.Rule;
        return new(rule.Minimum, rule.Forbid, responder, rule.ActorSeat, rule.OpponentSeat,
            rule.IncomingKinds, rule.RequiredKinds, rule.While, rule.MatchUseKind, rule.SkillName,
            rule.Audience, relation.Source, rule.ExcludeActor);
    }

    public void CaptureNativeUse(CardUseFrame supplied, CardUseDeclaredEvent? declaration = null)
    {
        if (frames.Find(supplied.Id) is not CardUseFrame use || use.AppliedResponseRulesCaptured) return;
        var action = use.Action;
        if (action is null)
        {
            if (declaration is null || !admission.IsDeclaredUse(use, declaration) || !admission.IsLegacyProducerUse(use)) return;
        }
        else if (action.Type != CardActionType.Use || action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind)
            throw new InvalidOperationException("Applied response rules require the original native declaration.");
        var captured = CaptureRules(use.SourceSeat, action?.EffectiveKind ?? use.CardKind,
            action is null ? null : Color(action), action is null ? AppliedResponseCaptureKind.ActionlessNativeUse : AppliedResponseCaptureKind.NativeUse,
            action?.ProviderSeat ?? use.SourceSeat, action);
        frames.Replace(use.Id, use with { AppliedResponseRulesCaptured = true,
            ResponseRules = captured.Count == 0 ? use.ResponseRules : [.. use.ResponseRules ?? [], .. captured] });
    }

    public void CaptureCounterspell(NullificationWindowFrame supplied, CardActionContext acceptedAction, int? issuedChainDepth = null)
    {
        if (frames.Find(supplied.Id) is not NullificationWindowFrame window)
            throw new InvalidOperationException("A counterspell rule requires its original live native window.");
        var depth = issuedChainDepth ?? window.ChainDepth;
        if (depth <= 0 || depth < window.ChainDepth || depth > window.ChainDepth + 1 ||
            acceptedAction.Type != CardActionType.Response || acceptedAction.EffectiveKind != CardKind.Nullification ||
            acceptedAction.ResponderSeat != acceptedAction.ActorSeat || !Valid(acceptedAction.ActorSeat) || !Valid(acceptedAction.ProviderSeat) ||
            acceptedAction.RequesterSeat is not null ||
            acceptedAction.OpponentSeat != window.SourceSeat ||
            acceptedAction.ParentActionId != (window.DelayedReturn is null ? (frames.Find(window.ParentFrameId) as CardUseFrame)?.Action?.ActionId : null))
            throw new InvalidOperationException("A counterspell rule requires the actual accepted response node.");
        if (window.ResponseRuleNode is { } previous && previous.ChainDepth == depth)
        {
            if (previous.Action.ActionId != acceptedAction.ActionId)
                throw new InvalidOperationException("An issued counterspell node cannot change on reentry.");
            return;
        }
        var captured = CaptureRules(acceptedAction.ActorSeat, acceptedAction.EffectiveKind, Color(acceptedAction),
            AppliedResponseCaptureKind.Counterspell, acceptedAction.ProviderSeat, acceptedAction);
        frames.Replace(window.Id, window with { ResponseRuleNode = new(depth, acceptedAction), ResponseRules = captured.Count == 0 ? null : captured });
    }

    private static bool? Color(CardActionContext action) => action.EffectiveIsRed ?? action.EffectiveSuit switch
    { Suit.Heart or Suit.Diamond => true, Suit.Spade or Suit.Club => false, _ => null };

    public void IssueResponseRule(long frameId, ResponseRule rule)
    {
        var frame = frames.Find(frameId) ?? throw new InvalidOperationException("A response rule lost its original frame.");
        if (frame is ProgramSkillFrame { CardAttack.ProgramUseInstructionIndex: { } instruction }) rule = rule with { InstructionIndex = instruction };
        else if (frame is not CardUseFrame) throw new InvalidOperationException("A response rule requires its owning use or issued direct-card instruction.");
        frames.Replace(frame.Id, frame with { ResponseRules = [.. frame.ResponseRules ?? [], rule] });
    }
    public void RemoveResponseRulesIssuedBy(long frameId, CardConversionSource source)
    {
        var frame = frames.Find(frameId) ?? throw new InvalidOperationException("A response rule lost its original frame.");
        if (frame.ResponseRules is { } rules) frames.Replace(frame.Id, frame with { ResponseRules = rules.Where(rule => rule.Source != source).ToArray() });
    }

    public NativeResponseQuery? CreateResponseQuery(long incomingFrameId, int opponentSeat, int responderSeat,
        CardKind incoming, CardKind required, IReadOnlyList<int> targets, int? providerSeat = null, int counterspellDepth = 0) =>
        frames.Find(incomingFrameId) switch
        {
            CardUseFrame use when counterspellDepth == 0 => new(incomingFrameId, null, use.SourceSeat, opponentSeat, responderSeat, providerSeat ?? responderSeat,
                incoming, required, targets, IncomingActionId: use.Action?.ActionId),
            ProgramSkillFrame { CardAttack: { ProgramUseInstructionIndex: { } instruction } attack } when counterspellDepth == 0 =>
                new(incomingFrameId, instruction, attack.CardUserSeat, opponentSeat, responderSeat, providerSeat ?? responderSeat, incoming, required, targets),
            _ => null
        };

    public NativeResponseQuery? CreateCounterspellQuery(NullificationWindowFrame supplied, int opponentSeat, int responderSeat, int? providerSeat = null)
    {
        if (frames.Find(supplied.Id) is not NullificationWindowFrame window) return null;
        if (window.ChainDepth > 0)
        {
            if (window.ResponseRuleNode is not { } node || node.ChainDepth != window.ChainDepth) return null;
            return new(window.Id, null, node.Action.ActorSeat, opponentSeat, responderSeat, providerSeat ?? responderSeat,
                // The counterspell is the incoming action; root trick targets
                // do not become targets of this independent response node.
                CardKind.Nullification, CardKind.Nullification, node.Action.TargetSeats, window.ChainDepth, node.Action.ActionId);
        }
        // A delayed effect has its own identity and never borrows a parent use.
        if (window.DelayedReturn is not null) return null;
        return CreateResponseQuery(window.ParentFrameId, opponentSeat, responderSeat, window.EffectCardKind,
            CardKind.Nullification, window.TargetSeats, providerSeat);
    }

    public ResponseDecision GetResponseDecision(NativeResponseQuery query)
    {
        var owner = frames.Find(query.IncomingFrameId);
        if (owner is not { ResponseRules: { Count: > 0 } rules }) return ResponseDecision.Default;
        CardKind? kind;
        if (owner is NullificationWindowFrame window)
        {
            if (query.CounterspellDepth <= 0 || window.ChainDepth != query.CounterspellDepth || window.ResponseRuleNode is not { } node ||
                node.ChainDepth != query.CounterspellDepth || node.Action.ActionId != query.IncomingActionId || node.Action.ActorSeat != query.ActorSeat)
                return ResponseDecision.Default;
            kind = node.Action.EffectiveKind;
        }
        else
        {
            if (query.CounterspellDepth != 0) return ResponseDecision.Default;
            kind = owner switch { CardUseFrame use => use.Action?.EffectiveKind ?? use.CardKind, ProgramSkillFrame program => program.CardAttack?.EffectiveCardKind, _ => null };
        }
        var minimum = 1; var forbidden = false;
        List<string>? names = null;
        ResponseRuleExpressionWorld? world = null;
        foreach (var rule in rules)
        {
            if (rule.InstructionIndex != query.InstructionIndex || rule.Audience == ResponseRuleAudience.Targets && !query.TargetSeats.Contains(query.ResponderSeat) ||
                rule.ExcludeActor && query.ActorSeat == query.ResponderSeat || rule.TargetSeat is { } target && target != query.ResponderSeat ||
                rule.ActorSeat is { } actor && actor != query.ActorSeat || rule.OpponentSeat is { } opponent && opponent != query.OpponentSeat ||
                rule.MatchUseKind && kind != query.IncomingKind || rule.IncomingKinds.Count != 0 && !ProgramCardKindMatch.Contains(rule.IncomingKinds, query.IncomingKind) ||
                rule.RequiredKinds.Count != 0 && !ProgramCardKindMatch.Contains(rule.RequiredKinds, query.RequiredKind) ||
                rule.While is { } condition && ProgramExpressions.EvaluateBool(condition, world ??= new(query, participants.Players, cards, health)) != true) continue;
            minimum = Math.Max(minimum, rule.Minimum); forbidden |= rule.Forbid;
            if (rule.SkillName is { } name && !(names ??= []).Contains(name, StringComparer.Ordinal)) names.Add(name);
        }
        return new(minimum, forbidden, names is null ? [] : Array.AsReadOnly(names.ToArray()));
    }

    public bool HasUnconditionalResponseProhibition(long frameId)
    {
        var owner = frames.Find(frameId);
        if (owner is not { ResponseRules: { } rules }) return false;
        var instruction = owner is ProgramSkillFrame { CardAttack.ProgramUseInstructionIndex: { } index } ? index : (int?)null;
        var actor = owner switch { CardUseFrame use => use.SourceSeat, ProgramSkillFrame program => program.CardAttack?.CardUserSeat, _ => null };
        return actor is { } currentActor && rules.Any(rule => rule.InstructionIndex == instruction &&
            rule.Audience == ResponseRuleAudience.All && rule.Forbid && !rule.ExcludeActor && rule.TargetSeat is null &&
            (rule.ActorSeat is null || rule.ActorSeat == currentActor) && rule.OpponentSeat is null &&
            rule.IncomingKinds.Count == 0 && rule.RequiredKinds.Count == 0 && rule.While is null && !rule.MatchUseKind);
    }

    public bool IsResponseForbidden(long frameId, int responderSeat, CardKind requiredKind, int? opponentSeat = null)
    {
        var owner = frames.Find(frameId);
        var (actor, kind, targets) = owner switch
        {
            CardUseFrame use => (use.SourceSeat, (CardKind?)(use.Action?.EffectiveKind ?? use.CardKind), use.TargetSeats),
            ProgramSkillFrame { CardAttack: { } attack } => (attack.CardUserSeat, attack.EffectiveCardKind, (IReadOnlyList<int>)[attack.TargetSeat]),
            _ => (-1, (CardKind?)null, (IReadOnlyList<int>)[])
        };
        return kind is { } incoming && CreateResponseQuery(frameId, opponentSeat ?? actor, responderSeat, incoming, requiredKind, targets) is { } query && GetResponseDecision(query).Forbidden;
    }
    public bool IsCounterspellResponseForbidden(NullificationWindowFrame window, int responderSeat, int? providerSeat = null) =>
        CreateCounterspellQuery(window, window.SourceSeat, responderSeat, providerSeat) is { } query && GetResponseDecision(query).Forbidden;

    public void Apply(ProgramSkillFrame supplied, ProgramAppliedResponseRelation declaration)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        int Resolve(ProgramParticipantReference reference) => reference.Kind switch
        {
            ProgramParticipantRef.Owner => frame.OwnerSeat,
            ProgramParticipantRef.SelectedTarget when frame.SelectedTargetSeats.Count == 1 => frame.SelectedTargetSeats[0],
            ProgramParticipantRef.EventSource when frame.WindowContext?.SourceSeat is { } source => source,
            ProgramParticipantRef.EventSource when frame.WindowContext?.CardUse is { } use => use.ActorSeat,
            ProgramParticipantRef.EventTarget when frame.WindowContext?.TargetSeat is { } target => target,
            _ => throw new InvalidOperationException("An applied response relation lost its declared participant.")
        };
        var spec = declaration.Spec;
        if (declaration.SourceBind is { } bind)
        {
            var source = frame.CardSetBindings.Single(binding => binding.Name == bind);
            if (source.CardIds.Count != 1) throw new InvalidOperationException("A response relation requires one frozen source card.");
            var card = cards.CardsAt(cards.GetLocation(source.CardIds[0])).Single(card => card.Id == source.CardIds[0]);
            var match = declaration.SourceMatchKind switch
            {
                ResponseRelationMatchKind.ExactCardName => new ResponseRelationCardMatch(ResponseRelationMatchKind.ExactCardName, card.Kind),
                ResponseRelationMatchKind.EffectiveColor => new(ResponseRelationMatchKind.EffectiveColor, IsRed: source.FrozenRevealedSuit switch
                    { Suit.Heart or Suit.Diamond => true, Suit.Spade or Suit.Club => false, _ => null }),
                _ => throw new InvalidOperationException("A response relation requires a declared source-card matcher.")
            };
            spec = new(spec.Rule, match, spec.CaptureKinds, spec.Until, spec.RemainingExpiry,
                spec.ActorMustBeTurnOwner, spec.SelfProvidedCounterspellOnly, spec.RequireLivingParticipantsAtCapture);
        }
        Grant(frame, declaration.RelationId, Resolve(declaration.ActorRef), declaration.ResponderRef is { } responder ? Resolve(responder) : null, spec);
    }

    private static bool SameSpec(AppliedResponseRelationSpec left, AppliedResponseRelationSpec right) =>
        left.Match == right.Match && left.CaptureKinds == right.CaptureKinds && left.RemainingExpiry == right.RemainingExpiry &&
        left.ActorMustBeTurnOwner == right.ActorMustBeTurnOwner && left.SelfProvidedCounterspellOnly == right.SelfProvidedCounterspellOnly &&
        left.RequireLivingParticipantsAtCapture == right.RequireLivingParticipantsAtCapture && left.Until.SequenceEqual(right.Until) &&
        SameRule(left.Rule, right.Rule);
    private static bool SameRule(ResponseRule left, ResponseRule right) => left.Minimum == right.Minimum && left.Forbid == right.Forbid &&
        left.TargetSeat == right.TargetSeat && left.ActorSeat == right.ActorSeat && left.OpponentSeat == right.OpponentSeat &&
        left.IncomingKinds.SequenceEqual(right.IncomingKinds) && left.RequiredKinds.SequenceEqual(right.RequiredKinds) &&
        left.MatchUseKind == right.MatchUseKind && left.SkillName == right.SkillName && left.Audience == right.Audience &&
        left.Source == right.Source && left.ExcludeActor == right.ExcludeActor && left.InstructionIndex == right.InstructionIndex &&
        (ReferenceEquals(left.While, right.While) || left.While is { } condition && right.While is { } other && condition.StructurallyEquals(other));
}
