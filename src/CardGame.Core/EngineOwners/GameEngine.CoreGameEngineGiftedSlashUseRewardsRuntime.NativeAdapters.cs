#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreGiftedSlashUseRewardsResolutionStackAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGiftedSlashUseRewardsResolutionStackOperations
    {
        public ref readonly global::CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class CoreGiftedSlashUseRewardsSkillRuntimeStateAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGiftedSlashUseRewardsSkillRuntimeStateOperations
    {
        public ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState => ref engine._skillRuntimeState;
    }

    [ProgramCapability]
    private sealed class CoreGiftedSlashUseRewardsContentRegistryAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGiftedSlashUseRewardsContentRegistryOperations
    {
        public ref readonly global::CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class CoreGiftedSlashUseRewardsKernelCoreGameEngineRuleProgressionAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGiftedSlashUseRewardsKernelCoreGameEngineRuleProgressionOperations
    {
        public void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class CoreGiftedSlashUseRewardsCoreObserverSubtreesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGiftedSlashUseRewardsCoreObserverSubtreesOperations2
    {
        public bool HasPaidDamageObserverSuffix(int rootIndex) => engine.ObserverSubtreesEntries.HasPaidDamageObserverSuffix(rootIndex);
    }

    [ProgramCapability]
    private sealed class CoreGiftedSlashUseRewardsCoreGiftedSlashUseRewardsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGiftedSlashUseRewardsCoreGiftedSlashUseRewardsOperations
    {
        public bool GiftedSlashFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => engine.GiftedSlashUseRewardsEntries.GiftedSlashFirstChild(frame, child);
    }

    [ProgramCapability]
    private sealed class CoreGiftedSlashUseRewardsCardZonesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreGiftedSlashUseRewardsCardZonesOperations
    {
        public ref readonly global::CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }
}
