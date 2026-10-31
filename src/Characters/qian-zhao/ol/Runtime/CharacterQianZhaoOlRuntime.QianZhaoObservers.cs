#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQianZhaoOlRuntime
{
    internal bool HasPaidWeifuInitialSelection(ProgramSkillFrame f)
    {
        if (f.Weifu is null)
            return false;
        AssertQianZhao(f);
        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 26620)]
    internal void AssertQianZhao(ProgramSkillFrame f)
    {
        if (f.Weifu is null && f.WeifuTargets is null && f.Kuansai is null)
            return;
        if ((f.Weifu is null ? 0 : 1) + (f.WeifuTargets is null ? 0 : 1) + (f.Kuansai is null ? 0 : 1) != 1 || f.InstructionIndex != 1 || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            throw new InvalidOperationException("Qian Zhao lost its unique owning instruction and content identity.");
        if (f.Weifu is { } w)
        {
            if (f.SkillId != "ol:weifu" || f.TriggerId is not null || f.WindowContext is not null || w.Source != QianZhaoSource(f) || w.GameplayHash != f.GameplayHash || w.InstructionIndex != f.InstructionIndex || w.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || w.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !Enum.IsDefined(w.Stage) || w.Before < 0 || w.After < w.Before || w.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || w.DrawCount is < 0 or > 1 || f.SelectedCardIds is not [var selected] || selected != w.CostCardId || f.SelectedTargetSeats.Count != 0 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveWeifuJudgment || w.CostFrom.OwnerSeat != f.OwnerSeat || w.CostFrom.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _coreProgramEventHistoryOperations.ProgramEventHistory<WeifuPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || paid.Source != w.Source || paid.GameplayHash != w.GameplayHash || paid.ActualTurn != w.ActualTurn || paid.TurnOwnerSeat != w.TurnOwnerSeat || paid.CardId != w.CostCardId || paid.From != w.CostFrom || paid.Category != w.CostCategory || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > paid.Before && m.Sequence <= paid.After && m.CardId == w.CostCardId && m.From == w.CostFrom && m.To == CardLocation.DiscardPile && m.Reason.Value == WeifuDiscardReason) != 1)
                throw new InvalidOperationException("Weifu lost its original legal HE cost, exact paid movement or actual turn.");
            if (w.Stage == WeifuStage.PaymentChildren ? w.JudgmentId is not null : w.JudgmentId is null)
                throw new InvalidOperationException("Weifu lost its owning native judgment cursor.");
            var results = _coreProgramEventHistoryOperations.ProgramEventHistory<WeifuJudgedEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (w.ResultCardId is { } card)
            {
                if (w.ResultKind is not { } kind || w.ResultCategory != global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind) || results is not [var result] || result != new WeifuJudgedEvent(f.Id, w.JudgmentId!.Value, card, kind, w.ResultCategory.Value) || _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentResolvedEvent>().Count(e => e.ResolutionId == w.JudgmentId && e.ParentResolutionId == f.Id && e.TargetSeat == f.OwnerSeat && e.Reason == WeifuJudgmentReason && e.CardId == card && e.CardKind == kind) != 1)
                    throw new InvalidOperationException("Weifu lost the finalized effective native judgment before its claim children.");
            }
            else if (results.Length != 0 || w.ResultKind is not null || w.ResultCategory is not null || w.Stage is WeifuStage.JudgmentChildren or WeifuStage.DrawChildren)
                throw new InvalidOperationException("Weifu cannot invent or omit its native result.");
            var grants = _coreProgramEventHistoryOperations.ProgramEventHistory<WeifuGrantedEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (w.Issued != (w.Stage == WeifuStage.DrawChildren) || grants.Length != (w.Issued && w.ResultCategory is SkillProgramCardCategory.Basic or SkillProgramCardCategory.Trick ? 1 : 0) || grants.Any(e => e.Source != w.Source || e.GameplayHash != w.GameplayHash || e.ActualTurn != w.ActualTurn || e.TurnOwnerSeat != w.TurnOwnerSeat || e.Category != w.ResultCategory || e.JudgmentId != w.JudgmentId) || _coreProgramEventHistoryOperations.ProgramEventHistory<WeifuDrawPaidEvent>().Count(e => e.FrameId == f.Id && e.Count == w.DrawCount) != (w.Issued ? 1 : 0))
                throw new InvalidOperationException("Weifu lost its once-issued category entitlement or actual draw payment.");
        }

        if (f.WeifuTargets is { } draft)
        {
            var use = RequireWeifuTargetUse(f);
            var width = use.CardKind == CardKind.BorrowedSword ? 2 : 1;
            if (draft.Source != QianZhaoSource(f) || draft.GameplayHash != f.GameplayHash || draft.InstructionIndex != f.InstructionIndex || draft.UseId != use.Id || draft.ActionId != use.Action!.ActionId || use.WeifuUse!.Offered || draft.Maximum != use.WeifuUse.Grants.Count || draft.Selected.Count % width != 0 || draft.Selected.Count / width > draft.Maximum || draft.Selected.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)))
                throw new InvalidOperationException("Weifu lost its issued category stack and immutable extra-target draft.");
        }

        if (f.Kuansai is { } k)
        {
            if (f.SkillId != "ol:kuansai" || f.TriggerId is null || f.WindowContext is not { } c || KuansaiUse(c)is not { } use || k.Source != QianZhaoSource(f) || k.GameplayHash != f.GameplayHash || k.InstructionIndex != f.InstructionIndex || k.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || k.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || k.UseId != use.Id || k.ActionId != use.Action!.ActionId || !Enum.IsDefined(k.Stage) || k.Targets.Distinct().Count() != k.Targets.Count || !k.Targets.SequenceEqual(KuansaiActualTargets(use)) || k.Targets.Count < k.ThresholdHp || k.Before < 0 || k.After < k.Before || k.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveKuansaiTargetChoice || _coreProgramEventHistoryOperations.ProgramEventHistory<KuansaiStartedEvent>().Count(e => e == new KuansaiStartedEvent(f.Id, k.Source, k.GameplayHash, k.ActualTurn, k.TurnOwnerSeat, k.UseId, k.ActionId, k.ThresholdHp, k.Targets.Count)) != 1 || (k.Stage == KuansaiStage.Target ? k.ChooserSeat is not null : k.ChooserSeat is not { } chooser || !k.Targets.Contains(chooser)) || _coreProgramEventHistoryOperations.ProgramEventHistory<KuansaiTargetChosenEvent>().Count(e => e.FrameId == f.Id && e.TargetSeat == k.ChooserSeat) != (k.Stage == KuansaiStage.Target ? 0 : 1))
                throw new InvalidOperationException("Kuansai lost the actual finalized targets, threshold or participant-owned choice.");
            if (k.Stage == KuansaiStage.Children)
            {
                if (k.Gift is not { } gift || k.ChooserSeat is not { } payer || _coreProgramEventHistoryOperations.ProgramEventHistory<KuansaiPaidEvent>().Count(e => e == new KuansaiPaidEvent(f.Id, payer, gift, k.CardId, k.From, k.Before, k.After)) != 1 || (gift ? k.CardId is not { } id || k.From is not { } from || from.OwnerSeat != payer || payer == f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > k.Before && m.Sequence <= k.After && m.CardId == id && m.From == from && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == KuansaiGiftReason) != 1 : k.CardId is not null || k.From is not null))
                    throw new InvalidOperationException("Kuansai lost its once-paid genuine HE transfer or recovery attempt.");
            }
            else if (k.Gift is not null || k.CardId is not null || k.From is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<KuansaiPaidEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("Kuansai cannot pay before its chosen participant submits a benefit.");
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } p && (f.WeifuTargets is not null || f.Kuansai is { Stage: not KuansaiStage.Children }))
        {
            var privateChoice = f.Kuansai is { Stage: KuansaiStage.Benefit };
            var chooser = privateChoice ? f.Kuansai!.ChooserSeat!.Value : f.OwnerSeat;
            var choices = f.WeifuTargets is not null ? WeifuTargetChoices(f) : KuansaiChoices(f);
            if (p.PlayerSeat != chooser || p.IsPrivate != privateChoice || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, choices))
                throw new InvalidOperationException("Qian Zhao lost its exact chooser and private HE identity boundary.");
        }
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26620)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26620)]
    internal bool ContinuePaidQianZhao(ProgramSkillFrame f)
    {
        if (f.Weifu is null && f.WeifuTargets is null && f.Kuansai is null && f.InstructionIndex is 0 or 1 && f.TriggerId is { } binding && f.ActivationId == binding && f.WindowContext is { CardUse: { } card } context && _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)?.WeifuUse is { Offered: false } && HasIssuedWeifuCandidate(new(f.OwnerSeat, f.SkillId, binding, f.SkillInstanceId, f.GameplayHash, 0), context))
            return true;
        if (f.Weifu is null && f.WeifuTargets is null && f.Kuansai is not { Stage: KuansaiStage.Children })
            return false;
        AssertQianZhao(f);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 150)]
    internal bool IsQianZhaoMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p)
    {
        var matches = p is { BeforeCount: 0, CoverageResultBind: null } && p.SubjectSeat == f.OwnerSeat && (e?.Op == SkillProgramEffectOp.ResolveWeifuJudgment && f.Weifu is { Stage: not WeifuStage.Judging } || e?.Op == SkillProgramEffectOp.ResolveKuansaiTargetChoice && f.Kuansai is { Stage: KuansaiStage.Children });
        if (matches)
            AssertQianZhao(f);
        return matches;
    }

    internal bool QianZhaoSilverLion(ProgramSkillFrame f, int target, CardMoveReason? reason) => f.Weifu is { Stage: WeifuStage.PaymentChildren, CostFrom.Zone: CardZoneKind.Equipment } w && target == f.OwnerSeat && reason?.Value == WeifuDiscardReason && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == w.CostCardId && m.CardKind == CardKind.SilverLion && m.From == w.CostFrom && m.Reason.Value == WeifuDiscardReason) || f.Kuansai is { Stage: KuansaiStage.Children, Gift: true, From: { Zone: CardZoneKind.Equipment } } k && target == k.ChooserSeat && reason?.Value == KuansaiGiftReason && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > k.Before && m.Sequence <= k.After && m.CardId == k.CardId && m.CardKind == CardKind.SilverLion && m.Reason.Value == KuansaiGiftReason);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 680)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 620)]
    internal bool IsQianZhaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => (f.Weifu is not null || f.Kuansai is not null) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Amount == 1 && (f.Kuansai is { Stage: KuansaiStage.Children, Gift: false } && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == f.InstructionIndex || recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && QianZhaoSilverLion(f, recovery.Attempt.TargetSeat, recovery.Attempt.Completion.MoveReason));
    internal bool QianZhaoFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Weifu is null && f.Kuansai is null)
            return false;
        AssertQianZhao(f);
        if (child is JudgmentFrame judgment)
            return f.Weifu is { Stage: WeifuStage.Judging } && IsWeifuJudgment(judgment, f);
        if (f.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (!global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) || moved.Batch.OriginOwnerSeat != f.OwnerSeat || moved.Batch.OriginSkillId != f.SkillId || moved.Batch.OriginSkillInstanceId != f.SkillInstanceId || moved.Batch.Movements.Count == 0)
                return false;
            var before = f.Weifu?.Before ?? f.Kuansai!.Before;
            var after = f.Weifu?.After ?? f.Kuansai!.After;
            return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > before && m.Sequence <= after && (f.Weifu is not null ? m.Reason.Value is WeifuDiscardReason or WeifuDrawReason or "skill-program.judgment.result" || m.Reason == CardMoveReasons.JudgmentFinish || m.Reason == CardMoveReasons.Reshuffle : m.Reason.Value == KuansaiGiftReason && m.CardId == f.Kuansai!.CardId && m.From == f.Kuansai.From && m.To == CardLocation.Hand(f.OwnerSeat)));
        }

        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is RecoveryReplacementFrame recovery)
            return IsQianZhaoRecoveryParent(f, recovery);
        if (child is not HpChangedTriggerWindowFrame hp || hp.Change.ParentFrameId != f.Id || hp.ResumeFrameId != f.Id || hp.Continuation != PostEventContinuation.AwaitedProgramMovement || hp.Change.Kind != HpChangeKind.Recovery || hp.Change.Amount != 1)
            return false;
        return f.Kuansai is { Stage: KuansaiStage.Children, Gift: false } && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat || hp.Change.SourceSeat == hp.Change.TargetSeat && QianZhaoSilverLion(f, hp.Change.TargetSeat, new(f.Weifu is not null ? WeifuDiscardReason : KuansaiGiftReason));
    }

    internal ProgramSkillFrame? QianZhaoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterQianZhaoOlOperations.QianZhaoFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 480)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 590)]
    internal bool HasQianZhaoDamageObserver(long id) => QianZhaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 620)]
    internal bool HasQianZhaoBeforeDamageObserver(long id) => QianZhaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 450)]
    internal bool IsQianZhaoDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && QianZhaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26620)]
    internal bool AdvanceQianZhaoSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && QianZhaoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
