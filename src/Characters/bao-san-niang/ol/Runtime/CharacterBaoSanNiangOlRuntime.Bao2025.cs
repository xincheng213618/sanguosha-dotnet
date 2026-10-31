#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBaoSanNiangOlRuntime
{
    internal static bool IsBaoOperation(SkillProgramEffectOp op) => RuntimeSkills.RuntimeRules_bao_san_niang_ol_Bao2025.IsBaoOperation(op);
    internal CardConversionSource BaoSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId);
    internal bool BaoSourceCurrent(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 320)]
    internal bool CanOfferBao2025(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c)
    {
        if (trigger.Effects is not [var e] || !IsBaoOperation(e.Op))
            return true;
        var owner = _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat];
        if (e.Op == SkillProgramEffectOp.PayTwoForChosenAssaultAndApplyTargetOutcomes)
            return c.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && c.OwnerSeat == owner.Seat && BaoCostCards(owner).Count >= 2 && !_coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, CardKind.BarbarianAssault, CardActionType.Use);
        if (e.Op == SkillProgramEffectOp.DyingRecipientChooseReplaceOrGrantThenRecover)
            return c.Window == SkillProgramTriggerWindow.DyingEntering && c.TargetSeat == owner.Seat && owner.Hp <= 0 && _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().Any(d => d.VictimSeat == owner.Seat) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != owner.Seat);
        if (c.Window != SkillProgramTriggerWindow.CardUseCompleted || c.CardUse is not { } cc || _coreCardUseLifecycleOperations.LifecycleCardUse(cc.ParentCardUseFrameId)is not { Step: ResolutionFrameStep.Completed, Action: { } action } use || action.ActionId != cc.CardActionId || !_coreNonBasicSlashProfileOperations.HasIssuedNonBasicSlashDistance(use.Id) || !action.ConversionChain.Any(s => s.SkillId == candidate.SkillId && s.SkillInstanceId == candidate.SkillInstanceId))
            return false;
        return use.TargetSeats.Any(t => _coreTableSeatQueriesOperations.IsValidPlayerSeat(t) && _coreMatchParticipantsPlayersAccess._players[t].IsAlive && _corePhysicalOwnedCardQueriesOperations.HasTargetCard(_coreMatchParticipantsPlayersAccess._players[t]));
    }

    internal IReadOnlyList<Card> BaoCostCards(CharacterState owner) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).Where(c => !c.IsGeneralWeapon).OrderBy(c => c.Id).ToArray());
    internal SkillProgramStepOutcome BeginBao2025(ProgramSkillFrame input, SkillProgramEffect e)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.Bao2025 is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } context || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [var configured] || configured != e || !CanOfferBao2025(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("The converted Slash, chosen assault or Dying choice lost its exact unpaid native boundary.");
        var use = context.CardUse is { } cc ? _coreCardUseLifecycleOperations.LifecycleCardUse(cc.ParentCardUseFrameId) : null;
        var receipt = new ProgramBao2025Receipt
        {
            Operation = e.Op,
            Source = BaoSource(f),
            GameplayHash = f.GameplayHash,
            WindowId = context.ParentFrameId,
            OriginalUseId = use?.Id,
            OriginalActionId = use?.Action?.ActionId,
            ActualTurn = _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber,
            ActualTurnOwner = _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat,
            Targets = use?.TargetSeats ?? [],
            Stage = e.Op == SkillProgramEffectOp.TakeConvertedSlashTargetAndResetLimitedSkill ? ProgramBao2025Stage.TakeTarget : e.Op == SkillProgramEffectOp.PayTwoForChosenAssaultAndApplyTargetOutcomes ? ProgramBao2025Stage.AssaultCost : ProgramBao2025Stage.DyingTarget
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBao2025StartedEvent(f.Id, receipt));
        PublishBaoChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice BaoChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"bao2025.{(f.Id)}.{(f.Bao2025!.Stage)}.{(branch)}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "bao2025", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> BaoChoices(ProgramSkillFrame f)
    {
        var r = f.Bao2025!;
        var choices = new List<PromptChoice>();
        switch (r.Stage)
        {
            case ProgramBao2025Stage.TakeTarget:
                choices.AddRange(r.Targets.Where(t => _coreMatchParticipantsPlayersAccess._players[t].IsAlive && _corePhysicalOwnedCardQueriesOperations.HasTargetCard(_coreMatchParticipantsPlayersAccess._players[t])).Select(t => BaoChoice(f, $"target-{(t)}", $"获得{(_coreMatchParticipantsPlayersAccess._players[t].Name)}的一张牌", targets: [t])));
                break;
            case ProgramBao2025Stage.TakeCard:
                choices.AddRange(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.Recipient!.Value, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Obtain).Select(c => PromptChoiceProjection.Freeze(c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "bao2025", ["branch"] = "take" } })));
                break;
            case ProgramBao2025Stage.ResetChoice:
                choices.Add(BaoChoice(f, "reset", "失去1点体力，所有角色的镇南视为未发动"));
                choices.Add(BaoChoice(f, "decline", "不失去体力"));
                break;
            case ProgramBao2025Stage.AssaultCost:
                var costs = BaoCostCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]);
                for (var i = 0; i < costs.Count; i++)
                    for (var j = i + 1; j < costs.Count; j++)
                        choices.Add(BaoChoice(f, $"cost-{(costs[i].Id)}-{(costs[j].Id)}", $"弃置{(costs[i].DisplayName)}、{(costs[j].DisplayName)}", [costs[i].Id, costs[j].Id]));
                choices.Add(BaoChoice(f, "decline", "取消发动"));
                break;
            case ProgramBao2025Stage.AssaultTargets:
                foreach (var target in BaoAssaultLegalTargets(f).Where(t => !r.SelectedTargets.Contains(t)))
                    choices.Add(BaoChoice(f, $"add-{(target)}", $"加入{(_coreMatchParticipantsPlayersAccess._players[target].Name)}", targets: [target]));
                choices.Add(BaoChoice(f, "confirm", $"确定对{(r.SelectedTargets.Count)}名角色使用南蛮入侵", targets: r.SelectedTargets));
                choices.Add(BaoChoice(f, "decline", "取消发动"));
                break;
            case ProgramBao2025Stage.DyingTarget:
                choices.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => BaoChoice(f, $"recipient-{(p.Seat)}", $"令{(p.Name)}选择", targets: [p.Seat])));
                choices.Add(BaoChoice(f, "decline", "取消发动"));
                break;
            case ProgramBao2025Stage.DyingRecipientChoice:
                choices.Add(BaoChoice(f, "replace", "失去所有技能，获得征南"));
                choices.Add(BaoChoice(f, "grant", "获得镇南"));
                break;
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal int[] BaoAssaultLegalTargets(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !_coreProgramCardPoliciesOperations.HasCardPolicy(p, SkillProgramCardPolicyKind.ExcludeGlobalTarget, CardKind.BarbarianAssault) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, CardKind.BarbarianAssault, Suit.None, false) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(f.OwnerSeat, p.Seat, CardKind.BarbarianAssault) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(f.OwnerSeat, p.Seat, Suit.None)).Select(p => p.Seat).ToArray();
    internal void PublishBaoChoice(ProgramSkillFrame f)
    {
        var choices = BaoChoices(f);
        if (choices.Count == 0)
        {
            FinishBao2025(f, false);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = f.Bao2025!with { Choices = choices }, Step = ResolutionFrameStep.AwaitingResponse });
        var chooser = f.Bao2025.Stage == ProgramBao2025Stage.DyingRecipientChoice ? f.Bao2025.Recipient!.Value : f.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, $"【{(skill.Name)}】请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("bao2025")]
    internal void ResolveBao2025Choice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The choice lost its owning frame.");
        RequireBao2025(f);
        var r = f.Bao2025!;
        var chooser = r.Stage == ProgramBao2025Stage.DyingRecipientChoice ? r.Recipient!.Value : f.OwnerSeat;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != chooser || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], r.Choices.Where(c => c.Id == choice.Id).ToArray()) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], BaoChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("The choice lost its exact chooser or current published opaque/card/target selection.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (branch == "decline")
        {
            FinishBao2025(f, r.GainedCardId is not null);
            return;
        }

        if (!BaoSourceCurrent(f) && r.GainedCardId is null && r.Stage != ProgramBao2025Stage.DyingRecipientChoice)
        {
            FinishBao2025(f, false);
            return;
        }

        if (r.Stage == ProgramBao2025Stage.TakeTarget)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = r with { Recipient = choice.Targets.Single(), Stage = ProgramBao2025Stage.TakeCard, Choices = [] } });
            PublishBaoChoice(f);
            return;
        }

        if (r.Stage == ProgramBao2025Stage.TakeCard)
        {
            PayBaoGain(f, choice);
            return;
        }

        if (r.Stage == ProgramBao2025Stage.ResetChoice)
        {
            var before = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = r with { Stage = ProgramBao2025Stage.ResetHpChildren, Choices = [], HpBefore = before, HpAfter = Math.Max(0, before - 1) }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var result = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, f.OwnerSeat, 1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBao2025HpPaidEvent(f.Id, r.Source, before, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp));
            if (result != SkillProgramStepOutcome.AwaitChild)
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (r.Stage == ProgramBao2025Stage.AssaultCost)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = r with { Cards = choice.Cards, From = choice.Cards.Select(id => _cardZonesOperations._cardZones.GetLocation(id)).ToArray(), Stage = ProgramBao2025Stage.AssaultTargets, Choices = [] } });
            PublishBaoChoice(f);
            return;
        }

        if (r.Stage == ProgramBao2025Stage.AssaultTargets)
        {
            if (branch != "confirm")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = r with { SelectedTargets = r.SelectedTargets.Append(choice.Targets.Single()).ToArray(), Choices = [] } });
                PublishBaoChoice(f);
                return;
            }

            PayBaoAssault(f);
            return;
        }

        if (r.Stage == ProgramBao2025Stage.DyingTarget)
        {
            _coreProgramLifecycleOperations.ConsumeProgramTriggerUsage(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = r with { Recipient = choice.Targets.Single(), Stage = ProgramBao2025Stage.DyingRecipientChoice, Choices = [] } });
            PublishBaoChoice(f);
            return;
        }

        if (r.Stage != ProgramBao2025Stage.DyingRecipientChoice)
            throw new InvalidOperationException("The selected instruction has already been paid.");
        var recipient = _coreMatchParticipantsPlayersAccess._players[r.Recipient!.Value];
        var replace = branch == "replace";
        var grants = replace ? recipient.SkillGrants.Grants.ToArray() : [];
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.GrantRevision);
        foreach (var grant in grants)
            if (!recipient.SkillGrants.RemoveGrant(grant.GrantId))
                throw new InvalidOperationException("A recipient's skill grant changed before replacement.");
        var granted = _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects.Single().SkillIds[replace ? 0 : 1];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = r with { Stage = ProgramBao2025Stage.DyingSkillChildren, GrantedSkillId = granted, ReplacedSkills = replace, LostGrantIds = grants.Select(g => g.GrantId).ToArray(), Choices = [] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreCharacterHostOperations.AcquireRuntimeSkills(recipient, $"dying-recipient:{(f.OwnerSeat)}:{(f.SkillInstanceId)}:{(f.Id)}", [granted]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DyingRecipientSkillChoiceEvent(f.Id, r.Source, recipient.Seat, replace, granted, f.Bao2025!.LostGrantIds));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void PayBaoGain(ProgramSkillFrame f, PromptChoice choice)
    {
        var r = f.Bao2025!;
        var target = r.Recipient!.Value;
        var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"], true);
        var from = new CardLocation(zone, target);
        var slot = int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[slot];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = r with { Stage = ProgramBao2025Stage.GainChildren, GainedCardId = card.Id, GainedCardKind = card.Kind, GainedFrom = from, MovementBefore = before, MovementAfter = before, Choices = [] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, card.IsGeneralWeapon && zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.Hand(f.OwnerSeat), CharacterBaoSanNiangOlDefinitions.BaoGainReason);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { Bao2025 = f.Bao2025!with { MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBao2025MovementIssuedEvent(f.Id, f.Bao2025!));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void PayBaoAssault(ProgramSkillFrame f)
    {
        var r = f.Bao2025!;
        if (r.Cards.Count != 2 || r.Cards.Distinct().Count() != 2 || r.Cards.Select((id, i) => _cardZonesOperations._cardZones.GetLocation(id) != r.From[i]).Any(v => v) || r.Cards.Any(id => !BaoCostCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Any(c => c.Id == id)) || r.SelectedTargets.Any(t => !BaoAssaultLegalTargets(f).Contains(t)) || !BaoSourceCurrent(f))
            throw new InvalidOperationException("Chosen assault lost its two actual unpaid HE costs or explicit legal targets.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = r with { Stage = ProgramBao2025Stage.AssaultCostChildren, MovementBefore = before, MovementAfter = before, Choices = [] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Cards, CharacterBaoSanNiangOlDefinitions.BaoCostReason);
        _coreProgramLifecycleOperations.ConsumeProgramTriggerUsage(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { Bao2025 = f.Bao2025!with { MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBao2025MovementIssuedEvent(f.Id, f.Bao2025!));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainBaoChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 21600)]
    internal bool ResumeBao2025(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Bao2025: { } r } f || f.Id != id)
            return false;
        RequireBao2025(f);
        if (r.Choices.Count > 0)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishBaoChoice(f);
            return true;
        }

        if (DrainBaoChildren(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.Bao2025!;
        if (r.Stage == ProgramBao2025Stage.GainChildren)
        {
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && CardUseCategoryCatalog.Get(r.GainedCardKind!.Value) != CardUseCategories.Basic)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = r with { Stage = ProgramBao2025Stage.ResetChoice }, PendingMovementContinuation = null });
                PublishBaoChoice(f);
            }
            else
                FinishBao2025(f, true);
            return true;
        }

        if (r.Stage == ProgramBao2025Stage.ResetHpChildren)
        {
            ResetBaoLimitedInstances(f);
            FinishBao2025(f, true);
            return true;
        }

        if (r.Stage == ProgramBao2025Stage.AssaultCostChildren)
        {
            IssueBaoAssault(f);
            return true;
        }

        if (r.Stage == ProgramBao2025Stage.AssaultOutcomes || r.Stage == ProgramBao2025Stage.AssaultOutcomeChildren)
        {
            AdvanceBaoOutcomes(f);
            return true;
        }

        if (r.Stage == ProgramBao2025Stage.DyingSkillChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = r with { Stage = ProgramBao2025Stage.DyingRecoveryChildren, RecoveryIssued = true } });
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp < 1)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1 - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp, null, null);
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return true;
        }

        if (r.Stage == ProgramBao2025Stage.DyingRecoveryChildren)
        {
            FinishBao2025(f, true);
            return true;
        }

        return false;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 21600)]
    internal bool ReturnBaoMovement(ProgramSkillFrame f) => f.Bao2025 is not null && ResumeBao2025(f.Id);
    internal void ResetBaoLimitedInstances(ProgramSkillFrame f)
    {
        var resetId = _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects.Single().SourceBind!;
        var entries = new List<LimitedSkillResetEntry>();
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(player).ProgramInstances.Where(i => i.SkillId == resetId))
                foreach (var trigger in instance.Program.Triggers.Where(t => t.UsageScope == SkillUsageScope.Game && t.UsageLimit == 1 && t.Effects is [{ Op: SkillProgramEffectOp.PayTwoForChosenAssaultAndApplyTargetOutcomes }]))
                {
                    var candidate = new ProgramTriggerCandidate(player.Seat, resetId, trigger.Id, instance.SkillInstanceId, instance.Program.GameplayHash, trigger.Priority);
                    var usage = _coreProgramLifecycleOperations.ProgramTriggerUsageId(candidate);
                    var before = _skillRuntimeStateOperations._skillRuntimeState.GetUsage(player.Seat, resetId, usage, SkillUsageScope.Game);
                    entries.Add(new(new(resetId, trigger.Id, player.Seat, instance.SkillInstanceId), instance.Program.GameplayHash, usage, before));
                    _skillRuntimeStateOperations._skillRuntimeState.ClearUsage(player.Seat, resetId, usage, SkillUsageScope.Game);
                }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramLimitedSkillInstancesResetEvent(f.Id, BaoSource(f), Array.AsReadOnly(entries.ToArray())));
    }

    internal void IssueBaoAssault(ProgramSkillFrame f)
    {
        var r = f.Bao2025!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishBao2025(f, true);
            return;
        }

        var targets = r.SelectedTargets.Where(t => _coreMatchParticipantsPlayersAccess._players[t].IsAlive).ToArray();
        var reserved = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = r with { Stage = ProgramBao2025Stage.AssaultUse, AssaultUseId = reserved }, PendingMovementContinuation = null });
        var card = new Card(0, CardKind.BarbarianAssault, Suit.None, 0);
        var useId = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, targets, CardKind.BarbarianAssault, physicalCardIds: [], chosenAssault: new(f.Id, f.InstructionIndex));
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId)!;
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(p => p.Id == use.ChosenAssault!.ProgramFrameId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { Bao2025 = parent.Bao2025!with { AssaultActionId = use.Action!.ActionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChosenAssaultUseIssuedEvent(f.Id, r.Source, useId, use.Action!.ActionId, r.SelectedTargets));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(useId, card, f.OwnerSeat, targets, LegalActionKind.BarbarianAssault, requiredCardKind: CardKind.Slash, playedCardKind: CardKind.BarbarianAssault);
    }

    [NativeUseReturn(220)]
    internal void ReturnBaoAssault(CardUseFrame use)
    {
        if (use.ChosenAssault is not { } ret)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != ret.ProgramFrameId || f.InstructionIndex != ret.InstructionIndex || f.Bao2025 is not { Stage: ProgramBao2025Stage.AssaultUse } r || r.AssaultUseId != use.Id || r.AssaultActionId != use.Action?.ActionId || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Any(e => e.ResolutionId == use.Id))
            throw new InvalidOperationException("The native assault lost its once-issued typed parent return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = r with { Stage = ProgramBao2025Stage.AssaultOutcomes }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void AdvanceBaoOutcomes(ProgramSkillFrame f)
    {
        var r = f.Bao2025!;
        while (r.OutcomeCursor < r.SelectedTargets.Count)
        {
            var target = r.SelectedTargets[r.OutcomeCursor];
            var damaged = r.DamagedTargets.Contains(target);
            int? discarded = null;
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = r with { Stage = ProgramBao2025Stage.AssaultOutcomeChildren, OutcomeCursor = r.OutcomeCursor + 1, MovementBefore = before, MovementAfter = before } });
            if (damaged && _coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            {
                var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target])).Where(c => !c.IsGeneralWeapon).ToArray();
                if (cards.Length > 0)
                {
                    var card = cards[_randomOperations._random.Next(cards.Length)];
                    discarded = card.Id;
                    _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, target, card, _cardZonesOperations._cardZones.GetLocation(card.Id), CharacterBaoSanNiangOlDefinitions.BaoOutcomeReason);
                }
            }
            else if (!damaged && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            {
                var grant = _turnCardUseEffectsOperations._turnCardUseEffects.GrantTargetCardQuotaAllowance(r.ActualTurn, r.ActualTurnOwner, f.Id, r.OutcomeCursor, new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId), target, r.AssaultUseId!.Value);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnTargetCardQuotaAllowanceGrantedEvent(grant));
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { Bao2025 = f.Bao2025!with { MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChosenAssaultTargetOutcomeEvent(f.Id, r.AssaultUseId!.Value, target, damaged, discarded));
            if (DrainBaoChildren(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            r = f.Bao2025!;
        }

        FinishBao2025(f, true);
    }

    internal void FinishBao2025(ProgramSkillFrame f, bool completed)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBao2025CompletedEvent(f.Id, BaoSource(f), completed));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Bao2025 = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed);
    }

    SkillProgramStepOutcome IBao2025ProgramHost.BeginBao2025(ProgramSkillFrame f, SkillProgramEffect e) => BeginBao2025(f, e);
}
