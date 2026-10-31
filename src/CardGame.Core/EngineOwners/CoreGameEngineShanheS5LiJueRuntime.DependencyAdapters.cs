#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreShanheS5LiJueCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheS5LiJueCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetProgramTrigger(frame);
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime))]
internal sealed class CoreShanheS5LiJueCharacterLiJueOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheS5LiJueCharacterLiJueOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime> _targetFactory = targetFactory;
    public int[] LangxiTargets(int owner) => this._targetFactory().LangxiTargets(owner);
    public bool LiJueCurrentSource(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().LiJueCurrentSource(f);
    public void PublishLangxi(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().PublishLangxi(f);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LangxiChoices(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().LangxiChoices(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreShanheS5LiJueCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreShanheS5LiJueCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
}
