#nullable enable

using System.Collections.Frozen;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaYiMouRuntime
{
    internal void BeginMouSmSecretUse(long useId, long eventStart, long logStart)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId)!;
        if (use.Action is not { } a || !CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind))
            return;
        var b = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat]).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseCommitted).FirstOrDefault(b => b.SkillId == MouSimaYiComposition.Pingliao && MouSimaYiComposition.Private(b.Trigger));
        var original = use.AcceptedDeclarationPayment?.MouSimaYiSecret;
        if (b is null && original is null)
            return;
        var lease = new MouSmSecretLease(original?.Source ?? new(b!.SkillId, b.Trigger.Id, use.SourceSeat, b.SkillInstanceId), original?.GameplayHash ?? b!.Program.GameplayHash, a.ActionId, eventStart, logStart);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use = use with { MouSimaYiSecret = lease });
        for (var i = 0; i < _coreCommandJournalStorageEventSequenceEventsPendingEventsAccess._pendingEvents.Count; i++)
            if (_coreCommandJournalStorageEventSequenceEventsPendingEventsAccess._eventSequence + i + 1 >= eventStart)
                CaptureMouSmSecretFact(_coreCommandJournalStorageEventSequenceEventsPendingEventsAccess._pendingEvents[i], _coreCommandJournalStorageEventSequenceEventsPendingEventsAccess._eventSequence + i + 1);
        foreach (var log in _coreGameLogLogAccess._log.Where(l => l.Sequence > logStart).ToArray())
            CaptureMouSmSecretLog(log);
    }

    internal static bool IsMouSmTargetFact(IGameEvent e, CardUseFrame u) => e switch
    {
        TargetsConfirmedEvent x => x.ResolutionId == u.Id,
        CardUseAppearanceCapturedEvent x => x.Action.ActionId == u.Action!.ActionId,
        CardActionAcceptedEvent x => x.Action.ActionId == u.Action!.ActionId,
        CardUsedEvent x => x.SourceSeat == u.SourceSeat && x.CardId == u.CardId && x.CardKind == u.CardKind && x.TargetSeat is { } t && u.TargetSeats.Contains(t),
        ProgramViewAsConvertedEvent x => x.FrameId == u.Id && x.IsUse,
        FangtianHalberdUsedEvent x => x.ResolutionId == u.Id,
        ZhuqueFanConvertedEvent x => x.ResolutionId == u.Id,
        ZhangbaSerpentSpearConvertedEvent x => x.ResolutionId == u.Id && x.IsUse,
        ProgramCardTargetCountAppliedEvent x => x.ResolutionId == u.Id,
        LihuoSlashUsedEvent x => x.ResolutionId == u.Id,
        RedAdditionalTargetsConsumedEvent x => x.OwnerSeat == u.SourceSeat && x.Targets.SequenceEqual(u.TargetSeats),
        _ => false
    };
    internal void CaptureMouSmSecretFact(IGameEvent payload, long sequence)
    {
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(u => u.MouSimaYiSecret is { Started: false, Released: false } && IsMouSmTargetFact(payload, u));
        if (use?.MouSimaYiSecret is not { } s || s.EventSequences.Contains(sequence))
            return;
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { MouSimaYiSecret = s with { EventSequences = s.EventSequences.Append(sequence).ToArray() } });
    }

    internal void CaptureMouSmSecretLog(GameLogEntry log)
    {
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(u => u.MouSimaYiSecret is { Started: false, Released: false } && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == u.Id && log.ActorSeat == u.SourceSeat && log.TargetSeat is { } t && u.TargetSeats.Contains(t));
        if (use?.MouSimaYiSecret is not { } s || s.LogSequences.Contains(log.Sequence))
            return;
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { MouSimaYiSecret = s with { LogSequences = s.LogSequences.Append(log.Sequence).ToArray() } });
    }

    internal GameEngine.MouSmPublicProjection? PrepareMouSmPublicProjection()
    {
        var leases = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Select(u => u.MouSimaYiSecret).OfType<MouSmSecretLease>().ToArray();
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Where(e => e is MouSmTargetsDisclosedEvent or MouSmSecretDeclarationRecordedEvent).ToArray();
        if (leases.Length == 0 && facts.Length == 0)
            return null;
        var disclosed = facts.OfType<MouSmTargetsDisclosedEvent>().ToArray();
        return new(leases.SelectMany(x => x.EventSequences).Concat(disclosed.SelectMany(x => x.HeldEventSequences)).ToFrozenSet(), leases.SelectMany(x => x.LogSequences).Concat(disclosed.SelectMany(x => x.HeldLogSequences)).ToFrozenSet(), facts.OfType<MouSmSecretDeclarationRecordedEvent>().Select(x => x.DeclarationId).ToFrozenSet());
    }

    internal static EventEnvelope? MouSmPublicEvent(EventEnvelope envelope, GameEngine.MouSmPublicProjection? projection)
    {
        if (projection is null)
            return envelope;
        if (projection.Events.Contains(envelope.Sequence))
            return null;
        if (envelope.Payload is CardDeclarationCommittedEvent declaration && projection.Declarations.Contains(declaration.DeclarationId))
            return envelope with
            {
                Payload = CommittedEventProjection.Freeze(declaration with { TargetSeats = Array.Empty<int>() })
            };
        return envelope;
    }

    internal IReadOnlyList<EventEnvelope> MouSmPublicEvents()
    {
        var projection = PrepareMouSmPublicProjection();
        return projection is null ? _coreCommandJournalStorageEventSequenceEventsPendingEventsAccess._events.AsReadOnly() : Array.AsReadOnly(_coreCommandJournalStorageEventSequenceEventsPendingEventsAccess._events.Select(e => MouSmPublicEvent(e, projection)).OfType<EventEnvelope>().ToArray());
    }

    internal IReadOnlyList<GameLogEntry> MouSmPublicLog()
    {
        var projection = PrepareMouSmPublicProjection();
        return projection is null ? _coreGameLogLogAccess._log.AsReadOnly() : Array.AsReadOnly(_coreGameLogLogAccess._log.Where(e => !projection.Logs.Contains(e.Sequence)).ToArray());
    }

    internal DeclaredCardPayment CaptureMouSmDeclaration(DeclaredCardPayment payment, CardDeclarationParentReturn parent)
    {
        if (parent.Purpose is not (CardDeclarationPurpose.Use or CardDeclarationPurpose.ProvidedSlash or CardDeclarationPurpose.BorrowedSword or CardDeclarationPurpose.BorrowedSwordProvidedSlash or CardDeclarationPurpose.QinglongProvidedSlash) || !CoreNativeCardClassificationsRuntime.IsSlashCard(parent.FinalEffectiveKind ?? payment.DeclaredKind))
            return payment;
        var b = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[parent.ActorSeat]).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseCommitted).FirstOrDefault(b => b.SkillId == MouSimaYiComposition.Pingliao && MouSimaYiComposition.Private(b.Trigger));
        if (b is null)
            return payment;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSmSecretDeclarationRecordedEvent(payment.DeclarationId, parent.ActorSeat));
        return payment with
        {
            MouSimaYiSecret = new(new(b.SkillId, b.Trigger.Id, parent.ActorSeat, b.SkillInstanceId), b.Program.GameplayHash)
        };
    }

    internal IReadOnlyList<int> MouSmDeclarationTargets(CardDeclarationFrame frame, int viewer, bool revealAll) => frame.Payment.MouSimaYiSecret is not null && !revealAll && viewer != frame.Return.ActorSeat ? Array.Empty<int>() : Array.AsReadOnly(frame.TargetSeats.ToArray());
    internal bool HoldMouSmAiAttack(int source, int target) => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(u => u.SourceSeat == source && u.TargetSeats.Contains(target) && u.MouSimaYiSecret is { Released: false });
    internal bool TryBeginMouSmPrivateWindow(NativeOwners.CardAttackHandle attack, MouSmPrivateCall call)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Action: { } a, MouSimaYiSecret: { Started: false, Released: false } s } u)
            return false;
        if (a.ActionId != s.ActionId || a.ActorSeat != s.Source.OwnerSeat || !CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) || s.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(s.Source.SkillId).Program?.GameplayHash || _resolutionStackOperations._resolutionStack[^1].Id != u.Id)
            throw new InvalidOperationException("Pingliao lost its original secret Slash before target windows.");
        var trigger = _contentRegistryOperations._contentRegistry.GetSkill(s.Source.SkillId).Program!.Triggers.Single(t => t.Id == s.Source.BindingId);
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreRuntimeOperations.ReplaceRuntimeFrame(u.Id, u = u with { MouSimaYiSecret = s with { Started = true, WindowId = id, Call = call } });
        var context = _coreCardActionsOperations.CreateCardActionProgramContext(a, SkillProgramTriggerWindow.CardUseCommitted, id, u.SourceSeat, -1, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[u.SourceSeat], a));
        context = context with
        {
            CardUse = context.CardUse!with
            {
                ParentCardUseFrameId = u.Id
            }
        };
        var candidate = new ProgramCardTriggerCandidate(u.SourceSeat, -1, s.Source.SkillId, s.Source.BindingId, s.GameplayHash, s.Source.SkillInstanceId, trigger.Priority, context);
        _coreRuntimeOperations.PushRuntimeFrame(new ProgramCardTriggerWindowFrame(id, u.Id, a, ProgramCardContinuation.CommittedSlash, Array.AsReadOnly(new[] { candidate }), AttackOwnerFrameId: u.Id) { MouSimaYiPrivate = new(u.Id, a.ActionId, call) });
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramCardTriggerWindowFrame>();
        return true;
    }

    internal bool ValidMouSmPrivateWindow(ProgramCardTriggerWindowFrame p) => p.MouSimaYiPrivate is { } back && p.ParentFrameId == back.UseId && p.AttackOwnerFrameId == back.UseId && p.Continuation == ProgramCardContinuation.CommittedSlash && _coreCardUseLifecycleOperations.LifecycleCardUse(back.UseId)is { Action: { } a, MouSimaYiSecret: { Started: true } s } u && s.WindowId == p.Id && s.ActionId == back.ActionId && s.Call == back.Call && a.ActionId == back.ActionId && p.Action.ActionId == back.ActionId && a.Type == CardActionType.Use && a.ActorSeat == s.Source.OwnerSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) && a.TargetSeats.SequenceEqual(p.Action.TargetSeats) && p.Candidates is [var c] && c.OwnerSeat == s.Source.OwnerSeat && c.SkillId == s.Source.SkillId && c.TriggerId == s.Source.BindingId && c.SkillInstanceId == s.Source.SkillInstanceId && c.GameplayHash == s.GameplayHash && s.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash;
    [ProgramIssuedTrigger(420)]
    internal bool IsIssuedMouSmPrivateCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != MouSimaYiComposition.Pingliao || context.Window != SkillProgramTriggerWindow.CardUseCommitted || context.OwnerSeat != candidate.OwnerSeat || context.SourceSeat != candidate.OwnerSeat || context.TargetSeat is not null || context.CardUse is not { } info || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame { MouSimaYiPrivate: { } back } p || !ValidMouSmPrivateWindow(p) || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count)
            return false;
        var c = p.Candidates[p.CandidateIndex];
        return c.OwnerSeat == candidate.OwnerSeat && c.SkillId == candidate.SkillId && c.TriggerId == candidate.BindingId && c.SkillInstanceId == candidate.SkillInstanceId && c.GameplayHash == candidate.GameplayHash && candidate.OccurrenceIndex == 0 && info.ParentCardUseFrameId == back.UseId && info.CardActionId == back.ActionId && info.ActorSeat == candidate.OwnerSeat && info.EffectiveKind == p.Action.EffectiveKind && info.DesignatedTargetSeats is { } designated && designated.SequenceEqual(p.Action.EffectiveDesignatedTargetSeats);
    }
    internal void DiscloseMouSmTargets(ProgramSkillFrame f)
    {
        if (!MouSimaYi.Valid(f) || f.MouSimaYi is not { Operation: SkillProgramEffectOp.ResolveMouSimaYiPingliao } r || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { Action: { } a, MouSimaYiSecret: { Started: true, Released: false } s } u || s.WindowId != r.ParentId || a.ActionId != r.ActionId || !u.TargetSeats.SequenceEqual(r.Targets))
            throw new InvalidOperationException("Pingliao cannot disclose another original Use or disclose twice.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(u.Id, u with { MouSimaYiSecret = s with { Released = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSmTargetsDisclosedEvent(u.Id, a.ActionId, u.SourceSeat, u.CardId, u.CardKind) { Targets = u.TargetSeats, HeldEventSequences = s.EventSequences, HeldLogSequences = s.LogSequences });
        foreach (var target in u.TargetSeats)
            _coreNativeAiDecisionsOperations.NotifyAiOfSlash(_coreMatchParticipantsPlayersAccess._players[u.SourceSeat], _coreMatchParticipantsPlayersAccess._players[target]);
        _coreMatchLogOperations.AddLog("CardUsed", $"{(_coreMatchParticipantsPlayersAccess._players[u.SourceSeat].Name)} 的【{(CardCatalog.Get(u.CardKind).DisplayName)}】指定了 {(string.Join("、", u.TargetSeats.Select(t => _coreMatchParticipantsPlayersAccess._players[t].Name)))}。", u.SourceSeat);
    }

    internal bool TryReturnMouSmPrivateWindow(ProgramCardTriggerWindowFrame p)
    {
        if (p.MouSimaYiPrivate is not { } back)
            return false;
        if (!ValidMouSmPrivateWindow(p) || _coreCardUseLifecycleOperations.LifecycleCardUse(back.UseId)is not { MouSimaYiSecret: { Released: true } } u || _resolutionStackOperations._resolutionStack[^1].Id != u.Id || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouSmTargetsDisclosedEvent>().Count(d => d.UseId == u.Id && d.ActionId == back.ActionId) != 1)
            throw new InvalidOperationException("Pingliao private target window did not disclose its exact paid Use once.");
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(u.Id);
        if (back.Call == MouSmPrivateCall.CommittedSlash && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, u.Action!, SkillProgramTriggerWindow.CardUseCommitted, u.Action!.TargetSeats, ProgramCardContinuation.CommittedSlash))
            return true;
        _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
        return true;
    }
}
