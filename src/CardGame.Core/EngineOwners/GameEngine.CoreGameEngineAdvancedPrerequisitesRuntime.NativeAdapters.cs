#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreAdvancedPrerequisitesSkillRuntimeStateAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAdvancedPrerequisitesSkillRuntimeStateOperations
    {
        public ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState => ref engine._skillRuntimeState;
    }

    [ProgramCapability]
    private sealed class CoreAdvancedPrerequisitesCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreAdvancedPrerequisitesCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
