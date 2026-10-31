#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CoreWushuangAiCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangAiCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAcquisitionRuntime))]
internal sealed class CoreWushuangAiCoreWushuangAcquisitionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAcquisitionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangAiCoreWushuangAcquisitionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangAcquisitionRuntime> _targetFactory = targetFactory;
    public bool TryBeginWushuangAcquisition(long windowId, global::CardGame.Core.WushuangShopOffer offer) => this._targetFactory().TryBeginWushuangAcquisition(windowId, offer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTacticRulesRuntime))]
internal sealed class CoreWushuangAiCoreWushuangTacticRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTacticRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangAiCoreWushuangTacticRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTacticRulesRuntime> _targetFactory = targetFactory;
    public void ChangeWushuangFreeRerolls(int seat, int delta, string reason) => this._targetFactory().ChangeWushuangFreeRerolls(seat, delta, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime))]
internal sealed class CoreWushuangAiCoreWushuangAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangAiCoreWushuangOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangRuntime> _targetFactory = targetFactory;
    public void AdvanceWushuangShopSeat(global::CardGame.Core.WushuangIntermissionFrame frame) => this._targetFactory().AdvanceWushuangShopSeat(frame);
    public string? ValidateWushuangPurchase(int seat, global::CardGame.Core.WushuangShopOffer? offer, string? replacement) => this._targetFactory().ValidateWushuangPurchase(seat, offer, replacement);
    public void BuyWushuangOffer(int seat, global::CardGame.Core.WushuangShopOffer offer, string? replacement, bool opening = false) => this._targetFactory().BuyWushuangOffer(seat, offer, replacement, opening);
    public void CompleteWushuangShopAction(long frameId) => this._targetFactory().CompleteWushuangShopAction(frameId);
    public void ChangeWushuangTokens(int seat, int delta, string reason) => this._targetFactory().ChangeWushuangTokens(seat, delta, reason);
    public void RefreshWushuangOffers(int seat, bool opening = false) => this._targetFactory().RefreshWushuangOffers(seat, opening);
    public global::CardGame.Core.IWushuangModeFeature RequireWushuangFeature { get => this._targetFactory().RequireWushuangFeature; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreWushuangAiCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangAiCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime))]
internal sealed class CoreWushuangAiCoreWushuangModeProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangAiCoreWushuangModeProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.WushuangSnapshot? CreateWushuangSnapshot(int viewerSeat) => this._targetFactory().CreateWushuangSnapshot(viewerSeat);
}
