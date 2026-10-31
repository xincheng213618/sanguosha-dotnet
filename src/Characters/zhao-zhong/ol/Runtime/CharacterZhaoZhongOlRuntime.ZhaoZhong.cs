#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoZhongOlRuntime
{
    internal const string ZhaoZhongAction = "zhao-zhong";
    internal const string DiciGiveReason = "skill-program.zhao-zhong.dici.give";
    internal const string DiciNextGiveReason = "skill-program.zhao-zhong.dici.next-give";
    internal const string BiriDrawReason = "skill-program.zhao-zhong.biri.draw";
    SkillProgramStepOutcome global::CardGame.Core.IZhaoZhongHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginZhaoZhong(f, op);
    internal SkillProgramStepOutcome BeginZhaoZhong(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ZhaoZhong is not null)
        {
            ContinueZhaoZhong(f.Id);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        var c = f.WindowContext;
        var stage = op == SkillProgramEffectOp.ResolvePengbiFirstActualTurn ? ZhaoZhongStage.PengbiSkill : op == SkillProgramEffectOp.ResolveDiciGiftRecoveryAndNeighborChain ? ZhaoZhongStage.InitialGive : op == SkillProgramEffectOp.ResolveIssuedDiciNextChainChoice ? ZhaoZhongStage.NextChoice : ZhaoZhongStage.Drawing;
        var r = new ZhaoZhongReceipt(ZhaoZhongSource(f), f.GameplayHash, op, c?.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, stage);
        if (op == SkillProgramEffectOp.ResolveDiciGiftRecoveryAndNeighborChain)
        {
            if (f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count == 0 || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<DiciAcceptedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat && e.Phase == phase))
                throw new InvalidOperationException("Dici requires a living other true Hand donor and an unspent actual Play phase.");
            r = r with
            {
                TargetSeat = target,
                Phase = phase
            };
        }
        else if (op == SkillProgramEffectOp.ResolveIssuedDiciNextChainChoice)
        {
            var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c!.OccurrenceIndex);
            if (!IsIssuedDiciChainCandidate(candidate, c) || DiciCandidateLease(candidate)is not { } lease)
                throw new InvalidOperationException("Dici's next-chain choice lost its independent issued lease.");
            r = r with
            {
                Paid = true,
                TargetSeat = lease.TargetSeat,
                LeaseId = lease.LeaseId
            };
        }
        else if (op == SkillProgramEffectOp.ResolveBiriFirstNonDrawGain)
        {
            var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c!.OccurrenceIndex);
            if (!IsIssuedBiriCandidate(candidate, c) || c.MovementBatch is not { } batch || _coreProgramEventHistoryOperations.ProgramEventHistory<BiriPotentialGainEvent>().FirstOrDefault(e => e.BatchId == batch.Id)is not { } potential || FirstBiriGain(potential.ActualTurn, potential.TurnOwner)is not { } first)
                throw new InvalidOperationException("Biri lost the first actual non-draw acquisition of this actual turn.");
            r = r with
            {
                Paid = true,
                ActualTurn = first.ActualTurn,
                TurnOwner = first.TurnOwner,
                Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoZhong = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhaoZhongStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.WindowId, r.ActualTurn, r.TurnOwner, r.TargetSeat));
        if (op == SkillProgramEffectOp.ResolveIssuedDiciNextChainChoice)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiciLeaseStartedEvent(r.LeaseId, r.WindowId, f.Id));
        if (op == SkillProgramEffectOp.ResolveBiriFirstNonDrawGain)
        {
            var potential = _coreProgramEventHistoryOperations.ProgramEventHistory<BiriPotentialGainEvent>().First(e => e.BatchId == r.WindowId);
            var first = FirstBiriGain(potential.ActualTurn, potential.TurnOwner)!;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BiriDrawIssuedEvent(f.Id, r.WindowId, first.Sequence, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner));
        }

        ContinueZhaoZhong(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int ZhaoZhongChooser(ProgramSkillFrame f) => f.ZhaoZhong!.Stage is ZhaoZhongStage.InitialGive or ZhaoZhongStage.NextChoice ? f.ZhaoZhong.TargetSeat : f.OwnerSeat;
    internal IReadOnlyList<PromptChoice> ZhaoZhongChoices(ProgramSkillFrame f)
    {
        var r = f.ZhaoZhong!;
        var result = new List<PromptChoice>();
        PromptChoice Choice(string branch, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null, string? skill = null)
        {
            var p = new Dictionary<string, string>
            {
                ["program-action"] = ZhaoZhongAction,
                [ZhaoZhongAction] = branch,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
            };
            if (skill is not null)
                p["skill-id"] = skill;
            return new(new($"zhao-zhong.{(f.Id)}.{(r.Stage)}.{(branch)}.{(skill)}.{(string.Join('-', cards ?? []))}.{(string.Join('-', targets ?? []))}"), label, cards ?? [], targets ?? [], p);
        }

        if (r.Stage == ZhaoZhongStage.PengbiSkill)
            foreach (var skill in new[]
            {
                "ol:yintian",
                "ol:biri"
            }

            )
                result.Add(Choice("pengbi-skill", "获得" + _contentRegistryOperations._contentRegistry.GetSkill(skill).Name, skill: skill));
        else if (r.Stage == ZhaoZhongStage.PengbiTarget)
        {
            var other = r.ChosenSkill == "ol:yintian" ? "ol:biri" : "ol:yintian";
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
                result.Add(Choice("pengbi-target", "令" + target.Name + "获得" + _contentRegistryOperations._contentRegistry.GetSkill(other).Name, targets: [target.Seat]));
            result.Add(Choice("pengbi-skip", "不令其他角色获得技能"));
        }
        else if (r.Stage is ZhaoZhongStage.InitialGive or ZhaoZhongStage.NextChoice)
        {
            if (_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]))
                    result.Add(Choice(r.Stage == ZhaoZhongStage.InitialGive ? "initial-give" : "next-give", "交给" + _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name + "：" + card.DisplayName, [card.Id]));
            if (r.Stage == ZhaoZhongStage.NextChoice && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
                result.Add(Choice("next-damage", "受到其造成的1点雷电伤害"));
        }

        return Array.AsReadOnly(result.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishZhaoZhong(ProgramSkillFrame f)
    {
        var r = f.ZhaoZhong!;
        if (!r.Paid && (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId)))
        {
            FinishZhaoZhong(f);
            return;
        }

        var choices = ZhaoZhongChoices(f);
        if (choices.Count == 0)
        {
            FinishZhaoZhong(f);
            return;
        }

        var chooser = ZhaoZhongChooser(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == ZhaoZhongStage.PengbiSkill ? "朋比：选择自身获得的技能。" : r.Stage == ZhaoZhongStage.PengbiTarget ? "朋比：可令一名其他角色获得另一个技能。" : r.Stage == ZhaoZhongStage.InitialGive ? "抵慈：交给其一张手牌。" : "抵慈：交给其一张手牌，或受到其造成的1点雷电伤害。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            IsPrivate = true,
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("zhao-zhong")]
    internal void AnswerZhaoZhong(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhaoZhong: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != ZhaoZhongChooser(f) || !ZhaoZhongChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Zhao Zhong requires the actual chooser's exact published private choice.");
        AssertZhaoZhong(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters[ZhaoZhongAction];
        if (branch == "pengbi-skill")
        {
            var skill = choice.Parameters["skill-id"];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoZhong = r with { Paid = true, ChosenSkill = skill, Stage = ZhaoZhongStage.SelfGrantChildren } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PengbiCommittedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, skill));
            GrantPengbiSkill(f, f.OwnerSeat, skill);
            ContinueZhaoZhong(f.Id);
            return;
        }

        if (branch == "pengbi-skip")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PengbiOtherChosenEvent(f.Id, null));
            FinishZhaoZhong(f);
            return;
        }

        if (branch == "pengbi-target")
        {
            var target = choice.Targets.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PengbiOtherChosenEvent(f.Id, target));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoZhong = r with { TargetSeat = target, Stage = ZhaoZhongStage.OtherGrantChildren } });
            GrantPengbiSkill(f, target, r.ChosenSkill == "ol:yintian" ? "ol:biri" : "ol:yintian");
            ContinueZhaoZhong(f.Id);
            return;
        }

        if (branch is "initial-give" or "next-give")
        {
            PayDiciGift(f, choice.Cards.Single(), branch == "next-give");
            return;
        }

        if (branch == "next-damage")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoZhong = r with { Stage = ZhaoZhongStage.NextDamageChildren } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiciNextChoiceEvent(f.Id, r.LeaseId, "damage"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiciThunderIssuedEvent(f.Id, r.LeaseId, f.OwnerSeat, r.TargetSeat));
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.TargetSeat, 1, nature: DamageNature.Thunder);
            return;
        }

        throw new InvalidOperationException("Unknown Zhao Zhong choice.");
    }

    internal bool DrainZhaoZhong(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);
    internal void ContinueZhaoZhong(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ZhaoZhong: { } r } f && f.Id == id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
        {
            AssertZhaoZhong(f);
            if (f.NativeDraw is not null || f.AttackAttempt is not null)
                return;
            if (r.Stage is ZhaoZhongStage.SelfGrantChildren or ZhaoZhongStage.OtherGrantChildren or ZhaoZhongStage.GiftChildren or ZhaoZhongStage.RecoveryChildren or ZhaoZhongStage.UnlinkChildren or ZhaoZhongStage.NeighborChildren or ZhaoZhongStage.NextGiftChildren or ZhaoZhongStage.NextDamageChildren or ZhaoZhongStage.DrawChildren)
            {
                if (DrainZhaoZhong(id))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.ZhaoZhong!;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishZhaoZhong(f);
                return;
            }

            if (r.Stage == ZhaoZhongStage.SelfGrantChildren)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    FinishZhaoZhong(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoZhong = r with { Stage = ZhaoZhongStage.PengbiTarget } });
                PublishZhaoZhong(f);
                return;
            }

            if (r.Stage is ZhaoZhongStage.OtherGrantChildren or ZhaoZhongStage.NextGiftChildren or ZhaoZhongStage.NextDamageChildren or ZhaoZhongStage.DrawChildren)
            {
                FinishZhaoZhong(f);
                return;
            }

            if (r.Stage == ZhaoZhongStage.Drawing)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    FinishZhaoZhong(f);
                    return;
                }

                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new(BiriDrawReason));
                return;
            }

            if (r.Paid && r.Op == SkillProgramEffectOp.ResolveDiciGiftRecoveryAndNeighborChain && !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
            {
                FinishZhaoZhong(f);
                return;
            }

            if (r.Stage == ZhaoZhongStage.GiftChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoZhong = r with { Stage = ZhaoZhongStage.RecoveryChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiciRecoveryIssuedEvent(f.Id, r.TargetSeat));
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, r.TargetSeat, 1, null, null);
                continue;
            }

            if (r.Stage == ZhaoZhongStage.RecoveryChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoZhong = r with { Stage = ZhaoZhongStage.UnlinkChildren } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiciUnlinkIssuedEvent(f.Id, r.TargetSeat));
                _coreProgramLifecycleOperations.SetProgramChainedState(f, false, r.TargetSeat);
                continue;
            }

            if (r.Stage == ZhaoZhongStage.UnlinkChildren)
            {
                var neighbors = DiciNeighbors(r.TargetSeat);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoZhong = r with { Stage = ZhaoZhongStage.Neighbors, Neighbors = neighbors, Cursor = 0 } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiciNeighborsFrozenEvent(f.Id, neighbors.Length));
                for (var i = 0; i < neighbors.Length; i++)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiciNeighborFrozenEvent(f.Id, i, neighbors[i]));
                continue;
            }

            if (r.Stage == ZhaoZhongStage.NeighborChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhaoZhong = r with { Stage = ZhaoZhongStage.Neighbors, Cursor = r.Cursor + 1 } });
                continue;
            }

            if (r.Stage == ZhaoZhongStage.Neighbors)
            {
                if (r.Cursor < r.Neighbors.Count)
                {
                    var seat = r.Neighbors[r.Cursor];
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoZhong = r with { Stage = ZhaoZhongStage.NeighborChildren } });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiciNeighborIssuedEvent(f.Id, r.Cursor, seat));
                    if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
                        _coreProgramLifecycleOperations.SetProgramChainedState(f, true, seat);
                    continue;
                }

                var ordinal = _coreProgramEventHistoryOperations.ProgramEventHistory<DiciLeaseIssuedEvent>().Count();
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiciLeaseIssuedEvent(f.Id, r.Source, r.GameplayHash, f.OwnerSeat, r.TargetSeat, ordinal, r.ActualTurn));
                FinishZhaoZhong(f);
                return;
            }

            PublishZhaoZhong(f);
            return;
        }
    }

    internal void FinishZhaoZhong(ProgramSkillFrame f)
    {
        var r = f.ZhaoZhong!;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhaoZhongCompletedEvent(f.Id, r.Op, r.Paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoZhong = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 28110)]
    internal bool ResumeZhaoZhong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhaoZhong: not null } f || f.Id != id || f.InstructionIndex == 0)
            return false;
        ContinueZhaoZhong(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 28110)]
    internal bool ReturnZhaoZhongMovement(ProgramSkillFrame f)
    {
        if (f.ZhaoZhong is null)
            return false;
        ContinueZhaoZhong(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolvePengbiFirstActualTurn, SkillProgramEffectOp.ResolveDiciGiftRecoveryAndNeighborChain, SkillProgramEffectOp.ResolveIssuedDiciNextChainChoice, SkillProgramEffectOp.ResolveBiriFirstNonDrawGain)]
    internal PromptChoice ChooseZhaoZhongAi(PendingDecision prompt, ProgramSkillFrame f)
    {
        if (f.ZhaoZhong!.Stage == ZhaoZhongStage.PengbiTarget)
        {
            var view = _corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat);
            return prompt.Choices.OrderByDescending(c => c.Targets.Count == 1 ? _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(view, c.Targets[0], new(1, 0, 0, 0, 0, 0, false, false)) : 0).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        }

        return prompt.Choices.First();
    }
}
