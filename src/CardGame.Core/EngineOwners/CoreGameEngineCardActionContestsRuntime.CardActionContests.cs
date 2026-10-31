#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardActionContestsRuntime
{

    void global::CardGame.Core.ICardActionContestProgramHost.ReplaceAllSlashTargets(ProgramSkillFrame f, string bind) => ReplaceAllProgramSlashTargets(f, bind);
    SkillProgramStepOutcome global::CardGame.Core.ICardActionContestProgramHost.StartCardActionPindian(ProgramSkillFrame f, SkillProgramEffect e) => StartProgramCardActionPindian(f, e);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 20)]
    internal bool CanOfferCardActionContest(CharacterState owner, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ReplaceAllSlashTargets))
        {
            var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == context.CardUse?.ParentCardUseFrameId);
            if (context.Window != SkillProgramTriggerWindow.CardUseCommitted || use?.Action is not { } action || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || action.ActorSeat == owner.Seat || use.TargetSeats.Count == 0 || use.TargetSeats.Contains(owner.Seat) || !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(action.ActorSeat, owner.Seat) || _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count == 0)
                return false;
        }

        foreach (var effect in trigger.Effects.Where(e => e.Op == SkillProgramEffectOp.StartCardActionPindian))
        {
            var opponent = effect.OpponentReference!.Kind == ProgramParticipantRef.Actor ? context.CardUse?.ActorSeat : context.TargetSeat;
            if (context.CardUse is not { } action || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || opponent is not { } seat || seat == owner.Seat || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || !_characterLuKangWumiaoOperations.HasWuLuKangPindianInput(owner) || !_characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[seat]) || !_coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(owner.Seat, seat))
                return false;
        }

        return true;
    }

    internal SkillProgramStepOutcome StartProgramCardActionPindian(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var context = frame.WindowContext ?? throw new InvalidOperationException("Card contest lost its boundary.");
        var opponent = effect.OpponentReference!.Kind == ProgramParticipantRef.Actor ? context.CardUse!.ActorSeat : context.TargetSeat!.Value;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(f => f.Id == context.CardUse!.ParentCardUseFrameId);
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.Action?.ActionId != context.CardUse!.CardActionId || opponent == frame.OwnerSeat || !_characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]) || !_characterLuKangWumiaoOperations.HasWuLuKangPindianInput(_coreMatchParticipantsPlayersAccess._players[opponent]) || !_coreEquipmentSlotGroupProgramsOperations.CanBePindianTarget(frame.OwnerSeat, opponent) || frame.PindianResultBindings.Any(b => b.Name == effect.ResultBind))
            throw new InvalidOperationException("Card contest lost its exact Slash or participants.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePindianOperations.BeginSharedPindian(frame.Id, new(frame.SkillId, skill.Name, skill.Name + " · 拼点", skill.Description), frame.OwnerSeat, opponent, programResultBind: effect.ResultBind!, programResultVisibility: effect.Visibility);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ReplaceAllProgramSlashTargets(ProgramSkillFrame frame, string bind)
    {
        var context = frame.WindowContext ?? throw new InvalidOperationException("Replacement lost its boundary.");
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == context.CardUse!.ParentCardUseFrameId);
        var use = (CardUseFrame)_resolutionStackOperations._resolutionStack[index];
        var action = use.Action!;
        var payment = frame.CardSetBindings.Single(b => b.Name == bind);
        if (context.Window != SkillProgramTriggerWindow.CardUseCommitted || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || action.ActionId != context.CardUse!.CardActionId || action.ActorSeat == frame.OwnerSeat || use.TargetSeats.Count == 0 || use.TargetSeats.Contains(frame.OwnerSeat) || payment.CardIds.Count != 1 || payment.SourceLocations is not [var location] || location != CardLocation.DrawPile)
            throw new InvalidOperationException("Replacement lost its actual owner-hand payment or Slash.");
        var targets = Array.AsReadOnly(action.EffectiveSuit is Suit.Spade or Suit.Club ? Array.Empty<int>() : new[] { frame.OwnerSeat });
        var updated = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, targets, action.PhysicalCards, action.ConversionChain, targets, action.EffectiveSuit, action.EffectiveRank, action.EffectiveIsRed, action.FactionOrigin));
        var attack = _coreCardActionsOperations.ProgramCardAttack ?? throw new InvalidOperationException("Replacement lost Slash continuation.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { TargetSeats = targets, TargetIndex = 0, Action = updated, SlashTargetsCancelled = use.SlashTargetsCancelled || targets.Count == 0, Continuations = use.Continuations with { FangtianHalberd = use.Continuations.FangtianHalberd is { Active: true } multi ? multi with { TargetSeats = targets } : use.Continuations.FangtianHalberd } });
        if (targets.Count > 0)
        {
            attack.SetDamageParticipants(attack.SourceSeat, frame.OwnerSeat);
            attack.SetIgnoresArmor(_coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]) || _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], use.CardKind));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SlashTargetsReplacedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, use.Id, use.TargetSeats, targets));
    }
}
