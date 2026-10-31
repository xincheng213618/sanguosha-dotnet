#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuCheErAnniversaryRuntime
{
    internal const string HuCheErAction = "anniversary-hu-che-er", HuCheErClaimReason = "skill-program.anniversary:daoji.obtain";
    internal bool HasAnniversaryHuCheErRules => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryHuCheErDaoji) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RecordAnniversaryHuCheErFuzhongGain);

    internal static CardConversionSource HuCheErSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static bool HuCheErWeapon(CardKind kind) => EquipmentCatalog.IsEquipment(kind) && EquipmentCatalog.Get(kind).Slot == EquipmentSlot.Weapon;
    internal CardUseFrame? HuCheErFirstWeapon(int owner, ProgramSkillWindowContext c)
    {
        if (c.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || c.CardUse is not { } card || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } parent || parent.ParentFrameId != card.ParentCardUseFrameId || _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is not { Action: { Type: CardActionType.Use } a } use || parent.Action.ActionId != a.ActionId || card.CardActionId != a.ActionId || card.ActorSeat != a.ActorSeat || owner == a.ActorSeat || !HuCheErWeapon(a.EffectiveKind) || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().FirstOrDefault(e => e.SourceSeat == a.ActorSeat && HuCheErWeapon(e.CardKind))is not { } first || first.ResolutionId != use.Id)
            return null;
        return use;
    }

    internal bool HuCheErLogicalGainBatch(int owner, CardMovementBatchContext batch)
    {
        // DrawCards may emit one physical MoveCard batch per entity. Its native
        // invocation invoice joins those movements into one actual acquisition;
        // only its validated final batch may award the one heavy mark.
        var draws = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawInvocationRecordedEvent>().Where(e => e.OwnerSeat == owner && e.TurnNumber == batch.TurnNumber && e.ParentFrameId == batch.ParentFrameId && batch.Movements.Any(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(owner) && m.Reason == e.Reason && m.Sequence > e.SequenceBefore && m.Sequence <= e.SequenceAfter)).ToArray();
        if (draws.Length == 0)
            return true;
        return draws is [var draw] && draw.LastBatchId == batch.Id && batch.NativeDrawInvocation is { } proof && proof.InvocationId == draw.InvocationId && proof.OwnerSeat == owner && proof.LastBatchId == batch.Id && proof.ParentFrameId == batch.ParentFrameId && proof.ActualTurnNumber == batch.TurnNumber && _coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(proof);
    }

    internal bool HuCheErGain(int owner, ProgramSkillWindowContext c) => c is { Window: SkillProgramTriggerWindow.CardsGained, MovementBatch: { MovementTiming: { } timing } batch } && batch.TurnNumber > 0 && batch.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && (timing.ActualTurnOwnerSeat != owner || !_coreActualHandGainProgramsOperations.ActualHandGainTurnOpen(owner)) && _coreActualHandGainProgramsOperations.MatchingActualHandGainIndexes(batch, owner, CardLocation.Hand(owner)).Length > 0 && HuCheErLogicalGainBatch(owner, batch) && !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHuCheErGainRecordedEvent>().Any(e => e.Source.OwnerSeat == owner && e.BatchId == batch.Id);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 49500)]
    internal bool CanOfferHuCheEr(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => c.SkillId switch
    {
        AnniversaryHuCheErComposition.Daoji => HuCheErFirstWeapon(c.OwnerSeat, w)is not null,
        AnniversaryHuCheErComposition.Fuzhong when t.Id == "foreign-turn-gain" => HuCheErGain(c.OwnerSeat, w),
        _ => true
    };
    SkillProgramStepOutcome IAnniversaryHuCheErHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveAnniversaryHuCheErDaoji => BeginHuCheEr(f),
        SkillProgramEffectOp.RecordAnniversaryHuCheErFuzhongGain => RecordHuCheErGain(f),
        SkillProgramEffectOp.SpendAnniversaryHuCheErFuzhong => SpendHuCheErFour(f),
        _ => throw new InvalidOperationException("Unknown Hu Che Er operation.")};
    internal SkillProgramStepOutcome BeginHuCheEr(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.AnniversaryHuCheEr is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } c || HuCheErFirstWeapon(f.OwnerSeat, c)is not { Action: { } a } use || !HuCheErParent(f))
            throw new InvalidOperationException("Daoji requires the exact first foreign actual Weapon Use and original candidate.");
        var r = new AnniversaryHuCheErReceipt(HuCheErSource(f), f.GameplayHash, c.ParentFrameId, use.Id, a.ActionId, a.ActorSeat, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, AnniversaryHuCheErStage.Choice);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHuCheEr = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHuCheErStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.UseId, r.ActionId, r.Actor, r.Turn, r.TurnOwner));
        PublishHuCheEr(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int[] HuCheErMaterials(ProgramSkillFrame f) => HuCheErFirstWeapon(f.OwnerSeat, f.WindowContext!)is { Action: { } a } use ? a.PhysicalCards.Select(c => c.CardId).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing && !_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(use.Id, id)).ToArray() : [];
    internal IReadOnlyList<PromptChoice> HuCheErChoices(ProgramSkillFrame f)
    {
        if (f.AnniversaryHuCheEr is not { Stage: AnniversaryHuCheErStage.Choice } r)
            return[];
        PromptChoice C(string branch, string text) => new(new($"hu-che-er.{(f.Id)}.{(branch)}"), text, [], [], new Dictionary<string, string> { ["program-action"] = HuCheErAction, ["branch"] = branch });
        var result = new List<PromptChoice>();
        if (HuCheErMaterials(f).Length > 0)
            result.Add(C("obtain", "立即获得此武器牌"));
        if (_coreMatchParticipantsPlayersAccess._players[r.Actor].IsAlive)
            result.Add(C("forbid", "其本回合不能使用或打出杀"));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishHuCheEr(ProgramSkillFrame f)
    {
        var choices = HuCheErChoices(f);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || choices.Count == 0)
        {
            FinishHuCheEr(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "盗戟：获得原武器牌，或令其本回合不能使用或打出杀。", [], [], f.OwnerSeat)
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

    [ProgramAnswer(HuCheErAction)]
    internal void AnswerHuCheEr(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryHuCheEr: { Stage: AnniversaryHuCheErStage.Choice } r } f || !ValidHuCheEr(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !HuCheErChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Daoji choice changed its owner, native Use or available original entity.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (choice.Parameters["branch"] == "forbid")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHuCheErSlashForbiddenEvent(f.Id, r.Source, r.Hash, r.UseId, r.ActionId, r.Actor, r.Turn, r.TurnOwner));
            FinishHuCheEr(f);
            return;
        }

        var ids = HuCheErMaterials(f);
        if (ids.Length == 0)
            throw new InvalidOperationException("Daoji cannot replace the paid original Weapon materials.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHuCheEr = r with { Stage = AnniversaryHuCheErStage.MovementChildren, Materials = ids, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new(HuCheErClaimReason), afterRequestSettled: settlement =>
        {
            _coreHappyWeiYanGainDestinationsOperations.ValidateHappyWeiYanGainRequestSettlement(settlement, f.Id, ids, CardLocation.Hand(f.OwnerSeat), before);
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { AnniversaryHuCheEr = current.AnniversaryHuCheEr!with { Gain = settlement, Moves = settlement.Movements, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHuCheErWeaponClaimedEvent(f.Id, r.UseId, r.ActionId, f.OwnerSeat, ids.Length, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }, producerFrameId: f.Id);
        ContinueHuCheEr(f.Id);
    }

    internal bool DrainHuCheEr(ProgramSkillFrame f) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueHuCheEr(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (!ValidHuCheEr(f))
            throw new InvalidOperationException("Daoji lost its original paid Weapon gain return.");
        if (f.AnniversaryHuCheEr!.Stage == AnniversaryHuCheErStage.MovementChildren)
        {
            if (DrainHuCheEr(f))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            FinishHuCheEr(f);
            return;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            FinishHuCheEr(f);
            return;
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishHuCheEr(f);
    }

    internal void FinishHuCheEr(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHuCheErCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHuCheEr = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 49500)]
    internal bool ResumeHuCheEr(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryHuCheEr: not null } f || f.Id != id)
            return false;
        ContinueHuCheEr(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 49500)]
    internal bool ReturnHuCheErMovement(ProgramSkillFrame f)
    {
        if (f.AnniversaryHuCheEr is null)
            return false;
        if (!HuCheErMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), f.PendingMovementContinuation!))
            throw new InvalidOperationException("Daoji cannot consume a foreign movement return.");
        ContinueHuCheEr(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryHuCheErDaoji)]
    internal PromptChoice AiHuCheEr(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
