#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime))]
internal sealed class CoreNativeDrawInvoiceRuntimePortsCharacterMiHengLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawInvoiceRuntimePortsCharacterMiHengLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> _targetFactory = targetFactory;
    public bool HasLeMiHengJiguCapability { get => this._targetFactory().HasLeMiHengJiguCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime))]
internal sealed class CoreNativeDrawInvoiceRuntimePortsCharacterZhaoZhongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawInvoiceRuntimePortsCharacterZhaoZhongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> _targetFactory = targetFactory;
    public bool HasZhaoZhongCapability { get => this._targetFactory().HasZhaoZhongCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime))]
internal sealed class CoreNativeDrawInvoiceRuntimePortsCharacterPangJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawInvoiceRuntimePortsCharacterPangJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> _targetFactory = targetFactory;
    public bool HasPangJiCapability { get => this._targetFactory().HasPangJiCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime))]
internal sealed class CoreNativeDrawInvoiceRuntimePortsCharacterTaoQianOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawInvoiceRuntimePortsCharacterTaoQianOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaoQianOlRuntime> _targetFactory = targetFactory;
    public bool HasTaoQianCapability { get => this._targetFactory().HasTaoQianCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime))]
internal sealed class CoreNativeDrawInvoiceRuntimePortsCharacterLiuZhangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawInvoiceRuntimePortsCharacterLiuZhangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZhangOlRuntime> _targetFactory = targetFactory;
    public bool HasFengweiCapability { get => this._targetFactory().HasFengweiCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime))]
internal sealed class CoreNativeDrawInvoiceRuntimePortsCoreHellKingProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawInvoiceRuntimePortsCoreHellKingProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellKingProgramsRuntime> _targetFactory = targetFactory;
    public bool HasHellZhiwangHolder { get => this._targetFactory().HasHellZhiwangHolder; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime))]
internal sealed class CoreNativeDrawInvoiceRuntimePortsCharacterFeiYiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawInvoiceRuntimePortsCharacterFeiYiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFeiYiOlRuntime> _targetFactory = targetFactory;
    public bool HasFeiYiHezhong { get => this._targetFactory().HasFeiYiHezhong; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreNativeDrawInvoiceRuntimePortsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawInvoiceRuntimePortsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterTangTangOlRuntime))]
internal sealed class CoreNativeDrawInvoiceRuntimePortsCharacterTangTangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangTangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawInvoiceRuntimePortsCharacterTangTangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTangTangOlRuntime> _targetFactory = targetFactory;
    public bool HasTangTangZhiyi { get => this._targetFactory().HasTangTangZhiyi; }

    public void CompleteTangTangNativeDrawQualification(global::CardGame.Core.NativeDrawInvocationProof proof) => this._targetFactory().CompleteTangTangNativeDrawQualification(proof);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYuanJiOlRuntime))]
internal sealed class CoreNativeDrawInvoiceRuntimePortsCharacterYuanJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawInvoiceRuntimePortsCharacterYuanJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanJiOlRuntime> _targetFactory = targetFactory;
    public bool HasYuanJiCapability { get => this._targetFactory().HasYuanJiCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime))]
internal sealed class CoreNativeDrawInvoiceRuntimePortsCharacterZhouFeiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawInvoiceRuntimePortsCharacterZhouFeiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiLeRuntime> _targetFactory = targetFactory;
    public bool HasLeZhouFeiLingkongCapability { get => this._targetFactory().HasLeZhouFeiLingkongCapability; }

    public void CompleteLingkongNativeDrawQualification(global::CardGame.Core.NativeDrawInvocationProof proof) => this._targetFactory().CompleteLingkongNativeDrawQualification(proof);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime))]
internal sealed class CoreNativeDrawInvoiceRuntimePortsCharacterWangCanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeDrawInvoiceRuntimePortsCharacterWangCanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangCanOlRuntime> _targetFactory = targetFactory;
    public bool HasSanwenCapability { get => this._targetFactory().HasSanwenCapability; }

    public void CompleteSanwenNativeDrawQualification(global::CardGame.Core.NativeDrawInvocationProof proof) => this._targetFactory().CompleteSanwenNativeDrawQualification(proof);
}
