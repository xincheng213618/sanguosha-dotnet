#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime))]
internal sealed class CoreNativeCardUseFramesCharacterMaChengOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseFramesCharacterMaChengOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChengOlRuntime> _targetFactory = targetFactory;
    public bool HasChenglieTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasChenglieTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime))]
internal sealed class CoreNativeCardUseFramesCharacterMaXiuMaTieOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseFramesCharacterMaXiuMaTieOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaXiuMaTieOlRuntime> _targetFactory = targetFactory;
    public bool HasKenshangTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasKenshangTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime))]
internal sealed class CoreNativeCardUseFramesCoreCurrentEnhancementSlashRedirectsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseFramesCoreCurrentEnhancementSlashRedirectsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentEnhancementSlashRedirectsRuntime> _targetFactory = targetFactory;
    public bool HasCurrentEnhancementVirtualProducer(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasCurrentEnhancementVirtualProducer(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime))]
internal sealed class CoreNativeCardUseFramesCharacterQianZhaoOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseFramesCharacterQianZhaoOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQianZhaoOlRuntime> _targetFactory = targetFactory;
    public bool HasWeifuTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasWeifuTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime))]
internal sealed class CoreNativeCardUseFramesCharacterMaWanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseFramesCharacterMaWanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaWanOlRuntime> _targetFactory = targetFactory;
    public bool HasMaWanHunjiangTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasMaWanHunjiangTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime))]
internal sealed class CoreNativeCardUseFramesCharacterYangFuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseFramesCharacterYangFuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYangFuOlRuntime> _targetFactory = targetFactory;
    public bool HasBingzhongTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasBingzhongTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreNativeCardUseFramesCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseFramesCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime))]
internal sealed class CoreNativeCardUseFramesCharacterLiuPanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseFramesCharacterLiuPanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuPanOlRuntime> _targetFactory = targetFactory;
    public bool HasPijingTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasPijingTargetTail(use);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime))]
internal sealed class CoreNativeCardUseFramesCharacterZhangYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeCardUseFramesCharacterZhangYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanOlRuntime> _targetFactory = targetFactory;
    public bool HasZhangYanLangdaoTargetTail(global::CardGame.Core.CardUseFrame use) => this._targetFactory().HasZhangYanLangdaoTargetTail(use);
}
