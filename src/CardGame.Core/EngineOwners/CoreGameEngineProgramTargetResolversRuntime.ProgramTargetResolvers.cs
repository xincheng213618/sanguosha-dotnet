#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramTargetResolversRuntime
{
    internal ProgramTargetResolverRegistry ProgramTargetResolvers => _programTargetResolversOperations._programTargetResolvers ??= new(CoreGameEngineProgramTargetResolversDefinitions.ProgramTargetResolverMetadata.Value, _coreProgramSelectionModulesOperations.ProgramSelectionQueries);
}
