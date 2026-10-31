#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoXiaoQiaoOlRuntime
{
    internal const string XingwuDepositReason = "skill-program.da-qiao-xiao-qiao.xingwu.deposit", XingwuPileReason = "skill-program.da-qiao-xiao-qiao.xingwu.pile-remove", XingwuHandReason = "skill-program.da-qiao-xiao-qiao.xingwu.hand-discard", XingwuEquipmentReason = "skill-program.da-qiao-xiao-qiao.xingwu.equipment-discard";
    internal static CardConversionSource DaQiaoXiaoQiaoSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_da_qiao_xiao_qiao_ol_DaQiaoXiaoQiao.DaQiaoXiaoQiaoSource(f);
    SkillProgramStepOutcome IDaQiaoXiaoQiaoHost.Resolve(ProgramSkillFrame f, bool maintain) => BeginDaQiaoXiaoQiao(f, maintain);
    internal bool DaQiaoXiaoQiaoParent(ProgramSkillFrame f) => f.WindowContext is { Window: SkillProgramTriggerWindow.DiscardPhaseStarting } c && c.OwnerSeat == f.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == f.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Discard && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { Window: SkillProgramTriggerWindow.DiscardPhaseStarting, Continuation: ProgramLifecycleContinuation.CompleteDiscardPhase } p && p.OwnerSeat == f.OwnerSeat && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]);
    internal bool DaQiaoXiaoQiaoLive(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreProgramLifecycleOperations.CanInvokeProgramSkill(f);
    internal SkillProgramStepOutcome BeginDaQiaoXiaoQiao(ProgramSkillFrame input, bool maintain)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (maintain)
        {
            if (f.SkillId != "ol:luoyan" || f.WindowContext?.Window != SkillProgramTriggerWindow.GameStarting)
                throw new InvalidOperationException("Luoyan requires its native setup binding.");
            SynchronizeDaQiaoXiaoQiaoLuoyan();
            return SkillProgramStepOutcome.Continue;
        }

        if (f.SkillId != "ol:xingwu" || f.TriggerId != "discard-start-dance" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.DaQiaoXiaoQiao is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !DaQiaoXiaoQiaoParent(f))
            throw new InvalidOperationException("Xingwu requires its exact unpaid actual Discard starting frame.");
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.GrantRevision);
        var pile = _corePublicPileSourcesOperations.EnsurePublicPileSource(f, int.MaxValue);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DaQiaoXiaoQiao = new(DaQiaoXiaoQiaoSource(f), f.GameplayHash, f.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, pile, DaQiaoXiaoQiaoStage.Deposit) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DaQiaoXiaoQiaoStartedEvent(f.Id, DaQiaoXiaoQiaoSource(f), f.GameplayHash, f.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
        PublishDaQiaoXiaoQiao(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<DaQiaoXiaoQiaoEntity> DaQiaoXiaoQiaoOwned(ProgramSkillFrame f, bool handOnly, bool discard) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(handOnly ? [] : _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).Where(c => !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(f.OwnerSeat, f.SkillId, f.SkillInstanceId, c) && (!discard || !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard))).Select(c => new DaQiaoXiaoQiaoEntity(c.Id, _cardZonesOperations._cardZones.GetLocation(c.Id))).ToArray());
    internal IReadOnlyList<DaQiaoXiaoQiaoEntity> DaQiaoXiaoQiaoPileCards(ProgramSkillFrame f) => Array.AsReadOnly(_corePublicPileSourcesOperations.PublicPileSources(f.OwnerSeat).Where(p => p.SkillId == "ol:xingwu").SelectMany(p => _corePublicPileSourcesOperations.PublicPileCards(p).Select(c => new DaQiaoXiaoQiaoEntity(c.Id, p.Location))).ToArray());
    internal IReadOnlyList<PromptChoice> DaQiaoXiaoQiaoChoices(ProgramSkillFrame f)
    {
        var r = f.DaQiaoXiaoQiao!;
        var choices = new List<PromptChoice>();
        void Add(string branch, string label, int[]? cards = null, int[]? targets = null) => choices.Add(new(new($"xingwu.{(f.Id)}.{(r.Stage)}.{(branch)}.{(string.Join('.', cards ?? []))}.{(string.Join('.', targets ?? []))}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { { "program-action", "da-qiao-xiao-qiao" }, { "branch", branch }, { "frame-id", f.Id.ToString(CultureInfo.InvariantCulture) } }));
        if (r.Stage == DaQiaoXiaoQiaoStage.Deposit)
        {
            foreach (var c in DaQiaoXiaoQiaoOwned(f, false, false))
                Add("deposit", "将【" + _coreCardAttackStateOperations.GetAttackCard(c.Id).DisplayName + "】置为星舞", [c.Id]);
            Add("skip-deposit", "不置牌，继续检查第二项");
        }

        if (r.Stage == DaQiaoXiaoQiaoStage.Burst)
        {
            if (DaQiaoXiaoQiaoPileCards(f).Count >= 3)
                Add("pile", "移去三张星舞");
            if (DaQiaoXiaoQiaoOwned(f, true, true).Count >= 2)
                Add("turnover", "翻面并弃置两张手牌");
            Add("finish", "不发动伤害项");
        }

        if (r.Stage is DaQiaoXiaoQiaoStage.PileCards or DaQiaoXiaoQiaoStage.HandCards)
        {
            var cards = r.Stage == DaQiaoXiaoQiaoStage.PileCards ? DaQiaoXiaoQiaoPileCards(f) : DaQiaoXiaoQiaoOwned(f, true, true);
            foreach (var c in cards.Where(c => !r.Selected.Contains(c)))
                Add("card", "选择【" + _coreCardAttackStateOperations.GetAttackCard(c.Id).DisplayName + "】", [c.Id]);
        }

        if (r.Stage == DaQiaoXiaoQiaoStage.Target)
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
                Add("target", "弃置 " + target.Name + " 的全部可弃装备，然后按性别造成伤害", targets: [target.Seat]);
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishDaQiaoXiaoQiao(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !f.DaQiaoXiaoQiao!.Paid && !DaQiaoXiaoQiaoLive(f))
        {
            FinishDaQiaoXiaoQiao(f);
            return;
        }

        var choices = DaQiaoXiaoQiaoChoices(f);
        if (choices.Count == 0)
        {
            FinishDaQiaoXiaoQiao(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "星舞：" + f.DaQiaoXiaoQiao!.Stage, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("da-qiao-xiao-qiao")]
    internal void AnswerDaQiaoXiaoQiao(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DaQiaoXiaoQiao: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], DaQiaoXiaoQiaoChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Xingwu lost its exact private chooser or payable entity.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (r.Stage == DaQiaoXiaoQiaoStage.Deposit)
        {
            if (branch == "skip-deposit")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DaQiaoXiaoQiao = r with { Stage = DaQiaoXiaoQiaoStage.Burst } });
                PublishDaQiaoXiaoQiao(f);
                return;
            }

            var c = DaQiaoXiaoQiaoOwned(f, false, false).Single(c => c.Id == choice.Cards.Single());
            MoveDaQiaoXiaoQiao(f, DaQiaoXiaoQiaoStage.DepositChildren, [c], r.Pile.Location, XingwuDepositReason, false);
            ContinueDaQiaoXiaoQiao(f.Id);
            return;
        }

        if (r.Stage == DaQiaoXiaoQiaoStage.Burst)
        {
            if (branch == "finish")
            {
                FinishDaQiaoXiaoQiao(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DaQiaoXiaoQiao = r with { Branch = branch, Selected = [], Stage = branch == "pile" ? DaQiaoXiaoQiaoStage.PileCards : DaQiaoXiaoQiaoStage.TurnoverChildren, Paid = r.Paid || branch == "turnover" } });
            if (branch == "turnover")
            {
                _coreProgramLifecycleOperations.SetProgramTargetFaceState(f.Id, f.OwnerSeat, f.OwnerSeat, !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsFaceDown);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DaQiaoXiaoQiaoTurnedEvent(f.Id, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsFaceDown));
                ContinueDaQiaoXiaoQiao(f.Id);
            }
            else
                PublishDaQiaoXiaoQiao(f);
            return;
        }

        if (r.Stage is DaQiaoXiaoQiaoStage.PileCards or DaQiaoXiaoQiaoStage.HandCards)
        {
            var legal = r.Stage == DaQiaoXiaoQiaoStage.PileCards ? DaQiaoXiaoQiaoPileCards(f) : DaQiaoXiaoQiaoOwned(f, true, true);
            var selected = r.Selected.Append(legal.Single(c => c.Id == choice.Cards.Single())).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DaQiaoXiaoQiao = r with { Selected = selected, Stage = selected.Length == (r.Branch == "pile" ? 3 : 2) ? DaQiaoXiaoQiaoStage.Target : r.Stage } });
            PublishDaQiaoXiaoQiao(f);
            return;
        }

        if (r.Stage != DaQiaoXiaoQiaoStage.Target)
            throw new InvalidOperationException("Unexpected Xingwu choice stage.");
        var target = choice.Targets.Single();
        var current = r.Branch == "pile" ? DaQiaoXiaoQiaoPileCards(f) : DaQiaoXiaoQiaoOwned(f, true, true);
        if (r.Selected.Count != (r.Branch == "pile" ? 3 : 2) || r.Selected.Any(c => !current.Contains(c)))
        {
            FinishDaQiaoXiaoQiao(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DaQiaoXiaoQiao = r with { TargetSeat = target } });
        MoveDaQiaoXiaoQiao(f, DaQiaoXiaoQiaoStage.CostChildren, r.Selected, CardLocation.DiscardPile, r.Branch == "pile" ? XingwuPileReason : XingwuHandReason, r.Branch != "pile");
        ContinueDaQiaoXiaoQiao(f.Id);
    }

    internal void MoveDaQiaoXiaoQiao(ProgramSkillFrame f, DaQiaoXiaoQiaoStage stage, IReadOnlyList<DaQiaoXiaoQiaoEntity> cards, CardLocation to, string reason, bool discard)
    {
        var r = f.DaQiaoXiaoQiao!;
        var grain = cards.Any(c => c.From.Zone == CardZoneKind.Equipment && _coreCardAttackStateOperations.GetAttackCard(c.Id).Kind == CardKind.WoodenOx) ? cards.Where(c => c.From.Zone == CardZoneKind.Equipment && _coreCardAttackStateOperations.GetAttackCard(c.Id).Kind == CardKind.WoodenOx).SelectMany(c => _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[c.From.OwnerSeat!.Value]).Select(x => x.Id)).ToArray() : [];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DaQiaoXiaoQiao = r with { Stage = stage, Paid = true, MovementStep = r.MovementStep + 1, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Entities = cards, Grain = grain } });
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active = active with { DaQiaoXiaoQiao = active.DaQiaoXiaoQiao!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            foreach (var m in moves)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DaQiaoXiaoQiaoEntityPaidEvent(f.Id, active.DaQiaoXiaoQiao!.MovementStep, m.CardId, m.From, m.To, m.Sequence));
        }

        if (cards.Count > 0)
        {
            if (discard)
                _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, cards.Select(c => c.Id).ToArray(), new(reason), Paid);
            else
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards.Select(c => c.Id).ToArray(), to, new(reason), Paid);
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DaQiaoXiaoQiao = f.DaQiaoXiaoQiao!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        r = f.DaQiaoXiaoQiao!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DaQiaoXiaoQiaoMovementPaidEvent(f.Id, r.MovementStep, stage, r.Before, r.After, cards.Count));
    }

    internal bool DrainDaQiaoXiaoQiao(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueDaQiaoXiaoQiao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DaQiaoXiaoQiao: { } r } f || f.Id != id)
            return;
        if (r.Stage is DaQiaoXiaoQiaoStage.Deposit or DaQiaoXiaoQiaoStage.Burst or DaQiaoXiaoQiaoStage.PileCards or DaQiaoXiaoQiaoStage.HandCards or DaQiaoXiaoQiaoStage.Target)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishDaQiaoXiaoQiao(f);
            return;
        }

        if (DrainDaQiaoXiaoQiao(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.DaQiaoXiaoQiao!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && r.Stage is not (DaQiaoXiaoQiaoStage.CostChildren or DaQiaoXiaoQiaoStage.EquipmentChildren or DaQiaoXiaoQiaoStage.DamageChildren))
        {
            FinishDaQiaoXiaoQiao(f);
            return;
        }

        if (r.Stage == DaQiaoXiaoQiaoStage.DepositChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DaQiaoXiaoQiao = r with { Stage = DaQiaoXiaoQiaoStage.Burst } });
            PublishDaQiaoXiaoQiao(f);
            return;
        }

        if (r.Stage == DaQiaoXiaoQiaoStage.TurnoverChildren)
        {
            if (DaQiaoXiaoQiaoOwned(f, true, true).Count < 2)
            {
                FinishDaQiaoXiaoQiao(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DaQiaoXiaoQiao = r with { Stage = DaQiaoXiaoQiaoStage.HandCards } });
            PublishDaQiaoXiaoQiao(f);
            return;
        }

        if (r.Stage == DaQiaoXiaoQiaoStage.CostChildren)
        {
            if (r.TargetSeat is not { } target || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            {
                FinishDaQiaoXiaoQiao(f);
                return;
            }

            var equipment = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, c, CardLocation.Equipment(target), OwnedCardMoveIntent.Discard) && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(f.OwnerSeat, f.SkillId, f.SkillInstanceId, c)).Select(c => new DaQiaoXiaoQiaoEntity(c.Id, CardLocation.Equipment(target))).ToArray();
            MoveDaQiaoXiaoQiao(f, DaQiaoXiaoQiaoStage.EquipmentChildren, equipment, CardLocation.DiscardPile, XingwuEquipmentReason, true);
            ContinueDaQiaoXiaoQiao(id);
            return;
        }

        if (r.Stage == DaQiaoXiaoQiaoStage.EquipmentChildren)
        {
            if (r.TargetSeat is not { } target || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            {
                FinishDaQiaoXiaoQiao(f);
                return;
            }

            var amount = _coreMatchParticipantsPlayersAccess._players[target].Gender == GeneralGender.Female ? 1 : 2;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DaQiaoXiaoQiao = r with { Stage = DaQiaoXiaoQiaoStage.DamageChildren, Damage = amount } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DaQiaoXiaoQiaoDamageIssuedEvent(id, target, amount));
            _ = _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, amount);
            return;
        }

        FinishDaQiaoXiaoQiao(f);
    }

    internal void FinishDaQiaoXiaoQiao(ProgramSkillFrame f)
    {
        var r = f.DaQiaoXiaoQiao!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DaQiaoXiaoQiaoCompletedEvent(f.Id, r.Branch, r.TargetSeat));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DaQiaoXiaoQiao = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27340)]
    internal bool ResumeDaQiaoXiaoQiao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DaQiaoXiaoQiao: not null } f || f.Id != id)
            return false;
        ContinueDaQiaoXiaoQiao(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27340)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27340)]
    internal bool ContinuePaidDaQiaoXiaoQiao(ProgramSkillFrame f) => f.DaQiaoXiaoQiao is { Paid: true } && ValidDaQiaoXiaoQiao(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveXingwuDepositAndBurst)]
    internal PromptChoice ChooseDaQiaoXiaoQiaoAi(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.DaQiaoXiaoQiao!;
        if (r.Stage == DaQiaoXiaoQiaoStage.Deposit)
            return p.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).FirstOrDefault() ?? p.Choices[0];
        if (r.Stage == DaQiaoXiaoQiaoStage.Burst)
            return p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "pile") ?? p.Choices.First(c => c.Parameters["branch"] == "finish");
        if (r.Stage == DaQiaoXiaoQiaoStage.Target)
            return p.Choices.OrderBy(c => CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]])).ThenByDescending(c => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[c.Targets[0]]).Count).ThenBy(c => c.Targets[0]).First();
        return p.Choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).First();
    }
}
