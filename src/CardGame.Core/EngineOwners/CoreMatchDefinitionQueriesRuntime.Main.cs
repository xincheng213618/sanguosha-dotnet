#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreMatchDefinitionQueriesRuntime
{
    internal int Seed => _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._options.Seed;
    internal ContentRegistry ContentRegistry => _contentRegistryOperations._contentRegistry;
    internal int PlayerCount => _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._playerCount;
    internal string ModeId => _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._modeDefinition.Id;
}
