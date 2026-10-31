#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangHengAnniversaryRuntime
{
    internal static CardConversionSource ZhangHengSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static CardMoveReason ZhangHengMoveReason => new("skill-program.anniversary:dangzai.move");

    internal Card[] ZhangHengMovable(int owner, int donor, IReadOnlyList<int>? selected = null)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || !_coreMatchParticipantsPlayersAccess._players[donor].IsAlive || owner == donor)
            return[];
        var chosen = selected ?? [];
        var kinds = chosen.Select(id => _coreActualTurnsOperations.GetJudgmentEffectiveCardKind(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToHashSet();
        return _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[donor]).Where(c => !chosen.Contains(c.Id) && !kinds.Contains(_coreActualTurnsOperations.GetJudgmentEffectiveCardKind(c)) && _coreProgramCardPaymentsOperations.CanMoveProgramCardToCorrespondingZone(c, CardZoneKind.Judgment, owner) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, c, CardLocation.Judgment(donor), OwnedCardMoveIntent.Transfer)).ToArray();
    }

    internal IEnumerable<int> ZhangHengDonors(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && ZhangHengMovable(owner, p.Seat).Length > 0).Select(p => p.Seat);
    SkillProgramStepOutcome IAnniversaryZhangHengHost.Begin(ProgramSkillFrame f) => BeginZhangHengDangzai(f);
    internal SkillProgramStepOutcome BeginZhangHengDangzai(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryZhangHeng is not null || f.InstructionIndex != 1 || f.SkillId != AnniversaryZhangHengComposition.Dangzai || f.TriggerId != "take-delayed-subset" || f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting } w || !ZhangHengDonors(f.OwnerSeat).Any())
            throw new InvalidOperationException("Dangzai requires the original Play-start boundary and another role's genuinely movable judgment entities.");
        var r = new AnniversaryZhangHengReceipt(ZhangHengSource(f), f.GameplayHash, w.ParentFrameId, _coreActualTurnProgressTurnNumberAccess._turnNumber, "donor");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangHeng = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangHengStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn));
        PublishZhangHeng(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ZhangHengChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryZhangHeng!;
        PromptChoice C(string key, string label, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => new(new($"zhang-heng.{(f.Id)}.{(r.Stage)}.{(r.Cards.Count)}.{(key)}"), label, cards, targets, new Dictionary<string, string> { ["program-action"] = "anniversary-zhang-heng", ["branch"] = key });
        if (r.Stage == "donor")
            return Array.AsReadOnly(ZhangHengDonors(f.OwnerSeat).Select(seat => C("donor", "移动" + _coreMatchParticipantsPlayersAccess._players[seat].Name + "的判定区牌", [], [seat])with { Id = new($"zhang-heng.{(f.Id)}.donor.{(seat)}") }).ToArray());
        if (r.Stage != "cards" || !_coreMatchParticipantsPlayersAccess._players[r.Donor].IsAlive)
            return[];
        var choices = ZhangHengMovable(f.OwnerSeat, r.Donor, r.Cards).Select(card => C("card", "选择【" + CardCatalog.Get(_coreActualTurnsOperations.GetJudgmentEffectiveCardKind(card)).DisplayName + "】", [card.Id], [])with { Id = new($"zhang-heng.{(f.Id)}.card.{(card.Id)}.{(r.Cards.Count)}") }).ToList();
        choices.Add(C("finish", r.Cards.Count == 0 ? "取消移动" : $"确认移动{(r.Cards.Count)}张牌", [], []));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishZhangHeng(ProgramSkillFrame f)
    {
        var choices = ZhangHengChoices(f);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || choices.Count == 0)
        {
            FinishZhangHeng(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.AnniversaryZhangHeng!.Stage == "donor" ? "挡灾：选择另一名角色的判定区。" : "挡灾：选择任意张可合法移入的判定区牌后确认。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("anniversary-zhang-heng")]
    internal void AnswerZhangHeng(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryZhangHeng: { } r } f || !ValidZhangHeng(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ZhangHengChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Dangzai rejects a changed original Play boundary, donor or physical judgment entity.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == "donor")
        {
            var donor = selected.Targets.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangHeng = r with { Stage = "cards", Donor = donor } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangHengDonorEvent(f.Id, donor));
            PublishZhangHeng(f);
            return;
        }

        if (selected.Parameters["branch"] == "card")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangHeng = r with { Cards = r.Cards.Append(selected.Cards.Single()).ToArray() } });
            PublishZhangHeng(f);
            return;
        }

        if (r.Cards.Count == 0)
        {
            FinishZhangHeng(f);
            return;
        }

        var cards = r.Cards.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray();
        if (!_coreMatchParticipantsPlayersAccess._players[r.Donor].IsAlive || cards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.Id) != CardLocation.Judgment(r.Donor) || !_coreProgramCardPaymentsOperations.CanMoveProgramCardToCorrespondingZone(c, CardZoneKind.Judgment, f.OwnerSeat)) || cards.Select(_coreActualTurnsOperations.GetJudgmentEffectiveCardKind).Distinct().Count() != cards.Length)
            throw new InvalidOperationException("Dangzai must revalidate the complete actual subset before its one original native transfer.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangHeng = r with { Stage = "children", Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.Judgment(r.Donor), CardLocation.Judgment(f.OwnerSeat), ZhangHengMoveReason, disclosure: CardMovementDisclosure.FaceUp);
        var live = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(live with { AnniversaryZhangHeng = live.AnniversaryZhangHeng!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangHengMovedEvent(f.Id, r.Donor, cards.Length, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueZhangHeng(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 49600)]
    internal bool ContinueZhangHeng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryZhangHeng: { } r } f || f.Id != id)
            return false;
        if (!ValidZhangHeng(f))
            throw new InvalidOperationException("Dangzai lost its original selected real judgment transfer or paid observer return.");
        if (r.Stage is "donor" or "cards")
        {
            PublishZhangHeng(f);
            return true;
        }

        if (_coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            return true;
        FinishZhangHeng(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 49600)]
    internal bool ReturnZhangHengMovement(ProgramSkillFrame f) => f.AnniversaryZhangHeng is not null && ContinueZhangHeng(f.Id);
    internal void FinishZhangHeng(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangHengCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangHeng = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryZhangHengDangzai)]
    internal PromptChoice ChooseZhangHeng(PendingDecision p) => p.Choices.FirstOrDefault(c => c.Cards.Count == 1) ?? p.Choices.First();
}
