#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramCardLegalityRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYuanShaoBoundaryMobileEntries _characterYuanShaoBoundaryMobileOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardLegalityAccess _cardLegalityOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramCardLegalityRuntime(global::CardGame.Core.RuntimeEntries.CharacterYuanShaoBoundaryMobileEntries characterYuanShaoBoundaryMobileOperations, global::CardGame.Core.EngineOwners.IKernelCardLegalityAccess cardLegalityOperations, global::CardGame.Core.EngineOwners.IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._characterYuanShaoBoundaryMobileOperations = characterYuanShaoBoundaryMobileOperations;
        this._cardLegalityOperations = cardLegalityOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
