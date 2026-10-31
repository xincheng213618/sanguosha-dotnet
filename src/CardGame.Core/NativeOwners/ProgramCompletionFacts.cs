#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramCompletionFacts(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHeQiOlEntries> getCharacterHeQiOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpConditionalSkillGrantsEntries> getCoreGameEngineHpConditionalSkillGrantsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineForcedPhaseAlcoholEntries> getCoreGameEngineForcedPhaseAlcoholRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetObtainProgramsEntries> getCoreGameEngineDamageTargetObtainProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetMountProgramsEntries> getCoreGameEngineDamageTargetMountProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime) : IProgramCompletionFacts
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
