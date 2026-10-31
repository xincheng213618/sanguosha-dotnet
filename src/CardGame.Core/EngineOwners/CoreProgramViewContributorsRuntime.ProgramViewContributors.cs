#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramViewContributorsRuntime
{
    internal ProgramViewContributors ViewContributors => _viewContributorsOperations._viewContributors ??= new(CoreProgramViewContributorsDefinitions.ViewContributionMetadata.Value, row => _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler(row.Method, row.DelegateType));
}
