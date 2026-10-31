#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreMatchSetupDefinitionsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SupportsGodFactionSelection { get; }
}

internal interface ICoreMatchSetupDefinitionsCorePrivateGeneralLibrariesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetPrivateGeneralLibraryFaction(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreMatchSetupDefinitionsCoreModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IMatchModeFactionPolicy ModeFactionPolicy { get; }
}
