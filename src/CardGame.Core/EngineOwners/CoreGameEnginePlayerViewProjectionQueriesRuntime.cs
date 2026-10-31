#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEnginePlayerViewProjectionQueriesRuntime : global::CardGame.Core.IHandCardProjectionRules
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateActivePlayerEffectQueriesOperations _kernelCoreGameEngineActivePlayerEffectQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelPlayerViewQueryFactories _kernelCoreGameEnginePlayerViewProjectionQueriesOperations;
    private readonly ICorePlayerViewProjectionQueriesPlayerViewProjectorOperations _playerViewProjectorOperations;
    internal CoreGameEnginePlayerViewProjectionQueriesRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, global::CardGame.Core.EngineOwners.IKernelCreateActivePlayerEffectQueriesOperations kernelCoreGameEngineActivePlayerEffectQueriesOperations, global::CardGame.Core.EngineOwners.IKernelPlayerViewQueryFactories kernelCoreGameEnginePlayerViewProjectionQueriesOperations, ICorePlayerViewProjectionQueriesPlayerViewProjectorOperations playerViewProjectorOperations)
    {
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._kernelCoreGameEngineActivePlayerEffectQueriesOperations = kernelCoreGameEngineActivePlayerEffectQueriesOperations;
        this._kernelCoreGameEnginePlayerViewProjectionQueriesOperations = kernelCoreGameEnginePlayerViewProjectionQueriesOperations;
        this._playerViewProjectorOperations = playerViewProjectorOperations;
    }
}
