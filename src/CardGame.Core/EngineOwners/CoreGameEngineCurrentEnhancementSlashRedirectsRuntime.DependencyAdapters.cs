#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreCurrentEnhancementSlashRedirectsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentEnhancementSlashRedirectsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreCurrentEnhancementSlashRedirectsCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentEnhancementSlashRedirectsCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreCurrentEnhancementSlashRedirectsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentEnhancementSlashRedirectsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime))]
internal sealed class CoreCurrentEnhancementSlashRedirectsCharacterLuBuHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentEnhancementSlashRedirectsCharacterLuBuHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime> _targetFactory = targetFactory;
    public bool IsHappyLuBuTrueVirtualProducer(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().IsHappyLuBuTrueVirtualProducer(use, parent, paused);
}
