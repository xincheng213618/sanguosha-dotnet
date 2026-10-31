#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramNativeSlashQueriesRuntime
{
    internal bool IsNativePureProviderFrontier(int actor)
    {
        var top = _resolutionStackOperations._resolutionStack.LastOrDefault();
        bool At(long id) => top?.Id == id || top is ResponseWindowFrame response && response.ParentFrameId == id;
        return _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { AwaitingProviders: true } f && f.CurrentCandidateSeat == actor && At(f.OwnerFrameId) || _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } d && d.CurrentCandidateSeat == actor && At(d.Attack.ResolutionId);
    }

    internal bool IsNativeForcedSlashUseFrontier(CharacterState owner)
    {
        if (IsNativePureProviderFrontier(owner.Seat))
            return false;
        var top = _resolutionStackOperations._resolutionStack.LastOrDefault();
        bool At(long id) => top?.Id == id || top is ResponseWindowFrame response && response.ParentFrameId == id;
        if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { ActiveAttack: null } borrowed && borrowed.WeaponOwnerSeat == owner.Seat && At(borrowed.ResolutionId))
            return true;
        if (_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } qinglong && qinglong.Attack.SourceSeat == owner.Seat && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(qinglong.Attack, _coreActiveNativeAttackQueriesOperations.ActiveCardAttack) && At(qinglong.Attack.ResolutionId))
            return true;
        if (top is not ProgramSkillFrame f || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)is not { } effect)
            return false;
        if (_characterLvKuangLvXiangOlOperations.IsQigongRequest(f))
            return f.LvKuangLvXiang!.Actor == owner.Seat;
        if (_characterCaoChunOlOperations.IsCaoChunInitialRequest(f) || _characterWuJingOlOperations.IsWuJingHejiRequest(f))
            return f.OwnerSeat == owner.Seat;
        return _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramActor(f, effect.Op, owner.Seat);
    }
}
