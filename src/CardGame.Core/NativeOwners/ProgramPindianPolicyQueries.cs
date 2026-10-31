#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPindianPolicyQueries(Func<RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CharacterLuKangWumiaoEntries> getCharacterLuKangWumiaoRuntime) : IProgramPindianPolicyQueries
{
    public bool ProhibitsPindianTarget(int seat) => getCoreGameEngineProgramCardPoliciesRuntime().HasCardPolicy(matchParticipantsState.ReferenceToPlayers[seat], SkillProgramCardPolicyKind.ProhibitPindianTarget);
    public bool CanDiscardForPindian(int seat) => getCharacterLuKangWumiaoRuntime().CanWuLuKangDiscardForPindian(matchParticipantsState.ReferenceToPlayers[seat]);
}
