#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAlcoholIdentityPoliciesRuntime
{
    internal bool HasAlcoholKingIdentityPolicy(CharacterState owner) => _coreProgramCardPoliciesOperations.HasCardPolicy(owner, SkillProgramCardPolicyKind.AlcoholKingIdentityRank, CardKind.Alcohol);
    internal int? CaptureAlcoholIdentityRank(CharacterState owner, Card card) => card.Kind == CardKind.Alcohol && (_cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Hand(owner.Seat) || _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.WoodenOxGrain(owner.Seat)) && HasAlcoholKingIdentityPolicy(owner) ? 13 : null;
    internal int EffectiveOwnedCardRank(CharacterState owner, Card card) => _characterGuanYuWumiaoOperations.JuewuForeignHandRank(owner, card) ?? CaptureAlcoholIdentityRank(owner, card) ?? card.Rank;
    internal int? CaptureAlcoholPindianRank(PindianFrame frame, CharacterState owner, Card card, bool source, bool top)
    {
        if (card.Kind != CardKind.Alcohol || !HasAlcoholKingIdentityPolicy(owner) || !ReferenceEquals(_resolutionStackOperations._resolutionStack.LastOrDefault(), frame) || frame.PindianStep != PindianStep.ChooseOpponentCard || owner.Seat != (source ? frame.SourceSeat : frame.OpponentSeat))
            return null;
        // Both arguments are the actual validated selections in RevealPindian,
        // including a precisely reserved source top or the opponent's current top.
        if (source && (frame.SourceCardId != card.Id || frame.SourceUsesDrawPileTop != top) || _cardZonesOperations._cardZones.GetLocation(card.Id) != (top ? source ? CardLocation.Processing : CardLocation.DrawPile : CardLocation.Hand(owner.Seat)) || source && top && (frame.ParentProcessingCardIds ?? []).Contains(card.Id))
            return null;
        return 13;
    }

    internal bool IsEffectiveHandKindExempt(CharacterState owner, Card card, IReadOnlySet<CardKind> kinds) => card.Kind == CardKind.MantianGuohai || _coreWushuangTacticRulesOperations.IsWushuangHandLimitExempt(owner, card) || kinds.Contains(card.Kind) || card.Kind == CardKind.Alcohol && kinds.Contains(CardKind.Slash) && HasAlcoholKingIdentityPolicy(owner);
    internal int? CaptureAlcoholActionRank(int providerSeat, CardKind kind, IReadOnlyList<int> ids)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || ids.Count != 1)
            return null;
        var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(ids[0])).Single(c => c.Id == ids[0]);
        return CaptureAlcoholIdentityRank(_coreMatchParticipantsPlayersAccess._players[providerSeat], card);
    }

    internal int? CaptureAlcoholResponseRank(CharacterState provider, CardKind kind, IReadOnlyList<CardActionCost> costs) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && costs.Count == 1 && costs[0].CardKind == CardKind.Alcohol && costs[0].From.OwnerSeat == provider.Seat && costs[0].From.Zone is CardZoneKind.Hand or CardZoneKind.WoodenOxGrain && HasAlcoholKingIdentityPolicy(provider) ? 13 : null;
    internal bool IsForeignTurnAlcoholUseForbidden(int actorSeat, CardKind kind, CardActionType type) => kind == CardKind.Alcohol && type == CardActionType.Use && _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber > 0 && actorSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat].IsAlive && _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat], SkillProgramCardPolicyKind.ForeignTurnAlcoholUseProhibition, CardKind.Alcohol);
    internal bool IsAlcoholIdentityOwnedLocation(CharacterState owner, Card card) => card.Kind == CardKind.Alcohol && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.WoodenOxGrain(owner.Seat) && HasAlcoholKingIdentityPolicy(owner);
    internal static bool HasDyingAlcoholProducer(SkillProgramTrigger trigger) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_AlcoholIdentityPolicies.HasDyingAlcoholProducer(trigger);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 840)]
    internal bool CanRunProgramDyingAlcoholPolicy(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int responder)
    {
        if (!HasDyingAlcoholProducer(trigger))
            return true;
        if (context.Window is not (SkillProgramTriggerWindow.DyingResponse or SkillProgramTriggerWindow.SelfDyingResponse) || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || dying.FrameId != context.ParentFrameId || dying.ResponderSeat != responder || context.TargetSeat != dying.VictimSeat)
            return false;
        // A pile provider does not use Wine: its exact dying victim is the actor.
        return !_characterZhouYuOlMouOperations.MouGuquPhysicalUseForbidden(dying.VictimSeat, []) && !IsForeignTurnAlcoholUseForbidden(dying.VictimSeat, CardKind.Alcohol, CardActionType.Use);
    }

    internal ProgramDyingAlcoholPermission? CaptureProgramDyingAlcoholPermission(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        // Keep historical producer state unchanged when this new policy is absent.
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.ForeignTurnAlcoholUseProhibition))
            return null;
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(candidate);
        if (!HasDyingAlcoholProducer(trigger))
            return null;
        if (!CanRunProgramDyingAlcoholPolicy(trigger, context, candidate.OwnerSeat) || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying)
            throw new InvalidOperationException("A prohibited Wine producer cannot issue a dying-use permission.");
        return new(dying.FrameId, dying.VictimSeat, candidate.OwnerSeat, candidate.SkillId, candidate.BindingId, candidate.SkillInstanceId, candidate.GameplayHash, context.Window);
    }

    internal bool HasFrozenProgramDyingAlcoholPermission(ProgramSkillFrame frame, SkillProgramEffectOp op)
    {
        if (frame.DyingAlcoholPermission is not { } permission || frame.WindowContext is not { } context || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || permission.DyingFrameId != dying.FrameId || context.ParentFrameId != dying.FrameId || permission.ActorSeat != dying.VictimSeat || permission.ResponderSeat != dying.ResponderSeat || permission.ResponderSeat != frame.OwnerSeat || permission.SkillId != frame.SkillId || permission.BindingId != frame.TriggerId || permission.SkillInstanceId != frame.SkillInstanceId || permission.GameplayHash != frame.GameplayHash || permission.Window != context.Window || context.TargetSeat != dying.VictimSeat || context.Window != (op == SkillProgramEffectOp.UseVirtualDyingAlcohol ? SkillProgramTriggerWindow.SelfDyingResponse : SkillProgramTriggerWindow.DyingResponse))
            return false;
        var trigger = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!.Triggers.Single(t => t.Id == frame.TriggerId);
        return ProgramInstructionResolver.Default.Features(trigger).HasOperation(op);
    }

    internal bool CanUseProgramDyingAlcohol(ProgramSkillFrame frame, SkillProgramEffectOp op) => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && !_characterZhouYuOlMouOperations.MouGuquPhysicalUseForbidden(dying.VictimSeat, []) && (HasFrozenProgramDyingAlcoholPermission(frame, op) || !IsForeignTurnAlcoholUseForbidden(dying.VictimSeat, CardKind.Alcohol, CardActionType.Use));
}
