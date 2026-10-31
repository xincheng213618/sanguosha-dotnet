namespace CardGame.Core;

internal sealed class CardMovementBatchBuilder(
    long id,
    long? parentFrameId,
    long? parentBatchId,
    long? awaitingProgramFrameId,
    int turnNumber,
    IReadOnlyDictionary<CardLocation, int> sourceCountsBefore,
    IReadOnlyDictionary<CardLocation, int> destinationCountsBefore,
    ProgramSkillFrame? originProgram)
{
    public long Id { get; } = id;
    public long? ParentFrameId { get; } = parentFrameId;
    public long? ParentBatchId { get; } = parentBatchId;
    public long? AwaitingProgramFrameId { get; } = awaitingProgramFrameId;
    private bool _startMetadataSealed;
    internal void SealStartMetadata() => _startMetadataSealed = true;
    private void SetStartMetadata<T>(ref T field, T value)
    {
        if (_startMetadataSealed)
            throw new InvalidOperationException("Card-movement start metadata is already sealed.");
        field = value;
    }

    private CardConversionSource? _invocationOrigin;
    public CardConversionSource? InvocationOrigin
    { get => _invocationOrigin; internal set => SetStartMetadata(ref _invocationOrigin, value); }
    public int TurnNumber { get; } = turnNumber;
    public IReadOnlyDictionary<CardLocation, int> SourceCountsBefore { get; } = sourceCountsBefore;
    public IReadOnlyDictionary<CardLocation, int> DestinationCountsBefore { get; } = destinationCountsBefore;
    public ProgramSkillFrame? OriginProgram { get; } = originProgram;
    private IReadOnlyList<EquipmentDepartureCandidate>? _equipmentDepartures;
    public IReadOnlyList<EquipmentDepartureCandidate>? EquipmentDepartures
    { get => _equipmentDepartures; internal set => SetStartMetadata(ref _equipmentDepartures, value); }
    private CardMovementTiming? _movementTiming;
    public CardMovementTiming? MovementTiming
    { get => _movementTiming; internal set => SetStartMetadata(ref _movementTiming, value); }
    private ActualDiscardRecoveryPhaseKey? _discardRecoveryPhase;
    public ActualDiscardRecoveryPhaseKey? DiscardRecoveryPhase
    { get => _discardRecoveryPhase; internal set => SetStartMetadata(ref _discardRecoveryPhase, value); }
    private IReadOnlyDictionary<int,IReadOnlyList<XiaHuiMovementSource>>? _xiaHuiOriginalSources;
    public IReadOnlyDictionary<int,IReadOnlyList<XiaHuiMovementSource>>? XiaHuiOriginalSources
    { get => _xiaHuiOriginalSources; internal set => SetStartMetadata(ref _xiaHuiOriginalSources, value); }
    private IReadOnlyList<RuanYuMiaoxianPaymentSource>? _ruanYuMiaoxianPaymentSources;
    public IReadOnlyList<RuanYuMiaoxianPaymentSource>? RuanYuMiaoxianPaymentSources
    { get => _ruanYuMiaoxianPaymentSources; internal set => SetStartMetadata(ref _ruanYuMiaoxianPaymentSources, value); }
    private int? _wenhePoisonRound;
    public int? WenhePoisonRound
    { get => _wenhePoisonRound; internal set => SetStartMetadata(ref _wenhePoisonRound, value); }
    private IReadOnlyList<MouDongZhaoLossSource> _mouYishiSources = [];
    public IReadOnlyList<MouDongZhaoLossSource> MouYishiSources
    { get => _mouYishiSources; internal set => SetStartMetadata(ref _mouYishiSources, value); }
    private IReadOnlyList<WuZishengMovementSource> _wuZishengSources = [];
    public IReadOnlyList<WuZishengMovementSource> WuZishengSources
    { get => _wuZishengSources; internal set => SetStartMetadata(ref _wuZishengSources, value); }
    private IReadOnlyList<TangTangOwnedOrigin>? _tangTangOwnedOrigins;
    public IReadOnlyList<TangTangOwnedOrigin>? TangTangOwnedOrigins
    { get => _tangTangOwnedOrigins; internal set => SetStartMetadata(ref _tangTangOwnedOrigins, value); }
    private IReadOnlyList<GuYinSourceSnapshot> _guYinSources = Array.AsReadOnly(Array.Empty<GuYinSourceSnapshot>());
    public IReadOnlyList<GuYinSourceSnapshot> GuYinSources
    { get => _guYinSources; internal set => SetStartMetadata(ref _guYinSources, Array.AsReadOnly(value.ToArray())); }
    private IReadOnlyList<BeifenLossSource> _beifenLossSources = Array.AsReadOnly(Array.Empty<BeifenLossSource>());
    public IReadOnlyList<BeifenLossSource> BeifenLossSources
    { get => _beifenLossSources; internal set => SetStartMetadata(ref _beifenLossSources, Array.AsReadOnly(value.ToArray())); }
    private IReadOnlyList<JiaoweiLossSource> _jiaoweiLossSources = Array.AsReadOnly(Array.Empty<JiaoweiLossSource>());
    public IReadOnlyList<JiaoweiLossSource> JiaoweiLossSources
    { get => _jiaoweiLossSources; internal set => SetStartMetadata(ref _jiaoweiLossSources, Array.AsReadOnly(value.ToArray())); }
}
