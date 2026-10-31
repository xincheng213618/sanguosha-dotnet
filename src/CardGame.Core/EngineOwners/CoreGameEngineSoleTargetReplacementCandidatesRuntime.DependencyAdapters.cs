#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime))]
internal sealed class CoreSoleTargetReplacementCandidatesCoreSoleTargetRedirectAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetReplacementCandidatesCoreSoleTargetRedirectOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetRedirectRuntime> _targetFactory = targetFactory;
    public bool IsExactSoleTargetRedirectReturn(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => this._targetFactory().IsExactSoleTargetRedirectReturn(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime))]
internal sealed class CoreSoleTargetReplacementCandidatesCoreSoleTargetProtectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetReplacementCandidatesCoreSoleTargetProtectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> _targetFactory = targetFactory;
    public int[] SoleProtectionMainTargets(global::CardGame.Core.CardUseFrame use) => this._targetFactory().SoleProtectionMainTargets(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreSoleTargetReplacementCandidatesCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetReplacementCandidatesCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime))]
internal sealed class CoreSoleTargetReplacementCandidatesCoreSoleTargetQuarantineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetReplacementCandidatesCoreSoleTargetQuarantineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> _targetFactory = targetFactory;
    public bool IsSoleTargetReplacementFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SoleTargetReplacedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before) => this._targetFactory().IsSoleTargetReplacementFact(use, fact, before);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreSoleTargetReplacementCandidatesCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSoleTargetReplacementCandidatesCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}
