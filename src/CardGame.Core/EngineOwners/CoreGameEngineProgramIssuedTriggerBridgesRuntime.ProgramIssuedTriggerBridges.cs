#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramIssuedTriggerBridgesRuntime
{
    internal ProgramIssuedTriggerRuntime ProgramIssuedTriggers => _programIssuedTriggersOperations._programIssuedTriggers ??= new(ProgramIssuedTriggerCatalog.Default, _kernelCoreProgramRuntimeModulesOperations.ResolveRuntimeHandlerTarget);
    internal ProgramTriggerProofRuntime ProgramTriggerProofs => _programTriggerProofsOperations._programTriggerProofs ??= new(ProgramTriggerProofCatalog.Default, _kernelCoreProgramRuntimeModulesOperations.ResolveRuntimeHandlerTarget);

    [ProgramIssuedTrigger(30, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger30(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _programPipelineHooksOperations._programPipelineHooks.IssuedTriggerCandidate(candidate, context) ? true : null;
}
