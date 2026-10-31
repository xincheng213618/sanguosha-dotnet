#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouChengYuAnniversaryRuntime))]
internal sealed class CharacterChengYuMouMouChengYuAnniversaryAdapter(global::System.Func<global::CardGame.Core.MouChengYuAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterChengYuMouMouChengYuAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouChengYuAnniversaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouChengYuAnniversaryRuntime Get() => this._targetFactory();
}
