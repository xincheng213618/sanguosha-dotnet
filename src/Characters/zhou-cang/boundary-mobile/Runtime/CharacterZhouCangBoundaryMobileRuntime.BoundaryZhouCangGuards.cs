#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouCangBoundaryMobileRuntime
{
    internal static bool BoundaryZhongyongChildren(BoundaryZhongyongStage stage) => stage is BoundaryZhongyongStage.AcquireSlashChildren or BoundaryZhongyongStage.AcquireDodgeChildren or BoundaryZhongyongStage.GiveSlashChildren or BoundaryZhongyongStage.GiveDodgeChildren;
    internal bool BoundaryZhongyongFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BoundaryZhongyong is not { } r || !BoundaryZhongyongChildren(r.Stage))
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed)
            return changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && r.Settlement!.Movements.Contains(m));
    }

    internal ProgramSkillFrame? BoundaryZhongyongObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.BoundaryZhongyongFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));

    internal bool IsBoundaryZhouCangPhysicalClaim(long useId, int cardId)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { Action: { } a } use || !a.PhysicalCards.Any(p => p.CardId == cardId))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhongyongStartedEvent>().Any(s => s.UseId == use.Id && s.ActionId == a.ActionId && s.Source.SkillId == BoundaryZhouCangComposition.Skill && s.Source.BindingId == BoundaryZhouCangComposition.Binding && s.Source.OwnerSeat == a.ActorSeat && _contentRegistryOperations._contentRegistry.GetSkill(s.Source.SkillId).Program?.GameplayHash == s.Hash && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhongyongMovementEvent>().Any(e => e.FrameId == s.FrameId && e.CardId == cardId && e.Stage is BoundaryZhongyongStage.AcquireSlashChildren or BoundaryZhongyongStage.GiveSlashChildren && e.From.Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == e.CardId && m.From == e.From && m.To == e.To && m.Sequence == e.Sequence && m.Reason.Value == BoundaryZhongyongReason)));
    }

    internal bool BoundaryZhongyongUseForbidden(int actor, IReadOnlyList<int> ids) => HasBoundaryZhouCang && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhongyongUseBannedEvent>().Any(e => e.Owner == actor && e.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && ids.Contains(e.CardId) && _cardZonesOperations._cardZones.GetLocation(e.CardId).OwnerSeat == actor && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.To == CardLocation.Hand(actor) && m.Reason.Value == BoundaryZhongyongReason));
    internal bool BoundaryZhongyongResponseIsUse(int actor, CardKind kind, bool dying = false) => dying || kind == CardKind.Nullification || _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[actor], kind);
    [ProgramCardLegality(ProgramCardLegalityStage.MaterialUseBan, 53300)]
    internal bool BoundaryZhongyongMaterialBan(ProgramCardMaterialQuery q) => (!q.IsResponse || BoundaryZhongyongResponseIsUse(q.ActorSeat, q.Kind)) && BoundaryZhongyongUseForbidden(q.ActorSeat, q.CardIds.ToArray());
    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 53300)]
    internal bool BoundaryZhongyongNativeResponseBan(ProgramNativeResponseQuery q) => BoundaryZhongyongResponseIsUse(q.Provider.Seat, q.Kind, q.DyingUse) && BoundaryZhongyongUseForbidden(q.Provider.Seat, q.CardIds);
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 53300)]
    internal bool BoundaryZhongyongViewAsBan(ProgramViewAsLegalityQuery q) => (!q.ForResponse || q.RequestedUseTarget is not null || BoundaryZhongyongResponseIsUse(q.Owner.Seat, q.Kind, q.DyingUse)) && BoundaryZhongyongUseForbidden(q.Owner.Seat, q.CardIds);
    [ProgramCardLegality(ProgramCardLegalityStage.AfterLegalActionKind, 53300)]
    internal bool BoundaryZhongyongLegalBan(ProgramLegalActionQuery q) => q.Action.Kind is not (LegalActionKind.Recast or LegalActionKind.UseProgramSkill or LegalActionKind.EndPlay or LegalActionKind.UseEquipmentEffect) && q.Action.CardId is { } id && BoundaryZhongyongUseForbidden(q.Actor.Seat, [id]);
    [ProgramCardLegality(ProgramCardLegalityStage.SubmittedUseValidation, 53300)]
    internal void BoundaryZhongyongSubmittedBan(ProgramSubmittedUseQuery q)
    {
        if (BoundaryZhongyongUseForbidden(q.ActorSeat, q.CardIds))
            throw new InvalidOperationException("Zhongyong's obtained physical entities cannot be used during this actual turn.");
    }
}
