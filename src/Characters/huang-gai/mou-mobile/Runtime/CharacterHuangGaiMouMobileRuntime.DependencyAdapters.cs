#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime))]
internal sealed class CharacterHuangGaiMouMobileCorePlayerViewProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterHuangGaiMouMobileCorePlayerViewProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime))]
internal sealed class CharacterHuangGaiMouMobileCoreProgramCardUsePaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterHuangGaiMouMobileCoreProgramCardUsePaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramCardUsePayments CardUsePayments { get => this._targetFactory().CardUsePayments; }
}
