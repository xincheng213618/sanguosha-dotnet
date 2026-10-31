#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime))]
internal sealed class CoreProgramDiscardOriginsCoreShanheStartingResourcesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDiscardOriginsCoreShanheStartingResourcesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> _targetFactory = targetFactory;
    public bool IsShanheStartingResourceDestroyedMaterial(global::CardGame.Core.CardMovementRecord move) => this._targetFactory().IsShanheStartingResourceDestroyedMaterial(move);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime))]
internal sealed class CoreProgramDiscardOriginsCharacterZhangFenAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDiscardOriginsCharacterZhangFenAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsDagongCheDestroyedMaterial(global::CardGame.Core.CardMovementRecord m) => this._targetFactory().IsDagongCheDestroyedMaterial(m);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime))]
internal sealed class CoreProgramDiscardOriginsCoreWushuangJinzhiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramDiscardOriginsCoreWushuangJinzhiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation? WushuangJinzhiDiscardSource(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().WushuangJinzhiDiscardSource(movement);
}
