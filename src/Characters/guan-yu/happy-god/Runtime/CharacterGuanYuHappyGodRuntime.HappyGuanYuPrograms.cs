#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuHappyGodRuntime
{
    internal bool HasHappyGuanYuHeartSlashCapability => _contentRegistryOperations._contentRegistry.Skills.ContainsKey(HappyGuanYuComposition.Wushen);

    [ProgramDamageHook(ProgramDamagePhase.FactModifiers, 41880)]
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> HappyGuanYuHeartSlashDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (attack.IsChainPropagation || attack.EffectiveCardKind is not { } kind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Action: { Type: CardActionType.Use, EffectiveSuit: Suit.Heart } action } use || use.CardKind != kind || action.EffectiveKind != kind || use.SourceSeat != action.ActorSeat || action.ActorSeat != attack.CardUserSeat)
            return[];
        return _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]).GetCardPolicies(SkillProgramCardPolicyKind.IgnoreSlashUseDistanceBySuit).Where(b => b.Source.SkillId == HappyGuanYuComposition.Wushen && b.Policy.Id == "heart-slash-distance").Select(b => (new CardUseEffectSource(b.Source.SkillId, b.Policy.Id, action.ActorSeat, b.Source.SkillInstanceId), 1));
    }

    internal bool IsHappyGuanYuDyingExitCandidate(ProgramTriggerCandidate c, int victim) => c.OwnerSeat == victim && c.SkillId == HappyGuanYuComposition.Wuhun && c.BindingId == "survived-nightmare";
    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 110, SkillProgramTriggerWindow.DyingExited)]
    internal bool IsHappyGuanYuDyingExitContext(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!IsHappyGuanYuDyingExitCandidate(c, c.OwnerSeat) || context.Window != SkillProgramTriggerWindow.DyingExited || context.SourceSeat != c.OwnerSeat || context.TargetSeat != c.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].Hp <= 0 || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { Window: SkillProgramTriggerWindow.DyingExited, Continuation: ProgramLifecycleContinuation.ResumeYanPuDyingExit, YanPuDyingExit: { } ret } parent || parent.OwnerSeat != c.OwnerSeat || ret.VictimSeat != c.OwnerSeat || ret.Round != _coreActualRoundProgressRoundNumberAccess._roundNumber || ret.Turn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || ret.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != c)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == parent.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == ret.DyingParentFrameId && _coreProgramEventHistoryOperations.ProgramEventHistory<DyingResolvedEvent>().Count(e => e.ResolutionId == ret.DyingFrameId && e.VictimSeat == c.OwnerSeat && e.Survived) == 1;
    }

    internal bool IsHappyGuanYuDyingExitReturn(ProgramSkillFrame f) => f.WindowContext is { } context && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } parent && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]) && IsHappyGuanYuDyingExitContext(parent.Candidates[parent.CandidateIndex], context);
}
