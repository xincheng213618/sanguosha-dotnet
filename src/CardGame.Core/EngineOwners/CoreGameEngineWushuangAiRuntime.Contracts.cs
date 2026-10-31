#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreWushuangAiCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreWushuangAiCoreWushuangAcquisitionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginWushuangAcquisition(long windowId, global::CardGame.Core.WushuangShopOffer offer);
}

internal interface ICoreWushuangAiContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreWushuangAiCoreWushuangTacticRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ChangeWushuangFreeRerolls(int seat, int delta, string reason);
}

internal interface ICoreWushuangAiCoreWushuangOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceWushuangShopSeat(global::CardGame.Core.WushuangIntermissionFrame frame);
    string? ValidateWushuangPurchase(int seat, global::CardGame.Core.WushuangShopOffer? offer, string? replacement);
    void BuyWushuangOffer(int seat, global::CardGame.Core.WushuangShopOffer offer, string? replacement, bool opening = false);
    void CompleteWushuangShopAction(long frameId);
    void ChangeWushuangTokens(int seat, int delta, string reason);
    void RefreshWushuangOffers(int seat, bool opening = false);
    global::CardGame.Core.IWushuangModeFeature RequireWushuangFeature { get; }
}

internal interface ICoreWushuangAiCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreWushuangAiResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreWushuangAiCoreWushuangModeProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.WushuangSnapshot? CreateWushuangSnapshot(int viewerSeat);
}
