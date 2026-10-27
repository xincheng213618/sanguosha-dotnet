namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string GouXiangArmorSkill = "shanhe:gouxiang-armor-14088";
    private const string GouXiangTakeReason = "skill-program.shanhe:gouxiang-armor-14088.random-hand";

    // Suit is already frozen by FreezeCompletedResponseUseSuit before payment;
    // declarations also retain their original suit/rank receipt. Only an
    // immutable single-entity rank is supplied after native response payment.
    private CardActionContext CaptureGouXiangActionAppearance(CardActionContext action)
    {
        if (!_contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveGouXiangResponse) ||
            action.Type != CardActionType.Response || action.EffectiveKind != CardKind.Dodge ||
            action.PhysicalCards is not [var cost]) return action;
        var material = GetAttackCard(cost.CardId);
        var declared = LiveDeclarationPayment(cost.CardId);
        var suit = action.EffectiveSuit ?? declared?.FrozenSuit;
        var rank = action.EffectiveRank ?? declared?.OriginalRank ?? (material.Rank > 0 ? material.Rank : (int?)null);
        if (suit == action.EffectiveSuit && rank == action.EffectiveRank) return action;
        return new(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat,
            action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats,
            action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, suit, rank,
            action.EffectiveIsRed, action.FactionOrigin);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62052,
        SkillProgramEffectOp.ResolveGouXiangResponse)]
    private bool CanOfferGouXiangArmor(ProgramTriggerLegalityContext context) =>
        context.SkillId == GouXiangArmorSkill && TryGetGouXiangAttacker(context.OwnerSeat,
            context.Candidate.SkillInstanceId, context.WindowContext, out _);

    private bool TryGetGouXiangAttacker(int ownerSeat, string instanceId, ProgramSkillWindowContext? context,
        out int attackerSeat)
    {
        attackerSeat = -1;
        if (context is not { Window: SkillProgramTriggerWindow.CardResponseCompleted, CardUse: { } useContext } ||
            !IsValidPlayerSeat(ownerSeat) || !_players[ownerSeat].IsAlive ||
            IsArmorIneffectiveForTurn(_players[ownerSeat]) ||
            !KongwuEffectiveEquipment(_players[ownerSeat]).Any(card => card.Kind == CardKind.GouXiang &&
                IsActiveProgramSourceEquipmentCard(ownerSeat, GouXiangArmorSkill, instanceId, card))) return false;
        var completed = _resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(window =>
            window.ResponseCompletion?.ActionId == useContext.CardActionId);
        if (completed is not { ResponseCompletion: { OriginalContinuation: ProgramCardContinuation.Dodge } returned,
                Action: { Type: CardActionType.Response, EffectiveKind: CardKind.Dodge } response } ||
            response.ActorSeat != ownerSeat || response.ProviderSeat != ownerSeat || response.RequesterSeat is not null ||
            returned.CompletionActorSeat != ownerSeat ||
            _resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Id == returned.ParentFrameId) is not
                { Action: { } slash } original ||
            !IsSlashCard(slash.EffectiveKind) || response.ParentActionId != slash.ActionId ||
            returned.AttackOwnerFrameId != original.Id || completed.AttackOwnerFrameId != original.Id ||
            response.OpponentSeat != slash.ActorSeat || slash.ActorSeat == ownerSeat ||
            !IsValidPlayerSeat(slash.ActorSeat) || !_players[slash.ActorSeat].IsAlive || GetHand(_players[slash.ActorSeat]).Count == 0)
            return false;
        var attack = new CardAttackHandle(this, original.Id);
        if (attack.SourceSeat != slash.ActorSeat || attack.TargetSeat != ownerSeat || attack.IgnoresArmor ||
            HasDirectedCardArmorBypass(original.Id, ownerSeat) ||
            !((response.EffectiveSuit is { } suit && suit != Suit.None && slash.EffectiveSuit == suit) ||
              (response.EffectiveRank is > 0 && slash.EffectiveRank == response.EffectiveRank))) return false;
        attackerSeat = slash.ActorSeat;
        return true;
    }

    [ProgramCapability]
    private sealed class GouXiangArmorCapability(GameEngine engine) : IGouXiangArmorHost
    {
        public void ObtainAttackerHandCard(ProgramSkillFrame frame) => engine.ObtainGouXiangAttackerHandCard(frame);
    }

    private void ObtainGouXiangAttackerHandCard(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != GouXiangArmorSkill || frame.TriggerId != "matching-dodge" || frame.InstructionIndex != 1 ||
            !TryGetGouXiangAttacker(frame.OwnerSeat, frame.SkillInstanceId, frame.WindowContext, out var attacker)) return;
        if (_resolutionStack.Count < 2 || _resolutionStack[^2] is not ProgramCardTriggerWindowFrame
                { ResponseCompletion: { CostsDrained: true } completed } window ||
            frame.WindowContext!.ParentFrameId != window.Id ||
            frame.WindowContext.CardUse!.CardActionId != completed.ActionId ||
            ProgramEventHistory<CardResponseCompletedEvent>().Count(fact => fact.ActionId == completed.ActionId &&
                fact.ParentFrameId == completed.ParentFrameId && fact.AttackOwnerFrameId == completed.AttackOwnerFrameId &&
                fact.ActorSeat == frame.OwnerSeat && fact.ProviderSeat == frame.OwnerSeat &&
                fact.EffectiveKind == CardKind.Dodge && fact.OriginalContinuation == ProgramCardContinuation.Dodge) != 1)
            throw new InvalidOperationException("Gouxiang lost its original completed native Dodge and paid response window.");
        var hand = GetHand(_players[attacker]);
        var material = hand[_random.Next(hand.Count)];
        // The generic executor has committed this instruction cursor. The shared
        // atomic gain and post-instruction windows retain the original producer.
        MoveProgramCardsFromMultipleSources([material.Id], CardLocation.Hand(frame.OwnerSeat),
            new(GouXiangTakeReason), disclosure: CardMovementDisclosure.FaceDown);
        AdvanceEventRulesAndQueueFact(new ProgramRandomHandCardsTakenEvent(frame.Id, frame.SkillId,
            frame.TriggerId, frame.OwnerSeat, Array.AsReadOnly(new[] { attacker }), 1));
    }
}
