#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramShanheSourceZhaoranEntries
{
    internal global::CardGame.Core.ShanheSourceZhaoranArmedEvent? ActiveShanheSourceZhaoranArm(int ownerSeat) => _resolve().ActiveShanheSourceZhaoranArm(ownerSeat);
    internal void ArmShanheSourceZhaoran(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ArmShanheSourceZhaoran(supplied);
    internal bool CanClaimShanheSourceZhaoranSuit(global::CardGame.Core.ProgramTriggerLegalityContext context) => _resolve().CanClaimShanheSourceZhaoranSuit(context);
    internal void CaptureShanheSourceZhaoranSuitLosses(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureShanheSourceZhaoranSuitLosses(batch);
    internal void ClaimShanheSourceZhaoranSuit(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ClaimShanheSourceZhaoranSuit(supplied);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureShanheSourceZhaoranSuitLosses(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureShanheSourceZhaoranSuitLosses(batch, completed);
    internal global::CardGame.Core.ShanheSourceZhaoranSuitLostEvent? EligibleShanheSourceZhaoranSuit(int owner, string instanceId, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().EligibleShanheSourceZhaoranSuit(owner, instanceId, context);
    internal bool HasShanheSourceZhaoranPublicHand(int ownerSeat) => _resolve().HasShanheSourceZhaoranPublicHand(ownerSeat);
    internal bool HasShanheSourceZhaoranRules { get => _resolve().HasShanheSourceZhaoranRules; }
    internal bool IsShanheSourceZhaoranMode { get => _resolve().IsShanheSourceZhaoranMode; }
}
