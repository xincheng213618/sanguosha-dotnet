#nullable enable
using AcquisitionNode = CardGame.Core.ProgramStageNode<CardGame.Core.ProgramSeatAcquisitionCall, CardGame.Core.ProgramSeatAcquisitionReceipt, CardGame.Core.ProgramSeatAcquisitionStage>;
using AttackReceipt = CardGame.Core.GameEngine.AttackCompletionReceipt;
using CardGame.Core.Campaigns;
using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text.Json.Serialization;

namespace CardGame.Core;
public sealed partial class GameEngine
{
internal sealed record AssistedFactionSlashCard(Card Card, CardKind Kind, CardConversionSource? Source);
internal sealed record AttackCompletionReceipt(long ResolutionId, bool DelayedTurn,
        long? ProgramFrameId, int? DelayedTurnSeat,
        bool RequestedSlashChild = false, bool DamageWasApplied = false, int FinalTargetSeat = 0, ProgramDamageTargetDuelOrigin? DamageTargetDuelReturn = null, ProgramDualColorDuelOrigin? DualColorDuelReturn = null,
        SharedSlashBenefitReturn? SharedSlashBenefit = null, bool SharedSlashCausedDamage = false,
        ForeignTurnContestSlashReturn? ForeignTurnContestSlashReturn = null, PindianWinnerSlashReturn? PindianWinnerSlashReturn = null, ConditionalDiscardDuelOrigin? ConditionalDiscardDuelReturn = null, ShiQuanDuelOrigin? ShiQuanDuelReturn = null, EndingHistoricalUseReturn? EndingHistoricalUseReturn = null, InspectedHandSlashReturn? InspectedHandSlashReturn = null, KuangfuSlashReturn? KuangfuSlashReturn = null, ForeignSelectedCardSlashReturn? ForeignSelectedCardSlashReturn = null, FriendRecastDuelOrigin? FriendRecastDuelReturn = null, CaoChunNativeSlash? CaoChunSlash = null, WangCanNativeReceipt? DenglouNative = null, QigongNativeReceipt? Qigong = null, ZhenyingDuelOrigin? ZhenyingDuel = null, HeJinMouzhuUseOrigin? HeJinMouzhu = null, WuAnGuoLiyongDuelOrigin? WuAnGuoLiyong = null);
internal interface ICardContinuationHandle { long OwnerFrameId { get; } }
    internal readonly record struct NuzhanModifiers(bool IgnoresSlashLimit, int DamageBonus, int PhysicalCardId);
    internal sealed record ProgramCardIdentityMatch(SkillProgram Program, SkillProgramCardIdentity Identity, CardConversionSource Source);
internal sealed record ChainedStateBasicNeed(ChainedStateBasicIntent Intent, long? ParentFrameId,
        long? RequestFrameId, long? ParentActionId, int Cursor, int? FixedTargetSeat);
internal sealed record DrawFundedBasicNeed(DrawFundedDistinctBasicIntent Intent, long? ParentFrameId,
        long? RequestFrameId, long? ParentActionId, int Cursor, int? FixedTargetSeat);
internal sealed record FactionProviderRewardReceipt(long OwnerFrameId, int DrawCount);

