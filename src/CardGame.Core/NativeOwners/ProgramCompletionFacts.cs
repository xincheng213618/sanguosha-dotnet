#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramCompletionFacts(EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, Func<RuntimeEntries.CharacterHeQiOlEntries> getCharacterHeQiOlRuntime, Func<RuntimeEntries.CoreGameEngineHpConditionalSkillGrantsEntries> getCoreGameEngineHpConditionalSkillGrantsRuntime, Func<RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries> getCoreGameEngineForcedPhaseAlcoholRuntime, Func<RuntimeEntries.CoreGameEngineDamageTargetObtainProgramsEntries> getCoreGameEngineDamageTargetObtainProgramsRuntime, Func<RuntimeEntries.CoreGameEngineDamageTargetMountProgramsEntries> getCoreGameEngineDamageTargetMountProgramsRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime) : IProgramCompletionFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public EngineStatus Status => matchOutcomeState.ReferenceToStatus;

    public int TriggerPriority(ProgramSkillFrame frame) => getCoreGameEngineProgramLifecycleRuntime().GetProgramTrigger(frame).Priority;
    public bool Alive(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool HasSkillInstance(int seat, string skillId, string instanceId) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[seat], skillId, instanceId);
    public bool CanContinueEquipmentSuit(ProgramSkillFrame frame) => getCharacterHeQiOlRuntime().CanContinueEquipmentSuitProgram(frame);
    public bool CanContinueHpConditional(ProgramSkillFrame frame) => getCoreGameEngineHpConditionalSkillGrantsRuntime().CanContinueHpConditionalProgram(frame);
    public bool CanContinuePhaseAlcohol(ProgramSkillFrame frame) => getCoreGameEngineForcedPhaseAlcoholRuntime().CanContinuePhaseAlcohol(frame);
    public bool ValidDamageTargetObtain(ProgramSkillFrame frame) => getCoreGameEngineDamageTargetObtainProgramsRuntime().IsValidDamageTargetObtain(frame);
    public bool ValidDamageTargetMount(ProgramSkillFrame frame) => getCoreGameEngineDamageTargetMountProgramsRuntime().IsValidDamageTargetMount(frame);
    public int LivingInAttackRange(int seat) => getCoreGameEngineProgramCardPaymentsRuntime().CountLivingInAttackRange(seat);
}
