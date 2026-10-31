#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuMobileGodRuntime
{
    internal const string MobileLbWuqian = "mobile-god:wuqian", MobileLbWushuang = "classic:wushuang", MobileLbGainReason = "skill.mobile-god.wuqian.damage-card";
    internal bool HasMobileWuqianCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMobileShenLuBuWuqian);

    internal IEnumerable<MobileLuBuWuqianIssuedEvent> ActiveMobileWuqian(int actor) => !HasMobileWuqianCapability ? [] : _coreProgramEventHistoryOperations.ProgramEventHistory<MobileLuBuWuqianIssuedEvent>().Where(e => e.Source.OwnerSeat == actor && !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileLuBuWuqianEndedEvent>().Any(end => end.FrameId == e.FrameId));
    internal bool HasMobileWuqianArmorDisabled(int targetSeat) => HasMobileWuqianCapability && _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Any(p => ActiveMobileWuqian(p.Seat).Any(e => e.TargetSeat == targetSeat));
    internal IEnumerable<RuleQueryContribution> MobileWuqianRuleContributions(CharacterState player, SkillRuleQuery query)
    {
        if (query != SkillRuleQuery.SlashLimit || !player.IsAlive || !_coreNativeSkillContextQueriesOperations.HasProgramSkill(player, MobileLbWuqian))
            yield break;
        var count = ActiveMobileWuqian(player.Seat).Select(e => e.TargetSeat).Distinct().Count(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive);
        if (count > 0)
            yield return new FiniteRuleQueryContribution($"mobile-wuqian:{(player.Seat)}:victims", SkillRuleOperation.Add, count);
    }

    internal bool MobileLbHasDamageHand(CharacterState owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(owner, c)));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 40325)]
    internal bool AllowsMobileLbDamageGain(ProgramTriggerLegalityContext g) => g.SkillId != MobileLbWuqian || g.Trigger.Effects.All(e => e.Op != SkillProgramEffectOp.GainMobileShenLuBuDamageCard) || !MobileLbHasDamageHand(g.Owner);
    SkillProgramStepOutcome global::CardGame.Core.IMobileShenLuBuHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginMobileLb(f, op);
    internal SkillProgramStepOutcome BeginMobileLb(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MobileLuBu is not null || f.SkillId != MobileLbWuqian || f.InstructionIndex != 1)
            throw new InvalidOperationException("Mobile Wuqian requires its original paid program instruction.");
        var source = new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
        var target = op == SkillProgramEffectOp.ResolveMobileShenLuBuWuqian ? f.SelectedTargetSeats.Single() : f.OwnerSeat;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileLuBu = new(source, f.GameplayHash, f.InstructionIndex, op, target, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) });
        if (op == SkillProgramEffectOp.ResolveMobileShenLuBuWuqian)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            var grantSource = $"mobile-wuqian:{(owner.Seat)}:{(f.Id)}";
            var grantId = grantSource + ":" + MobileLbWushuang;
            var instance = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ActiveGrants.FirstOrDefault(g => g.SkillId == MobileLbWushuang)?.SkillInstanceId ?? $"mobile-wuqian-shared:{(owner.Seat)}";
            foreach (var player in _coreMatchParticipantsPlayersAccess._players)
                _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileLuBuWuqianIssuedEvent(f.Id, source, target, grantId, instance, grantSource));
            if (!owner.SkillGrants.Grant(new SkillGrant(grantId, MobileLbWushuang, instance, grantSource)))
                throw new InvalidOperationException("One Wuqian issue cannot grant twice.");
            _coreConversionStatesOperations.RegisterTaggedConversionSkill(owner, MobileLbWushuang);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillsAcquiredEvent(owner.Seat, f.SkillId, Array.AsReadOnly(new[] { MobileLbWushuang })));
        }
        else
        {
            if (f.WindowContext?.Window != SkillProgramTriggerWindow.TurnEnding || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat)
                throw new InvalidOperationException("Wuqian's gain requires its actual ending stage.");
            if (!MobileLbHasDamageHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))
            {
                var candidates = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Concat(_cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile)).Where(c => global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(c.Kind)).ToArray();
                if (candidates.Length > 0)
                {
                    var chosen = candidates[_randomOperations._random.Next(candidates.Length)];
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileLuBu = f.MobileLuBu!with { Cards = [chosen.Id] } });
                    _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([chosen.Id], CardLocation.Hand(f.OwnerSeat), new(MobileLbGainReason), (_, _) =>
                    {
                        var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                        _coreRuntimeOperations.ReplaceRuntimeTop(current with { MobileLuBu = current.MobileLuBu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                    });
                    if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.AwaitChild)
                        return SkillProgramStepOutcome.AwaitChild;
                }
            }
        }

        ContinueMobileLb(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueMobileLb(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileLuBu: not null } f || f.Id != id)
            return;
        if (_coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileLuBu = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 40325)]
    internal bool ResumeMobileLb(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileLuBu: not null } f || f.Id != id)
            return false;
        ContinueMobileLb(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 40325)]
    internal bool ReturnMobileLbMovement(ProgramSkillFrame f) => f.MobileLuBu is not null && ResumeMobileLb(f.Id);
    internal bool ValidMobileLbGain(ProgramSkillFrame f) => f.MobileLuBu is { Op: SkillProgramEffectOp.GainMobileShenLuBuDamageCard, Cards: [var id] } r && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.Sequence > r.Before && m.Sequence <= r.After && m.From.Zone is CardZoneKind.DrawPile or CardZoneKind.DiscardPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MobileLbGainReason) == 1;
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 40325)]
    internal bool ProveMobileLbGain(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op == SkillProgramEffectOp.GainMobileShenLuBuDamageCard && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidMobileLbGain(f);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 40325)]
    internal void AssertMobileLb(ProgramSkillFrame f)
    {
        if (f.MobileLuBu is not { } r)
            return;
        if (r.Source != new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) || r.After < r.Before || r.Cards.Count > 1 || r.Cards.Count > 0 && !ValidMobileLbGain(f))
            throw new InvalidOperationException("Mobile Wuqian lost its exact source, original paid instruction or once-moved damage-card gain.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40325)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40325)]
    internal bool RetainMobileLb(ProgramSkillFrame f) => f.MobileLuBu is not null;
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 40325)]
    internal void ObserveMobileWuqianUses(IGameEvent payload)
    {
        if (!HasMobileWuqianCapability)
            return;
        if (payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { } use && global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(use.CardKind))
        {
            var issued = ActiveMobileWuqian(use.SourceSeat).Select(e => e.FrameId).ToArray();
            if (issued.Length == 0)
                return;
            var r = new MobileLuBuUseReceipt(use.Id, use.SourceSeat, use.Action?.ActionId, use.CardKind)
            {
                IssuedFrames = issued
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { MobileLuBuUse = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileLuBuUseCapturedEvent(use.Id, r.ActorSeat, r.ActionId, r.Kind));
        }
        else if (payload is CardUseFinishedEvent finished && _coreCardUseLifecycleOperations.LifecycleCardUse(finished.ResolutionId)is { MobileLuBuUse: { } r, CausedDamage: false } completed)
        {
            ValidateMobileLbUse(completed, r);
            foreach (var issue in _coreProgramEventHistoryOperations.ProgramEventHistory<MobileLuBuWuqianIssuedEvent>().Where(e => r.IssuedFrames.Contains(e.FrameId) && !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileLuBuWuqianEndedEvent>().Any(end => end.FrameId == e.FrameId)).ToArray())
            {
                var owner = _coreMatchParticipantsPlayersAccess._players[issue.Source.OwnerSeat];
                foreach (var player in _coreMatchParticipantsPlayersAccess._players)
                    _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
                owner.SkillGrants.RemoveGrant(issue.GrantId);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileLuBuWushuangRevokedEvent(owner.Seat, issue.GrantId, MobileLbWushuang, issue.SkillInstanceId, issue.SourceId, completed.Id));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileLuBuWuqianEndedEvent(issue.FrameId, completed.Id));
            }
        }
    }

    internal void ValidateMobileLbUse(CardUseFrame use, MobileLuBuUseReceipt r)
    {
        if (r.UseFrameId != use.Id || r.ActionId != use.Action?.ActionId || !global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(r.Kind) || r.IssuedFrames.Count == 0 || r.IssuedFrames.Distinct().Count() != r.IssuedFrames.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.SourceSeat == r.ActorSeat && e.CardKind == r.Kind) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileLuBuUseCapturedEvent>().Count(e => e.UseFrameId == use.Id && e.ActorSeat == r.ActorSeat && e.ActionId == r.ActionId && e.Kind == r.Kind) != 1 || r.IssuedFrames.Any(id => _coreProgramEventHistoryOperations.ProgramEventHistory<MobileLuBuWuqianIssuedEvent>().Count(e => e.FrameId == id && e.Source.OwnerSeat == r.ActorSeat) != 1))
            throw new InvalidOperationException("Mobile Wuqian retirement lost the actual declared damage-card Use and its frozen original issues.");
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 40325)]
    internal void AssertMobileLbUses()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.MobileLuBuUse is not null))
            ValidateMobileLbUse(use, use.MobileLuBuUse!);
    }
}
