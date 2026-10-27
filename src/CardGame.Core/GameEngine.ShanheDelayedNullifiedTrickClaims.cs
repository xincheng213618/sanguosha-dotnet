using System.Text.Json.Serialization;
using CardGame.Core.Campaigns;

namespace CardGame.Core;

// A delayed effect has already completed its original use. Its actual owner is
// this live Nullification window and its immutable native delayed-return token.
public sealed record ShanheDelayedNullifiedTrickClaim(long WindowFrameId, DelayedEffectContinuation Origin,
    int ChainDepth, int RecipientSeat, int ResponseCardId, string SkillId, string SkillInstanceId,
    string GameplayHash, string PolicyId, int MovementBefore, int MovementAfter = 0, bool MovementDrained = false);

public sealed record ShanheDelayedNullifiedTrickMaterialClaimedEvent(long WindowFrameId, int CardId,
    CardKind EffectiveKind, int JudgmentOwnerSeat, int RecipientSeat, int ChainDepth, int MovementSequence) : IGameEvent;

public sealed partial record NullificationWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheDelayedNullifiedTrickClaim? ShanheDelayedNullifiedTrickClaim { get; init; }
}

public sealed partial class GameEngine
{
    private bool TryBeginShanheDelayedNullifiedTrickClaim(NullificationWindowFrame window)
    {
        if (window.DelayedReturn is not { OriginalUseLightning: null, CardId: > 0 } delayed) return false;
        if (window.ShanheDelayedNullifiedTrickClaim is { } existing)
        {
            if (existing.MovementDrained) return false;
            if (TryBeginCardsMovedProgramWindow(window.Id)) return true;
            ReplaceRuntimeFrame(window.Id, window with
            { ShanheDelayedNullifiedTrickClaim = existing with { MovementDrained = true } });
            return false;
        }
        if (_winner != Winner.None || !window.EffectNullified || window.ChainDepth <= 0 ||
            !HasNullificationEffectOwner(window)) return false;
        var response = ProgramEventHistory<NullificationRespondedEvent>().LastOrDefault(fact =>
            fact.WindowFrameId == window.Id && fact.ResolutionId == window.ParentFrameId &&
            fact.EffectCardId == delayed.CardId && fact.EffectCardKind == delayed.EffectiveKind &&
            fact.ChainDepth == window.ChainDepth && fact.EffectNullified);
        if (response is null || !IsValidPlayerSeat(response.ResponderSeat) || !_players[response.ResponderSeat].IsAlive)
            return false;
        var binding = CardPolicies(_players[response.ResponderSeat], SkillProgramCardPolicyKind.ClaimNullifiedTrick,
            CardKind.Nullification).FirstOrDefault();
        if (binding.Source is null) return false;
        var card = GetJudgment(_players[delayed.OwnerSeat]).Single(value => value.Id == delayed.CardId);
        var receipt = new ShanheDelayedNullifiedTrickClaim(window.Id, delayed, window.ChainDepth,
            response.ResponderSeat, response.NullificationCardId, binding.Source.SkillId, binding.Source.SkillInstanceId,
            binding.Source.Program.GameplayHash, binding.Policy.Id, _movementSequence);
        if (_resolutionStack.LastOrDefault()?.Id != window.Id)
            throw new InvalidOperationException("A delayed material claim must remain on its original live counterspell window.");
        ReplaceRuntimeFrame(window.Id, window = window with { ShanheDelayedNullifiedTrickClaim = receipt });
        MoveCard(card, CardLocation.Judgment(delayed.OwnerSeat), CardLocation.Hand(receipt.RecipientSeat), ShanheCounterspellClaimReason);
        receipt = receipt with { MovementAfter = _movementSequence };
        ReplaceRuntimeFrame(window.Id, window = window with { ShanheDelayedNullifiedTrickClaim = receipt });
        var move = _cardMovements.Single(value => value.CardId == delayed.CardId && value.Sequence > receipt.MovementBefore &&
            value.Sequence <= receipt.MovementAfter && value.From == CardLocation.Judgment(delayed.OwnerSeat) &&
            value.To == CardLocation.Hand(receipt.RecipientSeat) && value.Reason == ShanheCounterspellClaimReason);
        AdvanceEventRulesAndQueueFact(new ShanheDelayedNullifiedTrickMaterialClaimedEvent(window.Id, delayed.CardId,
            delayed.EffectiveKind, delayed.OwnerSeat, receipt.RecipientSeat, window.ChainDepth, move.Sequence));
        return TryBeginShanheDelayedNullifiedTrickClaim(window);
    }

    private bool IsShanheDelayedNullifiedClaimAwaitingMovement(NullificationWindowFrame window) =>
        window.ShanheDelayedNullifiedTrickClaim is { MovementDrained: false } receipt &&
        receipt.WindowFrameId == window.Id && receipt.Origin == window.DelayedReturn;

    private bool IsShanheDelayedNullifiedClaimMovement(NullificationWindowFrame window, CardMovementBatchContext batch) =>
        IsShanheDelayedNullifiedClaimAwaitingMovement(window) && batch.ParentFrameId == window.Id &&
        batch.AwaitingProgramFrameId is null && batch.Movements.Count == 1 &&
        window.ShanheDelayedNullifiedTrickClaim is { } receipt && batch.Movements[0] is { } move &&
        move.Sequence > receipt.MovementBefore && move.Sequence <= receipt.MovementAfter &&
        move.CardId == receipt.Origin.CardId && move.From == CardLocation.Judgment(receipt.Origin.OwnerSeat) &&
        move.To == CardLocation.Hand(receipt.RecipientSeat) && move.Reason == ShanheCounterspellClaimReason;

