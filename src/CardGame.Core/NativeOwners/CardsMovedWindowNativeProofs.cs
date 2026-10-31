#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardsMovedWindowNativeProofs(Func<RuntimeEntries.CoreGameEngineShanheNullifiedTrickClaimsEntries> getCoreGameEngineShanheNullifiedTrickClaimsRuntime,Func<RuntimeEntries.CharacterCaoPiAnniversaryGodEntries> getCharacterCaoPiAnniversaryGodRuntime,Func<RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries> getCharacterHuaTuoAnniversaryGodRuntime,Func<RuntimeEntries.CoreGameEngineRoundEndingEntries> getCoreGameEngineRoundEndingRuntime,Func<RuntimeEntries.CoreGameEngineNianRevivalEntries> getCoreGameEngineNianRevivalRuntime) : ICardsMovedWindowNativeProofs
{
    public bool IsShanheNullifiedClaimAwaitingMovement(NullificationWindowFrame window) => getCoreGameEngineShanheNullifiedTrickClaimsRuntime().IsShanheNullifiedClaimAwaitingMovement(window);
    public bool IsShanheNullifiedClaimMovement(NullificationWindowFrame window, CardMovementBatchContext batch) => getCoreGameEngineShanheNullifiedTrickClaimsRuntime().IsShanheNullifiedClaimMovement(window, batch);
    public bool IsKemingEchoMovement(KemingSkillEchoFrame f, CardMovementBatchContext b) => getCharacterCaoPiAnniversaryGodRuntime().IsKemingEchoMovement(f, b);
    public bool IsSkillInvocationMovementBatch(SkillInvocationFrame f, CardMovementBatchContext batch) => getCharacterHuaTuoAnniversaryGodRuntime().IsSkillInvocationMovementBatch(f, batch);
    public bool IsRoundEndingMovement(RoundEndingBoundaryFrame parent, CardMovementBatchContext batch) => getCoreGameEngineRoundEndingRuntime().IsRoundEndingMovement(parent, batch);
    public bool IsNianRevivalMovementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => getCoreGameEngineNianRevivalRuntime().IsNianRevivalMovementBatch(f, batch);
}
