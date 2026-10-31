#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramPublicCardProtocolsRuntime
{
    string IProgramPublicCardProtocolQueries.CompletedUndamagedRevealBind => "completed-undamaged-target-reveal";

    bool IProgramPublicCardProtocolQueries.ValidMouLuSuReceipt(ProgramSkillFrame frame) => _characterLuSuMouOperations.Value.MouLuSuAnniversary.Valid(frame);
    bool IProgramPublicCardProtocolQueries.ValidRecipientCategoryMarkReceipt(ProgramSkillFrame frame) => _coreRecipientCategoryMarkObserversOperations.ValidRecipientCategoryMarkReceipt(frame);
}
