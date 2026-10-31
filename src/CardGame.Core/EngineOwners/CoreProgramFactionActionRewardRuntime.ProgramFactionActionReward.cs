#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramFactionActionRewardRuntime
{
    internal static bool IsFactionActionReward(SkillProgramEffect effect) => effect.Op is SkillProgramEffectOp.RewardOutOfTurnFactionSlash or SkillProgramEffectOp.RewardOutOfTurnFactionDodge;
    internal bool TracksFactionActionOrigin => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RewardOutOfTurnFactionSlash) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RewardOutOfTurnFactionDodge);

    internal CardActionContext CaptureFactionAction(CardActionContext action)
    {
        if (action.Type == CardActionType.Response)
            action = _characterLuSuMobileShiOperations.NormalizeShiLsResponseAction(action);
        action = _coreWushuangJinzhiOperations.CaptureWushuangJinzhiAppearance(action);
        action = _coreGouXiangArmorOperations.CaptureGouXiangActionAppearance(action);
        if (!TracksFactionActionOrigin || action.FactionOrigin is not null || !(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || action.EffectiveKind == CardKind.Dodge && _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RewardOutOfTurnFactionDodge)) || action.ActionId == 0)
            return action;
        return new(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, action.EffectiveSuit, action.EffectiveRank, action.EffectiveIsRed, new(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]), _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[action.ProviderSeat])));
    }

    internal CardActionContext CaptureReplacedFactionActor(CardActionContext action, int actorSeat)
    {
        if (action.FactionOrigin is not { } origin)
            return action;
        return new(action.ActionId, action.ParentActionId, action.Type, actorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.EffectiveDesignatedTargetSeats, action.EffectiveSuit, action.EffectiveRank, action.EffectiveIsRed, origin with { ActorFactionId = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[actorSeat]) });
    }

    internal int? FactionActionRewardChooser(int owner, SkillProgramTrigger trigger, CardActionContext action)
    {
        var effect = trigger.Effects.SingleOrDefault(IsFactionActionReward);
        if (effect is null || action.FactionOrigin is not { } origin || origin.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || (effect.Op == SkillProgramEffectOp.RewardOutOfTurnFactionDodge ? action.EffectiveKind != CardKind.Dodge : !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind)))
            return null;
        var actualSeat = action.ActorSeat == owner ? action.ProviderSeat : action.ActorSeat;
        var faction = action.ActorSeat == owner ? origin.ProviderFactionId : origin.ActorFactionId;
        if (actualSeat == owner || actualSeat == origin.ActualTurnOwnerSeat || faction != effect.ProviderFactionId || !_coreMatchParticipantsPlayersAccess._players[actualSeat].IsAlive)
            return null;
        return actualSeat;
    }

    internal bool WasFactionRewardOffered(int owner, string skillId, CardActionContext action) => action.Type == CardActionType.Use && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Action?.ActionId == action.ActionId)?.FactionRewardOffers?.Contains(new(owner, skillId)) == true;
    internal void RecordFactionRewardOffers(CardActionContext action, IReadOnlyList<ProgramCardTriggerCandidate> candidates)
    {
        if (action.Type != CardActionType.Use || !TracksFactionActionOrigin)
            return;
        var offers = candidates.Where(candidate => _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program!.Triggers.Single(trigger => trigger.Id == candidate.TriggerId).Effects.Any(IsFactionActionReward)).Select(candidate => new ProgramFactionRewardOffer(candidate.OwnerSeat, candidate.SkillId)).Distinct().ToArray();
        if (offers.Length == 0)
            return;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Action?.ActionId == action.ActionId);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, frame => frame with { FactionRewardOffers = Array.AsReadOnly((frame.FactionRewardOffers ?? []).Concat(offers).Distinct().ToArray()) });
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 10)]
    internal bool CanRunFactionActionReward(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(IsFactionActionReward))
            return true;
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(f => f.Id == context.ParentFrameId);
        return window is not null && context.OptionalChooserSeat is { } chooser && FactionActionRewardChooser(candidate.OwnerSeat, trigger, window.Action) == chooser && _coreLordSkillProjectionsOperations.HasSkillRoleQualification(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], candidate.SkillId, candidate.SkillInstanceId, Role.Lord);
    }
}
