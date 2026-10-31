#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageCounterpartAcquisitionRuntime))]
internal sealed class CoreProgramPairedColorResponseAiCoreDamageCounterpartAcquisitionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageCounterpartAcquisitionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPairedColorResponseAiCoreDamageCounterpartAcquisitionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageCounterpartAcquisitionRuntime> _targetFactory = targetFactory;
    public bool HasDamageCounterpartAcquisition(int owner, string skillId, string stateId, int counterpart) => this._targetFactory().HasDamageCounterpartAcquisition(owner, skillId, stateId, counterpart);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreProgramPairedColorResponseAiCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPairedColorResponseAiCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponsesRuntime))]
internal sealed class CoreProgramPairedColorResponseAiCorePairedColorResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPairedColorResponseAiCorePairedColorResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponsesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PairedColorResponseLinkedEvent? ExactPairedColorResponseLink(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => this._targetFactory().ExactPairedColorResponseLink(window);
}
