#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRoundOnceCardlessProgramRequestsRuntime
{
    internal const string RoundOnceCardlessProgramMarker = "round-once-cardless-request";
    internal bool HasRoundOnceCardlessProgramSlash(CharacterState actor, IEnumerable<int> targets) => _characterXiaoQiaoMouOlOperations.HasMouXqProgramSlash(actor, targets) || targets.Any(target => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessRules(actor, CardKind.Slash).Any(entry => RoundOnceCardlessProgramTargetLegal(actor, target, entry.Source)));
    internal bool RoundOnceCardlessProgramTargetLegal(CharacterState actor, int target, CardConversionSource source,
        bool ignoreDistance = false, bool countsTowardLimit = false) =>
        _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) &&
        _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target],
            CardKind.Slash, physicalSuit: Suit.None, physicalCardIds: [], ignoreDistance: ignoreDistance,
            conversionSource: source, countsTowardLimit: countsTowardLimit);
    internal IReadOnlyList<PromptChoice> RoundOnceCardlessProgramSlashChoices(ProgramSkillFrame frame, int actorSeat, IReadOnlyList<int> targets, string? resultBind = null)
    {
        if (frame.RoundOnceCardlessProgramRequest is not null || _slashRequests.GetOffer(frame, actorSeat) is not { } request)
            return[];
        var effect = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame);
        if (effect is null)
            return[];
        var allowed = _slashRequests.GetTargets(frame, actorSeat);
        if (allowed.Count == 0 || targets.Any(t => !allowed.Contains(t)) || resultBind is not null && resultBind != request.Spec.ResultBind)
            return[];
        var result = new List<PromptChoice>();
        foreach (var(source, _)in _coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessRules(_coreMatchParticipantsPlayersAccess._players[actorSeat], CardKind.Slash))
            foreach (var target in targets.Distinct().Where(t => RoundOnceCardlessProgramTargetLegal(_coreMatchParticipantsPlayersAccess._players[actorSeat], t, source, request.Spec.IgnoreDistance, request.Spec.CountsTowardSlashLimit)))
            {
                var parameters = new Dictionary<string, string>
                {
                    ["program-action"] = RoundOnceCardlessProgramMarker,
                    ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture),
                    ["instruction-index"] = frame.InstructionIndex.ToString(CultureInfo.InvariantCulture),
                    ["actor-seat"] = actorSeat.ToString(CultureInfo.InvariantCulture),
                    ["target-seat"] = target.ToString(CultureInfo.InvariantCulture),
                    ["request-op"] = effect.Op.ToString()
                };
                CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, source);
                result.Add(new(new ChoiceId($"round-cardless.request.{(frame.Id)}.{(frame.InstructionIndex)}.{(actorSeat)}.{(target)}.{(source.SkillInstanceId)}.{(source.BindingId)}"), $"发动【{(_coreCardConversionsOperations.ProgramConversionName(source))}】，视为对 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 使用【杀】。", [], [target], parameters));
            }

        return Array.AsReadOnly(result.ToArray());
    }
    internal bool TryResolveRoundOnceCardlessProgramSlash(PromptChoice choice, bool advance)
    {
        if (choice.Parameters.GetValueOrDefault("program-action") != RoundOnceCardlessProgramMarker)
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } prompt || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || !prompt.Choices.Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])) || choice.Cards.Count != 0 || choice.Targets is not [var target] || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || choice.Parameters.GetValueOrDefault("instruction-index") != f.InstructionIndex.ToString(CultureInfo.InvariantCulture) || choice.Parameters.GetValueOrDefault("actor-seat") != prompt.PlayerSeat.ToString(CultureInfo.InvariantCulture))
            throw new InvalidOperationException("Cardless requested Slash changed its exact native prompt, actor, target or owning instruction.");
        var effect = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f) ?? throw new InvalidOperationException("Cardless Slash lost its paused request effect.");
        var actor = _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat];
        var targets = _slashRequests.GetTargets(f, actor.Seat);
        if (!RoundOnceCardlessProgramSlashChoices(f, actor.Seat, targets, effect.ResultBind).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Cardless Slash request lost its source qualification or current native allowed targets.");
        var source = CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice);
        var acceptance = _slashRequests.Accept(f, actor.Seat, target);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var payment = _coreRoundOnceCardlessBasicOperations.ConsumeRoundOnceCardlessBasic(source, false);
        var receipt = new RoundOnceCardlessProgramRequestReceipt(f.Id, acceptance.Identity.Source,
            acceptance.Identity.GameplayHash, f.InstructionIndex, effect.Op, actor.Seat, target,
            acceptance.Identity.Spec.ResultBind, prompt.PromptId, prompt.Revision, f.WindowContext?.ParentFrameId, payment)
        { RequestAcceptance = acceptance };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RoundOnceCardlessProgramRequest = new(receipt) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundOnceCardlessProgramRequestAcceptedEvent(receipt));
        _slashRequestExecution.PrepareAcceptedUse(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), acceptance);
        var card = new Card(0, CardKind.Slash, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, [target], CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash), physicalCardIds: [], conversionSource: source, programSlashRequest: acceptance);
        BindRoundOnceCardlessProgramRequestUse(_coreCardUseLifecycleOperations.LifecycleCardUse(id)!);
        _coreRoundOnceCardlessNativeUsesOperations.IssueRoundOnceCardlessNativeUse(id, source);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats[0], card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        if (acceptance.Identity.Spec.CountsTowardSlashLimit &&
            _turnScope._phase == TurnPhase.Play && actor.Seat == _turnScope._currentSeat &&
            _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.UnlimitedUse != true &&
            !_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, source))
            _slashDebits.RecordSlashUseDebit(id, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, target, IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, _coreMatchParticipantsPlayersAccess._players[target]);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advance)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        return true;
    }

    internal void BindRoundOnceCardlessProgramRequestUse(CardUseFrame use)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { RoundOnceCardlessProgramRequest: { } p } parent)
            return;
        var r = p.Receipt;
        if (use.RoundOnceCardlessProgramRequest is { } bound)
        {
            if (bound != r || bound.NativeUseId != use.Id)
                throw new InvalidOperationException("Cardless requested Slash cannot bind a second native child.");
            return;
        }

        if (r.NativeUseId != 0 || r.ProgramFrameId != parent.Id || use.CardId != 0 || use.CardKind != CardKind.Slash || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use } action || action.ActorSeat != r.ActorSeat || action.ProviderSeat != r.ActorSeat || action.RequesterSeat is not null || action.ConversionChain is not [var source] || source != r.Payment.Source || action.PhysicalCards.Count != 0 || !use.TargetSeats.SequenceEqual(new[] { r.TargetSeat }) || !ValidRoundOnceCardlessProgramReceipt(parent, r, false))
            throw new InvalidOperationException("Cardless requested Slash changed its actual self-use, paid source, parent request or allowed target.");
        var final = r with
        {
            NativeUseId = use.Id,
            NativeActionId = action.ActionId
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { RoundOnceCardlessProgramRequest = p with { Receipt = final } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, f => f with { RoundOnceCardlessProgramRequest = final });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundOnceCardlessProgramRequestBoundEvent(final));
    }

    internal bool ValidRoundOnceCardlessProgramReceipt(ProgramSkillFrame parent, RoundOnceCardlessProgramRequestReceipt receipt, bool requireBound = true)
    {
        if (receipt.RequestAcceptance is not { } acceptance ||
            !_slashRequests.OwnsAcceptance(parent, acceptance, receipt.ActorSeat, receipt.TargetSeat) ||
            parent.RoundOnceCardlessProgramRequest is not { } pending || pending.Receipt != receipt ||
            parent.Id != receipt.ProgramFrameId || receipt.InstructionIndex != acceptance.Identity.InstructionIndex ||
            receipt.RequestSource != acceptance.Identity.Source || receipt.RequestGameplayHash != acceptance.Identity.GameplayHash ||
            parent.WindowContext?.ParentFrameId != receipt.OriginalWindowId ||
            receipt.Payment.Source.OwnerSeat != receipt.ActorSeat || receipt.ResultBind != acceptance.Identity.Spec.ResultBind ||
            receipt.OriginalPromptId.Value <= 0 || receipt.OriginalRevision < 0)
            return false;
        // The original owner stores the result of the real quota payment and
        // native binding. Public notifications are not execution proofs.
        return !requireBound || receipt.NativeUseId > 0 && receipt.NativeActionId > 0;
    }
    internal bool RoundOnceCardlessProgramRequestRidesOn(CardUseFrame use, ProgramSkillFrame parent) => use.RoundOnceCardlessProgramRequest is { } r && parent.RoundOnceCardlessProgramRequest is { NativeReturned: false } p && p.Receipt == r && ValidRoundOnceCardlessProgramReceipt(parent, r) && _coreRoundOnceCardlessNativeUsesOperations.IsRoundOnceCardlessBasicUse(use.Id) && use.CardAttack?.ProgramSkillCardUseFrameId == parent.Id && use.RoundOnceCardlessBasicUse?.CardActionId == r.NativeActionId;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 360)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 360)]
    internal bool CanContinueIssuedRoundOnceCardlessProgramRequest(ProgramSkillFrame parent) => parent.RoundOnceCardlessProgramRequest is { } p && ValidRoundOnceCardlessProgramReceipt(parent, p.Receipt);

    [NativeUseValidation(40)]
    internal void AssertRoundOnceCardlessProgramRequestCompletion(CardUseFrame use)
    {
        if (use.RoundOnceCardlessProgramRequest is not { } r)
            return;
        if (use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || use.PendingRecoveryAttempts is { Count: > 0 } || use.RecoveryPaidContinuation is not null || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w => w.ParentFrameId == use.Id) || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.ProgramFrameId)is not ProgramSkillFrame parent || !RoundOnceCardlessProgramRequestRidesOn(use, parent))
            throw new InvalidOperationException("Cardless request cannot return before its actual native Use and all paid children completed.");
    }

    [NativeUseReturn(50)]
    internal void ReturnRoundOnceCardlessProgramRequestUse(CardUseFrame completed)
    {
        if (completed.RoundOnceCardlessProgramRequest is not { } r)
            return;
        if (_resolutionStackOperations._resolutionStack.Any(f => f.Id == completed.Id) || completed.Step != ResolutionFrameStep.Completed || completed.RoundOnceCardlessBasicUse?.CardActionId != r.NativeActionId || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.ProgramFrameId)is not ProgramSkillFrame parent || parent.RoundOnceCardlessProgramRequest is not { NativeReturned: false } p || p.Receipt != r || !ValidRoundOnceCardlessProgramReceipt(parent, r))
            throw new InvalidOperationException("Cardless request can return only once after its own actual native child completed.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { RoundOnceCardlessProgramRequest = p with { NativeReturned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundOnceCardlessProgramRequestReturnedEvent(r));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 23402)]
    internal bool ResumeRoundOnceCardlessProgramRequest(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { RoundOnceCardlessProgramRequest: { NativeReturned: true } } parent || parent.Id != id)
            return false;
        if (!CanContinueIssuedRoundOnceCardlessProgramRequest(parent))
            throw new InvalidOperationException("Cardless request lost its once completed paid native return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { RoundOnceCardlessProgramRequest = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }
}
