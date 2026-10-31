#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRoundOnceCardlessProgramRequestsRuntime
{
    internal const string RoundOnceCardlessProgramMarker = "round-once-cardless-request";
    internal bool RoundOnceCardlessProgramActor(ProgramSkillFrame f, SkillProgramEffectOp op, int actor)
    {
        if (f.InstructionIndex < 1 || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(f.SkillId)?.Program is not { } program || program.GameplayHash != f.GameplayHash)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(f, program);
        if (f.InstructionIndex > plan.Instructions.Count || plan.GetPausedInstruction(f.InstructionIndex).Effect.Op != op)
            return false;
        if (op == SkillProgramEffectOp.ResolveAnniversaryMaTieQuxian)
            return _characterMaTieAnniversaryOperations.AnniversaryMaTieRequestActor(f, actor);
        if (op == SkillProgramEffectOp.RequestAnniversaryLeiBoSileiSlash)
            return actor == f.OwnerSeat && _characterLeiBoAnniversaryOperations.Value.IsAnniversaryLeiBoFixedSlash(f);
        if (op == SkillProgramEffectOp.ResolveSayingPlayStartObtainAndReply)
            return _characterSunYuanOlOperations.TryGetSayingSlashRequest(f, out var sayingActor, out _) && actor == sayingActor;
        if (op is SkillProgramEffectOp.ResolveChanshuangSimultaneousChoices or SkillProgramEffectOp.ResolveChanshuangEnding)
            return _characterLiYiOlOperations.TryGetChanshuangSlashRequest(f, out var requested, out _) && actor == requested;
        if (op == SkillProgramEffectOp.UseHejiAtCompletedSingleTarget)
            return actor == f.OwnerSeat && _characterWuJingOlOperations.IsWuJingHejiRequest(f);
        if (op == SkillProgramEffectOp.UseOwnerSlashAgainstTurnOwner)
            return actor == f.OwnerSeat && f.WindowContext?.Window == SkillProgramTriggerWindow.TurnEnding && f.WindowContext.TargetSeat == _coreActualTurnProgressCurrentSeatAccess._currentSeat && f.FixedTargetSlash is { } fixedDraft && fixedDraft.TargetSeat == _coreActualTurnProgressCurrentSeatAccess._currentSeat && fixedDraft.TargetSeat != actor;
        if (op == SkillProgramEffectOp.RequestSlashByNearest)
        {
            var cursors = f.NumberBindings.Where(b => b.Name == "participant-cursor-" + (f.InstructionIndex - 1)).ToArray();
            return f.ReexecuteParticipantInstruction && cursors is [var cursor] && cursor.Value >= 1 && cursor.Value < _coreMatchDefinitionPlayerCountAccess._playerCount && actor == _coreTableOrderOperations.TableStepSeat(f.OwnerSeat, cursor.Value);
        }

        return (op is SkillProgramEffectOp.RequestSlashByTarget or SkillProgramEffectOp.RequestSlashAgainstChosenTarget or SkillProgramEffectOp.RequestLegalSlashByNearest) && f.SelectedTargetSeats.SequenceEqual(new[] { actor });
    }

    internal int[] RoundOnceCardlessProgramTargets(ProgramSkillFrame f, int actor, SkillProgramEffectOp op)
    {
        if (!RoundOnceCardlessProgramActor(f, op, actor))
            return[];
        if (op == SkillProgramEffectOp.ResolveAnniversaryMaTieQuxian)
            return _characterMaTieAnniversaryOperations.AnniversaryMaTieRequestTargets(f, actor);
        if (op == SkillProgramEffectOp.RequestAnniversaryLeiBoSileiSlash)
            return[f.FixedTargetSlash!.TargetSeat];
        if (op == SkillProgramEffectOp.ResolveSayingPlayStartObtainAndReply)
            return _characterSunYuanOlOperations.TryGetSayingSlashRequest(f, out _, out var sayingTargets) ? sayingTargets.ToArray() : [];
        if (op is SkillProgramEffectOp.ResolveChanshuangSimultaneousChoices or SkillProgramEffectOp.ResolveChanshuangEnding)
            return _characterLiYiOlOperations.TryGetChanshuangSlashRequest(f, out _, out var targets) ? targets.ToArray() : [];
        return op switch
        {
            SkillProgramEffectOp.UseHejiAtCompletedSingleTarget => [f.WuJing!.TargetSeat],
            SkillProgramEffectOp.RequestSlashByTarget => [f.OwnerSeat],
            SkillProgramEffectOp.UseOwnerSlashAgainstTurnOwner when f.FixedTargetSlash is { } fixedDraft => [fixedDraft.TargetSeat],
            SkillProgramEffectOp.RequestSlashByNearest => _coreProgramSlashRequestOperations.GetNearestLivingCharacterSeats(actor),
            SkillProgramEffectOp.RequestSlashAgainstChosenTarget when f.AssistedSlashRequest is { TargetSeat: { } target } draft && draft.ActorSeat == actor => [target],
            SkillProgramEffectOp.RequestLegalSlashByNearest when f.NearestLegalSlashRequest is { AwaitingFaction: false } nearest && nearest.ActorSeat == actor && nearest.NearestSeats.SequenceEqual(_coreProgramSlashRequestOperations.GetNearestLivingCharacterSeats(actor)) => nearest.NearestSeats.ToArray(),
            _ => []
        };
    }

    internal bool HasRoundOnceCardlessProgramSlash(CharacterState actor, IEnumerable<int> targets) => _characterXiaoQiaoMouOlOperations.HasMouXqProgramSlash(actor, targets) || targets.Any(target => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessRules(actor, CardKind.Slash).Any(entry => RoundOnceCardlessProgramTargetLegal(actor, target, entry.Source)));
    internal bool RoundOnceCardlessProgramTargetLegal(CharacterState actor, int target, CardConversionSource source, bool ignoreDistance = false) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && actor.Seat != target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target, Suit.None) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[target], new(0, CardKind.Slash, Suit.None, 0), source, CardKind.Slash, ignoreDistance: ignoreDistance, noEffectiveRank: true, physicalCardIds: []);
    internal IReadOnlyList<PromptChoice> RoundOnceCardlessProgramSlashChoices(ProgramSkillFrame frame, int actorSeat, IReadOnlyList<int> targets, string? resultBind = null)
    {
        if (frame.RoundOnceCardlessProgramRequest is not null || (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame) && !_characterPeiYuanShaoOlOperations.HasIssuedFulueRequest(frame)) && !_characterLiYiOlOperations.IsChanshuangSlashRequest(frame) && !_characterSunYuanOlOperations.IsSayingSlashRequest(frame) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat))
            return[];
        var effect = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame);
        if (effect is null)
            return[];
        var allowed = RoundOnceCardlessProgramTargets(frame, actorSeat, effect.Op);
        if (allowed.Length == 0 || targets.Any(t => !allowed.Contains(t)) || resultBind is not null && resultBind != effect.ResultBind)
            return[];
        var result = new List<PromptChoice>();
        foreach (var(source, _)in _coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessRules(_coreMatchParticipantsPlayersAccess._players[actorSeat], CardKind.Slash))
            foreach (var target in targets.Distinct().Where(t => RoundOnceCardlessProgramTargetLegal(_coreMatchParticipantsPlayersAccess._players[actorSeat], t, source, frame.FixedTargetSlash?.IgnoreDistance == true)))
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
        var targets = RoundOnceCardlessProgramTargets(f, actor.Seat, effect.Op);
        if (!RoundOnceCardlessProgramSlashChoices(f, actor.Seat, targets, effect.ResultBind).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Cardless Slash request lost its source qualification or current native allowed targets.");
        var source = CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var payment = _coreRoundOnceCardlessBasicOperations.ConsumeRoundOnceCardlessBasic(source, false);
        var receipt = new RoundOnceCardlessProgramRequestReceipt(f.Id, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, f.InstructionIndex, effect.Op, actor.Seat, target, effect.ResultBind, prompt.PromptId, prompt.Revision, f.WindowContext?.ParentFrameId, payment);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RoundOnceCardlessProgramRequest = new(receipt) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundOnceCardlessProgramRequestAcceptedEvent(receipt));
        _characterMaTieAnniversaryOperations.PrepareAnniversaryMaTieSuppliedSlash(f, actor.Seat, actor.Seat, target, CardKind.Slash, [], source);
        if (_characterSunYuanOlOperations.IsSayingSlashRequest(f))
            _characterSunYuanOlOperations.PrepareSayingSuppliedSlash(f, actor.Seat, actor.Seat, target, CardKind.Slash, [], source);
        if (_characterLiYiOlOperations.IsChanshuangSlashRequest(f))
            _characterLiYiOlOperations.PrepareChanshuangSuppliedSlash(f, actor.Seat, actor.Seat, target, CardKind.Slash, [], source);
        if (effect.Op == SkillProgramEffectOp.RequestSlashByTarget)
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(f.Id, effect.ResultBind!, RequestSlashByTargetProgramOperationDescriptor.UsedSlashOption, actor.Seat, "卫境：视为使用【杀】。");
        else if (effect.Op == SkillProgramEffectOp.RequestSlashAgainstChosenTarget)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { AssistedSlashRequest = null });
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(f.Id, effect.ResultBind!, "used-slash", actor.Seat, "卫境：视为使用【杀】。");
        }
        else if (effect.Op is SkillProgramEffectOp.UseOwnerSlashAgainstTurnOwner or SkillProgramEffectOp.RequestAnniversaryLeiBoSileiSlash)
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { FixedTargetSlash = f.FixedTargetSlash!with { Issued = true } });
        else if (effect.Op == SkillProgramEffectOp.RequestLegalSlashByNearest)
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { NearestLegalSlashRequest = null });
        var card = new Card(0, CardKind.Slash, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, [target], CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash), physicalCardIds: [], conversionSource: source);
        BindRoundOnceCardlessProgramRequestUse(_coreCardUseLifecycleOperations.LifecycleCardUse(id)!);
        _coreRoundOnceCardlessNativeUsesOperations.IssueRoundOnceCardlessNativeUse(id, source);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats[0], card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
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

        if (r.NativeUseId != 0 || r.ProgramFrameId != parent.Id || !RoundOnceCardlessProgramActor(parent, r.RequestOp, r.ActorSeat) || use.CardId != 0 || use.CardKind != CardKind.Slash || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use } action || action.ActorSeat != r.ActorSeat || action.ProviderSeat != r.ActorSeat || action.RequesterSeat is not null || action.ConversionChain is not [var source] || source != r.Payment.Source || action.PhysicalCards.Count != 0 || !use.TargetSeats.SequenceEqual(new[] { r.TargetSeat }) || !ValidRoundOnceCardlessProgramReceipt(parent, r, false))
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

    internal bool ValidRoundOnceCardlessProgramReceipt(ProgramSkillFrame parent, RoundOnceCardlessProgramRequestReceipt r, bool requireBound = true)
    {
        if (parent.Id != r.ProgramFrameId || parent.InstructionIndex != r.InstructionIndex || r.RequestSource != new CardConversionSource(parent.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId) || parent.GameplayHash != r.RequestGameplayHash || parent.WindowContext?.ParentFrameId != r.OriginalWindowId || r.Payment.Source.OwnerSeat != r.ActorSeat || !RoundOnceCardlessProgramActor(parent, r.RequestOp, r.ActorSeat) || r.OriginalPromptId.Value <= 0 || r.OriginalRevision < 0 || r.Payment.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(r.Payment.Source.SkillId).Program!.GameplayHash || _corePlayCardHistoryOperations.ViewAsRule(r.Payment.Source)is not { OutputKind: CardKind.Slash, ForPlay: true, ForResponse: false, RoundOnceCardlessBasic: { } } || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundOnceCardlessBasicQuotaPaidEvent>().Count(e => e.Receipt == r.Payment) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundOnceCardlessProgramRequestAcceptedEvent>().Count(e => e.Receipt == (r with { NativeUseId = 0, NativeActionId = 0 })) != 1)
            return false;
        var effect = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(parent);
        if (effect?.ResultBind != r.ResultBind)
            return false;
        if (r.RequestOp is SkillProgramEffectOp.RequestSlashByTarget or SkillProgramEffectOp.RequestSlashAgainstChosenTarget)
        {
            if (parent.ChoiceBindings.Count(b => b.Name == r.ResultBind && b.ChooserSeat == r.ActorSeat && b.OptionId == "used-slash") != 1)
                return false;
        }

        return !requireBound || r.NativeUseId > 0 && r.NativeActionId > 0 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundOnceCardlessProgramRequestBoundEvent>().Count(e => e.Receipt == r) == 1;
    }

    internal bool RoundOnceCardlessProgramRequestRidesOn(CardUseFrame use, ProgramSkillFrame parent) => use.RoundOnceCardlessProgramRequest is { } r && parent.RoundOnceCardlessProgramRequest is { NativeReturned: false } p && p.Receipt == r && ValidRoundOnceCardlessProgramReceipt(parent, r) && _coreRoundOnceCardlessNativeUsesOperations.IsRoundOnceCardlessBasicUse(use.Id) && use.CardAttack?.ProgramSkillCardUseFrameId == parent.Id && use.RoundOnceCardlessBasicUse?.CardActionId == r.NativeActionId;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 360)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 360)]
    internal bool CanContinueIssuedRoundOnceCardlessProgramRequest(ProgramSkillFrame parent) => parent.RoundOnceCardlessProgramRequest is { } p && ValidRoundOnceCardlessProgramReceipt(parent, p.Receipt) && (!p.NativeReturned || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundOnceCardlessProgramRequestReturnedEvent>().Count(e => e.Receipt == p.Receipt) == 1);

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
        if (_resolutionStackOperations._resolutionStack.Any(f => f.Id == completed.Id) || completed.Step != ResolutionFrameStep.Completed || completed.RoundOnceCardlessBasicUse?.CardActionId != r.NativeActionId || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.ProgramFrameId)is not ProgramSkillFrame parent || parent.RoundOnceCardlessProgramRequest is not { NativeReturned: false } p || p.Receipt != r || !ValidRoundOnceCardlessProgramReceipt(parent, r) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundOnceCardlessProgramRequestReturnedEvent>().Any(e => e.Receipt == r))
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
        if (_characterSunYuanOlOperations.ResumeSayingSuppliedSlash(parent))
            return true;
        if (_characterLiYiOlOperations.ResumeChanshuangSuppliedSlash(parent))
            return true;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed || !_coreMatchParticipantsPlayersAccess._players[parent.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(parent) && !_characterPeiYuanShaoOlOperations.HasIssuedFulueRequest(parent))
            _coreSkillProgramsOperations.FinishProgramSkill(parent, true);
        else if (parent.FixedTargetSlash is { Issued: true })
            _coreFixedSlashAndDeclaredDeckProgramsOperations.TryResumeFixedSlashAndDeclaredDeck(id);
        else
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }
}
