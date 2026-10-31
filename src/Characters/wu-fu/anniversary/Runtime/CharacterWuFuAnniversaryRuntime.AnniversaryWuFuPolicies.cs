#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuFuAnniversaryRuntime
{
    internal bool WuFuPhaseOpen(ActualDiscardRecoveryPhaseKey phase) => phase.Kind == ActualDiscardRecoveryPhaseKind.Play && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase() == phase && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) && !_coreProgramEventHistoryOperations.ProgramEventHistory<ActualPhaseEndedEvent>().Any(e => e.Phase == phase);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 46601)]
    internal void CaptureWuFuPhaseGain(IGameEvent fact)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveAnniversaryWuFuJuekai) || fact is not CardMovedEvent { To.Zone: CardZoneKind.Hand, To.OwnerSeat: { } seat } moved || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || !WuFuPhaseOpen(phase) || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryWuFuPhaseGainEvent>().Any(e => e.Phase == phase && e.Recipient == seat))
            return;
        var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == moved.CardId && m.From == moved.From && m.To == moved.To && m.Reason == moved.Reason);
        if (movement is null || _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(movement) == CardLocation.Hand(seat))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryWuFuPhaseGainEvent(phase, seat, movement.Sequence));
    }

    internal bool IsAnniversaryWuFuHandUseForbidden(int seat, IReadOnlyList<int> ids) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveAnniversaryWuFuJuekai) && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryWuFuJuekaiEvent>().Any(e => WuFuPhaseOpen(e.Phase) && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryWuFuPhaseGainEvent>().Any(g => g.Phase == e.Phase && g.Recipient == seat)) && ids.Any(id => id > 0 && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(seat));
    internal bool WuFuResponseIsUse(int actor, CardKind kind, bool dying = false) => dying || kind == CardKind.Nullification || _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[actor], kind);
    [ProgramCardLegality(ProgramCardLegalityStage.MaterialUseBan, 46601)]
    internal bool WuFuMaterialBan(ProgramCardMaterialQuery q) => (!q.IsResponse || WuFuResponseIsUse(q.ActorSeat, q.Kind)) && IsAnniversaryWuFuHandUseForbidden(q.ActorSeat, q.CardIds.ToArray());
    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 46601)]
    internal bool WuFuNativeResponseBan(ProgramNativeResponseQuery q) => WuFuResponseIsUse(q.Provider.Seat, q.Kind, q.DyingUse) && IsAnniversaryWuFuHandUseForbidden(q.Provider.Seat, q.CardIds);
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 46601)]
    internal bool WuFuViewAsBan(ProgramViewAsLegalityQuery q) => (!q.ForResponse || q.RequestedUseTarget is not null || WuFuResponseIsUse(q.Owner.Seat, q.Kind, q.DyingUse)) && IsAnniversaryWuFuHandUseForbidden(q.Owner.Seat, q.CardIds);
    [ProgramCardLegality(ProgramCardLegalityStage.AfterLegalActionKind, 46601)]
    internal bool WuFuLegalUseBan(ProgramLegalActionQuery q) => q.Action.Kind is not (LegalActionKind.Recast or LegalActionKind.UseProgramSkill or LegalActionKind.EndPlay or LegalActionKind.UseEquipmentEffect) && q.Action.CardId is { } id && IsAnniversaryWuFuHandUseForbidden(q.Actor.Seat, [id]);
    [ProgramCardLegality(ProgramCardLegalityStage.SubmittedUseValidation, 46601)]
    internal void WuFuSubmittedUseBan(ProgramSubmittedUseQuery q)
    {
        if (IsAnniversaryWuFuHandUseForbidden(q.ActorSeat, q.CardIds))
            throw new InvalidOperationException("Juekai prohibits Hand use by actual recipients until this actual Play phase ends.");
    }
}
