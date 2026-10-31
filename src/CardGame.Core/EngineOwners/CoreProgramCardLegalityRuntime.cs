#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramCardLegalityRuntime
{
    private readonly RuntimeEntries.CharacterYuanShaoBoundaryMobileEntries _characterYuanShaoBoundaryMobileOperations;
    private readonly IKernelCardLegalityAccess _cardLegalityOperations;
    private readonly IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramCardLegalityRuntime(RuntimeEntries.CharacterYuanShaoBoundaryMobileEntries characterYuanShaoBoundaryMobileOperations, IKernelCardLegalityAccess cardLegalityOperations, IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._characterYuanShaoBoundaryMobileOperations = characterYuanShaoBoundaryMobileOperations;
        this._cardLegalityOperations = cardLegalityOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
