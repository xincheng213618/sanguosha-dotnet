using System.Text.Json.Serialization;
using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed record ShanheNullifiedTrickClaim(long CardUseFrameId, long ActionId, long WindowFrameId,
    int ChainDepth, int? TargetSeat, int RecipientSeat, int ResponseCardId, string SkillId,
    string SkillInstanceId, string GameplayHash, string PolicyId, IReadOnlyList<int> CardIds,
    int MovementBefore, int MovementAfter = 0, bool MovementDrained = false)
{
    private readonly IReadOnlyList<int> _cardIds = Array.AsReadOnly(CardIds.ToArray());
    public IReadOnlyList<int> CardIds
    {
        get => _cardIds;
        init => _cardIds = Array.AsReadOnly(value.ToArray());
    }
}

// Scalar-only committed fact: no collection can escape through an observer.
public sealed record ShanheNullifiedTrickMaterialClaimedEvent(long CardUseFrameId, long ActionId,
    long WindowFrameId, int ChainDepth, int RecipientSeat, int CardId, int MovementSequence) : IGameEvent;

public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheNullifiedTrickClaim? ShanheNullifiedTrickClaim { get; init; }
}

public sealed partial record CardsMovedTriggerWindowFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? ResumeShanheNullifiedClaimWindowId { get; init; }
}

public sealed partial class GameEngine
{
    private static readonly CardMoveReason ShanheCounterspellClaimReason = new("shanhe.counterspell-claim");

