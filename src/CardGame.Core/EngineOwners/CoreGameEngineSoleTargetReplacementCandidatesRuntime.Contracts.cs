#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreSoleTargetReplacementCandidatesCoreSoleTargetRedirectOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactSoleTargetRedirectReturn(global::CardGame.Core.ProgramCardTriggerWindowFrame frame);
}

internal interface ICoreSoleTargetReplacementCandidatesCoreSoleTargetProtectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] SoleProtectionMainTargets(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSoleTargetReplacementCandidatesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreSoleTargetReplacementCandidatesCoreSoleTargetQuarantineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSoleTargetReplacementFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SoleTargetReplacedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before);
}

internal interface ICoreSoleTargetReplacementCandidatesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreSoleTargetReplacementCandidatesCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}
