#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime))]
internal sealed class CoreProgramAttackAttemptCoreProgramDamageHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAttackAttemptCoreProgramDamageHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDamageHooks DamageHooks { get => this._targetFactory().DamageHooks; }
}
