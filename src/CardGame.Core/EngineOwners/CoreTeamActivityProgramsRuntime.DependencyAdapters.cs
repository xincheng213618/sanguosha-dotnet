#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreTeamActivityProgramsCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTeamActivityProgramsCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime))]
internal sealed class CoreTeamActivityProgramsCoreModeAllegianceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTeamActivityProgramsCoreModeAllegianceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeAllegianceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.IMatchModeAllegiancePolicy ModeAllegiancePolicy { get => this._targetFactory().ModeAllegiancePolicy; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime))]
internal sealed class CoreTeamActivityProgramsCoreExtraEffectsAndRankGuessingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTeamActivityProgramsCoreExtraEffectsAndRankGuessingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime> _targetFactory = targetFactory;
    public bool TryGetExtraEffectUse(int owner, global::CardGame.Core.ProgramSkillWindowContext? context, out global::CardGame.Core.CardUseFrame use) => this._targetFactory().TryGetExtraEffectUse(owner, context, out use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreTeamActivityProgramsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTeamActivityProgramsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTeamActivitiesRuntime))]
internal sealed class CoreTeamActivityProgramsCoreTeamActivitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTeamActivitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTeamActivityProgramsCoreTeamActivitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTeamActivitiesRuntime> _targetFactory = targetFactory;
    public bool SamePublicTeam(global::CardGame.Core.CharacterState a, global::CardGame.Core.CharacterState b) => this._targetFactory().SamePublicTeam(a, b);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreTeamActivityProgramsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreTeamActivityProgramsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> GetProgramTargetSeats(int ownerSeat, global::CardGame.Core.SkillProgramTargetKind targetKind, global::CardGame.Core.ProgramSkillWindowContext? windowContext = default, global::CardGame.Core.PlayerMarkerKind? marker = default, global::CardGame.Core.ProgramParticipantReference? actorReference = default) => this._targetFactory().GetProgramTargetSeats(ownerSeat, targetKind, windowContext, marker, actorReference);
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}
