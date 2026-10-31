#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuZhiMouMobileRuntime
{
    internal const string MouLuZhiMingren = "mou-mobile:mingren", MouLuZhiZhenliang = "mou-mobile:zhenliang";
    internal const string MouLuZhiDiscardReason = "skill-program.mou-mobile:zhenliang.discard", MouLuZhiDrawReason = "skill-program.mou-mobile:lu-zhi.draw";
    internal bool HasMouLuZhiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeMouLuZhiMingren) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouLuZhiZhenliangYang);

    internal static CardConversionSource MouLuZhiSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_lu_zhi_mou_mobile_MouLuZhi.MouLuZhiSource(f);
    internal MouLuZhiRen[] LiveMouLuZhiRen(int owner)
    {
        if (!HasMouLuZhiCapability || !_cardZonesOperations._cardZones.HasPrivatePersistentCards(owner))
            return[];
        var removed = _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiRenRemovedEvent>().Select(e => e.Ren).ToHashSet();
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiRenStoredEvent>().Select(e => e.Ren).Where(r => r.Source.OwnerSeat == owner && !removed.Contains(r) && _cardZonesOperations._cardZones.GetLocation(r.CardId) == r.Location).ToArray();
    }

    internal MouLuZhiRen? MatchingMouLuZhiRen(int owner, string instance)
    {
        var grant = _coreMatchParticipantsPlayersAccess._players[owner].SkillGrants.Grants.FirstOrDefault(g => g.SkillInstanceId == instance);
        return LiveMouLuZhiRen(owner).FirstOrDefault(r => r.Source.SkillInstanceId == instance || grant is not null && _coreMatchParticipantsPlayersAccess._players[owner].SkillGrants.Grants.Any(g => g.SkillId == MouLuZhiMingren && g.SkillInstanceId == r.Source.SkillInstanceId && g.SourceId == grant.SourceId));
    }

    internal Card[] MouLuZhiCostCards(int owner, bool? red, string? instance = null) => red is null ? [] : _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner])).Where(c => _coreActionDiscardColorsOperations.CapturePhysicalCardColor(owner, c) == red && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && (instance is null || !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, MouLuZhiZhenliang, instance, c))).OrderBy(c => c.Id).ToArray();
    internal SkillProgramStepOutcome StartMouLuZhi(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouLuZhi is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Mou Lu Zhi requires its live owning native entry.");
        var ren = MatchingMouLuZhiRen(f.OwnerSeat, f.SkillInstanceId);
        ActualDiscardRecoveryPhaseKey? phase = null;
        if (op == SkillProgramEffectOp.ResolveMouLuZhiZhenliangYang)
        {
            if (f.SkillId != MouLuZhiZhenliang || f.ActivationId != "yang-damage" || f.TriggerId is not null || f.WindowContext is not null || !CanStartMouLuZhiYang(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) || ren is null)
                throw new InvalidOperationException("Zhenliang Yang requires a payable actual own Play.");
            phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
        }
        else if (f.ActivationId != f.TriggerId || f.WindowContext is not { } context || context.OwnerSeat != f.OwnerSeat || (op == SkillProgramEffectOp.InitializeMouLuZhiMingren ? f.SkillId != MouLuZhiMingren || f.TriggerId != "initial-ren" || context.Window != SkillProgramTriggerWindow.GameStarting || ren is not null : op == SkillProgramEffectOp.ExchangeMouLuZhiMingren ? f.SkillId != MouLuZhiMingren || f.TriggerId != "exchange-ren" || context.Window != SkillProgramTriggerWindow.TurnEnding || ren is null : f.SkillId != MouLuZhiZhenliang || !MouLuZhiCompletion(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), context)))
            throw new InvalidOperationException("Mingren/Zhenliang requires its genuine qualified native window.");
        var stage = op == SkillProgramEffectOp.InitializeMouLuZhiMingren ? MouLuZhiStage.Drawing : op == SkillProgramEffectOp.ExchangeMouLuZhiMingren ? MouLuZhiStage.RenChoice : MouLuZhiStage.TargetChoice;
        var r = new MouLuZhiReceipt(MouLuZhiSource(f), f.GameplayHash, op)
        {
            Stage = stage,
            Ren = ren,
            Phase = phase,
            ActionId = f.WindowContext?.CardUse?.CardActionId,
            IsRed = ren is null ? null : _coreActionDiscardColorsOperations.CapturePhysicalCardColor(f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(ren.CardId)),
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            TargetSeat = f.OwnerSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuZhi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ActionId, phase));
        if (stage == MouLuZhiStage.Drawing)
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 2, new(MouLuZhiDrawReason));
        else
            PublishMouLuZhi(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice MouLuZhiChoice(ProgramSkillFrame f, string branch, string label, int? card = null, int? target = null) => new(new($"mou-lu-zhi.{(f.Id)}.{(f.MouLuZhi!.Cards.Count)}.{(branch)}.{(card ?? target ?? 0)}"), label, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "mou-lu-zhi", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> MouLuZhiChoices(ProgramSkillFrame f)
    {
        var r = f.MouLuZhi!;
        if (r.Stage == MouLuZhiStage.RenChoice)
            return Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => MouLuZhiChoice(f, "ren", $"扣置【{(c.DisplayName)}】为任", c.Id)).ToArray());
        if (r.Stage == MouLuZhiStage.CostChoice)
            return Array.AsReadOnly(MouLuZhiCostCards(f.OwnerSeat, r.IsRed, f.SkillInstanceId).Where(c => !r.Cards.Contains(c.Id)).Select(c => MouLuZhiChoice(f, "cost", $"弃置【{(c.DisplayName)}】（{(r.Cards.Count + 1)}/{(r.RequiredCards)}）", c.Id)).ToArray());
        var targets = r.Op == SkillProgramEffectOp.ResolveMouLuZhiZhenliangYang ? MouLuZhiYangTargets(f.OwnerSeat, r.Ren!, f.SkillInstanceId) : _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray();
        return Array.AsReadOnly(targets.Select(seat => MouLuZhiChoice(f, "target", _coreMatchParticipantsPlayersAccess._players[seat].Name + (r.Op == SkillProgramEffectOp.ResolveMouLuZhiZhenliangYang ? $"：弃置{(MouLuZhiX(f.OwnerSeat, seat))}张牌，造成1点伤害" : "：摸两张牌"), target: seat)).ToArray());
    }

    internal void PublishMouLuZhi(ProgramSkillFrame f)
    {
        var choices = MouLuZhiChoices(f);
        if (choices.Count == 0)
        {
            FinishMouLuZhi(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Description, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-lu-zhi")]
    internal void AnswerMouLuZhi(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MouLuZhi ?? throw new InvalidOperationException("Lu Zhi lost its owning choice.");
        if (!ValidMouLuZhi(f) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouLuZhiChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Lu Zhi only accepts its current private choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == MouLuZhiStage.RenChoice)
        {
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(selected.Cards.Single());
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuZhi = r = r with { Stage = MouLuZhiStage.Paying, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var location = r.Ren?.Location ?? new CardLocation(CardZoneKind.PrivatePersistentPile, f.OwnerSeat, $"mou-lu-zhi-ren:{(f.SkillInstanceId)}");
            _cardZonesOperations._cardZones.EnsurePrivatePersistentPile(location);
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(f.OwnerSeat), CardLocation.Processing, new("skill-program.mou-mobile:mingren.deposit"), disclosure: CardMovementDisclosure.FaceDown);
            if (r.Ren is { } old)
            {
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(old.CardId), old.Location, CardLocation.Hand(f.OwnerSeat), new("skill-program.mou-mobile:mingren.exchange"), disclosure: CardMovementDisclosure.FaceDown);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiRenRemovedEvent(old));
            }

            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, location, new("skill-program.mou-mobile:mingren.store"), disclosure: CardMovementDisclosure.FaceDown);
            var ren = new MouLuZhiRen(r.Source, r.GameplayHash, card.Id, location, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiRenStoredEvent(ren));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuZhi = f.MouLuZhi!with { Stage = MouLuZhiStage.MovementChildren, Ren = ren, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            SealMouLuZhiMovements(f.Id, before);
            ContinueMouLuZhi(f.Id);
            return;
        }

        if (r.Stage == MouLuZhiStage.TargetChoice)
        {
            var target = selected.Targets.Single();
            if (r.Op == SkillProgramEffectOp.ResolveMouLuZhiZhenliangYang)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuZhi = r with { Stage = MouLuZhiStage.CostChoice, TargetSeat = target, RequiredCards = MouLuZhiX(f.OwnerSeat, target) } });
                PublishMouLuZhi(f);
                return;
            }

            _coreProgramConversionPolarityOperations.CommitProgramConversionPolarity(f);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuZhi = r with { Stage = MouLuZhiStage.Drawing, TargetSeat = target } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, target, 2, new(MouLuZhiDrawReason));
            return;
        }

        var ids = r.Cards.Append(selected.Cards.Single()).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuZhi = r = r with { Cards = ids } });
        if (ids.Length < r.RequiredCards)
        {
            PublishMouLuZhi(f);
            return;
        }

        if (!_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || !ids.All(id => MouLuZhiCostCards(f.OwnerSeat, r.IsRed, f.SkillInstanceId).Any(c => c.Id == id)))
            throw new InvalidOperationException("Zhenliang lost its exact accepted HE cost before payment.");
        _coreProgramConversionPolarityOperations.CommitProgramConversionPolarity(f);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var start = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuZhi = r with { Stage = MouLuZhiStage.Paying, Before = start, After = start }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, new(MouLuZhiDiscardReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.MouLuZhi!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuZhi = r = r with { Stage = MouLuZhiStage.MovementChildren, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiPaidEvent(f.Id, r.TargetSeat, r.RequiredCards, r.IsRed, r.Before, r.After));
        SealMouLuZhiMovements(f.Id, start);
        ContinueMouLuZhi(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.InitializeMouLuZhiMingren, SkillProgramEffectOp.ResolveMouLuZhiZhenliangYin)]
    internal void ReturnMouLuZhiDraw(ProgramSkillFrame f, NativeDrawOutcome d)
    {
        if (!ValidMouLuZhi(f) || f.MouLuZhi is not { Stage: MouLuZhiStage.Drawing, Draw: null } r || d.RecipientSeat != r.TargetSeat || d.RequestedCount != 2)
            throw new InvalidOperationException("Lu Zhi requires its one issued native Draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuZhi = r with { Stage = MouLuZhiStage.DrawChildren, Draw = d, Before = d.SequenceBefore, After = d.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiDrawReturnedEvent(f.Id, d.RecipientSeat, 2, d.ActualDrawIds.Count, d.ReplacementGainedIds.Count, d.AttemptFrameId));
        SealMouLuZhiMovements(f.Id, d.SequenceBefore);
        ContinueMouLuZhi(f.Id);
    }

    internal void ContinueMouLuZhi(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuZhi: { } r } f || f.Id != id || r.Stage is not (MouLuZhiStage.MovementChildren or MouLuZhiStage.DrawChildren))
            return;
        if (!ValidMouLuZhi(f))
            throw new InvalidOperationException("Lu Zhi lost its paid native continuation.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouLuZhi!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMouLuZhi(f);
            return;
        }

        if (r.Op == SkillProgramEffectOp.InitializeMouLuZhiMingren && r.Stage == MouLuZhiStage.DrawChildren && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuZhi = r with { Stage = MouLuZhiStage.RenChoice }, PendingMovementContinuation = null });
            PublishMouLuZhi(f);
            return;
        }

        if (r.Op == SkillProgramEffectOp.ResolveMouLuZhiZhenliangYang && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuZhi = r with { Stage = MouLuZhiStage.DamageIssued }, PendingMovementContinuation = null });
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.TargetSeat, 1, new(ProgramParticipantRef.Owner));
            return;
        }

        FinishMouLuZhi(f);
    }

    internal void FinishMouLuZhi(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuZhi = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.InitializeMouLuZhiMingren, SkillProgramEffectOp.ExchangeMouLuZhiMingren, SkillProgramEffectOp.ResolveMouLuZhiZhenliangYang, SkillProgramEffectOp.ResolveMouLuZhiZhenliangYin)]
    internal PromptChoice AiMouLuZhi(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderByDescending(c => c.Targets.Count == 1 && (f.MouLuZhi!.Op == SkillProgramEffectOp.ResolveMouLuZhiZhenliangYang ? !CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]]) : CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]]))).ThenBy(c => c.Cards.Count == 1 ? CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) : 0).First();
    SkillProgramStepOutcome IMouLuZhiHost.Start(ProgramSkillFrame f, SkillProgramEffectOp op) => StartMouLuZhi(f, op);
}