    internal readonly record struct ProjectionDependency(long Revision, bool Alive, int Hp, Role Role, bool PrimarySelected, bool PrimaryRevealed, bool SecondarySelected, bool SecondaryRevealed);
    internal sealed record NativeFactionAvatar(PrivateGeneralLibrarySource Source, string GeneralId, string? SkillId, string? SelectedFactionId);
    internal sealed record NativeFactionObservation(string? FactionId, global::CardGame.Core.GameEngine.NativeFactionAvatar? Avatar);
    internal sealed record NamedNextSlashReserve(int OwnerSeat, string SkillId, int TurnNumber, int TurnSeat, int Amount);
    internal sealed record OriginalHandStateKey(int OwnerSeat, string SkillId, string StateId);
    internal sealed record OriginalHandEntity(int CardId, long DealSequence, long? FirstLossSequence);
    internal sealed record OriginalHandEntityState(long InitializationFrameId, CardConversionSource Source, string GameplayHash, IReadOnlyList<global::CardGame.Core.GameEngine.OriginalHandEntity> Entities);
internal interface IDamageAttempt
    {
        long ResolutionId { get; }
        int SourceSeat { get; }
        int CardUserSeat { get; }
        int TargetSeat { get; }
        Card? Card { get; }
        IReadOnlyList<Card> PhysicalCards { get; }
        CardKind? EffectiveCardKind { get; }
        CardConversionSource? ConversionSource { get; }
        int DamageAmount { get; }
        bool DamageAmountFinalized { get; }
        bool IgnoresArmor { get; }
        bool IsDelayedJudgmentDamage { get; }
        bool IsProgramJudgmentDamage { get; }
        bool IsSourceLess { get; }
        DamageNature? DamageNatureOverride { get; }
        bool IsChainPropagation { get; }
        bool DamageWasApplied { get; }
        bool BeforeDamageProgramsResolved { get; }
        bool DamageRedirected { get; }
        bool PendingRedBladeDamageBonus { get; set; }
        int SourceToTargetDistanceAtDamage { get; }
        void SetIgnoresArmor(bool value);
        void FinalizeDamageAmount(int bonus, int? maximum = null);
        void ReduceFinalizedDamageAmount(int amount);
        void IncreaseFinalizedDamageAmount(int amount);
        void MarkBeforeDamageProgramsResolved();
        void MarkDamageApplied();
        void CaptureDamageDistance(int distance);
        void MarkDyingResolvedForDamage(long damageFrameId);
        bool HasResolvedDyingForDamage(long damageFrameId);
        void RedirectFinalizedDamageTarget(int targetSeat, global::CardGame.Core.GameEngine.ProgramDamageTransferFollowup followup);
        bool TryConsumeProgramDamageTransferFollowup(out global::CardGame.Core.GameEngine.ProgramDamageTransferFollowup followup);
        void SetChainedTargets(IReadOnlyList<int> targetSeats);
        bool TryAdvanceChainedTarget(Func<int, bool> isAlive, bool resetDamageForTargetModifiers,
            out int fromSeat);
    }
    internal sealed record ProgramBooleanStateLease(CardUseEffectSource Source, string StateId, int TurnNumber, int TurnSeat, SkillProgramDamageModifierExpiration Expiration);
internal sealed record ProgramDamageTransferFollowup(
        string SkillId, int OwnerSeat, int TargetSeat, bool DrawLostHp);
internal sealed record ProgramOrdinaryTrickUseOption(
        ChoiceId Id,
        CardKind EffectiveCardKind,
        LegalActionKind ActionKind,
        IReadOnlyList<int> TargetSeats,
        int? TargetCardId,
        CardKind? RequiredCardKind,
        string Description)
    {
        public bool NextActualUseAdjusted { get; init; }
    }
    internal sealed record ProgramPlayDamageHandLimit(int OwnerSeat, string SkillId, string SkillInstanceId, int TurnNumber, int PhaseInstanceId, int DamageCount);
    internal sealed record ProgramGameFactionRangeTargets(int OwnerSeat, string SkillId, string FactionId, IReadOnlyList<int> TargetSeats);

