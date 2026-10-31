#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreCurrentEnhancementVirtualReturnCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentEnhancementVirtualReturnCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreCurrentEnhancementVirtualReturnCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentEnhancementVirtualReturnCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime))]
internal sealed class CoreCurrentEnhancementVirtualReturnCoreCurrentEnhancementSlashRedirectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentEnhancementVirtualReturnCoreCurrentEnhancementSlashRedirectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime> _targetFactory = targetFactory;
    public bool HasCurrentEnhancementVirtualProducer(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasCurrentEnhancementVirtualProducer(use);
    public bool IsCurrentEnhancementSlashRedirectFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CurrentEnhancementSlashRedirectedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history) => this._targetFactory().IsCurrentEnhancementSlashRedirectFact(use, fact, before, history);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime))]
internal sealed class CoreCurrentEnhancementVirtualReturnCharacterLuBuHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCurrentEnhancementVirtualReturnCharacterLuBuHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuHappyGodRuntime> _targetFactory = targetFactory;
    public bool IsHappyLuBuTrueVirtualProducer(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.SkillProgramEffect paused) => this._targetFactory().IsHappyLuBuTrueVirtualProducer(use, parent, paused);
}
