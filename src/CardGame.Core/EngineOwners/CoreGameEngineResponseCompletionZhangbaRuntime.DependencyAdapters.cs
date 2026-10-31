#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoYunMobileShiRuntime))]
internal sealed class CoreResponseCompletionZhangbaCharacterZhaoYunMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionZhangbaCharacterZhaoYunMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoYunMobileShiRuntime> _targetFactory = targetFactory;
    public bool HasMobileShiCuifengCapability { get => this._targetFactory().HasMobileShiCuifengCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime))]
internal sealed class CoreResponseCompletionZhangbaCoreActionDiscardColorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionZhangbaCoreActionDiscardColorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> _targetFactory = targetFactory;
    public bool? CapturePhysicalCardColor(int seat, global::CardGame.Core.Card card) => this._targetFactory().CapturePhysicalCardColor(seat, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed class CoreResponseCompletionZhangbaCoreNativeOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseCompletionZhangbaCoreNativeOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => this._targetFactory().FindOwnedCardLocation(actor, card);
}
