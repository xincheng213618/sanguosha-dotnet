#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramViewContributorsRuntime
{
    internal ProgramViewContributors ViewContributors => _viewContributorsOperations._viewContributors ??= new(global::CardGame.Core.EngineOwners.CoreProgramViewContributorsDefinitions.ViewContributionMetadata.Value, row => _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler(row.Method, row.DelegateType));
}
