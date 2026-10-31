#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramTargetRulesRuntime
{
    internal ProgramTargetRules TargetRules => _targetRulesOperations._targetRules ??= new(CoreProgramTargetRulesDefinitions.TargetRuleMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler);
}