    internal readonly record struct ProgramBooleanStateKey(int OwnerSeat, string SkillId, string SkillInstanceId, string StateId);
internal readonly record struct ProgramSkillChangeContext(ProgramSkillFrame Frame, CharacterState Owner);
    internal sealed record RecipientCategoryMarkKey(int HolderSeat, string SourceSkillId, string StateId);
    internal sealed record ShenwuDraftSetup(IReadOnlyList<string> SharedGods, string LeaderId);
internal readonly record struct SignedDamageProfile(int OwnerHp, int OtherHp, bool WaiveHp, bool AnyColor,
        bool AllowEquipment, int DeadRoleMask, string? QualifierInstanceId);
internal sealed record SourceDingxiUsePlan(int CardId, CardKind Kind, LegalActionKind Action, CardKind? Required);
    internal sealed record ProgramSuppression(int SourceSeat, int TargetSeat, string SkillId, int CreatedTurn, IReadOnlyList<string> GrantIds);
    internal sealed record TurnDefaultStatKey(int OwnerSeat, string SkillId, string StateId);
    internal sealed record TurnDefaultStatAssignment(int Value, long FrameId, int SlotIndex);
    internal sealed record TurnDefaultStatState(CardConversionSource Source, string GameplayHash, int ActualTurnNumber, bool HasPool, IReadOnlyList<int> Slots, int UsedSlotMask, IReadOnlyDictionary<TurnDefaultStatKind, global::CardGame.Core.GameEngine.TurnDefaultStatAssignment> Assigned, IReadOnlyList<int> NextSlots);
internal sealed record NativeEquipmentSuspensionProjection(ResolutionFrame Parent, CardUseFrame AttackRoot,
        SkillInvocationFrame Invocation, NativeEquipmentInvocationReceipt Receipt, int ParentIndex);
internal sealed record NativeFactionSkillSuspensionProjection(ResolutionFrame Parent,
        SkillInvocationFrame Invocation, int ParentIndex);
internal sealed record NativeBuZhiDiscardInvocationDraft(long Id, long FrameId, CardConversionSource Source, string GameplayHash,
        int InstructionIndex, int Turn, int TurnOwner, CardMoveReason Reason, long Before,
        IReadOnlyList<CardZoneKind> Zones, IReadOnlyList<NativeBuZhiDiscardMaterial> Eligible) : IProgramOwnedZoneDiscardInvocation;
internal sealed record CaoYuDeathMinimumCapture(long DeathFrameId,long ParentFrameId,int VictimSeat,int? KillerSeat,DeathReturnKind ReturnKind,int ActualTurn,int TurnOwner,IReadOnlyList<ProgramTriggerCandidate> Sources,IReadOnlyList<int> HandCounts,IReadOnlyList<bool> Alive)
 {
  public IReadOnlyList<ProgramTriggerCandidate> Sources{get;init;}=Array.AsReadOnly(Sources.ToArray());
  public IReadOnlyList<int> HandCounts{get;init;}=Array.AsReadOnly(HandCounts.ToArray());
  public IReadOnlyList<bool> Alive{get;init;}=Array.AsReadOnly(Alive.ToArray());
 }
internal sealed record ShenDaRepeatPlan(string Id, CardKind Kind, LegalActionKind ActionKind,
        IReadOnlyList<int> TargetSeats, string Description, int? TargetCardId = null, CardKind? RequiredKind = null);
internal sealed record JiezhenConversionState(int OwnerSeat, IReadOnlyList<string> ReplacedSkillIds,
        string BazhenGrantId);
    internal sealed record ShiHcLedgerGrant(ShiHcCujunSlashGrantedEvent Grant, int Index);
    internal sealed record ShiHcLedgerPlayStart(int Seat, int Index);
internal sealed record MouKurouAiCandidate(int CardId, int RecipientSeat, ProgramAiEstimate Estimate);
internal sealed record MouJwSlashPlan(LegalAction Action, IReadOnlyList<int> Materials, PromptChoice? Multi = null);
internal sealed record MobileShiLuYuShengTrack(ActualDiscardRecoveryPhaseKey? Phase,
        IReadOnlyList<int> GainedSeats, IReadOnlyList<int> MarkedCards, int DebtRequired, int DebtProgress, bool Refunded);
internal sealed record MaTieSlashPlan(LegalAction Action, IReadOnlyList<int> Materials, PromptChoice? Multi = null);
internal sealed record QinLangUseOption(LegalActionKind Kind,IReadOnlyList<int> TargetSeats,int? TargetCardId,CardKind? RequiredKind,string Description);
internal sealed record MouSmPublicProjection(IReadOnlySet<long> Events,IReadOnlySet<long> Logs,IReadOnlySet<long> Declarations);
internal sealed record ShiScCompletedUse(long NativeId, CardActionContext Action, IReadOnlyList<int> Targets);
internal sealed record XunXuUsePlan(LegalActionKind Action, IReadOnlyList<int> Targets, int? Card = null, CardKind? Required = null);
internal sealed record YanjiaoCandidate(int Id, int Rank, CardLocation Location,
        string DisplayName, string RankText);
internal sealed record ShiZrFuanRecordInfo(Suit Suit, bool ViaRelaunch);

    internal sealed record ZhangXunDueLease(ZhangXunSuizhengLeaseEvent Lease, int Ordinal, int StartIndex, int DueTurn);
    internal readonly record struct YunzhengQualificationSeat(long Revision, int Hp, bool Alive, bool Concealed, bool PrimarySelected, bool PrimaryRevealed, bool SecondarySelected, bool SecondaryRevealed, Role Role);
}
