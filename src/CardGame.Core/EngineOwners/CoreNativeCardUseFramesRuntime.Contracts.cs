#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeCardUseFramesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeCardUseFramesCharacterMaChengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasChenglieTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseFramesCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasKenshangTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseFramesCoreCurrentEnhancementSlashRedirectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCurrentEnhancementVirtualProducer(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseFramesCharacterQianZhaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeifuTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseFramesCharacterMaWanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMaWanHunjiangTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseFramesCharacterYangFuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBingzhongTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseFramesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreNativeCardUseFramesCharacterLiuPanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPijingTargetTail(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreNativeCardUseFramesCharacterZhangYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZhangYanLangdaoTargetTail(global::CardGame.Core.CardUseFrame use);
}