    private bool HasShanheClaimedDelayedOwner(NullificationWindowFrame window, DelayedEffectContinuation delayed)
    {
        if (window.ShanheDelayedNullifiedTrickClaim is not { } receipt) return false;
        ValidateShanheDelayedNullifiedTrickClaim(window, receipt);
        return receipt.Origin == delayed;
    }

    private bool TryCompleteShanheClaimedDelayedNullification(NullificationWindowFrame window, DelayedEffectContinuation delayed)
    {
        if (window.ShanheDelayedNullifiedTrickClaim is not { } receipt) return false;
        if (!receipt.MovementDrained || !HasShanheClaimedDelayedOwner(window, delayed))
            throw new InvalidOperationException("A claimed delayed effect cannot finish before its actual gain children return.");
        AddLog("CardEffect", $"【{CardCatalog.Get(delayed.EffectiveKind).DisplayName}】的效果被无懈。", delayed.OwnerSeat);
        if (delayed.EffectiveKind == CardKind.Lightning)
            AdvanceEventRulesAndQueueFact(new LightningResolvedEvent(window.Id, delayed.CardId, delayed.OwnerSeat,
                null, Hit: false, NextTargetSeat: null, DamageAmount: 0));
        if (_winner == Winner.None && _players[delayed.OwnerSeat].IsAlive)
            BeginDelayedJudgmentOrTurnStart(_players[delayed.OwnerSeat]);
        return true;
    }

    private void ValidateShanheDelayedNullifiedTrickClaim(NullificationWindowFrame window, ShanheDelayedNullifiedTrickClaim receipt)
    {
        var delayed = receipt.Origin;
        if (_options.ShanheBattle is null || _modeDefinition.Id != ShanheBattleSetup.BattleModeId ||
            window.DelayedReturn != delayed || delayed.OriginalUseLightning is not null || delayed.CardId <= 0 ||
            window.Id != receipt.WindowFrameId || window.ParentFrameId != delayed.ParentFrameId ||
            window.EffectCardId != delayed.CardId || window.EffectCardKind != delayed.EffectiveKind ||
            window.SourceSeat != delayed.OwnerSeat || !window.TargetSeats.SequenceEqual([delayed.OwnerSeat]) ||
            !window.EffectNullified || receipt.ChainDepth != window.ChainDepth || receipt.ChainDepth <= 0 ||
            !IsDelayedCard(delayed.EffectiveKind) || !IsValidPlayerSeat(receipt.RecipientSeat) ||
            delayed.TurnNumber != _turnNumber || delayed.Phase != _phase || delayed.OwnerSeat != _currentSeat ||
            receipt.MovementAfter <= receipt.MovementBefore || string.IsNullOrWhiteSpace(receipt.SkillInstanceId) ||
            _contentRegistry.Skills.GetValueOrDefault(receipt.SkillId)?.Program is not { } program ||
            program.GameplayHash != receipt.GameplayHash || !program.CardPolicies.Any(policy =>
                policy.Id == receipt.PolicyId && policy.Kind == SkillProgramCardPolicyKind.ClaimNullifiedTrick) ||
            !ProgramEventHistory<NullificationRespondedEvent>().Any(fact => fact.WindowFrameId == window.Id &&
                fact.ResolutionId == window.ParentFrameId && fact.EffectCardId == delayed.CardId &&
                fact.EffectCardKind == delayed.EffectiveKind && fact.ChainDepth == receipt.ChainDepth && fact.EffectNullified &&
                fact.ResponderSeat == receipt.RecipientSeat && fact.NullificationCardId == receipt.ResponseCardId) ||
            ProgramEventHistory<ShanheDelayedNullifiedTrickMaterialClaimedEvent>().Count(fact => fact.WindowFrameId == window.Id &&
                fact.CardId == delayed.CardId && fact.EffectiveKind == delayed.EffectiveKind && fact.JudgmentOwnerSeat == delayed.OwnerSeat &&
                fact.RecipientSeat == receipt.RecipientSeat && fact.ChainDepth == receipt.ChainDepth &&
                _cardMovements.Any(move => move.Sequence == fact.MovementSequence && move.Sequence > receipt.MovementBefore &&
                    move.Sequence <= receipt.MovementAfter && move.CardId == delayed.CardId && move.From == CardLocation.Judgment(delayed.OwnerSeat) &&
                    move.To == CardLocation.Hand(receipt.RecipientSeat) && move.Reason == ShanheCounterspellClaimReason)) != 1)
            throw new InvalidOperationException("A delayed claim lost its exact original window, delayed-return token or actual final-counterspell movement.");
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 80901)]
    private void AssertShanheDelayedNullifiedTrickClaims()
    {
        foreach (var window in _resolutionStack.OfType<NullificationWindowFrame>())
            if (window.ShanheDelayedNullifiedTrickClaim is { } receipt) ValidateShanheDelayedNullifiedTrickClaim(window, receipt);
    }
}
