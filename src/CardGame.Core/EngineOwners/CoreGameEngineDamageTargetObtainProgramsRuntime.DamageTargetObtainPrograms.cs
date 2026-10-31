#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDamageTargetObtainProgramsRuntime
{
    internal const string DamageTargetObtainReason = "skill-program.damage-target-obtain.obtain";
    internal const string DamageTargetBenefitReason = "skill-program.damage-target-obtain.benefit-draw";
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1120, SkillProgramEffectOp.ObtainDamageTargetCardAndResolveCategory)]
    internal bool CanOfferDamageTargetObtain(int owner, ProgramSkillWindowContext c) => c.TargetSeat is { } victim && victim != owner && _coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[victim].IsAlive && _coreDamageTargetMountProgramsOperations.ActualDamageTargetMountUse(owner, c)is not null && CoreGameEngineDamageTargetMountProgramsDefinitions.DamageTargetMountZones.Any(z => _cardZonesOperations._cardZones.CardsAt(new(z, victim)).Count > 0);
    internal IReadOnlyList<PromptChoice> DamageTargetObtainChoices(ProgramSkillFrame f) => _coreDamageTargetRegionObtainOperations.UsesDamageTargetRegionObtain(f) ? _coreDamageTargetRegionObtainOperations.DamageTargetRegionObtainChoices(f) : _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.DamageTargetObtain!.VictimSeat, CoreGameEngineDamageTargetMountProgramsDefinitions.DamageTargetMountZones, OwnedCardMoveIntent.Obtain).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "damage-target-obtain" } }).ToArray();
    internal SkillProgramStepOutcome BeginDamageTargetObtain(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var c = f.WindowContext ?? throw new InvalidOperationException("Obtain lost its damage context.");
        if (!CanOfferDamageTargetObtain(f.OwnerSeat, c))
            return SkillProgramStepOutcome.Continue;
        var use = _coreDamageTargetMountProgramsOperations.ActualDamageTargetMountUse(f.OwnerSeat, c)!;
        if (f.DamageTargetObtain is not null)
            throw new InvalidOperationException("Obtain cannot pay twice.");
        f = f with
        {
            DamageTargetObtain = new(f.InstructionIndex, f.OwnerSeat, c.TargetSeat!.Value, c.ParentFrameId, c.DamageFrameId!.Value, use.Id, use.Action!.ActionId, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f);
        var choices = DamageTargetObtainChoices(f);
        PublishDamageTargetObtainPrompt(f, f.OwnerSeat, _coreDamageTargetRegionObtainOperations.UsesDamageTargetRegionObtain(f) ? "请选择伤害目标的一个区域，获得其中全部牌。" : "请选择伤害目标区域里的一张牌获得。", choices, true);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishDamageTargetObtainPrompt(ProgramSkillFrame f, int chooser, string text, IReadOnlyList<PromptChoice> choices, bool privateCards)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, chooser, text, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = privateCards,
            TargetSeat = f.DamageTargetObtain!.VictimSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray())
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("damage-target-obtain", "damage-target-obtain-region", "damage-target-duel")]
    internal void ResolveDamageTargetObtainChoice(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Obtain choice lost its parent.");
        var d = f.DamageTargetObtain ?? throw new InvalidOperationException("Obtain choice lost its draft.");
        var duel = d.Stage == ProgramDamageTargetObtainStage.SelectingDuelTarget;
        var choices = duel ? DamageTargetDuelChoices(f) : DamageTargetObtainChoices(f);
        if (!IsValidDamageTargetObtain(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (duel ? d.VictimSeat : d.OwnerSeat) || !choices.Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) && c.Targets.SequenceEqual(selected.Targets) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(selected.Parameters.OrderBy(x => x.Key))))
            throw new InvalidOperationException("Obtain choice must name its exact current published slot or legal target.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (duel)
        {
            IssueDamageTargetDuel(f, selected.Targets.Single());
            return;
        }

        if (d.Stage != ProgramDamageTargetObtainStage.SelectingCard || d.Receipt is not null)
            throw new InvalidOperationException("Obtain payment was already resolved.");
        if (!_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !CanOfferDamageTargetObtain(f.OwnerSeat, f.WindowContext!))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "获得前技能来源或参与者已失效。");
            return;
        }

        if (_coreDamageTargetRegionObtainOperations.UsesDamageTargetRegionObtain(f))
        {
            _coreDamageTargetRegionObtainOperations.IssueDamageTargetRegionObtain(f, selected);
            return;
        }

        var from = new CardLocation(Enum.Parse<CardZoneKind>(selected.Parameters["source-zone"]), d.VictimSeat);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(selected.Parameters["slot-index"], CultureInfo.InvariantCulture)];
        var to = card.IsGeneralWeapon && from.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.Hand(f.OwnerSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DamageTargetObtain = d with { Stage = ProgramDamageTargetObtainStage.ObtainChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, to, new(DamageTargetObtainReason), record =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { DamageTargetObtain = current.DamageTargetObtain!with { ObtainMovementSequence = record.Sequence, Receipt = record.To == CardLocation.Hand(d.OwnerSeat) || _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(record, CardLocation.Hand(d.OwnerSeat)) ? new(record.Sequence, record.CardId, record.CardKind, record.From, record.To, record.Reason.Value, _coreEquipmentContentOperations.EquipmentRules.IsEquipment(record.CardKind)) : null } });
        });
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(f.Id);
    }

    internal bool IsValidDamageTargetObtain(ProgramSkillFrame f)
    {
        if (f.DamageTargetObtain is not { } d || f.TriggerId is null || f.InstructionIndex != 1 || d.InstructionIndex != f.InstructionIndex || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } c || d.OwnerSeat != f.OwnerSeat || d.VictimSeat == d.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(d.VictimSeat) || c.TargetSeat != d.VictimSeat || c.SourceSeat != d.OwnerSeat || c.ParentFrameId != d.DamageWindowId || c.DamageFrameId != d.DamageFrameId || d.SkillId != f.SkillId || d.BindingId != CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) || d.SkillInstanceId != f.SkillInstanceId || d.GameplayHash != f.GameplayHash || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions is not [{ Op: SkillProgramEffectOp.ObtainDamageTargetCardAndResolveCategory }] || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == d.DamageWindowId)is not { } window || window.ParentFrameId != d.DamageFrameId || window.SourceSeat != d.OwnerSeat || window.TargetSeat != d.VictimSeat || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(x => x.Id == d.DamageFrameId)is not { } damage || damage.ParentFrameId != d.OuterCardUseFrameId || damage.SourceSeat != d.OwnerSeat || damage.TargetSeat != d.VictimSeat || damage.Amount <= 0 || _coreCardUseLifecycleOperations.LifecycleCardUse(d.OuterCardUseFrameId)?.Action is not { Type: CardActionType.Use } a || a.ActionId != d.OuterActionId || a.ActorSeat != d.OwnerSeat || !CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind))
            return false;
        if (_coreDamageTargetRegionObtainOperations.UsesDamageTargetRegionObtain(f))
            return _coreDamageTargetRegionObtainOperations.IsValidDamageTargetRegionObtain(f, d);
        if (d.RegionObtain is not null)
            return false;
        if (d.Stage == ProgramDamageTargetObtainStage.SelectingCard)
            return d.Receipt is null && d.ObtainMovementSequence is null;
        if (d.ObtainMovementSequence is not { } sequence || _coreCardMovementJournalCardMovementsAccess._cardMovements.SingleOrDefault(m => m.Sequence == sequence)is not { } movement || movement.From.OwnerSeat != d.VictimSeat || !CoreGameEngineDamageTargetMountProgramsDefinitions.DamageTargetMountZones.Contains(movement.From.Zone) || movement.Reason.Value != DamageTargetObtainReason)
            return false;
        if (d.Receipt is not { } r)
            return (d.Stage is ProgramDamageTargetObtainStage.ObtainChildren or ProgramDamageTargetObtainStage.Complete) && movement.To == CardLocation.OutsideGame && (movement.CardKind == CardKind.GeneralWeapon && movement.From.Zone == CardZoneKind.Equipment || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(movement));
        if (r.MovementSequence != sequence)
            return false;
        return d.Stage != ProgramDamageTargetObtainStage.SelectingCard && r.Reason == DamageTargetObtainReason && r.From.OwnerSeat == d.VictimSeat && CoreGameEngineDamageTargetMountProgramsDefinitions.DamageTargetMountZones.Contains(r.From.Zone) && (r.To == CardLocation.Hand(d.OwnerSeat) || _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(movement, CardLocation.Hand(d.OwnerSeat))) && r.IsEquipment == _coreEquipmentContentOperations.EquipmentRules.IsEquipment(r.PrintedKind) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == r.MovementSequence && m.CardId == r.CardId && m.CardKind == r.PrintedKind && m.From == r.From && m.To == r.To && m.Reason.Value == r.Reason);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 830)]
    internal bool ResumeDamageTargetObtain(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.DamageTargetObtain is not { } d || d.Stage is ProgramDamageTargetObtainStage.SelectingCard or ProgramDamageTargetObtainStage.SelectingDuelTarget or ProgramDamageTargetObtainStage.DuelIssued or ProgramDamageTargetObtainStage.Complete)
            return false;
        if (!IsValidDamageTargetObtain(f))
            throw new InvalidOperationException("Obtain tail lost its exact actual receipt.");
        if (f.PendingMovementContinuation is not null)
        {
            if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        }

        if (d.Stage == ProgramDamageTargetObtainStage.DrawChildren || CoreGameEngineDamageTargetRegionObtainRuntime.DamageTargetObtainedCount(d) == 0 || !_coreMatchParticipantsPlayersAccess._players[d.VictimSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishDamageTargetObtain(f);
            return true;
        }

        if (!CoreGameEngineDamageTargetRegionObtainRuntime.DamageTargetObtainedEquipment(d))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { DamageTargetObtain = d with { Stage = ProgramDamageTargetObtainStage.DrawChildren }, PendingMovementContinuation = new(d.OwnerSeat, 0, null) });
            _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[d.VictimSeat], 1, log: true, reason: new(DamageTargetBenefitReason));
            if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                _coreRuntimeOperations.ReturnRuntimeProgramMovement(id);
            return true;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[d.OwnerSeat].IsAlive)
        {
            FinishDamageTargetObtain(f);
            return true;
        }

        f = f with
        {
            DamageTargetObtain = d with
            {
                Stage = ProgramDamageTargetObtainStage.SelectingDuelTarget
            }
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f);
        var choices = DamageTargetDuelChoices(f);
        if (choices.Count == 0)
        {
            FinishDamageTargetObtain(f);
            return true;
        }

        PublishDamageTargetObtainPrompt(f, d.VictimSeat, "利驭：请指定吕布使用【决斗】的另一名角色。", choices, false);
        return true;
    }

    internal void FinishDamageTargetObtain(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DamageTargetObtain = f.DamageTargetObtain!with { Stage = ProgramDamageTargetObtainStage.Complete }, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool CanIssueDamageTargetDuel(ProgramSkillFrame f, int target)
    {
        var d = f.DamageTargetObtain!;
        return _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[d.VictimSeat].IsAlive && target != d.VictimSeat && target != d.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreMatchParticipantsPlayersAccess._players[d.OwnerSeat].IsAlive && !_coreCardUseModulesOperations.IsCardUseForbidden(d.OwnerSeat, CardKind.Duel, CardActionType.Use) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(d.OwnerSeat, SkillProgramCardTargetRestriction.SelfOnly) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], CardKind.Duel, Suit.None) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(d.OwnerSeat, target, CardKind.Duel);
    }

    internal IReadOnlyList<PromptChoice> DamageTargetDuelChoices(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players.Where(p => CanIssueDamageTargetDuel(f, p.Seat)).Select(p => new PromptChoice(new ChoiceId($"damage-target-duel.frame-{(f.Id)}.seat-{(p.Seat)}"), $"指定 {(p.Name)}。", [], [p.Seat], new Dictionary<string, string> { ["program-action"] = "damage-target-duel", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray();
    internal void IssueDamageTargetDuel(ProgramSkillFrame f, int target)
    {
        var d = f.DamageTargetObtain!;
        if (d.Stage != ProgramDamageTargetObtainStage.SelectingDuelTarget || !CoreGameEngineDamageTargetRegionObtainRuntime.DamageTargetObtainedEquipment(d) || !IsValidDamageTargetObtain(f) || !CanIssueDamageTargetDuel(f, target))
            throw new InvalidOperationException("Duel issuance lost its paid exact legal producer.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { DamageTargetObtain = d with { Stage = ProgramDamageTargetObtainStage.DuelIssued } });
        // Suspend the actual outer attack on its own owner. No duplicate pending attack or use-ID sidecar.
        _coreCardAttackStateOperations.UpdateCardAttackState(d.OuterCardUseFrameId, s => s!with { Active = false });
        var origin = new ProgramDamageTargetDuelOrigin(f.Id, f.InstructionIndex, CoreGameEngineDamageTargetRegionObtainRuntime.DamageTargetObtainAnchor(d), d.OuterCardUseFrameId, d.DamageWindowId, d.DamageFrameId, d.OuterActionId, d.OwnerSeat, d.VictimSeat, target, f.SkillId, d.BindingId, f.SkillInstanceId, f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber)
        {
            BaseDamage = d.RegionObtain is not null ? CoreGameEngineDamageTargetRegionObtainRuntime.DamageTargetObtainedCount(d) : null
        };
        var card = new Card(0, CardKind.Duel, Suit.None, 0);
        var use = _coreNativeCardUseOperations.BeginCardUse(card, d.OwnerSeat, [target], CardKind.Duel, physicalCardIds: [], damageTargetDuelOrigin: origin);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(use, card, d.OwnerSeat, [target], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
    }

    internal bool IsDamageTargetDuelUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { DamageTargetDuelOrigin: not null, SelectedActorDuelOrigin: null, CardId: 0, CardKind: CardKind.Duel, PhysicalCardIds.Count: 0 };
    internal bool IsIssuedZeroEntityDuel(long id) => _characterDongChengMouOperations.IsMouDcZeroUse(id) && _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind == CardKind.Duel || _coreDamageColorDuelProgramsOperations.IsSelectedActorDuelUse(id) || IsDamageTargetDuelUse(id) || _coreDualColorDuelProgramsOperations.Value.IsDualColorDuelUse(id) || _coreConditionalDiscardDuelReturnsOperations.Value.IsConditionalDiscardDuelUse(id) || _characterHuangQuanMobileShiOperations.IsShiQuanDuelUse(id) || _coreFriendEventsOperations.IsFriendRecastDuelUse(id) || _characterHaoPuOlOperations.IsZhenyingDuelUse(id) || _characterHeJinOlOperations.IsHeJinMouzhuDuelUse(id) || _characterWuAnGuoOlOperations.IsWuAnGuoLiyongDuelUse(id) || _characterFanJiangZhangDaOlOperations.IsJueshengDuelUse(id) || _characterXuHuangMouMobileOperations.IsMouXuHuangZeroUse(id) || _characterWenChouMouOlOperations.IsMouWenChouZeroDuel(id);

    [NativeUseReturn(230)]
    internal void ReturnDamageTargetDuel(CardUseFrame use)
    {
        if (use.DamageTargetDuelOrigin is not { AttackStarted: false } o || CharacterLuBuMouMobileRuntime.IsMouLbDuelOrigin(o))
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != o.ParentProgramFrameId || f.DamageTargetObtain?.Stage != ProgramDamageTargetObtainStage.DuelIssued || !MatchesDamageTargetDuelParent(f, o))
            throw new InvalidOperationException("Nullified Duel lost its typed paid parent.");
        CompleteDamageTargetDuelReturn(f);
    }

    internal bool MatchesDamageTargetDuelParent(ProgramSkillFrame f, ProgramDamageTargetDuelOrigin o) => _characterLuBuMouMobileOperations.Value.MatchesMouLbDuelParent(f, o) || IsValidDamageTargetObtain(f) && f.DamageTargetObtain is { Stage: ProgramDamageTargetObtainStage.DuelIssued } d && CoreGameEngineDamageTargetRegionObtainRuntime.DamageTargetObtainedEquipment(d) && o.ParentProgramFrameId == f.Id && o.InstructionIndex == f.InstructionIndex && o.ReceiptSequence == CoreGameEngineDamageTargetRegionObtainRuntime.DamageTargetObtainAnchor(d) && _coreDamageTargetRegionObtainOperations.DamageTargetDuelBaseMatches(d, o) && o.OuterCardUseFrameId == d.OuterCardUseFrameId && o.OuterActionId == d.OuterActionId && o.DamageWindowId == d.DamageWindowId && o.DamageFrameId == d.DamageFrameId && o.InitialActorSeat == d.OwnerSeat && o.VictimChooserSeat == d.VictimSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(o.InitialTargetSeat) && o.InitialTargetSeat != d.OwnerSeat && o.InitialTargetSeat != d.VictimSeat && o.SkillId == f.SkillId && o.BindingId == d.BindingId && o.SkillInstanceId == f.SkillInstanceId && o.GameplayHash == f.GameplayHash && o.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber;
    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 100)]
    internal bool CompleteIssuedDamageTargetDuelAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.ProgramFrameId is not { } parentId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != parentId || frame.DamageTargetObtain is not { Stage: ProgramDamageTargetObtainStage.DuelIssued })
            return false;
        if (completion.DamageTargetDuelReturn is not { AttackStarted: true } origin || !MatchesDamageTargetDuelParent(frame, origin))
            throw new InvalidOperationException("True Duel completion lost its exact typed receipt return.");
        CompleteDamageTargetDuelReturn(frame);
        return true;
    }

    internal void CompleteDamageTargetDuelReturn(ProgramSkillFrame f)
    {
        if (!IsValidDamageTargetObtain(f) || f.DamageTargetObtain is not { Stage: ProgramDamageTargetObtainStage.DuelIssued } d || _coreCardUseLifecycleOperations.LifecycleCardUse(d.OuterCardUseFrameId)?.CardAttack is not { Active: false })
            throw new InvalidOperationException("Completed Duel lost its suspended outer attack.");
        _coreCardAttackStateOperations.UpdateCardAttackState(d.OuterCardUseFrameId, s => s!with { Active = true });
        FinishDamageTargetObtain(f);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ObtainDamageTargetCardAndResolveCategory)]
    internal PromptChoice SelectAiDamageTargetObtain(PendingDecision decision, ProgramSkillFrame f)
    {
        if (f.DamageTargetObtain?.Stage != ProgramDamageTargetObtainStage.SelectingDuelTarget)
            return _coreDamageTargetRegionObtainOperations.UsesDamageTargetRegionObtain(f) ? _coreDamageTargetRegionObtainOperations.SelectAiDamageTargetRegion(decision, f) : _coreProgramOtherOwnedCardDiscardsOperations.SelectAiProgramOtherOwnedCardDiscard(decision, f);
        // The victim is choosing a harmful target; use that chooser's filtered public relationship model.
        var draft = f.DamageTargetObtain!;
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
        var hint = new SkillProgramAiHint(0, 0, 0, 0, 0, draft.RegionObtain?.ActualObtainedCount ?? 1, false, false);
        if (draft.RegionObtain is { } region)
            hint = hint with
            {
                TargetDamage = region.ActualObtainedCount,
                TargetHealthEffects = [new(PublicHealthEffectKind.Damage, region.ActualObtainedCount)]
            };
        return decision.Choices.OrderByDescending(c => _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ScoreProgramTarget(view, c.Targets.Single(), hint)).ThenBy(c => c.Targets.Single()).First();
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 660)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 810)]
    internal bool CanContinuePaidDamageTargetObtain(ProgramSkillFrame f) => IsValidDamageTargetObtain(f) && (f.DamageTargetObtain is { RegionObtain.Paid: true } || f.DamageTargetObtain is { Receipt: not null } && (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || f.DamageTargetObtain is { Receipt.IsEquipment: false, Stage: ProgramDamageTargetObtainStage.DrawChildren or ProgramDamageTargetObtainStage.Complete }));
    SkillProgramStepOutcome IDamageTargetObtainProgramHost.ObtainDamageTargetCardAndResolveCategory(ProgramSkillFrame f) => BeginDamageTargetObtain(f);
}
