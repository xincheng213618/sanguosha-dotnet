#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangYanOlRuntime
{
    internal IReadOnlyList<LangdaoOption> ZhangYanAvailableOptions(CardConversionSource source) => Array.AsReadOnly(Enum.GetValues<LangdaoOption>().Where(option => !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYanLangdaoOptionRemovedEvent>().Any(e => e.Source.OwnerSeat == source.OwnerSeat && e.Source.SkillId == source.SkillId && e.Source.SkillInstanceId == source.SkillInstanceId && e.Option == option)).ToArray());
    internal CardUseFrame? ZhangYanLangdaoParent(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c)
    {
        if (c.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || c.CardUse is not { } card || card.ActorSeat != candidate.OwnerSeat || c.OwnerSeat != candidate.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is not ProgramCardTriggerWindowFrame w || w.ParentFrameId != card.ParentCardUseFrameId || w.Action.ActionId != card.CardActionId || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || CoreGameEngineCardActionsRuntime.ToSharedCandidate(w.Candidates[w.CandidateIndex]) != candidate || _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } a } use || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || a.ActionId != card.CardActionId || a.ActorSeat != candidate.OwnerSeat || use.SourceSeat != candidate.OwnerSeat)
            return null;
        return use;
    }

    internal int[] ZhangYanExtraTargets(ProgramSkillFrame f, CardUseFrame use) => _coreMatchParticipantsPlayersAccess._players.Where(p => !use.TargetSeats.Contains(p.Seat) && !f.ZhangYan!.AddedTargets.Contains(p.Seat) && _coreProgramCardUseRolesOperations.CanBeProgramCardUseRoleTarget(use, p)).Select(p => p.Seat).Order().ToArray();
    internal void ResolveZhangYanLangdao(ProgramSkillFrame f)
    {
        var r = f.ZhangYan!;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(f.WindowContext!.CardUse!.ParentCardUseFrameId)!;
        if (r.Votes.Count != 2 || r.Votes[0].Seat != f.OwnerSeat || r.Votes[1].Seat != r.PhaseOwner || r.Votes.Any(v => !r.Options.Contains(v.Option)) || use.ZhangYanLangdao is not null || use.TargetSeats is not [var original] || original != r.PhaseOwner)
            throw new InvalidOperationException("Langdao needs two independent sealed choices on its original unique target.");
        for (var i = 0; i < r.Votes.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYanLangdaoVoteRevealedEvent(f.Id, i, r.Votes[i].Seat, r.Votes[i].Option));
        var damage = r.Votes.Count(v => v.Option == LangdaoOption.Damage);
        var extra = r.Votes.Count(v => v.Option == LangdaoOption.ExtraTarget);
        var noResponse = r.Votes.Any(v => v.Option == LangdaoOption.NoResponse);
        var issued = new ZhangYanLangdaoUse(f.Id, r.WindowId, use.Action!.ActionId, r.Source, r.GameplayHash, use.SourceSeat, original, damage, extra, noResponse)
        {
            OriginalTargets = use.TargetSeats,
            SelectedOptions = r.Votes.Select(v => v.Option).Distinct().ToArray()
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ZhangYanLangdao = issued });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYanLangdaoIssuedEvent(use.Id, issued.ActionId, f.Id, r.Source, r.GameplayHash, issued.OriginalActor, original, damage, extra, noResponse));
        if (extra == 0)
        {
            FinishZhangYan(f, true);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhangYan = r with { Stage = ZhangYanStage.LangdaoTargets } });
        if (ZhangYanExtraTargets(f, _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!).Length == 0)
        {
            CompleteZhangYanLangdaoTargets(f);
            return;
        }

        PublishZhangYan(f);
    }

    internal void CompleteZhangYanLangdaoTargets(ProgramSkillFrame f)
    {
        var r = f.ZhangYan!;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(f.WindowContext!.CardUse!.ParentCardUseFrameId)!;
        var issued = use.ZhangYanLangdao!;
        if (r.AddedTargets.Count > issued.ExtraCount || r.AddedTargets.Distinct().Count() != r.AddedTargets.Count || r.AddedTargets.Any(t => use.TargetSeats.Contains(t) || !_coreProgramCardUseRolesOperations.CanBeProgramCardUseRoleTarget(use, _coreMatchParticipantsPlayersAccess._players[t])))
            throw new InvalidOperationException("Langdao's added target must remain a genuine normally in-range Slash target.");
        var targets = Array.AsReadOnly(use.TargetSeats.Concat(r.AddedTargets).ToArray());
        var action = _coreProgramCardUseRolesOperations.CloneRoleAction(use.Action!, use.SourceSeat, targets);
        _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action, ZhangYanLangdao = issued with { AddedTargets = r.AddedTargets }, Enhancements = r.AddedTargets.Count > 0 ? use.Enhancements | CurrentCardEnhancement.ExtraTarget : use.Enhancements, EnhancementOwnerSeat = r.AddedTargets.Count > 0 ? f.OwnerSeat : use.EnhancementOwnerSeat }, action);
        for (var i = 0; i < r.AddedTargets.Count; i++)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYanLangdaoTargetAddedEvent(f.Id, use.Id, action.ActionId, r.AddedTargets[i], i));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CurrentCardEnhancedEvent(use.Id, action.ActionId, f.OwnerSeat, CurrentCardEnhancement.ExtraTarget, r.AddedTargets[i]));
        }

        if (r.AddedTargets.Count > 0)
            _coreCurrentUseTargetCandidateRefreshOperations.RefreshCurrentUseAddedTargetCandidates(_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!, r.AddedTargets);
        FinishZhangYan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
    }

    internal bool IsZhangYanLangdaoIssued(CardUseFrame use)
    {
        if (use.ZhangYanLangdao is not { } r || use.Action is not { Type: CardActionType.Use } a || a.ActionId != r.ActionId || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || r.Source.OwnerSeat != r.OriginalActor || !_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, r.OriginalActor, a.ProviderSeat) || r.DamageBonus is < 0 or > 2 || r.ExtraCount is < 0 or > 2 || r.OriginalTargets is not [var original] || original != r.OriginalTarget || r.SelectedOptions.Count is < 1 or > 2 || r.SelectedOptions.Distinct().Count() != r.SelectedOptions.Count || r.AddedTargets.Count > r.ExtraCount || r.AddedTargets.Distinct().Count() != r.AddedTargets.Count || r.AddedTargets.Any(r.OriginalTargets.Contains))
            return false;
        var votes = _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYanLangdaoVoteRevealedEvent>().Where(e => e.FrameId == r.ProgramFrameId).OrderBy(e => e.Index).ToArray();
        return votes is [{ Index: 0 } first, { Index: 1 } second] && first.Seat == r.OriginalActor && second.Seat == r.OriginalTarget && votes.Count(v => v.Option == LangdaoOption.Damage) == r.DamageBonus && votes.Count(v => v.Option == LangdaoOption.ExtraTarget) == r.ExtraCount && votes.Any(v => v.Option == LangdaoOption.NoResponse) == r.NoResponse && votes.Select(v => v.Option).Distinct().SequenceEqual(r.SelectedOptions) && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYanLangdaoIssuedEvent>().Count(e => e.UseId == use.Id && e.ActionId == a.ActionId && e.FrameId == r.ProgramFrameId && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.OriginalActor == r.OriginalActor && e.OriginalTarget == r.OriginalTarget && e.DamageBonus == r.DamageBonus && e.ExtraCount == r.ExtraCount && e.NoResponse == r.NoResponse) == 1;
    }

    internal bool IsZhangYanLangdaoUnrespondable(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { ZhangYanLangdao: { NoResponse: true } } use && IsZhangYanLangdaoIssued(use);
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 140)]
    internal int ZhangYanLangdaoDamageBonus(GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { ZhangYanLangdao: { } r } use && IsZhangYanLangdaoIssued(use) ? r.DamageBonus : 0;
    internal bool HasZhangYanLangdaoTargetTail(CardUseFrame use)
    {
        if (use.ZhangYanLangdao is not { AddedTargets.Count: > 0 } r || !IsZhangYanLangdaoIssued(use))
            return false;
        var actual = r.OriginalTargets.Concat(r.AddedTargets).ToArray();
        if (!r.AddedTargets.Select((seat, i) => _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYanLangdaoTargetAddedEvent>().Count(e => e.FrameId == r.ProgramFrameId && e.UseId == use.Id && e.ActionId == r.ActionId && e.Target == seat && e.Index == i) == 1).All(x => x))
            return false;
        foreach (var e in _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYanLangdaoRedirectedEvent>().Where(e => e.UseId == use.Id))
        {
            if (e.ActionId != r.ActionId || e.TargetIndex < 0 || e.TargetIndex >= actual.Length || actual[e.TargetIndex] != e.OriginalTarget || e.Source.OwnerSeat != e.OriginalTarget || _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program is not { } p || p.GameplayHash != e.GameplayHash || ProgramInstructionResolver.Default.Find(p, ProgramInstructionSourceKind.Trigger, e.Source.BindingId)is not { } plan || e.InstructionIndex < 1 || e.InstructionIndex > plan.Instructions.Count || plan.GetPausedInstruction(e.InstructionIndex).Effect.Op != SkillProgramEffectOp.RedirectCurrentAttack || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(f => f.FrameId == e.ProgramFrameId && f.OwnerSeat == e.Source.OwnerSeat && f.SkillId == e.Source.SkillId && f.BindingId == e.Source.BindingId && f.SkillInstanceId == e.Source.SkillInstanceId && f.Window == SkillProgramTriggerWindow.SlashTargetRedirecting) != 1)
                return false;
            actual[e.TargetIndex] = e.NewTarget;
        }

        return use.TargetSeats.SequenceEqual(actual);
    }

    internal void RecordZhangYanLangdaoRedirect(ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { ZhangYanLangdao: { AddedTargets.Count: > 0 } r, Action: { } a } use)
            return;
        if (_corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(redirector)?.Op != SkillProgramEffectOp.RedirectCurrentAttack || redirector.WindowContext is not { Window: SkillProgramTriggerWindow.SlashTargetRedirecting, CardUse: { } c } ctx || c.ParentCardUseFrameId != use.Id || c.CardActionId != a.ActionId || ctx.TargetSeat != originalSeat || redirector.OwnerSeat != originalSeat || use.TargetIndex < 0 || use.TargetIndex >= use.TargetSeats.Count || use.TargetSeats[use.TargetIndex] != newSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYanLangdaoRedirectedEvent>().Any(e => e.ProgramFrameId == redirector.Id && e.UseId == useId))
            throw new InvalidOperationException("Langdao redirect lost its actual original cursor and native paid source.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYanLangdaoRedirectedEvent(redirector.Id, useId, a.ActionId, ZhangYanSource(redirector), redirector.GameplayHash, redirector.InstructionIndex, use.TargetIndex, originalSeat, newSeat));
    }

    internal bool TryGetZhangYanVirtualPrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, out IReadOnlyList<int> primary)
    {
        primary = [];
        if (!HasZhangYanLangdaoTargetTail(use) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.CardAttack is not { CardId: null, PhysicalCardIds.Count: 0 } attack || attack.ProgramSkillCardUseFrameId != parent.Id || use.Action is not { Type: CardActionType.Use } a || a.ProviderSeat != a.ActorSeat || a.PhysicalCards.Count != 0 || parent.InstructionIndex < 1)
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1].Id != parent.Id || _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program is not { } p || p.GameplayHash != parent.GameplayHash)
            return false;
        var declared = _coreProgramEventHistoryOperations.ProgramEventHistory<TargetsConfirmedEvent>().SingleOrDefault(e => e.ResolutionId == use.Id);
        if (declared is not { TargetSeats.Count: 1 })
            return false;
        var original = declared.TargetSeats[0];
        var op = ProgramInstructionResolver.Default.Resolve(parent, p).GetPausedInstruction(parent.InstructionIndex).Effect;
        var exact = op.Op switch
        {
            SkillProgramEffectOp.OfferVirtualSlashOrDraw => parent.SelectedTargetSeats.SequenceEqual([a.ActorSeat]) && a.ConversionChain.Count == 0,
            SkillProgramEffectOp.UseVirtualSlash => parent.OwnerSeat == a.ActorSeat && a.ConversionChain.SequenceEqual([new CardConversionSource(parent.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId)]) && (op.TargetReference is null ? parent.SelectedTargetSeats.SequenceEqual([original]) : op.TargetReference.Kind == ProgramParticipantRef.EventTarget && parent.WindowContext?.TargetSeat == original),
            SkillProgramEffectOp.UseVirtualCard or SkillProgramEffectOp.OfferUnlimitedVirtualSlash => parent.OwnerSeat == a.ActorSeat && parent.SelectedTargetSeats.SequenceEqual([original]) && op.OutputKind == CardKind.Slash && op.TargetRestriction is SkillProgramCardTargetRestriction.DistanceUnlimitedAgainstTarget or SkillProgramCardTargetRestriction.NormalSlashTarget,
            _ => false
        };
        if (!exact)
            return false;
        primary = Array.AsReadOnly(new[] { use.TargetSeats[0] });
        return true;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27001)]
    internal void ObserveZhangYanLangdaoOutcome(IGameEvent payload)
    {
        if (payload is PlayerDiedEvent dead && _coreDyingCursorRuntimeOperations.ActiveDying is { Continuation: DyingContinuationKind.Damage } dying && dying.VictimSeat == dead.VictimSeat && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == dying.ParentFrameId)is DamageFrame damage && damage.TargetSeat == dead.VictimSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(damage.ParentFrameId)is { ZhangYanLangdao: { Killed: false } r } use && _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId == use.Id && IsZhangYanLangdaoIssued(use))
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ZhangYanLangdao = r with { Killed = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYanLangdaoKilledEvent(use.Id, r.ActionId, dying.Id, damage.Id, dead.VictimSeat));
        }

        if (payload is not CardUseFinishedEvent finished || _coreCardUseLifecycleOperations.LifecycleCardUse(finished.ResolutionId)is not { ZhangYanLangdao: { Settled: false } receipt } card || !IsZhangYanLangdaoIssued(card))
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(card.Id, u => u with { ZhangYanLangdao = receipt with { Settled = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYanLangdaoSettledEvent(card.Id, receipt.ActionId, receipt.Source, receipt.Killed));
        if (!receipt.Killed)
            foreach (var option in receipt.SelectedOptions)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYanLangdaoOptionRemovedEvent(card.Id, receipt.ActionId, receipt.Source, option));
    }
}
