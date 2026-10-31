#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MoZhangFeiRuntime))]
internal sealed class CharacterZhangFeiOlMagicMoZhangFeiAdapter(global::System.Func<global::CardGame.Core.MoZhangFeiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterZhangFeiOlMagicMoZhangFeiOperations
{
    private readonly global::System.Func<global::CardGame.Core.MoZhangFeiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MoZhangFeiRuntime Get() => this._targetFactory();
}
