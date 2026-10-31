#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramTargetRulesRuntime
{
    internal ProgramTargetRules TargetRules => _targetRulesOperations._targetRules ??= new(global::CardGame.Core.EngineOwners.CoreProgramTargetRulesDefinitions.TargetRuleMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler);
}
