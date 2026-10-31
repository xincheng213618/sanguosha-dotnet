#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuoJunOlRuntime
{
    internal const string HuoJunDiscardReason = "skill-program.huo-jun.fenrui.discard";
    internal static string HuoJunSlotName(EquipmentSlot s) => RuntimeSkills.RuntimeRules_huo_jun_ol_HuoJunFenrui.HuoJunSlotName(s);
    internal CardConversionSource HuoJunSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IHuoJunProgramHost.RestoreSlotAndUseMatchingEquipment(ProgramSkillFrame f) => BeginHuoJunFenrui(f);
    internal bool HuoJunHasAbolishedSlot(int seat) => Enum.GetValues<EquipmentSlot>().Any(s => _coreMatchParticipantsPlayersAccess._players[seat].EquipmentSlotCapacity(s) == 0);
    internal bool HuoJunDiscardable(int seat, CardZoneKind zone, Card c) => !c.IsGeneralWeapon && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, c, new(zone, seat), OwnedCardMoveIntent.Discard);
    internal bool HuoJunHasDiscard(int seat) => new[]
    {
        CardZoneKind.Hand,
        CardZoneKind.Equipment
    }.Any(z => _cardZonesOperations._cardZones.CardsAt(new(z, seat)).Any(c => HuoJunDiscardable(seat, z, c)));
    internal bool HuoJunDamageUsed(int owner, string skill) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuoJunFenruiDamageCommittedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26210)]
    internal bool CanOfferHuoJunFenrui(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => !t.Effects.Any(e => e.Op == SkillProgramEffectOp.RestoreSlotAndUseMatchingEquipment) || context.Window == SkillProgramTriggerWindow.TurnEnding && HuoJunHasAbolishedSlot(c.OwnerSeat) && HuoJunHasDiscard(c.OwnerSeat);
    internal SkillProgramStepOutcome BeginHuoJunFenrui(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.HuoJunFenrui is not null || f.TriggerId is null || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } context || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.OwnerSeat != f.OwnerSeat || window.ItemIndex < 0 || window.ItemIndex >= window.Items.Count || window.Items[window.ItemIndex] is not { Kind: TurnEndingBoundaryItemKind.Program, Candidate: { } candidate } || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) || !HuoJunHasAbolishedSlot(f.OwnerSeat) || !HuoJunHasDiscard(f.OwnerSeat))
            throw new InvalidOperationException("Fenrui requires its actual ending candidate, one discardable entity and an abolished equipment category.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuoJunFenrui = new(HuoJunSource(f), f.GameplayHash, f.InstructionIndex, window.Id, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, HuoJunFenruiStage.Cost) });
        PublishHuoJunFenrui(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> HuoJunFenruiChoices(ProgramSkillFrame f)
    {
        var r = f.HuoJunFenrui!;
        if (r.Stage == HuoJunFenruiStage.Cost)
            return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (zone, card) => HuoJunDiscardable(f.OwnerSeat, zone, card)).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "huo-jun-fenrui", ["branch"] = "cost" } }).ToArray());
        PromptChoice Choice(string branch, int value, string label, IReadOnlyList<int>? targets = null) => new(new($"huojun.{(f.Id)}.{(r.Stage)}.{(branch)}.{(value)}"), label, [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "huo-jun-fenrui", ["branch"] = branch, ["value"] = value.ToString(CultureInfo.InvariantCulture) });
        if (r.Stage == HuoJunFenruiStage.Restore)
            return Array.AsReadOnly(Enum.GetValues<EquipmentSlot>().Where(s => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].EquipmentSlotCapacity(s) == 0).Select(s => Choice("restore", (int)s, "复原" + HuoJunSlotName(s))).ToArray());
        if (r.Stage != HuoJunFenruiStage.DamageChoice)
            return[];
        var result = new List<PromptChoice>();
        var own = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
        if (!HuoJunDamageUsed(f.OwnerSeat, f.SkillId))
            result.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Count < own).Select(p => Choice("damage", p.Seat, $"对 {(p.Name)} 造成{(own - _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Count)}点伤害", [p.Seat])));
        result.Add(Choice("finish", 0, "不造成伤害"));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishHuoJunFenrui(ProgramSkillFrame f)
    {
        var choices = HuoJunFenruiChoices(f);
        if (choices.Count == 0)
        {
            FinishHuoJunFenrui(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.HuoJunFenrui!.Stage switch
        {
            HuoJunFenruiStage.Cost => "奋锐：弃置一张手牌或装备牌",
            HuoJunFenruiStage.Restore => "奋锐：复原一个装备栏",
            _ => "奋锐：是否对装备数更少的角色造成伤害？"
        }, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("huo-jun-fenrui")]
    internal void AnswerHuoJunFenrui(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuoJunFenrui: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], HuoJunFenruiChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Fenrui lost its exact private payer, restored slot or current equipment-count target.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || r.CostCardId is null && !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
        {
            FinishHuoJunFenrui(f);
            return;
        }

        var branch = choice.Parameters["branch"];
        if (branch == "cost")
        {
            var id = choice.Cards.Single();
            var from = _cardZonesOperations._cardZones.GetLocation(id);
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, HuoJunFenrui = r with { Stage = HuoJunFenruiStage.CostChildren, CostCardId = id, CostFrom = from, SequenceBefore = before, SequenceAfter = before } });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [id], new(HuoJunDiscardReason));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            r = f.HuoJunFenrui!with
            {
                SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HuoJunFenrui = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuoJunFenruiCostPaidEvent(f.Id, r.Source, id, from, before, r.SequenceAfter));
        }
        else if (branch == "restore")
        {
            var slot = (EquipmentSlot)int.Parse(choice.Parameters["value"], CultureInfo.InvariantCulture);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, HuoJunFenrui = r with { Stage = HuoJunFenruiStage.RestoreChildren, RestoredSlot = slot } });
            if (!_coreAdvancedLifecycleOperations.RestoreEquipmentSlot(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], slot))
                throw new InvalidOperationException("Fenrui must restore its one actual abolished native slot.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuoJunFenruiSlotRestoredEvent(f.Id, f.OwnerSeat, slot));
        }
        else if (branch == "damage")
        {
            var target = choice.Targets.Single();
            var own = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
            var other = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).Count;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, HuoJunFenrui = r with { Stage = HuoJunFenruiStage.DamageIssued, DamageTargetSeat = target, DamageAmount = own - other } });
            // "若如此做" consumes the branch at the actual native attempt, including prevention or redirection.
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuoJunFenruiDamageCommittedEvent(f.Id, r.Source, target, own, other, own - other));
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, own - other);
            return;
        }
        else if (branch == "finish")
        {
            FinishHuoJunFenrui(f);
            return;
        }
        else
            throw new InvalidOperationException("Unknown Fenrui choice.");
        ContinueHuoJunFenrui(f.Id);
    }

    internal bool DrainHuoJunFenruiChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueHuoJunFenrui(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { HuoJunFenrui: { } r } f && f.Id == id)
        {
            if (DrainHuoJunFenruiChildren(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.HuoJunFenrui!;
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishHuoJunFenrui(f);
                return;
            }

            switch (r.Stage)
            {
                case HuoJunFenruiStage.Cost:
                case HuoJunFenruiStage.Restore:
                case HuoJunFenruiStage.DamageChoice:
                    PublishHuoJunFenrui(f);
                    return;
                case HuoJunFenruiStage.CostChildren:
                    if (!HuoJunHasAbolishedSlot(f.OwnerSeat))
                    {
                        FinishHuoJunFenrui(f);
                        return;
                    }

                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { HuoJunFenrui = r with { Stage = HuoJunFenruiStage.Restore } });
                    continue;
                case HuoJunFenruiStage.RestoreChildren:
                    IssueHuoJunRandomEquipment(f);
                    return;
                case HuoJunFenruiStage.NativeEquipment:
                    throw new InvalidOperationException("Fenrui's native equipment use must return through its exact native receipt.");
                case HuoJunFenruiStage.EquipmentChildren:
                    if (HuoJunDamageUsed(f.OwnerSeat, f.SkillId))
                    {
                        FinishHuoJunFenrui(f);
                        return;
                    }

                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { HuoJunFenrui = r with { Stage = HuoJunFenruiStage.DamageChoice } });
                    continue;
                case HuoJunFenruiStage.DamageIssued:
                    if (f.AttackAttempt is not null)
                    {
                        _coreNativeDamageCompletionOperations.CompleteDamageAttack(_kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(f.Id));
                        return;
                    }

                    FinishHuoJunFenrui(f);
                    return;
                default:
                    throw new InvalidOperationException("Unknown Fenrui stage.");
            }
        }
    }

    internal void FinishHuoJunFenrui(ProgramSkillFrame f)
    {
        var paid = f.HuoJunFenrui?.CostCardId is not null;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuoJunFenrui = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, paid);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26210)]
    internal bool ResumeHuoJunFenrui(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuoJunFenrui: not null } f || f.Id != id)
            return false;
        ContinueHuoJunFenrui(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26210)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26210)]
    internal bool CanContinuePaidHuoJunFenrui(ProgramSkillFrame f) => f.HuoJunFenrui is { CostCardId: not null } r && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuoJunFenruiCostPaidEvent>().Any(e => e.FrameId == f.Id && e.Source == r.Source);
    [ProgramAiChoice(SkillProgramEffectOp.RestoreSlotAndUseMatchingEquipment)]
    internal PromptChoice AiHuoJunFenrui(PendingDecision p, ProgramSkillFrame f) => f.HuoJunFenrui!.Stage == HuoJunFenruiStage.Cost ? p.Choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First() : p.Choices.OrderByDescending(c => c.Parameters["branch"] == "damage" ? _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count - _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]).Count : 0).First();
}
