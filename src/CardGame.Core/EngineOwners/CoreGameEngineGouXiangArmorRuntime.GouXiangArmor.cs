#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGouXiangArmorRuntime
{
    internal const string GouXiangArmorSkill = "shanhe:gouxiang-armor-14088";
    internal const string GouXiangTakeReason = "skill-program.shanhe:gouxiang-armor-14088.random-hand";
    internal CardActionContext CaptureGouXiangActionAppearance(CardActionContext action)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveGouXiangResponse) || action.Type != CardActionType.Response || action.EffectiveKind != CardKind.Dodge || action.PhysicalCards is not [var cost])
            return action;
        var material = _coreCardAttackStateOperations.GetAttackCard(cost.CardId);
        var declared = _coreCardDeclarationsOperations.LiveDeclarationPayment(cost.CardId);
        var suit = action.EffectiveSuit ?? declared?.FrozenSuit;
        var rank = action.EffectiveRank ?? declared?.OriginalRank ?? (material.Rank > 0 ? material.Rank : (int? )null);
        if (suit == action.EffectiveSuit && rank == action.EffectiveRank)
            return action;
        return new(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, suit, rank, action.EffectiveIsRed, action.FactionOrigin);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62052, SkillProgramEffectOp.ResolveGouXiangResponse)]
    internal bool CanOfferGouXiangArmor(ProgramTriggerLegalityContext context) => context.SkillId == GouXiangArmorSkill && TryGetGouXiangAttacker(context.OwnerSeat, context.Candidate.SkillInstanceId, context.WindowContext, out _);
    internal bool TryGetGouXiangAttacker(int ownerSeat, string instanceId, ProgramSkillWindowContext? context, out int attackerSeat)
    {
        attackerSeat = -1;
        if (context is not { Window: SkillProgramTriggerWindow.CardResponseCompleted, CardUse: { } useContext } || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || _coreExtendedViewAsOperations.IsArmorIneffectiveForTurn(_coreMatchParticipantsPlayersAccess._players[ownerSeat]) || !_characterHuCheErMouOperations.KongwuEffectiveEquipment(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Any(card => card.Kind == CardKind.GouXiang && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(ownerSeat, GouXiangArmorSkill, instanceId, card)))
            return false;
        var completed = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(window => window.ResponseCompletion?.ActionId == useContext.CardActionId);
        if (completed is not { ResponseCompletion: { OriginalContinuation: ProgramCardContinuation.Dodge } returned, Action: { Type: CardActionType.Response, EffectiveKind: CardKind.Dodge } response } || response.ActorSeat != ownerSeat || response.ProviderSeat != ownerSeat || response.RequesterSeat is not null || returned.CompletionActorSeat != ownerSeat || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Id == returned.ParentFrameId)is not { Action: { } slash } original || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(slash.EffectiveKind) || response.ParentActionId != slash.ActionId || returned.AttackOwnerFrameId != original.Id || completed.AttackOwnerFrameId != original.Id || response.OpponentSeat != slash.ActorSeat || slash.ActorSeat == ownerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(slash.ActorSeat) || !_coreMatchParticipantsPlayersAccess._players[slash.ActorSeat].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[slash.ActorSeat]).Count == 0)
            return false;
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(original.Id);
        if (attack.SourceSeat != slash.ActorSeat || attack.TargetSeat != ownerSeat || attack.IgnoresArmor || _coreDirectedCardPolicyQueriesOperations.HasDirectedCardArmorBypass(original.Id, ownerSeat) || !((response.EffectiveSuit is { } suit && suit != Suit.None && slash.EffectiveSuit == suit) || (response.EffectiveRank is> 0 && slash.EffectiveRank == response.EffectiveRank)))
            return false;
        attackerSeat = slash.ActorSeat;
        return true;
    }

    void global::CardGame.Core.IGouXiangArmorHost.ObtainAttackerHandCard(ProgramSkillFrame frame) => ObtainGouXiangAttackerHandCard(frame);
    internal void ObtainGouXiangAttackerHandCard(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != GouXiangArmorSkill || frame.TriggerId != "matching-dodge" || frame.InstructionIndex != 1 || !TryGetGouXiangAttacker(frame.OwnerSeat, frame.SkillInstanceId, frame.WindowContext, out var attacker))
            return;
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramCardTriggerWindowFrame { ResponseCompletion: { CostsDrained: true } completed } window || frame.WindowContext!.ParentFrameId != window.Id || frame.WindowContext.CardUse!.CardActionId != completed.ActionId || _coreProgramEventHistoryOperations.ProgramEventHistory<CardResponseCompletedEvent>().Count(fact => fact.ActionId == completed.ActionId && fact.ParentFrameId == completed.ParentFrameId && fact.AttackOwnerFrameId == completed.AttackOwnerFrameId && fact.ActorSeat == frame.OwnerSeat && fact.ProviderSeat == frame.OwnerSeat && fact.EffectiveKind == CardKind.Dodge && fact.OriginalContinuation == ProgramCardContinuation.Dodge) != 1)
            throw new InvalidOperationException("Gouxiang lost its original completed native Dodge and paid response window.");
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[attacker]);
        var material = hand[_randomOperations._random.Next(hand.Count)];
        // The generic executor has committed this instruction cursor. The shared
        // atomic gain and post-instruction windows retain the original producer.
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([material.Id], CardLocation.Hand(frame.OwnerSeat), new(GouXiangTakeReason), disclosure: CardMovementDisclosure.FaceDown);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramRandomHandCardsTakenEvent(frame.Id, frame.SkillId, frame.TriggerId, frame.OwnerSeat, Array.AsReadOnly(new[] { attacker }), 1));
    }
}
