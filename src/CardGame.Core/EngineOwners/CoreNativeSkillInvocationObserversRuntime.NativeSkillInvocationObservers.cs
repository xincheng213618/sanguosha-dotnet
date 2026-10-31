#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeSkillInvocationObserversRuntime
{
    internal global::CardGame.Core.GameEngine.NativeFactionSkillSuspensionProjection? FindNativeFactionSkillInvocation(long requestOwnerId, int ownerSeat, FactionResponsePolicySource source, string occurrenceKey, SkillInvocationReturnKind returnKind)
    {
        for (var index = 0; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            var parent = _resolutionStackOperations._resolutionStack[index];
            var sameOwner = parent.Id == requestOwnerId || parent is ResponseWindowFrame response && response.ParentFrameId == requestOwnerId && index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == requestOwnerId && _resolutionStackOperations._resolutionStack[index - 1].Step == ResolutionFrameStep.AwaitingResponse;
            if (!sameOwner || _resolutionStackOperations._resolutionStack[index + 1] is not SkillInvocationFrame f || f.ParentFrameId != parent.Id || f.ActorSeat != ownerSeat || f.SkillKey != source.SkillId || f.SkillInstanceId != source.SkillInstanceId || f.OccurrenceKey != occurrenceKey || f.ReturnKind != returnKind || !_characterHuaTuoAnniversaryGodOperations.SkillInvocationStructuralEdge(parent, f) || _coreProgramEventHistoryOperations.ProgramEventHistory<SkillInvocationStartedEvent>().Count(e => e == new SkillInvocationStartedEvent(f.Id, parent.Id, ownerSeat, source.SkillId, occurrenceKey, source.SkillInstanceId, f.SkillName, f.GameplayHash, f.RoundNumber, f.TurnNumber, f.TurnOwnerSeat, returnKind)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<SkillInvocationCompletedEvent>().Any(e => e.FrameId == f.Id) || !_coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreNativeEquipmentInvocationsOperations.NativeEquipmentObserverEdge(child), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying)))
                continue;
            return new(parent, f, index);
        }

        return null;
    }

    internal bool HasFactionDefenseSkillInvocation(global::CardGame.Core.NativeOwners.FactionDefenseHandle defense)
    {
        if (defense.Source.RequiredKind != CardKind.Dodge || defense.CandidateIndex < 0 || defense.CandidateIndex >= defense.CandidateSeats.Count || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense, defense) || FindNativeFactionSkillInvocation(defense.OwnerFrameId, defense.OwnerSeat, defense.Source, defense.InvocationOccurrenceKey, SkillInvocationReturnKind.FactionDefense)is not { } projection || projection.Parent is not ResponseWindowFrame { Step: ResolutionFrameStep.AwaitingResponse } response || response.ParentFrameId != defense.OwnerFrameId || response.ResponderSeat != defense.OwnerSeat || response.RequiredCardKind != CardKind.Dodge || response.IncomingCard != defense.Attack.EffectiveCardKind || defense.OwnerSeat != defense.Attack.TargetSeat || !defense.Attack.FactionDefenseAttempted || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, defense.Attack))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<FactionDefenseRequestedEvent>().LastOrDefault(e => e.ResolutionId == defense.OwnerFrameId)is { } issued && issued.OwnerSeat == defense.OwnerSeat && issued.SkillId == defense.Source.SkillId && issued.CandidateSeats.SequenceEqual(defense.CandidateSeats);
    }

    internal bool HasFactionSlashSkillInvocation(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request)
    {
        if (request.IsProgramSkillUse || request.PolicySource is not { RequiredKind: CardKind.Slash } source || source.FactionId != request.ProviderFactionId || request.RequiredKind != CardKind.Slash || !request.AwaitingProviders || request.AwaitingZhuqueFanChoice || request.ActiveAttack is not null || request.CandidateIndex < 0 || request.CandidateIndex >= request.CandidateSeats.Count || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest, request) || FindNativeFactionSkillInvocation(request.OwnerFrameId, request.OwnerSeat, source, request.InvocationOccurrenceKey, SkillInvocationReturnKind.FactionSlash)is not { } projection || !NativeFactionSlashInvocationHostMatches(request, projection.Parent))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<FactionSlashRequestedEvent>().LastOrDefault(e => e.ResolutionId == request.OwnerFrameId)is { } issued && issued.OwnerSeat == request.OwnerSeat && issued.SkillId == source.SkillId && issued.CandidateSeats.SequenceEqual(request.CandidateSeats) && issued.IsActiveUse == (request.IsBorrowedSwordUse || request.IsQinglongCrescentBladeUse) && issued.TargetSeat == request.TargetSeat;
    }

    internal bool NativeFactionSlashInvocationHostMatches(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, ResolutionFrame parent)
    {
        if (request.IsQinglongCrescentBladeUse)
            return parent is CardUseFrame { Step: ResolutionFrameStep.AwaitingResponse } use && use.Id == request.OwnerFrameId && request.QinglongCrescentBlade is { } qinglong && qinglong.OwnerFrameId == use.Id && qinglong.FactionSlashAttempted && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade, qinglong) && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, qinglong.Attack) && qinglong.Attack.SourceSeat == request.OwnerSeat && qinglong.Attack.TargetSeat == request.TargetSeat;
        if (parent is not ResponseWindowFrame { Step: ResolutionFrameStep.AwaitingResponse } response || response.ParentFrameId != request.OwnerFrameId || response.ResponderSeat != request.OwnerSeat || response.RequiredCardKind != CardKind.Slash)
            return false;
        if (request.IsBorrowedSwordUse)
            return response.IncomingCard == CardKind.BorrowedSword && request.BorrowedSword is { } borrowed && borrowed.ResolutionId == request.OwnerFrameId && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, borrowed) && !borrowed.AwaitingSlashChoice && borrowed.FactionSlashAttempted && borrowed.ActiveAttack is null && borrowed.WeaponOwnerSeat == request.OwnerSeat && borrowed.SlashTargetSeat == request.TargetSeat;
        if (request.ResponseAttack is not { } attack || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || attack.ResolutionId != request.OwnerFrameId || response.IncomingCard != attack.EffectiveCardKind)
            return false;
        return request.Purpose switch
        {
            FactionCardRequestPurpose.DuelResponse => _coreNativeCardContinuationCursorsOperations.ActiveDuel is { FactionSlashAttempted: true } duel && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(duel.Attack, attack) && duel.ResponderSeat == request.OwnerSeat && response.SourceSeat == duel.OpponentSeat && response.IncomingCard == CardKind.Duel,
            FactionCardRequestPurpose.GroupResponse => _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack, RequiredCardKind: CardKind.Slash } group && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(group.CurrentAttack, attack) && attack.FactionSlashAttempted && request.OwnerSeat == attack.TargetSeat && response.SourceSeat == group.SourceSeat,
            _ => false
        };
    }

    internal bool HasNativeFactionSkillInvocation(long attackId) => _coreActiveNativeAttackQueriesOperations.ActiveCardAttack?.ResolutionId == attackId && (_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } defense && defense.OwnerFrameId == attackId && HasFactionDefenseSkillInvocation(defense) || _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { } request && (request.ResponseAttack?.ResolutionId == attackId || request.QinglongCrescentBlade?.OwnerFrameId == attackId) && HasFactionSlashSkillInvocation(request));
}
