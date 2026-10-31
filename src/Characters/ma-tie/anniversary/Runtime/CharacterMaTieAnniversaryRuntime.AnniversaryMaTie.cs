#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaTieAnniversaryRuntime
{
    internal const string MaTieAction = "anniversary-ma-tie", MaTieObtainReason = "skill-program.anniversary:quxian.obtain";
    internal static CardConversionSource MaTieSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryMaTieHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveAnniversaryMaTieQuxian => BeginMaTie(f),
        SkillProgramEffectOp.ResolveAnniversaryMaTieQuxianReward => BeginMaTieReward(f),
        SkillProgramEffectOp.RecordAnniversaryMaTieZhuiji => RecordMaTieZhuiji(f),
        _ => throw new InvalidOperationException("Unknown Ma Tie operation.")};
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 46700)]
    internal bool CanOfferMaTie(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c)
    {
        if (candidate.SkillId == AnniversaryMaTieComposition.Quxian)
        {
            if (trigger.Id == "issued-slash-reward")
                return MaTieRewardUse(candidate.OwnerSeat, c)is { AnniversaryMaTieOrigin: { } origin } && origin.Source.SkillInstanceId == candidate.SkillInstanceId;
            return c.Window is SkillProgramTriggerWindow.TurnStartBeforeNormalFlow or SkillProgramTriggerWindow.AfterTurnEnded && c.SourceSeat == candidate.OwnerSeat && c.TargetSeat == candidate.OwnerSeat && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == candidate.OwnerSeat;
        }

        if (candidate.SkillId != AnniversaryMaTieComposition.Zhuiji)
            return true;
        return MaTieCardWindow(c, SkillProgramTriggerWindow.CardUseCompleted)is { Action: { Type: CardActionType.Use } a } && a.ActorSeat == candidate.OwnerSeat && a.EffectiveDesignatedTargetSeats.Any(t => t != candidate.OwnerSeat);
    }

    internal SkillProgramStepOutcome BeginMaTie(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryMaTie is not null || !MaTieTurnParent(f) || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Quxian requires its exact optional own actual start or after-turn-ended candidate.");
        var r = new AnniversaryMaTieReceipt(MaTieSource(f), f.GameplayHash, f.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, AnniversaryMaTieStage.ObtainChildren)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaTie = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaTieStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn, r.TurnOwner));
        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).FirstOrDefault(c => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind) && !c.IsGeneralWeapon);
        if (card is not null)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaTie = r with { ObtainedCard = card.Id } });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(f.OwnerSeat), new(MaTieObtainReason), (_, _) =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { AnniversaryMaTie = current.AnniversaryMaTie!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            });
            if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.AwaitChild)
                return SkillProgramStepOutcome.AwaitChild;
        }

        ContinueMaTie(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal PromptChoice MaTieChoice(ProgramSkillFrame f, string branch, string text, int? target = null, int option = -1) => new(new($"ma-tie.{(f.Id)}.{(f.AnniversaryMaTie!.Cursor)}.{(branch)}.{(target ?? option)}"), text, [], target is { } t ? [t] : [], new Dictionary<string, string> { ["program-action"] = MaTieAction, ["branch"] = branch, ["option"] = option.ToString(CultureInfo.InvariantCulture) });
    internal IReadOnlyList<PromptChoice> MaTieChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryMaTie!;
        if (r.Stage == AnniversaryMaTieStage.Target)
            return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => MaTieChoice(f, "target", "令攻击范围内的角色依次对 " + p.Name + " 使用杀", p.Seat)).ToArray());
        if (r.Stage != AnniversaryMaTieStage.SlashChoice || r.Cursor >= r.Actors.Count || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Actors[r.Cursor]].IsAlive)
            return[];
        var actor = r.Actors[r.Cursor];
        var result = new List<PromptChoice>();
        var plans = MaTieSlashPlans(f);
        for (var i = 0; i < plans.Count; i++)
            result.Add(new(new($"ma-tie.{(f.Id)}.{(r.Cursor)}.slash.{(i)}"), plans[i].Action.Description, plans[i].Materials, plans[i].Action.TargetSeats, new Dictionary<string, string> { ["program-action"] = MaTieAction, ["branch"] = "slash", ["option"] = i.ToString(CultureInfo.InvariantCulture) }));
        result.AddRange(_characterMaXiuMaTieOlOperations.KenshangProgramSlashChoices(f, actor, [r.Target]));
        result.AddRange(_characterChengJiChengCuiOlOperations.TousuiProgramSlashChoices(f, actor, [r.Target]));
        if (CanRequestMaTieFaction(f))
            result.Add(MaTieChoice(f, "faction", "发动自己的势力供杀能力"));
        result.Add(MaTieChoice(f, "decline", "不使用杀"));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishMaTie(ProgramSkillFrame f)
    {
        var r = f.AnniversaryMaTie!;
        var choices = MaTieChoices(f);
        if (choices.Count == 0)
        {
            FinishMaTie(f);
            return;
        }

        var actor = r.Stage == AnniversaryMaTieStage.Target ? f.OwnerSeat : r.Actors[r.Cursor];
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var prompt = new PendingDecision(DecisionKind.ProgramTrigger, actor, r.Stage == AnniversaryMaTieStage.Target ? "驱险：选择一名其他角色。" : $"驱险：可对 {(_coreMatchParticipantsPlayersAccess._players[r.Target].Name)} 使用杀；若无人使其受到伤害，未使用者各失去实际使用人数点体力。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = actor,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse, AnniversaryMaTie = r with { OriginalDecision = RequestedDeckBasicFrame.FreezeDecision(prompt) } });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = prompt;
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[actor].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(MaTieAction)]
    internal void AnswerMaTie(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryMaTie: { } r } f || !ValidMaTie(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == AnniversaryMaTieStage.Target ? f.OwnerSeat : r.Actors[r.Cursor]) || !MaTieChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [c])))
            throw new InvalidOperationException("Quxian changed its private original actor, target or native plan.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (choice.Parameters["branch"])
        {
            case "target":
                var target = choice.Targets.Single();
                var queue = Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(i => _coreTableOrderOperations.TableStepSeat(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, i)).Where(s => s != target && _coreMatchParticipantsPlayersAccess._players[s].IsAlive && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(s, target)).ToArray();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaTie = r with { Target = target, Actors = queue, Stage = AnniversaryMaTieStage.SlashChoice } });
                for (var i = 0; i < queue.Length; i++)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaTieQueueEvent(f.Id, target, i, queue[i]));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaTieSelectedEvent(f.Id, target, queue.Length));
                ContinueMaTie(f.Id);
                return;
            case "slash":
                IssueMaTieSlash(f, int.Parse(choice.Parameters["option"], CultureInfo.InvariantCulture));
                return;
            case "faction":
                BeginMaTieFaction(f);
                return;
            case "decline":
                NextMaTieActor(f);
                return;
            default:
                throw new InvalidOperationException("Unknown Quxian choice.");
        }
    }

    internal bool DrainMaTie(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    internal void ContinueMaTie(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.AnniversaryMaTie!;
        if (r.Stage is AnniversaryMaTieStage.ObtainChildren or AnniversaryMaTieStage.RewardChildren or AnniversaryMaTieStage.HpChildren)
        {
            if (DrainMaTie(f))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            r = f.AnniversaryMaTie!;
            if (r.Stage == AnniversaryMaTieStage.RewardChildren)
            {
                FinishMaTie(f);
                return;
            }

            if (r.Stage == AnniversaryMaTieStage.ObtainChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaTie = r with { Stage = AnniversaryMaTieStage.Target } });
                PublishMaTie(f);
                return;
            }

            if (!r.HpPaid && _coreMatchParticipantsPlayersAccess._players[r.HpTarget].IsAlive)
            {
                var result = _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, r.HpTarget, r.Used.Count, (before, after, lost) =>
                {
                    var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                    var receipt = current.AnniversaryMaTie!;
                    _coreRuntimeOperations.ReplaceRuntimeFrame(current.Id, current with { AnniversaryMaTie = receipt with { HpPaid = true, HpBefore = before, HpAfter = after } });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaTieHpPaidEvent(f.Id, receipt.PenaltyCursor, receipt.HpTarget, lost, before, after));
                });
                if (result == SkillProgramStepOutcome.AwaitChild)
                    return;
                if (DrainMaTie(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
                    return;
            }

            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            r = f.AnniversaryMaTie!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaTie = r with { Stage = AnniversaryMaTieStage.Penalties, PenaltyCursor = r.PenaltyCursor + 1, HpTarget = -1, HpPaid = false, HpBefore = 0, HpAfter = 0 } });
            ContinueMaTie(f.Id);
            return;
        }

        if (r.Stage == AnniversaryMaTieStage.NativeReturned)
        {
            NextMaTieActor(f);
            return;
        }

        if (r.Stage == AnniversaryMaTieStage.SlashChoice)
        {
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishMaTie(f);
                return;
            }

            if (r.Cursor >= r.Actors.Count || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaTie = r with { Stage = AnniversaryMaTieStage.Penalties } });
                ContinueMaTie(f.Id);
                return;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[r.Actors[r.Cursor]].IsAlive)
            {
                NextMaTieActor(f);
                return;
            }

            PublishMaTie(f);
            return;
        }

        if (r.Stage == AnniversaryMaTieStage.Penalties)
        {
            if (r.TargetDamaged || r.Used.Count == 0 || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishMaTie(f);
                return;
            }

            var cursor = r.PenaltyCursor;
            while (cursor < r.Actors.Count && (r.Used.Contains(r.Actors[cursor]) || !_coreMatchParticipantsPlayersAccess._players[r.Actors[cursor]].IsAlive))
                cursor++;
            if (cursor >= r.Actors.Count)
            {
                FinishMaTie(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaTie = r with { Stage = AnniversaryMaTieStage.HpChildren, PenaltyCursor = cursor, HpTarget = r.Actors[cursor], HpPaid = false } });
            ContinueMaTie(f.Id);
            return;
        }

        if (r.Stage == AnniversaryMaTieStage.Target)
            PublishMaTie(f);
    }

    internal void NextMaTieActor(ProgramSkillFrame f)
    {
        var r = f.AnniversaryMaTie!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, RoundOnceCardlessProgramRequest = null, AnniversaryMaTie = r with { Stage = AnniversaryMaTieStage.SlashChoice, Cursor = r.Cursor + 1, SelectedAction = null, Materials = [], Provider = -1, UseId = 0, ActionId = 0, FactionPolicy = null, FactionTried = false, OriginalDecision = null } });
        ContinueMaTie(f.Id);
    }

    internal void FinishMaTie(ProgramSkillFrame f)
    {
        var r = f.AnniversaryMaTie!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaTieCompletedEvent(f.Id, r.Used.Count, r.TargetDamaged));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaTie = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryMaTieQuxian)]
    internal PromptChoice ChooseMaTie(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.AnniversaryMaTie!;
        if (r.Stage == AnniversaryMaTieStage.Target)
            return p.Choices.OrderBy(c => global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]) ? 1 : 0).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        if (!global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[r.Actors[r.Cursor]], _coreMatchParticipantsPlayersAccess._players[r.Target]))
            return p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "slash") ?? p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "faction") ?? p.Choices.Last();
        return p.Choices.Last();
    }
}
