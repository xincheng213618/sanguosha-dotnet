#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramCardUsePaymentsCardUsePaymentsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramCardUsePaymentsCardUsePaymentsOperations
    {
        public ref global::CardGame.Core.ProgramCardUsePayments? _cardUsePayments => ref engine._cardUsePayments;
    }

    [ProgramCapability]
    private sealed class CoreProgramCardUsePaymentsKernelCoreProgramRuntimeModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramCardUsePaymentsKernelCoreProgramRuntimeModulesOperations
    {
        public global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType) => engine.BindRuntimeHandler(method, delegateType);
    }
}
