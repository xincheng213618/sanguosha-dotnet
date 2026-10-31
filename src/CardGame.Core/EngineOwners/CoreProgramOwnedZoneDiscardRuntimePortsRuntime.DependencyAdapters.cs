#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime))]
internal sealed class CoreProgramOwnedZoneDiscardRuntimePortsCoreHellMengpoProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedZoneDiscardRuntimePortsCoreHellMengpoProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> _targetFactory = targetFactory;
    public bool HasHellShiyouCapability { get => this._targetFactory().HasHellShiyouCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime))]
internal sealed class CoreProgramOwnedZoneDiscardRuntimePortsCoreProgramDiscardOriginsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedZoneDiscardRuntimePortsCoreProgramDiscardOriginsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation? GetProgramDiscardSource(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().GetProgramDiscardSource(movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBuZhiOlRuntime))]
internal sealed class CoreProgramOwnedZoneDiscardRuntimePortsCharacterBuZhiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuZhiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramOwnedZoneDiscardRuntimePortsCharacterBuZhiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuZhiOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.NativeBuZhiDiscardInvocationDraft? CaptureNativeBuZhiDiscardInvocation(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().CaptureNativeBuZhiDiscardInvocation(frame, zones, reason);
    public void CompleteNativeBuZhiDiscardInvocation(global::CardGame.Core.GameEngine.NativeBuZhiDiscardInvocationDraft? draft, global::System.Collections.Generic.IReadOnlyList<int> actualIds) => this._targetFactory().CompleteNativeBuZhiDiscardInvocation(draft, actualIds);
}
