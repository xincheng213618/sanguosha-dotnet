#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoXiaoQiaoHappyGodRuntime
{
    internal HappyQiaoCardMaximumProof? PrepareHappyQiaoCardMaximumHp(global::CardGame.Core.GameEngine.IDamageAttempt attack, int newMaximum)
    {
        if (!HasHappyQiaoHealth(attack.TargetSeat))
            return null;
        if (attack is not global::CardGame.Core.NativeOwners.CardAttackHandle || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != attack.ResolutionId || attack.DamageWasApplied || attack.DamageAmountFinalized || attack.IsChainPropagation || _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).MaximumHpReplacement is not null || newMaximum < 0)
            throw new InvalidOperationException("A Qiao card maximum replacement requires the real unpaid native attack.");
        var before = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].HappyQiaoHealth!;
        return new(before, before.WithMaximum(newMaximum));
    }

    internal bool MatchesHappyQiaoCardMaximumHp(CardDamageMaximumHpReplacementReceipt receipt) => receipt is { HpBefore: null, HpAfter: null, HappyQiaoMaximumProof: { } proof } && HasHappyQiaoHealth(receipt.TargetSeat) && receipt.MaximumHpBefore == proof.Before.MaximumHp && receipt.MaximumHpAfter == proof.After.MaximumHp && receipt.MaximumHpBefore > 0 && receipt.MaximumHpAfter >= 0 && proof.After == proof.Before.WithMaximum(receipt.MaximumHpAfter);
    internal bool ApplyHappyQiaoCardMaximumHp(long ownerId, CardDamageMaximumHpReplacementReceipt receipt)
    {
        if (receipt.HappyQiaoMaximumProof is not { } proof)
            return false;
        if (!MatchesHappyQiaoCardMaximumHp(receipt) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != ownerId || _coreCardAttackStateOperations.GetCardAttackState(ownerId)is not { } attack || attack.MaximumHpReplacement != receipt || attack.SourceSeat != receipt.SourceSeat || attack.TargetSeat != receipt.TargetSeat || attack.DamageWasApplied || attack.DamageAmountFinalized || attack.IsChainPropagation || _coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].HappyQiaoHealth != proof.Before || _coreProgramEventHistoryOperations.ProgramEventHistory<CardDamageReplacedWithMaximumHpLossEvent>().Any(e => e.ParentResolutionId == ownerId))
            throw new InvalidOperationException("A Qiao maximum replacement lost its exact unpaid attack receipt.");
        // Maximum HP is one synchronized rule for both sisters. In particular,
        // this must not assign Hp through a query-selected/default single pool.
        _coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].MaxHp = receipt.MaximumHpAfter;
        if (_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].HappyQiaoHealth != proof.After)
            throw new InvalidOperationException("Maximum HP replacement did not retain both original clamps.");
        return true;
    }

    internal bool PublishHappyQiaoCardMaximumHp(long ownerId, CardDamageMaximumHpReplacementReceipt receipt)
    {
        if (receipt.HappyQiaoMaximumProof is not { } proof)
            return false;
        var target = _coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat];
        if (!MatchesHappyQiaoCardMaximumHp(receipt) || _coreCardAttackStateOperations.GetCardAttackState(ownerId).MaximumHpReplacement != receipt || _coreProgramEventHistoryOperations.ProgramEventHistory<CardDamageReplacedWithMaximumHpLossEvent>().Count(e => e.ParentResolutionId == ownerId && e.Receipt == receipt) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyQiaoMaximumChangedEvent>().Any(e => e.ParentFrameId == ownerId && e.OwnerSeat == receipt.TargetSeat) || (receipt.MaximumHpAfter == 0 ? target.IsAlive || target.HappyQiaoHealth != new HappyQiaoDualHealth(0, 0, 0) || !_coreProgramEventHistoryOperations.ProgramEventHistory<HappyQiaoWholeDeathEvent>().Any(e => e.OwnerSeat == receipt.TargetSeat && e.Before == proof.After) : target.HappyQiaoHealth != proof.After))
            throw new InvalidOperationException("Qiao maximum facts must publish once from the issued clamps, after zero-maximum native death.");
        PublishHappyQiaoMaximumChange(ownerId, receipt.TargetSeat, proof.Before, proof.After, receipt.Source.SkillId);
        return true;
    }
}
