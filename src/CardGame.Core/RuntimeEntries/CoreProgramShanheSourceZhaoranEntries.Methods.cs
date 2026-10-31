#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramShanheSourceZhaoranEntries
{
    internal ShanheSourceZhaoranArmedEvent? ActiveShanheSourceZhaoranArm(int ownerSeat) => _resolve().ActiveShanheSourceZhaoranArm(ownerSeat);
    internal void ArmShanheSourceZhaoran(ProgramSkillFrame supplied) => _resolve().ArmShanheSourceZhaoran(supplied);
    internal bool CanClaimShanheSourceZhaoranSuit(ProgramTriggerLegalityContext context) => _resolve().CanClaimShanheSourceZhaoranSuit(context);
    internal void CaptureShanheSourceZhaoranSuitLosses(CardMovementBatchContext batch) => _resolve().CaptureShanheSourceZhaoranSuitLosses(batch);
    internal void ClaimShanheSourceZhaoranSuit(ProgramSkillFrame supplied) => _resolve().ClaimShanheSourceZhaoranSuit(supplied);
    internal CardMovementBatchContext CompleteMovementBatchCaptureShanheSourceZhaoranSuitLosses(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureShanheSourceZhaoranSuitLosses(batch, completed);
    internal ShanheSourceZhaoranSuitLostEvent? EligibleShanheSourceZhaoranSuit(int owner, string instanceId, ProgramSkillWindowContext? context) => _resolve().EligibleShanheSourceZhaoranSuit(owner, instanceId, context);
    internal bool HasShanheSourceZhaoranPublicHand(int ownerSeat) => _resolve().HasShanheSourceZhaoranPublicHand(ownerSeat);
    internal bool HasShanheSourceZhaoranRules { get => _resolve().HasShanheSourceZhaoranRules; }
    internal bool IsShanheSourceZhaoranMode { get => _resolve().IsShanheSourceZhaoranMode; }
}
