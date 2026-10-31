#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuHongOlRuntime))]
internal sealed class CoreProgramAbolishAreasOperationCharacterLiuHongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuHongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAbolishAreasOperationCharacterLiuHongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuHongOlRuntime> _targetFactory = targetFactory;
    public void ResolveTuxingAfterSlotAbolish(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int abolishedSlotCount) => this._targetFactory().ResolveTuxingAfterSlotAbolish(frame, recipientSeat, abolishedSlotCount);
}
