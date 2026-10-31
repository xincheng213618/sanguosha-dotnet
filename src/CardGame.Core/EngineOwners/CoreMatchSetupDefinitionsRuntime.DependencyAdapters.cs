#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreMatchSetupDefinitionsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchSetupDefinitionsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public bool SupportsGodFactionSelection { get => this._targetFactory().SupportsGodFactionSelection; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime))]
internal sealed class CoreMatchSetupDefinitionsCorePrivateGeneralLibrariesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchSetupDefinitionsCorePrivateGeneralLibrariesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateGeneralLibrariesRuntime> _targetFactory = targetFactory;
    public string? GetPrivateGeneralLibraryFaction(global::CardGame.Core.CharacterState owner) => this._targetFactory().GetPrivateGeneralLibraryFaction(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime))]
internal sealed class CoreMatchSetupDefinitionsCoreModeRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMatchSetupDefinitionsCoreModeRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.IMatchModeFactionPolicy ModeFactionPolicy { get => this._targetFactory().ModeFactionPolicy; }
}