    [ProgramPipelineHook(ProgramPipelinePhase.BeforeNullificationFinished, 80900)]
    private bool TryBeginShanheNullifiedTrickClaim(NullificationWindowFrame window)
    {
        if (_options.ShanheBattle is null || _modeDefinition.Id != ShanheBattleSetup.BattleModeId)
            return false;
        if (window.DelayedReturn is not null) return TryBeginShanheDelayedNullifiedTrickClaim(window);
        if (LifecycleCardUse(window.ParentFrameId) is not { Action: { Type: CardActionType.Use } action } use) return false;
        if (use.ShanheNullifiedTrickClaim is { } existing)
        {
            if (existing.WindowFrameId != window.Id || existing.MovementDrained) return false;
            if (TryBeginCardsMovedProgramWindow(window.Id)) return true;
            UpdateLifecycleCardUse(use.Id, current => current with
            { ShanheNullifiedTrickClaim = existing with { MovementDrained = true } });
            return false;
        }
        if (_winner != Winner.None || !window.EffectNullified || window.ChainDepth <= 0 ||
            HasCurrentCardEnhancement(use.Id, CurrentCardEnhancement.Uncancelable) || window.DelayedReturn is not null)
            return false;
        var response = ProgramEventHistory<NullificationRespondedEvent>().LastOrDefault(fact =>
            fact.ResolutionId == use.Id && fact.WindowFrameId == window.Id && fact.ChainDepth == window.ChainDepth &&
            fact.EffectNullified && fact.EffectCardId == window.EffectCardId && fact.EffectCardKind == window.EffectCardKind);
        if (response is null || !IsValidPlayerSeat(response.ResponderSeat) || !_players[response.ResponderSeat].IsAlive)
            return false;
        var binding = CardPolicies(_players[response.ResponderSeat], SkillProgramCardPolicyKind.ClaimNullifiedTrick,
            CardKind.Nullification).FirstOrDefault();
        if (binding.Source is null) return false;
        var ids = (use.PhysicalCardIds ?? [use.CardId]).Where(id => id > 0 &&
            _cardZones.GetLocation(id) == CardLocation.Processing && !IsClaimedUseCardEntity(use.Id, id)).ToArray();
        if (ids.Length == 0) return false;
        if (_resolutionStack.LastOrDefault()?.Id != window.Id || action.ActorSeat != window.SourceSeat ||
            ids.Distinct().Count() != ids.Length || ids.Any(id => !action.PhysicalCards.Any(cost => cost.CardId == id)))
            throw new InvalidOperationException("A Shanhe counterspell claim lost its exact final window and original paid materials.");

        var receipt = new ShanheNullifiedTrickClaim(use.Id, action.ActionId, window.Id, window.ChainDepth,
            GetNullificationTargetSeat(window), response.ResponderSeat, response.NullificationCardId,
            binding.Source.SkillId, binding.Source.SkillInstanceId, binding.Source.Program.GameplayHash, binding.Policy.Id,
            Array.AsReadOnly(ids), _movementSequence);
        // The still-resolving use owns the entities before any movement observers
        // can inspect the parent. No later target or completion can claim again.
        UpdateLifecycleCardUse(use.Id, current => current with { ShanheNullifiedTrickClaim = receipt });
        MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(receipt.RecipientSeat), ShanheCounterspellClaimReason);
        receipt = receipt with { MovementAfter = _movementSequence };
        UpdateLifecycleCardUse(use.Id, current => current with { ShanheNullifiedTrickClaim = receipt });
        foreach (var id in ids)
        {
            var move = _cardMovements.Single(item => item.CardId == id && item.Sequence > receipt.MovementBefore &&
                item.Sequence <= receipt.MovementAfter && item.From == CardLocation.Processing &&
                item.To == CardLocation.Hand(receipt.RecipientSeat) && item.Reason == ShanheCounterspellClaimReason);
            AdvanceEventRulesAndQueueFact(new ShanheNullifiedTrickMaterialClaimedEvent(use.Id, action.ActionId,
                window.Id, window.ChainDepth, receipt.RecipientSeat, id, move.Sequence));
        }
        return TryBeginShanheNullifiedTrickClaim(window);
    }

    private bool IsShanheNullifiedTrickMaterialClaim(long useId, int cardId) =>
        LifecycleCardUse(useId) is { Action: { } action, ShanheNullifiedTrickClaim: { } receipt } &&
        receipt.CardUseFrameId == useId && receipt.ActionId == action.ActionId && receipt.CardIds.Contains(cardId);

    private bool IsShanheNullifiedClaimAwaitingMovement(NullificationWindowFrame window) =>
        IsShanheDelayedNullifiedClaimAwaitingMovement(window) ||
        LifecycleCardUse(window.ParentFrameId)?.ShanheNullifiedTrickClaim is { MovementDrained: false } receipt &&
        receipt.CardUseFrameId == window.ParentFrameId && receipt.WindowFrameId == window.Id;

    private bool IsShanheNullifiedClaimMovement(NullificationWindowFrame window, CardMovementBatchContext batch) =>
        IsShanheDelayedNullifiedClaimMovement(window, batch) ||
        IsShanheNullifiedClaimAwaitingMovement(window) && batch.ParentFrameId == window.Id &&
        batch.AwaitingProgramFrameId is null && batch.Movements.Count > 0 &&
        LifecycleCardUse(window.ParentFrameId)?.ShanheNullifiedTrickClaim is { } receipt &&
        batch.Movements.All(move => move.Sequence > receipt.MovementBefore && move.Sequence <= receipt.MovementAfter &&
            receipt.CardIds.Contains(move.CardId) && move.From == CardLocation.Processing &&
            move.To == CardLocation.Hand(receipt.RecipientSeat) && move.Reason == ShanheCounterspellClaimReason);

    private static bool HasUnlistedShanheClaimMovementReturn(CardsMovedTriggerWindowFrame child) =>
        child.ResumeColdDamageReplacementFrameId is not null || child.ResumeSkillInvocationFrameId is not null ||
        child.ResumeRoundEndingBoundaryId is not null || child.ResumeMouZhitianFireFrameId is not null ||
        child.QiaoZhouPreparationReturn is not null || child.SoleTargetQuarantineEndReturn is not null || child.WenhePoisonFireAttackReturn is not null;

    private static bool HasOnlyShanheNullifiedClaimMovementReturn(CardsMovedTriggerWindowFrame child) =>
        child.ResumeShanheNullifiedClaimWindowId is not null && child.Id == child.Batch.Id && child.ResumeProgramFrameId is null &&
        !HasAlternateProgramMovementReturn(child with { ResumeShanheNullifiedClaimWindowId = null }) &&
        !HasUnlistedShanheClaimMovementReturn(child);

    private void ReturnShanheNullifiedClaimMovement(CardsMovedTriggerWindowFrame child)
    {
        if (!HasOnlyShanheNullifiedClaimMovementReturn(child) || child.ResumeShanheNullifiedClaimWindowId is not { } id ||
            _resolutionStack.LastOrDefault() is not NullificationWindowFrame window || window.Id != id ||
            !IsShanheNullifiedClaimMovement(window, child.Batch))
            throw new InvalidOperationException("A Shanhe counterspell claim lost its original awaited movement return.");
        FinishNullificationWindow(window);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 80900)]
    private void AssertShanheNullifiedTrickClaims()
    {
        foreach (var use in _resolutionStack.OfType<CardUseFrame>().Where(frame => frame.ShanheNullifiedTrickClaim is not null))
        {
            var receipt = use.ShanheNullifiedTrickClaim!;
            if (_options.ShanheBattle is null || _modeDefinition.Id != ShanheBattleSetup.BattleModeId ||
                use.Action is not { Type: CardActionType.Use } action || receipt.CardUseFrameId != use.Id ||
                receipt.ActionId != action.ActionId || receipt.WindowFrameId <= use.Id || receipt.ChainDepth <= 0 ||
                !IsValidPlayerSeat(receipt.RecipientSeat) || receipt.CardIds.Count == 0 ||
                receipt.CardIds.Distinct().Count() != receipt.CardIds.Count ||
                receipt.CardIds.Any(id => !action.PhysicalCards.Any(cost => cost.CardId == id)) ||
                receipt.MovementAfter <= receipt.MovementBefore || string.IsNullOrWhiteSpace(receipt.SkillInstanceId) ||
                _contentRegistry.Skills.GetValueOrDefault(receipt.SkillId)?.Program is not { } program ||
                program.GameplayHash != receipt.GameplayHash || !program.CardPolicies.Any(policy =>
                    policy.Id == receipt.PolicyId && policy.Kind == SkillProgramCardPolicyKind.ClaimNullifiedTrick) ||
                !ProgramEventHistory<NullificationRespondedEvent>().Any(fact => fact.WindowFrameId == receipt.WindowFrameId &&
                    fact.ResolutionId == use.Id && fact.ChainDepth == receipt.ChainDepth && fact.EffectNullified &&
                    fact.ResponderSeat == receipt.RecipientSeat && fact.NullificationCardId == receipt.ResponseCardId) ||
                receipt.CardIds.Any(id => ProgramEventHistory<ShanheNullifiedTrickMaterialClaimedEvent>().Count(fact =>
                    fact.CardUseFrameId == use.Id && fact.ActionId == receipt.ActionId && fact.WindowFrameId == receipt.WindowFrameId &&
                    fact.ChainDepth == receipt.ChainDepth && fact.RecipientSeat == receipt.RecipientSeat && fact.CardId == id &&
                    _cardMovements.Any(move => move.Sequence == fact.MovementSequence && move.Sequence > receipt.MovementBefore &&
                        move.Sequence <= receipt.MovementAfter && move.CardId == id && move.From == CardLocation.Processing &&
                        move.To == CardLocation.Hand(receipt.RecipientSeat) && move.Reason == ShanheCounterspellClaimReason)) != 1))
                throw new InvalidOperationException("A Shanhe material claim lost its original paid use, final counterspell or exact one-time movement proof.");
        }
    }
}
