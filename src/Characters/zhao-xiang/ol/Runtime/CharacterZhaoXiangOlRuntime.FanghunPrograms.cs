#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoXiangOlRuntime
{
    internal bool HasMeiYingConversionCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GainMeiYingAfterActualSlashDamage);

    internal bool IsMeiYingConversion(CardConversionSource source) => HasMeiYingConversionCapability && _contentRegistryOperations._contentRegistry.ProgramDependencies.GetTriggerOperationSkillIds(SkillProgramEffectOp.GainMeiYingAfterActualSlashDamage).Contains(source.SkillId, StringComparer.Ordinal) && _contentRegistryOperations._contentRegistry.Skills.TryGetValue(source.SkillId, out var definition) && definition.Program is { } p && p.ViewAs.SingleOrDefault(r => r.Id == source.BindingId)is { InputCount: 1 } rule && (rule.OutputKind == CardKind.Slash && rule.InputKinds.SequenceEqual([CardKind.Dodge]) || rule.OutputKind == CardKind.Dodge && rule.InputKinds.SequenceEqual([CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash]));
    internal bool CanPayMeiYingConversion(CharacterState owner, CardConversionSource source) => !IsMeiYingConversion(source) || source.OwnerSeat == owner.Seat && owner.IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, source.SkillId, source.SkillInstanceId) && _coreSkillProgramsOperations.EnabledSkillPrograms(owner).Any(p => p.Id == source.SkillId) && owner.Markers.GetValueOrDefault(PlayerMarkerKind.MeiYing) > 0;
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 10)]
    internal void ObserveAcceptedMeiYingConversion(CardActionAcceptedEvent accepted)
    {
        if (accepted.Action.ConversionChain.Count != 0)
            PayMeiYingConversion(accepted.Action);
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.AcceptedConversionCost, 10)]
    internal void PayMeiYingConversion(CardActionContext action)
    {
        if (!HasMeiYingConversionCapability || action.ConversionChain.Count == 0)
            return;
        var sources = action.ConversionChain.Where(IsMeiYingConversion).ToArray();
        if (sources.Length == 0)
            return;
        var existing = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhaoXiangConversionPayment>().Where(e => e.ActionId == action.ActionId).ToArray();
        if (existing.Length > 0)
        {
            if (existing is [var paid] && sources is [var repeated] && paid.Source == repeated && paid.Source.OwnerSeat == action.ProviderSeat && paid.ActualActorSeat == action.ActorSeat && paid.ProviderSeat == action.ProviderSeat && paid.RequesterSeat == action.RequesterSeat && action.PhysicalCards is [var same] && same.CardId == paid.PhysicalCardId && same.CardKind == paid.InputKind && same.From == CardLocation.Hand(action.ProviderSeat) && action.EffectiveKind == paid.OutputKind && paid.Removed == 1 && paid.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(repeated.SkillId).Program?.GameplayHash && paid.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)
                return;
            throw new InvalidOperationException("A repeated accepted action changed its already paid MeiYing material or exact source.");
        }

        if (sources is not [var source] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(action.ActorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(action.ProviderSeat) || action.RequesterSeat is { } requester && requester != action.ActorSeat || source.OwnerSeat != action.ProviderSeat || action.PhysicalCards is not [var material] || material.From != CardLocation.Hand(action.ProviderSeat) || !CanPayMeiYingConversion(_coreMatchParticipantsPlayersAccess._players[action.ProviderSeat], source) || _corePlayCardHistoryOperations.ViewAsRule(source)is not { } rule || !ProgramCardKindMatch.Contains(rule.InputKinds, material.CardKind) || rule.OutputKind != action.EffectiveKind)
            throw new InvalidOperationException("MeiYing cost lost its exact accepted actor, one material and current conversion source.");
        var owner = _coreMatchParticipantsPlayersAccess._players[action.ProviderSeat];
        _coreProgramMarkerPaymentsOperations.PayProgramMarkerCost(owner, new(PlayerMarkerKind.MeiYing, 1), source.SkillId, source.BindingId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhaoXiangConversionPayment(action.ActionId, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, material.CardId, material.CardKind, action.EffectiveKind, 1, owner.Markers.GetValueOrDefault(PlayerMarkerKind.MeiYing)));
    }

    internal int ZhaoXiangCumulativeRemoved(int owner) => checked(_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhaoXiangConversionPayment>().Where(e => e.Source.OwnerSeat == owner).Sum(e => e.Removed) + _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhaoXiangExchangePaidEvent>().Where(e => e.Source.OwnerSeat == owner).Sum(e => e.Removed));
    internal bool CanGainMeiYingFromDamage(int owner, SkillProgramTrigger trigger, int source, int target, bool sourceLess) => !ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.GainMeiYingAfterActualSlashDamage) || target == owner || !sourceLess && source == owner;
    internal void GainMeiYingAfterActualSlashDamage(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = f.WindowContext;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (f.InstructionIndex != 1 || f.TriggerId is null || c is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, DamageFrameId: { } damageId } || index <= 0 || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame parent || parent.Id != c.ParentFrameId || parent.ParentFrameId != damageId || parent.SourceCard is not (CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash) || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex].ToProgramCandidate()) || !CanGainMeiYingFromDamage(f.OwnerSeat, _coreProgramLifecycleOperations.GetProgramTrigger(f), parent.SourceSeat, parent.TargetSeat, c.Facts?.DamageSourceIsOwner != true && c.SourceSeat is null) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhaoXiangMeiYingGainedEvent>().Any(e => e.DamageWindowId == parent.Id && e.Source.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("Proportional MeiYing lost one actual owner-related applied Slash damage.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var total = checked(owner.Markers.GetValueOrDefault(PlayerMarkerKind.MeiYing) + c.Amount);
        _coreStrategicProgramsOperations.SetStrategicMarker(f.OwnerSeat, PlayerMarkerKind.MeiYing, f.OwnerSeat, total, f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhaoXiangMeiYingGainedEvent(f.Id, parent.Id, damageId, new(f.SkillId, f.TriggerId, f.OwnerSeat, f.SkillInstanceId), c.Amount, total));
    }

    internal ProgramCardTriggerWindowFrame? ZhaoXiangConversionParent(ProgramSkillFrame f) => f.WindowContext?.ParentFrameId is { } id ? _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(p => p.Id == id) : null;
    internal ZhaoXiangConversionPayment? ZhaoXiangActionPayment(ProgramSkillFrame f)
    {
        var parent = ZhaoXiangConversionParent(f);
        if (parent is null || parent.Action.ProviderSeat != f.OwnerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex])))
            return null;
        var payments = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhaoXiangConversionPayment>().Where(e => e.ActionId == parent.Action.ActionId && e.Source.SkillId == f.SkillId && e.Source.SkillInstanceId == f.SkillInstanceId && e.Source.OwnerSeat == f.OwnerSeat && e.GameplayHash == f.GameplayHash && e.Removed == 1 && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.ActualActorSeat == parent.Action.ActorSeat && e.ProviderSeat == parent.Action.ProviderSeat && e.RequesterSeat == parent.Action.RequesterSeat && parent.Action.ConversionChain.Contains(e.Source) && parent.Action.PhysicalCards is [var card] && card.CardId == e.PhysicalCardId && card.CardKind == e.InputKind && parent.Action.EffectiveKind == e.OutputKind).ToArray();
        return payments is [var paid] ? paid : null;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 620)]
    internal bool CanOfferMeiYingConversionDraw(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.DrawAfterPaidMeiYingConversion) || context.CardUse is { } use && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhaoXiangConversionDrawPaidEvent>().Any(e => e.ActionId == use.CardActionId) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhaoXiangConversionPayment>().Any(p => p.ActionId == use.CardActionId && p.Source.OwnerSeat == candidate.OwnerSeat && p.ProviderSeat == candidate.OwnerSeat && p.ActualActorSeat == use.ActorSeat && p.Source.SkillId == candidate.SkillId && p.Source.SkillInstanceId == candidate.SkillInstanceId && p.GameplayHash == candidate.GameplayHash);
    internal SkillProgramStepOutcome DrawAfterPaidMeiYingConversion(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var paid = ZhaoXiangActionPayment(f);
        if (f.ZhaoXiang is not null || f.InstructionIndex != 1 || paid is null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhaoXiangConversionDrawPaidEvent>().Any(e => e.ActionId == paid.ActionId))
            throw new InvalidOperationException("MeiYing reward lost one accepted paid conversion action.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var r = new ZhaoXiangReceipt
        {
            Source = new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId),
            GameplayHash = f.GameplayHash,
            ParentFrameId = f.WindowContext!.ParentFrameId,
            Turn = _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber,
            InstructionIndex = f.InstructionIndex,
            Stage = "movement",
            ActionId = paid.ActionId,
            Before = before,
            After = before
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoXiang = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 1, null, null, SkillProgramCardSetVisibility.Private, new(ZhaoXiangDrawReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ZhaoXiangDrawReason);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoXiang = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualDraw = actual } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhaoXiangConversionDrawPaidEvent(f.Id, paid.ActionId, f.OwnerSeat, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, actual));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramCardLegality(ProgramCardLegalityStage.ConversionSourceBan, 10)]
    internal bool MeiYingConversionUnavailable(ProgramViewAsSourceQuery query) => !CanPayMeiYingConversion(query.Owner, query.Source);
}
