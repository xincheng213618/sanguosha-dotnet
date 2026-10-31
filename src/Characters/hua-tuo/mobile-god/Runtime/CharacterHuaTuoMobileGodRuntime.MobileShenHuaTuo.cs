#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaTuoMobileGodRuntime
{
    internal const string MobileHtWuling = "mobile-god:wuling", MobileHtYouyi = "mobile-god:youyi", MobileHtEffects = "mobile-god:wuling-effects", MobileHtDeer = "mobile-god:wuling-deer";
    internal static CardConversionSource MobileHtSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal MobileWulingTaughtEvent? MobileHtCourse(int seat) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileWulingTaughtEvent>().LastOrDefault(c => c.TargetSeat == seat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileWulingAdvancedEvent>().LastOrDefault(e => e.CourseId == c.CourseId)?.Active == true);
    internal int MobileHtCourseIndex(long id) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileWulingAdvancedEvent>().Last(e => e.CourseId == id).Index;
    internal MobileWulingAnimal[] MobileHtCourseOrder(long id) => _characterHuaTuoMobileGodMobileWulingCoursesAccess._mobileWulingCourses.TryGetValue(id, out var c) ? c.Order.ToArray() : throw new InvalidOperationException("Wuling lost its private accepted persistent course.");
    internal bool MobileHtSpirit(int seat, MobileWulingAnimal animal) => _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[seat], MobileHtEffects) && MobileHtCourse(seat)is { } c && MobileHtCourseOrder(c.CourseId)[MobileHtCourseIndex(c.CourseId)] == animal;
    internal int[] MobileHtTeachingTargets() => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && MobileHtCourse(p.Seat)is null && p.Markers.GetValueOrDefault(PlayerMarkerKind.MobileWuling) == 0).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatAccess._currentSeat, p.Seat)).Select(p => p.Seat).ToArray();
    internal DiscardRecoveryEntity[] MobileHtDiscarded(int owner)
    {
        if (_coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } phase || phase.ActorSeat != owner)
            return[];
        var invocations = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => e.Phase == phase).Select(e => e.InvocationId).ToHashSet();
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Where(e => invocations.Contains(e.InvocationId)).OrderBy(e => e.CompletionSequence).Select(e => new DiscardRecoveryEntity(e.CardId, e.CompletionSequence, e.From)).Where(_coreActualDiscardRecoveryPhasesOperations.ActualDiscardEntityStillAvailable).DistinctBy(e => e.CardId).ToArray();
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 39100)]
    internal bool CanActivateMobileHt(ProgramActivationLegalityContext gate) => gate.Program.Id switch
    {
        MobileHtWuling => MobileHtTeachingTargets().Length > 0,
        MobileHtYouyi => _cardZonesOperations._cardZones.CardsAt(CardLocation.Benevolence).Count > 0,
        _ => true
    };
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 39100)]
    internal bool CanTriggerMobileHt(ProgramTriggerLegalityContext gate) => gate.SkillId switch
    {
        MobileHtYouyi => MobileHtDiscarded(gate.OwnerSeat).Length > 0,
        MobileHtEffects => gate.Trigger.Id == "bear-first-damage" ? MobileHtSpirit(gate.OwnerSeat, MobileWulingAnimal.Bear) : MobileHtCourse(gate.OwnerSeat)is not null,
        _ => true
    };
    SkillProgramStepOutcome IMobileHuaTuoHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => BeginMobileHt(frame, op);
    internal SkillProgramStepOutcome BeginMobileHt(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MobileHuaTuo is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Mobile Hua Tuo requires its original unpaid activation or native trigger.");
        var r = new MobileHuaTuoReceipt(MobileHtSource(f), f.GameplayHash, op, f.InstructionIndex, MobileHuaTuoStage.Children);
        if (op == SkillProgramEffectOp.TeachMobileShenHuaTuoWuling)
        {
            if (f.TriggerId is not null || f.SkillId != MobileHtWuling || MobileHtTeachingTargets().Length == 0)
                throw new InvalidOperationException("Wuling requires a living unmarked recipient.");
            r = r with
            {
                Stage = MobileHuaTuoStage.Recipient,
                CourseId = f.Id
            };
        }
        else if (op == SkillProgramEffectOp.AdvanceMobileShenHuaTuoWuling)
        {
            if (f.SkillId != MobileHtEffects || f.WindowContext?.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || MobileHtCourse(f.OwnerSeat)is not { } course)
                throw new InvalidOperationException("Wuling rotation requires its actual recipient's preparation.");
            r = r with
            {
                CourseId = course.CourseId,
                TargetSeat = course.TargetSeat,
                Order = MobileHtCourseOrder(course.CourseId),
                AnimalIndex = MobileHtCourseIndex(course.CourseId) + 1,
                Stage = MobileHuaTuoStage.Entry
            };
        }
        else if (op == SkillProgramEffectOp.DepositMobileShenHuaTuoYouyi)
        {
            if (f.SkillId != MobileHtYouyi || f.WindowContext?.Window != SkillProgramTriggerWindow.DiscardPhaseEnded)
                throw new InvalidOperationException("Youyi requires its original actually executed discard phase.");
            r = r with
            {
                Cards = MobileHtDiscarded(f.OwnerSeat).Select(e => e.CardId).ToArray(),
                Before = _coreCardMovementJournalMovementSequenceAccess._movementSequence
            };
        }
        else
        {
            if (f.SkillId != MobileHtYouyi || f.TriggerId is not null || _cardZonesOperations._cardZones.CardsAt(CardLocation.Benevolence).Count == 0)
                throw new InvalidOperationException("Youyi requires the actual entire nonempty shared Ren area.");
            r = r with
            {
                Cards = _cardZonesOperations._cardZones.CardsAt(CardLocation.Benevolence).Select(c => c.Id).ToArray(),
                Before = _coreCardMovementJournalMovementSequenceAccess._movementSequence,
                Recipients = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatAccess._currentSeat, p.Seat)).Select(p => p.Seat).ToArray()
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileHuaTuo = r });
        if (op is SkillProgramEffectOp.DepositMobileShenHuaTuoYouyi or SkillProgramEffectOp.RecoverMobileShenHuaTuoYouyi)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            foreach (var id in r.Cards)
            {
                var from = _cardZonesOperations._cardZones.GetLocation(id);
                var expected = op == SkillProgramEffectOp.DepositMobileShenHuaTuoYouyi ? CardLocation.DiscardPile : CardLocation.Benevolence;
                if (from != expected)
                    throw new InvalidOperationException("Youyi lost a frozen payable shared-area entity.");
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), from, op == SkillProgramEffectOp.DepositMobileShenHuaTuoYouyi ? CardLocation.Benevolence : CardLocation.DiscardPile, new(op == SkillProgramEffectOp.DepositMobileShenHuaTuoYouyi ? "skill.mobile-shen-hua-tuo.youyi.deposit" : "skill.mobile-shen-hua-tuo.youyi.pay"));
            }

            if (op == SkillProgramEffectOp.DepositMobileShenHuaTuoYouyi)
                while (_cardZonesOperations._cardZones.CardsAt(CardLocation.Benevolence).Count > 6)
                    _corePhysicalMovementFacadesOperations.MoveCard(_cardZonesOperations._cardZones.CardsAt(CardLocation.Benevolence)[0], CardLocation.Benevolence, CardLocation.DiscardPile, new("benevolence.overflow"));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { MobileHuaTuo = r with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileHuaTuoPaidEvent(f.Id, 0, op == SkillProgramEffectOp.DepositMobileShenHuaTuoYouyi ? "deposit" : "ren-payment", f.OwnerSeat, r.Before, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
        }

        ContinueMobileHt(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice MobileHtChoice(ProgramSkillFrame f, string option, string label, int chooser, int? target = null, int? card = null) => new(new($"mobile-hua-tuo.{(f.Id)}.{(f.MobileHuaTuo!.Order.Count)}.{(option)}"), label, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "mobile-hua-tuo", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = option, ["chooser"] = chooser.ToString(CultureInfo.InvariantCulture) });
    internal IReadOnlyList<PromptChoice> MobileHtChoices(ProgramSkillFrame f)
    {
        var r = f.MobileHuaTuo!;
        if (r.Stage == MobileHuaTuoStage.Recipient)
            return MobileHtTeachingTargets().Select(s => MobileHtChoice(f, "recipient:" + s, "向" + _coreMatchParticipantsPlayersAccess._players[s].Name + "传授五禽戏", f.OwnerSeat, target: s)).ToArray();
        if (r.Stage == MobileHuaTuoStage.Order)
            return Enum.GetValues<MobileWulingAnimal>().Except(r.Order).Select(a => MobileHtChoice(f, "animal:" + (int)a, $"第{(r.Order.Count + 1)}种：{(MobileHtAnimalName(a))}", f.OwnerSeat)).ToArray();
        if (r.Stage == MobileHuaTuoStage.Ape)
            return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != r.TargetSeat).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(r.TargetSeat, p.Seat)).SelectMany(p => _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(r.TargetSeat, c, CardLocation.Equipment(p.Seat), OwnedCardMoveIntent.Obtain)).Select(c => MobileHtChoice(f, $"ape:{(p.Seat)}:{(c.Id)}", "获得" + p.Name + "的【" + c.DisplayName + "】", r.TargetSeat, p.Seat, c.Id))).ToArray();
        return[];
    }

    internal static string MobileHtAnimalName(MobileWulingAnimal animal) => animal switch
    {
        MobileWulingAnimal.Tiger => "虎灵",
        MobileWulingAnimal.Crane => "鹤灵",
        MobileWulingAnimal.Bear => "熊灵",
        MobileWulingAnimal.Ape => "猿灵",
        _ => "鹿灵"
    };
    internal void PublishMobileHt(ProgramSkillFrame f)
    {
        var choices = MobileHtChoices(f);
        if (choices.Count == 0)
        {
            FinishMobileHt(f);
            return;
        }

        var r = f.MobileHuaTuo!;
        var chooser = r.Stage == MobileHuaTuoStage.Ape ? r.TargetSeat : f.OwnerSeat;
        var s = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == MobileHuaTuoStage.Order ? "请选择五禽戏的先后顺序。" : "请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, s.Name, s.Name, s.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mobile-hua-tuo")]
    internal void AnswerMobileHt(PromptChoice choice)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MobileHuaTuo ?? throw new InvalidOperationException("Mobile Hua Tuo lost its owning choice.");
        if (!CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], MobileHtChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Wuling requires the exact published order or public equipment entity.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == MobileHuaTuoStage.Recipient)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileHuaTuo = r with { TargetSeat = choice.Targets.Single(), Stage = MobileHuaTuoStage.Order } });
        else if (r.Stage == MobileHuaTuoStage.Order)
        {
            var animal = (MobileWulingAnimal)int.Parse(choice.Parameters["option"].Split(':')[1], CultureInfo.InvariantCulture);
            r = r with
            {
                Order = r.Order.Append(animal).ToArray()
            };
            if (r.Order.Count == 5)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || MobileHtCourse(r.TargetSeat)is not null)
                {
                    FinishMobileHt(f);
                    return;
                }

                _characterHuaTuoMobileGodMobileWulingCoursesAccess._mobileWulingCourses.Add(f.Id, new(r.Source, r.TargetSeat) { Order = r.Order });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileWulingTaughtEvent(f.Id, r.Source, r.TargetSeat));
                r = r with
                {
                    Stage = MobileHuaTuoStage.Entry
                };
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileHuaTuo = r });
        }
        else
        {
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single());
            var from = CardLocation.Equipment(choice.Targets.Single());
            var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileHuaTuo = r with { Stage = MobileHuaTuoStage.Children, Before = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(card, from, card.IsGeneralWeapon ? CardLocation.OutsideGame : CardLocation.Hand(r.TargetSeat), new("skill.mobile-shen-hua-tuo.wuling.ape"));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { MobileHuaTuo = f.MobileHuaTuo!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileHuaTuoPaidEvent(f.Id, 0, "ape", r.TargetSeat, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
        }

        ContinueMobileHt(f.Id);
    }

    internal bool DrainMobileHt(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMobileHt(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { MobileHuaTuo: { } r } f && f.Id == id)
        {
            if (r.Stage == MobileHuaTuoStage.Drawing)
                return;
            if (r.Stage == MobileHuaTuoStage.Entry)
            {
                if (r.AnimalIndex >= r.Order.Count)
                {
                    EndMobileHtCourse(r.CourseId, f.Id);
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileHuaTuo = r with { Stage = MobileHuaTuoStage.Children } });
                    continue;
                }

                ApplyMobileHtAnimal(f);
                continue;
            }

            if (r.Stage == MobileHuaTuoStage.EntryChildren)
            {
                if (DrainMobileHt(id))
                    return;
                BeginMobileHtEntryBenefit(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                continue;
            }

            if (r.Stage == MobileHuaTuoStage.DeerRecoveryChildren)
            {
                if (DrainMobileHt(id))
                    return;
                ClearMobileHtDeerJudgments(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                continue;
            }

            if (r.Stage == MobileHuaTuoStage.Children)
            {
                if (DrainMobileHt(id))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.MobileHuaTuo!;
                if (r.Op == SkillProgramEffectOp.RecoverMobileShenHuaTuoYouyi)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileHuaTuo = r with { Stage = MobileHuaTuoStage.RecoverAll } });
                    continue;
                }

                FinishMobileHt(f);
                return;
            }

            if (r.Stage == MobileHuaTuoStage.RecoverAll)
            {
                if (r.Cursor >= r.Recipients.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
                {
                    FinishMobileHt(f);
                    return;
                }

                var seat = r.Recipients[r.Cursor];
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileHuaTuo = r with { Cursor = r.Cursor + 1, Stage = MobileHuaTuoStage.Children } });
                MobileHtRecoverOne(f, seat);
                continue;
            }

            PublishMobileHt(f);
            return;
        }
    }

    internal void MobileHtRecoverOne(ProgramSkillFrame f, int seat)
    {
        var p = _coreMatchParticipantsPlayersAccess._players[seat];
        if (!p.IsAlive || !p.HasHpValue || p.Hp >= p.MaxHp)
            return;
        if (!_coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(f.Id, f.OwnerSeat, seat, 1, new(RecoveryAttemptProducer.Program, f.InstructionIndex)))
        {
            var recovery = _coreNativeRecoveryFramesOperations.BeginRecovery(f.Id, f.OwnerSeat, seat, 1);
            p.Hp++;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(f.OwnerSeat, seat, 1, p.Hp));
            _coreRuntimeOperations.PopResolutionFrame(recovery, ResolutionFrameKind.Recovery);
        }
    }

    internal void FinishMobileHt(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileHuaTuo = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 39100)]
    internal bool ResumeMobileHt(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileHuaTuo: not null } f || f.Id != id)
            return false;
        ContinueMobileHt(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 39100)]
    internal bool ReturnMobileHtMovement(ProgramSkillFrame f) => f.MobileHuaTuo is not null && ResumeMobileHt(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 39100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 39100)]
    internal bool ContinueIssuedMobileHt(ProgramSkillFrame f) => f.MobileHuaTuo is not null;
    [ProgramAiChoice(SkillProgramEffectOp.TeachMobileShenHuaTuoWuling)]
    internal PromptChoice ChooseTeachMobileHt(PendingDecision p, ProgramSkillFrame f) => p.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.AdvanceMobileShenHuaTuoWuling)]
    internal PromptChoice ChooseAdvanceMobileHt(PendingDecision p, ProgramSkillFrame f) => p.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.DepositMobileShenHuaTuoYouyi)]
    internal PromptChoice ChooseDepositMobileHt(PendingDecision p, ProgramSkillFrame f) => p.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.RecoverMobileShenHuaTuoYouyi)]
    internal PromptChoice ChooseRecoverMobileHt(PendingDecision p, ProgramSkillFrame f) => p.Choices[0];
}
