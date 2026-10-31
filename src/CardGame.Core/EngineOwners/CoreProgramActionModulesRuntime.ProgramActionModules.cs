#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramActionModulesRuntime
{
    internal ProgramActionRouter ProgramActions => _programActionsOperations._programActions ??= new(CoreProgramActionModulesDefinitions.ProgramActionMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler);
}
