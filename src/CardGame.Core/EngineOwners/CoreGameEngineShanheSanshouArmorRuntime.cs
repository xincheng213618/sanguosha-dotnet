#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheSanshouArmorRuntime
{
    private readonly RuntimeEntries.CharacterHuCheErMouEntries _characterHuCheErMouOperations;
    private readonly RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries _coreDirectedCardPolicyQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineEquipmentSkillsEntries _coreEquipmentSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineExtendedViewAsEntries _coreExtendedViewAsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    internal CoreGameEngineShanheSanshouArmorRuntime(RuntimeEntries.CharacterHuCheErMouEntries characterHuCheErMouOperations, RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries coreDirectedCardPolicyQueriesOperations, RuntimeEntries.CoreGameEngineEquipmentSkillsEntries coreEquipmentSkillsOperations, RuntimeEntries.CoreGameEngineExtendedViewAsEntries coreExtendedViewAsOperations, RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations)
    {
        this._characterHuCheErMouOperations = characterHuCheErMouOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreExtendedViewAsOperations = coreExtendedViewAsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
    }
}
