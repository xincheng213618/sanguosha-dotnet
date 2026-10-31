#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CharacterGuanYuAnniversaryGodCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterGuanYuAnniversaryGodCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime))]
internal sealed class CharacterGuanYuAnniversaryGodCoreAttributedMarkerEventsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterGuanYuAnniversaryGodCoreAttributedMarkerEventsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> _targetFactory = targetFactory;
    public void MutateParticipantMarker(global::CardGame.Core.ProgramSkillFrame frame, int seat, global::CardGame.Core.PlayerMarkerKind marker, int delta, bool activeOnly = false) => this._targetFactory().MutateParticipantMarker(frame, seat, marker, delta, activeOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime))]
internal sealed class CharacterGuanYuAnniversaryGodCoreDamageCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterGuanYuAnniversaryGodCoreDamageCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DamageTriggerWindowFrame? ActiveDamageTrigger { get => this._targetFactory().ActiveDamageTrigger; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CharacterGuanYuAnniversaryGodCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterGuanYuAnniversaryGodCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CharacterGuanYuAnniversaryGodCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterGuanYuAnniversaryGodCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.IDamageAttempt GetDamageTriggerAttack(global::CardGame.Core.DamageTriggerWindowFrame frame) => this._targetFactory().GetDamageTriggerAttack(frame);
}
