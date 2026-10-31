#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramDistanceRulesRuntime
{
    internal ProgramDistanceRules DistanceRules => _distanceRulesOperations._distanceRules ??= new(CoreProgramDistanceRulesDefinitions.DistanceRuleMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler);
}
