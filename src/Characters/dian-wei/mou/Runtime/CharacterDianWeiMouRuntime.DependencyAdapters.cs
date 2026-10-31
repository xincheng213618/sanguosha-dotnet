#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouDianWeiRuntime))]
internal sealed class CharacterDianWeiMouMouDianWeiAdapter(global::System.Func<global::CardGame.Core.MouDianWeiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDianWeiMouMouDianWeiOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouDianWeiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouDianWeiRuntime Get() => this._targetFactory();
}
