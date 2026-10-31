#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeCardContinuationCursorsRuntime
{
    internal NativeOwners.DuelHandle? ActiveDuel
    {
        get => _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.Duel?.Active == true)is { } id ? _kernelCoreMainOperations.CreateDuelHandle(id) : null;
        set
        {
            var id = value?.OwnerFrameId ?? _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.Duel?.Active == true);
            if (id is { } owner)
                _coreCardAttackStateOperations.UpdateCardContinuations(owner, state => state with { Duel = state.Duel is { } current ? current with { Active = value is not null } : null });
        }
    }

    internal NativeOwners.GroupCardHandle? ActiveGroupCard
    {
        get => _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.GroupCard?.Active == true)is { } id ? _kernelCoreMainOperations.CreateGroupCardHandle(id) : null;
        set
        {
            var id = value?.OwnerFrameId ?? _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.GroupCard?.Active == true);
            if (id is { } owner)
                _coreCardAttackStateOperations.UpdateCardContinuations(owner, state => state with { GroupCard = state.GroupCard is { } current ? current with { Active = value is not null } : null });
        }
    }

    internal NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword
    {
        get => _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.BorrowedSword?.Active == true)is { } id ? _kernelCoreMainOperations.CreateBorrowedSwordHandle(id) : null;
        set
        {
            var id = value?.OwnerFrameId ?? _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.BorrowedSword?.Active == true);
            if (id is { } owner)
                _coreCardAttackStateOperations.UpdateCardContinuations(owner, state => state with { BorrowedSword = state.BorrowedSword is { } current ? current with { Active = value is not null } : null });
        }
    }

    internal NativeOwners.StoneAxeHandle? ActiveStoneAxe
    {
        get => _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.StoneAxe?.Active == true)is { } id ? _kernelCoreMainOperations.CreateStoneAxeHandle(id) : null;
        set
        {
            var id = value?.OwnerFrameId ?? _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.StoneAxe?.Active == true);
            if (id is { } owner)
                _coreCardAttackStateOperations.UpdateCardContinuations(owner, state => state with { StoneAxe = state.StoneAxe is { } current ? current with { Active = value is not null } : null });
        }
    }

    internal NativeOwners.CixiongDoubleSwordsHandle? ActiveCixiongDoubleSwords
    {
        get => _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.CixiongDoubleSwords?.Active == true)is { } id ? _kernelCoreMainOperations.CreateCixiongDoubleSwordsHandle(id) : null;
        set
        {
            var id = value?.OwnerFrameId ?? _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.CixiongDoubleSwords?.Active == true);
            if (id is { } owner)
                _coreCardAttackStateOperations.UpdateCardContinuations(owner, state => state with { CixiongDoubleSwords = state.CixiongDoubleSwords is { } current ? current with { Active = value is not null } : null });
        }
    }

    internal NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade
    {
        get => _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.QinglongCrescentBlade?.Active == true)is { } id ? _kernelCoreMainOperations.CreateQinglongCrescentBladeHandle(id) : null;
        set
        {
            var id = value?.OwnerFrameId ?? _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.QinglongCrescentBlade?.Active == true);
            if (id is { } owner)
                _coreCardAttackStateOperations.UpdateCardContinuations(owner, state => state with { QinglongCrescentBlade = state.QinglongCrescentBlade is { } current ? current with { Active = value is not null } : null });
        }
    }

    internal NativeOwners.QinglongFollowupHandle? ActiveQinglongFollowup
    {
        get => _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.QinglongFollowup?.Active == true)is { } id ? _kernelCoreMainOperations.CreateQinglongFollowupHandle(id) : null;
        set
        {
            var id = value?.OwnerFrameId ?? _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.QinglongFollowup?.Active == true);
            if (id is { } owner)
                _coreCardAttackStateOperations.UpdateCardContinuations(owner, state => state with { QinglongFollowup = state.QinglongFollowup is { } current ? current with { Active = value is not null } : null });
        }
    }

    internal NativeOwners.IceSwordHandle? ActiveIceSword
    {
        get => _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.IceSword?.Active == true)is { } id ? _kernelCoreMainOperations.CreateIceSwordHandle(id) : null;
        set
        {
            var id = value?.OwnerFrameId ?? _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.IceSword?.Active == true);
            if (id is { } owner)
            {
                var discard = value is null ? _coreCardAttackStateOperations.GetCardContinuations(owner).IceSword?.LogicalDiscardInvocation : null;
                _coreCardAttackStateOperations.UpdateCardContinuations(owner, state => state with { IceSword = state.IceSword is { } current ? current with { Active = value is not null, LogicalDiscardInvocation = value is null ? null : current.LogicalDiscardInvocation } : null });
                _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(discard);
            }
        }
    }

    internal NativeOwners.QilinBowHandle? ActiveQilinBow
    {
        get => _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.QilinBow?.Active == true)is { } id ? _kernelCoreMainOperations.CreateQilinBowHandle(id) : null;
        set
        {
            var id = value?.OwnerFrameId ?? _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.QilinBow?.Active == true);
            if (id is { } owner)
                _coreCardAttackStateOperations.UpdateCardContinuations(owner, state => state with { QilinBow = state.QilinBow is { } current ? current with { Active = value is not null } : null });
        }
    }

    internal NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd
    {
        get => _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.FangtianHalberd?.Active == true)is { } id ? _kernelCoreMainOperations.CreateFangtianHalberdHandle(id) : null;
        set
        {
            var id = value?.OwnerFrameId ?? _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.FangtianHalberd?.Active == true);
            if (id is { } owner)
                _coreCardAttackStateOperations.UpdateCardContinuations(owner, state => state with { FangtianHalberd = state.FangtianHalberd is { } current ? current with { Active = value is not null } : null });
        }
    }

    internal NativeOwners.FactionDefenseHandle? ActiveFactionDefense
    {
        get => _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.FactionDefense?.Active == true)is { } id ? _kernelCoreMainOperations.CreateFactionDefenseHandle(id) : null;
        set
        {
            var id = value?.OwnerFrameId ?? _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.FactionDefense?.Active == true);
            if (id is { } owner)
                _coreCardAttackStateOperations.UpdateCardContinuations(owner, state => state with { FactionDefense = state.FactionDefense is { } current ? current with { Active = value is not null } : null });
        }
    }

    internal NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest
    {
        get => _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.FactionCardRequest?.Active == true)is { } id ? _kernelCoreMainOperations.CreateFactionCardRequestHandle(id) : null;
        set
        {
            var id = value?.OwnerFrameId ?? _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.FactionCardRequest?.Active == true);
            if (id is { } owner)
                _coreCardAttackStateOperations.UpdateCardContinuations(owner, state => state with { FactionCardRequest = state.FactionCardRequest is { } current ? current with { Active = value is not null } : null });
        }
    }

    internal TargetCardSelectionFrame? ActiveTargetCardSelection => _resolutionStackOperations._resolutionStack.OfType<TargetCardSelectionFrame>().LastOrDefault();
    internal NullificationWindowFrame? ActiveNullificationWindow => _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().LastOrDefault(window => !_characterXiaoQiaoMouOlOperations.MouXiaoQiaoOl.PublicRevealSuspends(window.Id) && !_characterDiaoChanLeOperations.IsDiouSuspendedCounterspell(window) && !_characterLiuChengMouMobileOperations.IsMouLiuChengSuspendedCounterspell(window));
    internal CardUseFrame? ActiveFireAttack => _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { FireAttackSelection: not null } frame ? frame : null;
}
