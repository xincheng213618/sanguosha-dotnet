#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreNativeIceSwordCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeIceSwordCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class CoreNativeIceSwordKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeIceSwordKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreNativeIceSwordKernelCoreMainAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreNativeIceSwordKernelCoreMainOperations
    {
        public global::CardGame.Core.NativeOwners.IceSwordHandle CreateIceSwordHandle(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int preventedDamageAmount) => engine.ComposeNativeIceSwordHandle(attack, preventedDamageAmount);
    }
}
