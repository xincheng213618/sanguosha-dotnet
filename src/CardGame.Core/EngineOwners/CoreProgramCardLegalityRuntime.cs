#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramCardLegalityRuntime
{
    private readonly ICoreProgramCardLegalityCharacterYuanShaoBoundaryMobileOperations _characterYuanShaoBoundaryMobileOperations;
    private readonly ICoreProgramCardLegalityCardLegalityOperations _cardLegalityOperations;
    private readonly ICoreProgramCardLegalityKernelCoreProgramRuntimeModulesOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramCardLegalityRuntime(ICoreProgramCardLegalityCharacterYuanShaoBoundaryMobileOperations characterYuanShaoBoundaryMobileOperations, ICoreProgramCardLegalityCardLegalityOperations cardLegalityOperations, ICoreProgramCardLegalityKernelCoreProgramRuntimeModulesOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._characterYuanShaoBoundaryMobileOperations = characterYuanShaoBoundaryMobileOperations;
        this._cardLegalityOperations = cardLegalityOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
