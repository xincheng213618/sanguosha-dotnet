#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramDistanceRulesRuntime
{
    internal ProgramDistanceRules DistanceRules => _distanceRulesOperations._distanceRules ??= new(global::CardGame.Core.EngineOwners.CoreProgramDistanceRulesDefinitions.DistanceRuleMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler);
}
