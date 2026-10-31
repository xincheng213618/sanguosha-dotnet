#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime))]
internal sealed class CoreShanheSchoolLateCoreShanheS5QuyiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheSchoolLateCoreShanheS5QuyiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5QuyiRuntime> _targetFactory = targetFactory;
    public int ShanheS5JiaoziModifierAmount(global::CardGame.Core.SkillProgramDamageModifier modifier) => this._targetFactory().ShanheS5JiaoziModifierAmount(modifier);
}
