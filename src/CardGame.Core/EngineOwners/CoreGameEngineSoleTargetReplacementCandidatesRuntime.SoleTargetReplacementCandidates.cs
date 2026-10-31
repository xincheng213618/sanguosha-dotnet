#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSoleTargetReplacementCandidatesRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 10)]
    internal bool CanRunSoleTargetReplacementTargetCandidate(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (context.Window == SkillProgramTriggerWindow.SlashTargetRedirecting && trigger.OwnerRelation == SkillProgramCardActionOwnerRelation.Target && _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { SoleTargetRedirectReturn: not null } redirect && _coreSoleTargetRedirectOperations.IsExactSoleTargetRedirectReturn(redirect))
            return redirect.CandidateIndex >= redirect.SoleTargetRedirectReturn!.TargetEpisodeCandidateStartIndex && _coreCardUseLifecycleOperations.LifecycleCardUse(redirect.ParentFrameId)?.CardAttack?.TargetSeat == candidate.OwnerSeat;
        // Candidate contexts retain their original designation facts. A paid
        // replacement must still qualify a Target binding against this use's
        // current main targets before an original candidate can run.
        if (context.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || trigger.OwnerRelation != SkillProgramCardActionOwnerRelation.Target || context.CardUse is not { } identity || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is not { TargetsAdjusted: true, Action: { } action, SoleTargetProtectionOrigin: { } origin } use || identity.CardActionId != action.ActionId || origin.CardUseFrameId != use.Id || origin.ActionId != action.ActionId || origin.ActorSeat != use.SourceSeat || origin.ProviderSeat != action.ProviderSeat || origin.EffectiveKind != use.CardKind || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SoleTargetReplacedEvent>().Any(fact => _coreSoleTargetQuarantineOperations.IsSoleTargetReplacementFact(use, fact, fact.BeforeTargets)))
            return true;
        return _coreSoleTargetProtectionOperations.SoleProtectionMainTargets(use).Contains(candidate.OwnerSeat);
    }
}
