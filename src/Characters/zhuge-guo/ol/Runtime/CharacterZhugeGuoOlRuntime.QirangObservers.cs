#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeGuoOlRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 90)]
    internal void CaptureQirangDeclaredUse(IGameEvent payload)
    {
        if (!_characterZhugeGuoOlHasIssuedZhugeGuoStateAccess._hasIssuedZhugeGuoState && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ObtainDeckTrickForActualEquipmentUse))
            return;
        if (payload is not CardUseDeclaredEvent declared || _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is not { Action: { Type: CardActionType.Use } a } use)
            return;
        if (_coreEquipmentContentOperations.EquipmentRules.IsEquipment(a.EffectiveKind) && a.PhysicalCards is [var material] && _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ObtainDeckTrickForActualEquipmentUse) && _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat], SkillProgramTriggerWindow.CardUseTargetsFinalized).Any(c => ProgramInstructionResolver.Default.Features(_coreProgramLifecycleOperations.GetProgramTrigger(c)).HasOperation(SkillProgramEffectOp.ObtainDeckTrickForActualEquipmentUse)))
        {
            var origin = new QirangEquipmentOrigin(use.Id, a, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use = use with { QirangEquipmentOrigin = origin });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QirangEquipmentOriginCapturedEvent(use.Id, a.ActionId, a.ActorSeat, material.CardId, origin.MovementBefore));
        }

        if (!_characterZhugeGuoOlHasIssuedZhugeGuoStateAccess._hasIssuedZhugeGuoState || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(a.EffectiveKind) || a.ActorSeat != a.ProviderSeat || a.PhysicalCards is not [var cost] || cost.From != CardLocation.Hand(a.ActorSeat) || _cardZonesOperations._cardZones.GetLocation(cost.CardId) != cost.From || _coreMatchParticipantsPlayersAccess._players[a.ActorSeat].QirangEntityLeases is not { Count: > 0 } leases)
            return;
        var lease = leases.FirstOrDefault(l => l.CardId == cost.CardId && l.PrintedKind == a.EffectiveKind && l.OwnerSeat == a.ActorSeat && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat], l.Source.SkillId, l.Source.SkillInstanceId));
        if (lease is null)
            return;
        var q = new QirangUseQualification(lease, use.Id, a, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { QirangQualification = q });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QirangUseQualifiedEvent(use.Id, a.ActionId, a.ActorSeat, cost.CardId, lease.GainSequence, q.MovementBefore));
    }

    internal bool ValidQirangQualification(QirangUseQualification q)
    {
        var l = q.Lease;
        var a = q.OriginalAction;
        return l.Source.OwnerSeat == l.OwnerSeat && a.ActorSeat == l.OwnerSeat && a.ProviderSeat == l.OwnerSeat && a.Type == CardActionType.Use && a.PhysicalCards is [var physical] && physical.CardId == l.CardId && physical.From == CardLocation.Hand(l.OwnerSeat) && physical.CardKind == l.PrintedKind && a.EffectiveKind == l.PrintedKind && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(l.PrintedKind) && l.GainSequence > 0 && q.MovementBefore >= l.GainSequence && _contentRegistryOperations._contentRegistry.GetSkill(l.Source.SkillId).Program?.GameplayHash == l.GameplayHash && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QirangEntityLeaseIssuedEvent>().Count(e => e.Lease == l) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QirangUseQualifiedEvent>().Count(e => e == new QirangUseQualifiedEvent(q.CardUseFrameId, a.ActionId, l.OwnerSeat, l.CardId, l.GainSequence, q.MovementBefore)) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == l.CardId && m.Sequence == l.GainSequence && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(l.OwnerSeat) && m.Reason.Value == QirangGainReason) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == l.CardId && m.Sequence > q.MovementBefore && m.From == physical.From && m.To == CardLocation.Processing && m.Reason == (a.EffectiveKind == CardKind.IronChain ? CardMoveReasons.IronChainUse : CardMoveReasons.Use)) == 1;
    }
}
