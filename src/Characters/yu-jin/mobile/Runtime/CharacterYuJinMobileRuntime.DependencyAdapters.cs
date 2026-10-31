#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramPaidCardRuntime))]
internal sealed class CharacterYuJinMobileProgramPaidCardAdapter(global::System.Func<global::CardGame.Core.ProgramPaidCardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterYuJinMobileProgramPaidCardOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramPaidCardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramPaidCardRuntime Get() => this._targetFactory();
}
