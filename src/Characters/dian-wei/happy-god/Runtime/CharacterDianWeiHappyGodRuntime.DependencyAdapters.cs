#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramLifecycleWindowRuntime))]
internal sealed class CharacterDianWeiHappyGodProgramLifecycleWindowAdapter(global::System.Func<global::CardGame.Core.ProgramLifecycleWindowRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterDianWeiHappyGodProgramLifecycleWindowOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramLifecycleWindowRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramLifecycleWindowRuntime Get() => this._targetFactory();
}
