#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangGuanOlRuntime
{
    internal const string MiuyanGainReason = "skill-program.wang-guan.miuyan.gain", ShiluDrawReason = "skill-program.wang-guan.shilu.draw", ShiluRevealBind = "shilu-public-hand";
    internal static CardConversionSource WangGuanSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_wang_guan_ol_WangGuan.WangGuanSource(f);
    SkillProgramStepOutcome IWangGuanHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginWangGuan(f, op);
    internal SkillProgramStepOutcome BeginWangGuan(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.WangGuan is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context)
            throw new InvalidOperationException("Wang Guan requires an unissued exact native trigger instruction.");
        WangGuanComposition.ValidateProgram(f.SkillId, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        var miuyan = op == SkillProgramEffectOp.ResolveMiuyanCompletedUse;
        MiuyanUseInvoice? invoice = null;
        long native;
        var hp = 0;
        if (miuyan)
        {
            var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority);
            if (!CanRunIssuedMiuyanCandidate(c, context))
                throw new InvalidOperationException("Miuyan lost its exact completed native Fire Attack candidate.");
            invoice = MiuyanCandidateUse(c, context)!.MiuyanUse!;
            native = invoice.UseId;
        }
        else
        {
            if (op != SkillProgramEffectOp.ResolveShiluDrawRevealIdentity || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || context is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, DamageFrameId: { } damage } || context.TargetSeat != f.OwnerSeat || !_resolutionStackOperations._resolutionStack.OfType<DamageFrame>().Any(d => d.Id == damage && d.TargetSeat == f.OwnerSeat && d.Amount == context.Amount))
                throw new InvalidOperationException("Shilu requires a real positive whole damage to its living owner.");
            native = damage;
            hp = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp);
        }

        var r = new WangGuanReceipt(WangGuanSource(f), f.GameplayHash, op, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, context.ParentFrameId, native, hp, miuyan ? WangGuanStage.MiuyanGainChildren : WangGuanStage.ShiluDrawChildren, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, 0)
        {
            Use = invoice
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangGuan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangGuanStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ActualTurn, r.TurnOwner, r.ParentId, native, hp));
        if (miuyan)
        {
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(native)!;
            var positive = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CompletedUndamagedUseDamageRecordedEvent>().Any(e => e.CardUseFrameId == native && e.Amount > 0);
            if (use.CausedDamage != positive)
                throw new InvalidOperationException("Miuyan requires the actual positive whole-Use damage ledger, never an attempted Fire Attack damage fact.");
            if (invoice!.PreviousState == SkillPolarity.Yin && !positive)
            {
                _coreProgramSkillChangesOperations.IssueSkillState(new(f.Id, f.InstructionIndex - 1,
                    new(invoice.Source.SkillId, invoice.Source.BindingId, invoice.Source.OwnerSeat, invoice.Source.SkillInstanceId),
                    native, invoice.ActionId), invoice.Source.OwnerSeat,
                    new(SkillStateAction.Suppress, SkillStateScope.Named, SkillStateDuration.CurrentRound)
                        { CharacterOnly = false }, ["ol:miuyan"]);
                FinishWangGuan(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            if (invoice.PreviousState != SkillPolarity.Yang || !positive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishWangGuan(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            var entities = MiuyanClaimEntities(invoice, f.OwnerSeat);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangGuan = r with { Entities = entities }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            for (var i = 0; i < entities.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MiuyanClaimEntityFrozenEvent(f.Id, i, entities[i]));
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            if (entities.Count > 0)
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(entities.Select(e => e.CardId).ToArray(), CardLocation.Hand(f.OwnerSeat), new(MiuyanGainReason));
            for (var i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.CardId == entity.CardId && m.Reason.Value == MiuyanGainReason);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MiuyanEntityObtainedEvent(f.Id, i, entity, move.Sequence));
            }

            RecordWangGuanMovement(f.Id, before, entities.Count);
        }
        else
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            var actual = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], hp, true, new(ShiluDrawReason)).Count;
            RecordWangGuanMovement(f.Id, before, actual);
        }

        ContinueWangGuan(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void RecordWangGuanMovement(long id, long before, int count)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.WangGuan!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WangGuan = r with { Before = before, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangGuanMovementIssuedEvent(id, r.Stage, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count));
    }

    internal IReadOnlyList<PromptChoice> WangGuanChoices(ProgramSkillFrame f)
    {
        var r = f.WangGuan!;
        if (r.Stage == WangGuanStage.ShiluTarget)
            return Array.AsReadOnly(ShiluTargets(f.OwnerSeat).Select(s => new PromptChoice(new($"shilu.{(f.Id)}.target.{(s)}"), $"展示{(_coreMatchParticipantsPlayersAccess._players[s].Name)}的一张手牌", [], [s], new Dictionary<string, string> { { "program-action", "wang-guan" }, { "branch", "target" } })).ToArray());
        if (r.Stage == WangGuanStage.ShiluCard)
            return Array.AsReadOnly(r.Entities.Select(e => new PromptChoice(new($"shilu.{(f.Id)}.hand.{(e.Slot)}"), $"展示第{(e.Slot + 1)}张手牌", [], [], new Dictionary<string, string> { { "program-action", "wang-guan" }, { "branch", "card" }, { "slot-index", e.Slot.ToString() } })).ToArray());
        return[];
    }

    internal void PublishWangGuan(ProgramSkillFrame f)
    {
        var choices = WangGuanChoices(f);
        if (choices.Count == 0)
        {
            FinishWangGuan(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.WangGuan!.Stage == WangGuanStage.ShiluTarget ? "失路：选择攻击范围内的一名其他角色。" : "失路：选择其一张未知手牌公开展示并视为【杀】。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.WangGuan.HolderSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("wang-guan")]
    internal void AnswerWangGuan(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WangGuan: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !WangGuanChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Shilu requires its exact private opaque hand selection.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishWangGuan(f);
            return;
        }

        if (r.Stage == WangGuanStage.ShiluTarget)
        {
            var holder = choice.Targets.Single();
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holder]);
            var entities = hand.Select((c, i) => new WangGuanHandEntity(c.Id, c.Kind, holder, i, _characterLuoXianOlOperations.CurrentHandEntry(holder, c.Id)!.Value)).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangGuan = r with { Stage = WangGuanStage.ShiluCard, HolderSeat = holder, Entities = entities } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiluHolderChosenEvent(f.Id, holder, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            for (var i = 0; i < entities.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiluCandidateFrozenEvent(f.Id, i, entities[i]));
            PublishWangGuan(f);
            return;
        }

        if (r.Stage != WangGuanStage.ShiluCard || r.HolderSeat is not { } target)
            throw new InvalidOperationException("Shilu cannot select an entity before its post-draw holder.");
        var selected = r.Entities.Single(e => e.Slot.ToString() == choice.Parameters["slot-index"]);
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _characterLuoXianOlOperations.CurrentHandEntry(target, selected.CardId) != selected.Entry)
            throw new InvalidOperationException("Shilu cannot replace its frozen original opaque hand entity.");
        _coreProgramRevealsOperations.CommitProgramHandReveal(f.Id, target, ShiluRevealBind, [selected.CardId]);
        var holderFact = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiluHolderChosenEvent>().Single(e => e.FrameId == f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiluHandIdentityGrantedEvent(f.Id, r.Source, r.GameplayHash, r.NativeId, target, selected.CardId, selected.Entry, holderFact.MovementCutoff));
        FinishWangGuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ContinueWangGuan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WangGuan: { } r } f || f.Id != id)
            return;
        if (r.Stage is WangGuanStage.ShiluTarget or WangGuanStage.ShiluCard)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishWangGuan(f);
            return;
        }

        if (_coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.WangGuan!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Op == SkillProgramEffectOp.ResolveMiuyanCompletedUse || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || ShiluTargets(f.OwnerSeat).Count == 0)
        {
            FinishWangGuan(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangGuan = r with { Stage = WangGuanStage.ShiluTarget } });
        PublishWangGuan(f);
    }

    internal void FinishWangGuan(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangGuanCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WangGuan = null, PendingMovementContinuation = null, CardSetBindings = Array.AsReadOnly(f.CardSetBindings.Where(b => b.Name != ShiluRevealBind).ToArray()) });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27210)]
    internal bool ResumeWangGuan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WangGuan: not null } f || f.Id != id)
            return false;
        ContinueWangGuan(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27210)]
    internal bool ReturnWangGuanMovement(ProgramSkillFrame f)
    {
        if (f.WangGuan is null)
            return false;
        ContinueWangGuan(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMiuyanCompletedUse, SkillProgramEffectOp.ResolveShiluDrawRevealIdentity)]
    internal PromptChoice ChooseWangGuanAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderByDescending(c => c.Targets.Count > 0 ? _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat), c.Targets[0], new SkillProgramAiHint(1, 0, 0, 0, 0, 0, false, false)) : 0).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
}
