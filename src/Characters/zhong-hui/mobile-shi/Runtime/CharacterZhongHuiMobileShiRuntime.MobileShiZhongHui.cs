#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhongHuiMobileShiRuntime
{
    internal const string ShiZhHuiSizi = ShiZhHuiComposition.SiziSkill, ShiZhHuiXiezhi = ShiZhHuiComposition.XiezhiSkill, ShiZhHuiYunan = ShiZhHuiComposition.YunanSkill;
    internal static CardConversionSource ShiZhHuiSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool HasShiZhHuiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveShiZhHuiSizi);

    internal bool HasShiZhHuiKechang(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants.Any(grant => grant.IsEnabled && grant.SkillId == ShiZhHuiComposition.KeChangSkill) || _coreNationalSkillsOperations.EnabledContentSkillIds(_coreMatchParticipantsPlayersAccess._players[seat]).Contains(ShiZhHuiComposition.KeChangSkill, StringComparer.Ordinal);
    internal int ShiZhHuiKechangLevel(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZhHuiYunanAwakenedEvent>().Where(entry => entry.Source.OwnerSeat == seat).Select(entry => entry.Level).LastOrDefault();
    internal bool ShiZhHuiRoundHasDeath => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZhHuiDeathRecordedEvent>().Any(death => death.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber);
    internal ShiZhHuiSiziActivatedEvent? ShiZhHuiActiveSizi => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZhHuiSiziActivatedEvent>().LastOrDefault(activation => !_coreProgramEventHistoryOperations.ProgramEventHistory<ShiZhHuiSiziExpiredEvent>().Any(expired => expired.ActivationOrdinal == activation.Ordinal));
    internal bool ShiZhHuiSiziActive => ShiZhHuiActiveSizi is not null;

    internal IReadOnlyList<int> ShiZhHuiEndingVictims(int turnNumber, int ownerHp, int consumed)
    {
        var users = _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZhHuiSlashUsedEvent>().Where(use => use.TurnNumber == turnNumber).Select(use => use.ActorSeat).Distinct().Where(seat => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).ToArray();
        if (users.Length > 0 || consumed <= ownerHp || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat) || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat].IsAlive)
            return Array.AsReadOnly(users);
        // Effect 3: with no slash used and a consumed X above the activation HP, the turn player loses 1 HP.
        return Array.AsReadOnly(new[] { _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat });
    }

    SkillProgramStepOutcome global::CardGame.Core.IShiZhHuiHost.ResolveSizi(ProgramSkillFrame frame) => BeginShiZhHui(frame);
    SkillProgramStepOutcome global::CardGame.Core.IShiZhHuiHost.ResolveSiziEnding(ProgramSkillFrame frame) => BeginShiZhHui(frame);
    SkillProgramStepOutcome global::CardGame.Core.IShiZhHuiHost.ResolveXiezhi(ProgramSkillFrame frame) => BeginShiZhHui(frame);
    SkillProgramStepOutcome global::CardGame.Core.IShiZhHuiHost.ResolveYunan(ProgramSkillFrame frame) => BeginShiZhHui(frame);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 67700, SkillProgramEffectOp.ResolveShiZhHuiSizi)]
    internal bool CanStartShiZhHuiSizi(CharacterState owner, string skill) => skill != ShiZhHuiSizi || ShiZhHuiCharge(owner.Seat) > 0;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 67700)]
    internal bool CanOfferShiZhHui(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId == ShiZhHuiSizi && trigger.Id == ShiZhHuiComposition.SiziEndingTrigger)
            return ShiZhHuiSiziActive;
        if (candidate.SkillId == ShiZhHuiYunan && trigger.Id == ShiZhHuiComposition.YunanTrigger)
            return _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && dying.KillerSeat == candidate.OwnerSeat && ShiZhHuiRoundHasDeath;
        return true;
    }

    internal SkillProgramStepOutcome BeginShiZhHui(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ShiZhHui is not null)
            throw new InvalidOperationException("Shi Zhong Hui restarted a running operation.");
        var r = f.SkillId == ShiZhHuiSizi && f.ActivationId == ShiZhHuiComposition.SiziActivation && f.TriggerId is null ? StartShiZhHuiSizi(f) : f.SkillId == ShiZhHuiSizi && f.TriggerId == ShiZhHuiComposition.SiziEndingTrigger ? StartShiZhHuiSiziEnding(f) : f.SkillId == ShiZhHuiXiezhi && f.TriggerId == ShiZhHuiComposition.XiezhiTrigger ? StartShiZhHuiXiezhi(f) : f.SkillId == ShiZhHuiYunan && f.TriggerId == ShiZhHuiComposition.YunanTrigger ? StartShiZhHuiYunan(f) : throw new InvalidOperationException("Shi Zhong Hui instruction has an incompatible owning skill.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiZhHui = r });
        ContinueShiZhHui(f.Id);
        return r.Operation == "sizi" ? SkillProgramStepOutcome.AwaitChoice : SkillProgramStepOutcome.AwaitChild;
    }

    internal ShiZhHuiReceipt StartShiZhHuiSizi(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || ShiZhHuiCharge(f.OwnerSeat) <= 0)
            throw new InvalidOperationException("Sizi requires its living owner to hold at least one charge point.");
        return new("sizi", _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber)
        {
            Source = ShiZhHuiSource(f),
            GameplayHash = f.GameplayHash
        };
    }

    internal ShiZhHuiReceipt StartShiZhHuiSiziEnding(ProgramSkillFrame f)
    {
        if (ShiZhHuiActiveSizi is not { } activation)
            throw new InvalidOperationException("Sizi ending requires an active effect.");
        return new("sizi-ending", _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber)
        {
            Source = ShiZhHuiSource(f),
            GameplayHash = f.GameplayHash,
            Ordinal = activation.Ordinal
        };
    }

    internal ShiZhHuiReceipt StartShiZhHuiXiezhi(ProgramSkillFrame f)
    {
        _ = f.WindowContext?.HpChange ?? throw new InvalidOperationException("Xiezhi lost its health-change window.");
        return new("xiezhi", _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber)
        {
            Source = ShiZhHuiSource(f),
            GameplayHash = f.GameplayHash
        };
    }

    internal ShiZhHuiReceipt StartShiZhHuiYunan(ProgramSkillFrame f)
    {
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying ?? throw new InvalidOperationException("Yunan lost its dying entry.");
        if (dying.KillerSeat != f.OwnerSeat)
            throw new InvalidOperationException("Yunan requires a dying state caused by its owner.");
        return new("yunan", _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, TargetSeat: dying.VictimSeat)
        {
            Source = ShiZhHuiSource(f),
            GameplayHash = f.GameplayHash,
            DyingFrameId = dying.FrameId
        };
    }

    internal IReadOnlyList<PromptChoice> ShiZhHuiSiziChoices(ProgramSkillFrame f)
    {
        var r = f.ShiZhHui!;
        var charge = ShiZhHuiCharge(f.OwnerSeat);
        var choices = new List<PromptChoice>();
        for (var count = 1; count <= Math.Min(charge, ShiZhHuiComposition.ChargeCapacity); count++)
            choices.Add(new(new($"shi-zhong-hui.{(f.Id)}.{(r.Stage)}.consume.{(count)}"), $"消耗{(count)}点蓄力点（效果持续至{(count)}个回合结束后或你的回合开始时）", [], [], new Dictionary<string, string> { ["program-action"] = ShiZhHuiComposition.SiziProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "consume", ["count"] = count.ToString(CultureInfo.InvariantCulture), }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishShiZhHui(ProgramSkillFrame f)
    {
        var r = f.ShiZhHui!;
        var choices = r.Operation == "sizi" ? ShiZhHuiSiziChoices(f) : [];
        if (choices.Count == 0 || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishShiZhHuiOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "肆恣：选择要消耗的蓄力点数。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(ShiZhHuiComposition.SiziProgramAction)]
    internal void AnswerShiZhHui(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiZhHui: { Operation: "sizi" } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShiZhHuiSiziChoices(f).Any(choice => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Zhong Hui choice lost its current frame, chooser or legal branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var consumed = int.Parse(selected.Parameters["count"], CultureInfo.InvariantCulture);
        if (consumed is < 1 or > ShiZhHuiComposition.ChargeCapacity || consumed > ShiZhHuiCharge(f.OwnerSeat))
            throw new InvalidOperationException("Sizi consumed an unavailable charge amount.");
        ChangeShiZhHuiCharge(f.Id, f.OwnerSeat, -consumed, ShiZhHuiComposition.SiziChargeReason, out _);
        var ordinal = _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZhHuiSiziActivatedEvent>().Count + 1;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZhHuiSiziActivatedEvent(f.Id, r.Source!, f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, ordinal, consumed, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiZhHui = r with { Consumed = consumed, Ordinal = ordinal } });
        FinishShiZhHuiOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ContinueShiZhHui(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiZhHui: { } r } f || f.Id != id)
            return;
        if (r.Operation == "sizi-ending" && r.Stage is 6 or 12)
        {
            if (DrainShiZhHui(f))
                return;
            r = _coreProgramLifecycleOperations.GetActiveProgramFrame(id).ShiZhHui!;
        }

        switch (r.Operation, r.Stage)
        {
            case ("sizi", 0):
                PublishShiZhHui(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            case ("sizi-ending", 0) or ("sizi-ending", 6) or ("sizi-ending", 12):
                ContinueShiZhHuiEnding(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            case ("xiezhi", 0):
                ContinueShiZhHuiXiezhi(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            case ("yunan", 0):
                ContinueShiZhHuiYunan(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            default:
                throw new InvalidOperationException("Shi Zhong Hui has an unknown continuation stage.");
        }
    }

    internal void ContinueShiZhHuiEnding(ProgramSkillFrame f)
    {
        var r = f.ShiZhHui!;
        var activation = ShiZhHuiActiveSizi ?? throw new InvalidOperationException("Sizi ending lost its active effect.");
        if (r.Ordinal != activation.Ordinal)
            throw new InvalidOperationException("Sizi ending resolved a different activation than its receipt.");
        if (r.Stage == 0)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishShiZhHuiOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            }

            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 2, null, null, SkillProgramCardSetVisibility.Private, new(ShiZhHuiComposition.SiziEndingDrawReason));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiZhHui = r with { Stage = 6 } });
        }

        r = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id).ShiZhHui!;
        if (r.Stage == 6)
        {
            var victims = ShiZhHuiEndingVictims(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, activation.OwnerHp, activation.Consumed);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiZhHui = r with { Victims = victims, VictimCursor = 0, Stage = 12 } });
        }

        ContinueShiZhHuiEndingLosses(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), activation);
    }

    internal void ContinueShiZhHuiEndingLosses(ProgramSkillFrame f, ShiZhHuiSiziActivatedEvent activation)
    {
        var r = f.ShiZhHui!;
        var victims = r.Victims ?? throw new InvalidOperationException("Sizi ending lost its victim ledger.");
        while (r.VictimCursor < victims.Count)
        {
            var victim = victims[r.VictimCursor];
            if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) || !_coreMatchParticipantsPlayersAccess._players[victim].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiZhHui = r with { VictimCursor = r.VictimCursor + 1 } });
                r = f.ShiZhHui!;
                continue;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiZhHui = r with { VictimCursor = r.VictimCursor + 1 } });
            if (_corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, victim, 1) == SkillProgramStepOutcome.AwaitChild)
                return;
            r = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id).ShiZhHui!;
        }

        var elapsed = _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZhHuiSiziElapsedEvent>().Count(entry => entry.ActivationOrdinal == activation.Ordinal) + 1;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZhHuiSiziElapsedEvent(activation.Ordinal, elapsed));
        if (elapsed >= activation.Consumed)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZhHuiSiziExpiredEvent(activation.Ordinal, "turn-count"));
        FinishShiZhHuiOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ContinueShiZhHuiXiezhi(ProgramSkillFrame f)
    {
        var r = f.ShiZhHui!;
        var change = f.WindowContext?.HpChange ?? throw new InvalidOperationException("Xiezhi lost its health-change window.");
        var delta = Math.Abs(change.HpAfter - change.HpBefore);
        if (delta > 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            ChangeShiZhHuiCharge(f.Id, f.OwnerSeat, delta, ShiZhHuiComposition.XiezhiChargeReason, out var excess);
            if (excess > 0)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZhHuiXiezhiOverflowEvent(f.Id, f.OwnerSeat, excess, ShiZhHuiOverflowCount(f.OwnerSeat) + 1));
        }

        FinishShiZhHuiOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ContinueShiZhHuiYunan(ProgramSkillFrame f)
    {
        var r = f.ShiZhHui!;
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying ?? throw new InvalidOperationException("Yunan lost its dying entry.");
        ApplyShiZhHuiFactionChange(f, "qun");
        var level = Math.Min(2, ShiZhHuiKechangLevel(f.OwnerSeat) + 1);
        if (!HasShiZhHuiKechang(f.OwnerSeat))
            _coreProgramSkillChangesOperations.GrantProgramSkills(f, [ShiZhHuiComposition.KeChangSkill]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZhHuiYunanAwakenedEvent(f.Id, r.Source!, f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, dying.VictimSeat, level, "qun"));
        FinishShiZhHuiOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void FinishShiZhHuiOperation(ProgramSkillFrame f)
    {
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
    }

    internal bool DrainShiZhHui(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 67700)]
    internal bool ResumeShiZhHui(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiZhHui: not null } f || f.Id != id)
            return false;
        ContinueShiZhHui(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 67700)]
    internal bool ReturnShiZhHuiMovement(ProgramSkillFrame f) => f.ShiZhHui is not null && ResumeShiZhHui(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 67700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 67700)]
    internal bool ContinuePaidShiZhHui(ProgramSkillFrame f) => f.ShiZhHui is { Stage: > 0 };
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 67700)]
    internal int ShiZhHuiSiziDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => attack is global::CardGame.Core.NativeOwners.CardAttackHandle card && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(card.EffectiveCardKind ?? CardKind.Slash) && ShiZhHuiSiziActive ? 1 : 0;
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 67700)]
    internal bool DistanceCardUseExemption67700ShiZhHuiKechang(ProgramCardDistanceQuery query) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(query.Kind) && string.Equals(_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(query.Actor), "qun", StringComparison.Ordinal) && _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && HasShiZhHuiKechang(player.Seat));

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiZhHuiSizi)]
    internal PromptChoice ChooseShiZhHuiAi(PendingDecision decision, ProgramSkillFrame f)
    {
        // Prefer the largest consumption that stays at or below the owner's HP so
        // the effect-3 qualification keeps targeting foreign turn players.
        var hp = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp;
        return decision.Choices.Where(choice => choice.Parameters["branch"] == "consume").OrderByDescending(choice => int.Parse(choice.Parameters["count"], CultureInfo.InvariantCulture)).FirstOrDefault(choice => int.Parse(choice.Parameters["count"], CultureInfo.InvariantCulture) <= hp) ?? decision.Choices.First();
    }
}
