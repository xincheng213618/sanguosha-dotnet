#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEnginePlayerViewProjectionQueriesRuntime : IHandCardProjectionRules
{
    private readonly RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly IKernelCreateActivePlayerEffectQueriesOperations _kernelCoreGameEngineActivePlayerEffectQueriesOperations;
    private readonly IKernelPlayerViewQueryFactories _kernelCoreGameEnginePlayerViewProjectionQueriesOperations;
    private readonly ICorePlayerViewProjectionQueriesPlayerViewProjectorOperations _playerViewProjectorOperations;
    internal CoreGameEnginePlayerViewProjectionQueriesRuntime(RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, IKernelProgramPipelineHooksAccess programPipelineHooksOperations, RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, IKernelCreateActivePlayerEffectQueriesOperations kernelCoreGameEngineActivePlayerEffectQueriesOperations, IKernelPlayerViewQueryFactories kernelCoreGameEnginePlayerViewProjectionQueriesOperations, ICorePlayerViewProjectionQueriesPlayerViewProjectorOperations playerViewProjectorOperations)
    {
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._kernelCoreGameEngineActivePlayerEffectQueriesOperations = kernelCoreGameEngineActivePlayerEffectQueriesOperations;
        this._kernelCoreGameEnginePlayerViewProjectionQueriesOperations = kernelCoreGameEnginePlayerViewProjectionQueriesOperations;
        this._playerViewProjectorOperations = playerViewProjectorOperations;
    }
}
