#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime))]
internal sealed class CoreProgramPublicCardProtocolsCoreRecipientCategoryMarkObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPublicCardProtocolsCoreRecipientCategoryMarkObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientCategoryMarkObserversRuntime> _targetFactory = targetFactory;
    public bool ValidRecipientCategoryMarkReceipt(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidRecipientCategoryMarkReceipt(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime))]
internal sealed class CoreProgramPublicCardProtocolsCharacterLuSuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPublicCardProtocolsCharacterLuSuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMouRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouLuSuAnniversaryRuntime MouLuSuAnniversary { get => this._targetFactory().MouLuSuAnniversary; }
}
