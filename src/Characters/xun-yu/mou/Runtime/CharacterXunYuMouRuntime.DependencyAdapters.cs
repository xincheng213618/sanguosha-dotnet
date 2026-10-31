#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.MouXunYuAnniversaryRuntime))]
internal sealed class CharacterXunYuMouMouXunYuAnniversaryAdapter(global::System.Func<global::CardGame.Core.MouXunYuAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterXunYuMouMouXunYuAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.MouXunYuAnniversaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MouXunYuAnniversaryRuntime Get() => this._targetFactory();
}
