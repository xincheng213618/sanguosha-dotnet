#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiJueOlRuntime
{
    internal const string YisuanGainReason = "skill-program.yisuan.actual-used-trick";
    internal static CardConversionSource LiJueSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_li_jue_ol_LiJuePrograms.LiJueSource(f);
    internal bool LiJueCurrentSource(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].HasHpValue && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 440)]
    internal bool CanOfferLiJue(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.ChooseHpBoundOtherAndDealRandomDamage))
            return LangxiTargets(candidate.OwnerSeat).Length > 0;
        if (ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.PayMaxHpAndObtainActualUsedTrick))
            return context.MovementBatch is { LiJueUseDiscard: { } invoice } batch && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].HasHpValue && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].MaxHp > 0 && ValidLiJueUseDiscard(invoice, batch, candidate.OwnerSeat);
        return true;
    }

    internal SkillProgramStepOutcome BeginLiJue(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.LiJue is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } context || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || !LiJueCurrentSource(f))
            throw new InvalidOperationException("LiJue requires the current unpaid native own phase source.");
        var r = new LiJueReceipt(LiJueSource(f), f.GameplayHash, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, context.ParentFrameId, effect.Op == SkillProgramEffectOp.ChooseHpBoundOtherAndDealRandomDamage ? LiJueStage.ChoosingTarget : LiJueStage.MaximumHpChildren);
        if (effect.Op == SkillProgramEffectOp.ChooseHpBoundOtherAndDealRandomDamage)
        {
            if (context.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)
                throw new InvalidOperationException("Langxi requires actual Prepare.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiJue = r });
            PublishLangxi(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (context.Window != SkillProgramTriggerWindow.CardsMoved || context.MovementBatch is not { LiJueUseDiscard: { } invoice } batch || !ValidLiJueUseDiscard(invoice, batch, f.OwnerSeat))
            throw new InvalidOperationException("Yisuan lost its actual native used-Trick movement.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        r = r with
        {
            OriginalUse = invoice,
            MaxBefore = owner.MaxHp,
            MaxAfter = owner.MaxHp - 1,
            HpBefore = owner.Hp,
            HpAfter = Math.Min(owner.Hp, owner.MaxHp - 1)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiJue = r });
        if (r.MaxAfter < 0)
            throw new InvalidOperationException("Yisuan cannot spend an absent maximum HP.");
        owner.MaxHp = r.MaxAfter;
        owner.Hp = r.HpAfter;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(owner.Seat, -1, owner.MaxHp, f.SkillId) { HpBeforeClamp = r.HpBefore });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YisuanMaximumHpPaidEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.WindowId, invoice.NativeUseId, invoice.Action.ActionId, r.MaxBefore, r.MaxAfter, r.HpBefore, r.HpAfter));
        if (r.MaxAfter == 0)
        {
            _coreNativePlayerDeathOperations.BeginPlayerDeath(f.Id, owner, null, null, null, causingProgramSkillFrameId: f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void FinishLiJue(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiJue = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 23700)]
    internal bool ResumeLiJue(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LiJue: { } r } f || f.Id != id || r.Stage == LiJueStage.ChoosingTarget)
            return false;
        RequireLiJue(f);
        if (r.Stage == LiJueStage.DamageChildren)
        {
            FinishLiJue(f);
            return true;
        }

        // A maximum-HP payment owns a normal Program return. Only the later
        // physical gain has an actual issued movement continuation to return to.
        var continuation = f.PendingMovementContinuation is not null ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, continuation) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, continuation) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id))
            return true;
        if (r.Stage == LiJueStage.GainChildren)
        {
            FinishLiJue(f);
            return true;
        }

        var original = r.OriginalUse!;
        var cards = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && original.FinishedMovements.All(m => _cardZonesOperations._cardZones.GetLocation(m.CardId) == CardLocation.DiscardPile) ? original.FinishedMovements.Select(m => m.CardId).ToArray() : [];
        r = r with
        {
            Stage = LiJueStage.GainChildren,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            ActualGain = 0
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { LiJue = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (cards.Length == 0)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YisuanGainPaidEvent(f.Id, r.Source, r.GameplayHash, original.NativeUseId, original.Action.ActionId, r.Before, r.After, 0));
            FinishLiJue(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return true;
        }

        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards, CardLocation.Hand(f.OwnerSeat), new(YisuanGainReason), (_, moves) =>
        {
            var paid = r with
            {
                After = moves.Last().Sequence,
                ActualGain = moves.Count
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { LiJue = paid });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YisuanGainPaidEvent(f.Id, paid.Source, paid.GameplayHash, original.NativeUseId, original.Action.ActionId, paid.Before, paid.After, paid.ActualGain));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 23700)]
    internal bool ReturnLiJueMovement(ProgramSkillFrame f) => f.LiJue is not null && ResumeLiJue(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 200)]
    internal bool CanContinueLiJue(ProgramSkillFrame f)
    {
        if (f.LiJue is not { Stage: LiJueStage.DamageChildren or LiJueStage.MaximumHpChildren or LiJueStage.GainChildren })
            return false;
        RequireLiJue(f);
        return true;
    }
    internal void RequireLiJue(ProgramSkillFrame f)
    {
        if (f.LiJue is not { } r || r.Source != LiJueSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.WindowId != f.WindowContext?.ParentFrameId)
            throw new InvalidOperationException("LiJue lost its exact source, hash, instruction or actual native window.");
        if (r.Stage == LiJueStage.ChoosingTarget)
        {
            if (r.Roll is not null || r.TargetSeat is not null || r.OriginalUse is not null)
                throw new InvalidOperationException("Langxi cannot borrow a previously issued random receipt.");
            return;
        }

        if (r.Stage == LiJueStage.DamageChildren)
        {
            if (r.Roll is null || r.Roll.Value < _coreShanheS5LiJueOperations.ShanheS5LangxiMinimumDamage(f) || r.Roll.Value > 2 || r.TargetSeat is not { } target || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LangxiRandomResolvedEvent>().Count(e => e == new LangxiRandomResolvedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.WindowId, target, r.Roll.Value)) != 1)
                throw new InvalidOperationException("Langxi cannot resume an unissued or changed true random result.");
            return;
        }

        if (r.OriginalUse is not { } invoice || f.WindowContext?.MovementBatch is not { } batch || !ValidLiJueUseDiscard(invoice, batch, f.OwnerSeat, false) || r.MaxBefore < 1 || r.MaxAfter != r.MaxBefore - 1 || r.HpAfter != Math.Min(r.HpBefore, r.MaxAfter) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<YisuanMaximumHpPaidEvent>().Count(e => e == new YisuanMaximumHpPaidEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.WindowId, invoice.NativeUseId, invoice.Action.ActionId, r.MaxBefore, r.MaxAfter, r.HpBefore, r.HpAfter)) != 1 || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<MaximumHpChangedEvent>().Any(e => e.PlayerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.Delta == -1 && e.MaximumHp == r.MaxAfter))
            throw new InvalidOperationException("Yisuan lost its exact used entity or actually paid maximum HP.");
        if (r.Stage == LiJueStage.GainChildren && (r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.ActualGain < 0 || r.ActualGain > invoice.FinishedMovements.Count || f.PendingMovementContinuation is not { SubjectSeat: var actor, BeforeCount: 0, CoverageResultBind: null } || actor != f.OwnerSeat || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<YisuanGainPaidEvent>().Count(e => e == new YisuanGainPaidEvent(f.Id, r.Source, r.GameplayHash, invoice.NativeUseId, invoice.Action.ActionId, r.Before, r.After, r.ActualGain)) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == YisuanGainReason && invoice.FinishedMovements.Any(c => c.CardId == m.CardId)) != r.ActualGain))
            throw new InvalidOperationException("Yisuan cannot repeat a completed actual gain interval.");
    }

    SkillProgramStepOutcome global::CardGame.Core.ILiJueProgramHost.BeginLiJue(ProgramSkillFrame f, SkillProgramEffect e) => BeginLiJue(f, e);
}
