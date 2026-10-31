#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime))]
internal sealed class CoreProgramStateCardContinuationCoreSoleIncomingRangeDrawAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramStateCardContinuationCoreSoleIncomingRangeDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> _targetFactory = targetFactory;
    public bool IsSoleIncomingRangeDrawStateReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().IsSoleIncomingRangeDrawStateReturn(use, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime))]
internal sealed class CoreProgramStateCardContinuationCoreNativeCardEffectDrawsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramStateCardContinuationCoreNativeCardEffectDrawsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardEffectDrawsRuntime> _targetFactory = targetFactory;
    public bool IsNativeCardEffectDrawStateReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => this._targetFactory().IsNativeCardEffectDrawStateReturn(use, frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreProgramStateCardContinuationCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramStateCardContinuationCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public bool IsWumiaoFloodStateReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => this._targetFactory().IsWumiaoFloodStateReturn(use, frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime))]
internal sealed class CoreProgramStateCardContinuationCoreWenheCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramStateCardContinuationCoreWenheCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> _targetFactory = targetFactory;
    public bool IsWenheCardEffectStateReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame state) => this._targetFactory().IsWenheCardEffectStateReturn(use, state);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed class CoreProgramStateCardContinuationCoreDragonBoatCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramStateCardContinuationCoreDragonBoatCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _targetFactory = targetFactory;
    public bool IsDragonBoatCardEffectStateReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => this._targetFactory().IsDragonBoatCardEffectStateReturn(use, frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime))]
internal sealed class CoreProgramStateCardContinuationCoreGuanduCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramStateCardContinuationCoreGuanduCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> _targetFactory = targetFactory;
    public bool IsGuanduCardEffectStateReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => this._targetFactory().IsGuanduCardEffectStateReturn(use, frame);
}
