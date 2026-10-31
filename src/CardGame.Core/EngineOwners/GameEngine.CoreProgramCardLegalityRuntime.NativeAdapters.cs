#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramCardLegalityCardLegalityAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramCardLegalityCardLegalityOperations
    {
        public ref global::CardGame.Core.ProgramCardLegality? _cardLegality => ref engine._cardLegality;
    }

    [ProgramCapability]
    private sealed class CoreProgramCardLegalityKernelCoreProgramRuntimeModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramCardLegalityKernelCoreProgramRuntimeModulesOperations
    {
        public global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType) => engine.BindRuntimeHandler(method, delegateType);
    }
}
