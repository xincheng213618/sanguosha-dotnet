#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramPindianPolicyQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries> getCharacterLuKangWumiaoRuntime) : IProgramPindianPolicyQueries
{
    public bool ProhibitsPindianTarget(int seat) => getCoreGameEngineProgramCardPoliciesRuntime().HasCardPolicy(matchParticipantsState.ReferenceToPlayers[seat], SkillProgramCardPolicyKind.ProhibitPindianTarget);
    public bool CanDiscardForPindian(int seat) => getCharacterLuKangWumiaoRuntime().CanWuLuKangDiscardForPindian(matchParticipantsState.ReferenceToPlayers[seat]);
}
