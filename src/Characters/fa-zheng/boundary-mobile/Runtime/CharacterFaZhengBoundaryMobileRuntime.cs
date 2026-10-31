#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CharacterFaZhengBoundaryMobileRuntime
{
    private readonly RuntimeEntries.CharacterJiangWeiBoundaryMobileEntries _characterJiangWeiBoundaryMobileOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreGameEngineAssistedPhysicalCardsEntries _coreAssistedPhysicalCardsOperations;
    private readonly RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries _corePrivateOfferReturnsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries _coreProgramDodgeResponsesOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterFaZhengBoundaryMobileRuntime(RuntimeEntries.CharacterJiangWeiBoundaryMobileEntries characterJiangWeiBoundaryMobileOperations, IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreGameEngineAssistedPhysicalCardsEntries coreAssistedPhysicalCardsOperations, RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries corePrivateOfferReturnsOperations, RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries coreProgramDodgeResponsesOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterJiangWeiBoundaryMobileOperations = characterJiangWeiBoundaryMobileOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAssistedPhysicalCardsOperations = coreAssistedPhysicalCardsOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
