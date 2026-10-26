using System.Reflection;
using System.Runtime.ExceptionServices;
using CardGame.Core;

internal static class WushuangLegacyOpeningFixture
{
    // Explicit historical mechanism fixtures may preset a formerly offered
    // tactic. The public production offer and purchase validators stay intact.
    public static void CopyModeFactories(IContentRegistryBuilder builder, ContentRegistry source, int? tacticId,
        int? opponentTacticId = null)
    {
        if (tacticId is not { } preset)
        {
            if (opponentTacticId is not null) throw new ArgumentException("An opponent preset also requires the human preset.");
            builder.CopyModeFeatureFactoriesFrom(source); return;
        }
        var factory = new LegacyOpeningTacticFactory(preset, opponentTacticId);
        foreach (var original in source.ModeFeatureFactories.Where(original => original.Id != "wushuang:mode-runtime"))
            if (!builder.HasModeFeatureFactory(original.Id)) builder.AddModeFeatureFactory(original);
        if (builder.HasModeFeatureFactory(factory.Id)) return;
        // Feature implementations are deliberately outside the content hash.
        // This unowned fixture marker makes the configured preset itself part
        // of immutable content, including for callers with a shared package ID.
        builder.AddSkill(new(factory.Id + ":configuration", "旧开局机制预置", factory.Id));
        builder.AddModeFeatureFactory(factory);
    }

    private sealed class LegacyOpeningTacticFactory(int tacticId, int? opponentTacticId) : IGameModeFeatureFactory
    {
        public string Id => $"fixture:wushuang-legacy-opening-seat0-{tacticId}" +
            (opponentTacticId is { } opponent ? $"-seat1-{opponent}" : "");
        public string ModeId => WushuangMode.ModeId;
        public IGameModeFeature Create(ModeFeatureCreationContext context)
        {
            var native = (IWushuangModeFeature)new CardGame.Modes.Wushuang.WushuangModeFeatureFactory().Create(context);
            var feature = DispatchProxy.Create<IWushuangLegacyModeFeature, WushuangLegacyOpeningProxy>();
            ((WushuangLegacyOpeningProxy)(object)feature).Configure(native, context.GetHost<IWushuangModeHost>(),
                tacticId, opponentTacticId);
            return feature;
        }
    }
}

// Preserve every native optional port: exposing only IWushuangModeFeature
// would remove the mode's opening, rule observers, snapshots and numeric rules.
public interface IWushuangLegacyModeFeature : IWushuangModeFeature, IMatchModeCommandHandler,
    IMatchModeOpeningBoundary, IMatchModeTurnIntermission, IMatchModeRuleObserverProvider,
    IMatchModeNumericRules, IMatchModeLifecycle, IMatchModeSnapshotProjector,
    IMatchModeNativeFrameProvider, IMatchModePromptAnswerProvider { }

public class WushuangLegacyOpeningProxy : DispatchProxy
{
    private IWushuangModeFeature _native = null!;
    private IWushuangModeHost _host = null!;
    private (int Seat, int TacticId)[] _presets = [];
    private int _nextPreset;
    private long _openingFrameId;
    private bool _issued;

    public void Configure(IWushuangModeFeature native, IWushuangModeHost host, int tacticId, int? opponentTacticId)
    {
        _native = native; _host = host;
        _presets = opponentTacticId is { } opponent ? [(0, tacticId), (1, opponent)] : [(0, tacticId)];
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        ArgumentNullException.ThrowIfNull(targetMethod);
        // Opening acquisition receipts return through this native operation.
        // Keep their paid parent intact, then issue the next historical seat
        // before the ordinary AI can choose a different opening reward.
        if (_issued && _nextPreset < _presets.Length &&
            targetMethod.Name == nameof(IWushuangModeFeature.AdvanceWushuangShopSeat) &&
            args is [WushuangIntermissionFrame returning] && returning.Id == _openingFrameId)
        {
            IssueNextPreset(); return null;
        }
        object? result;
        try { result = targetMethod.Invoke(_native, args); }
        catch (TargetInvocationException failure) when (failure.InnerException is not null)
        { ExceptionDispatchInfo.Capture(failure.InnerException).Throw(); throw; }
        if (targetMethod.Name is not (nameof(IMatchModeOpeningBoundary.TryBeginOpening) or
                nameof(IWushuangModeFeature.TryBeginWushuangOpening)) || result is not true || _issued)
            return result;
        var frame = _native.ActiveWushuangWindow(0) ?? throw new InvalidOperationException("Legacy opening lost its native parent.");
        if (!frame.Opening || frame.SeatCursor != 0 || _host.PendingDecision?.PlayerSeat != 0)
            throw new InvalidOperationException("Legacy opening requires the original human opening boundary.");
        _issued = true;
        _openingFrameId = frame.Id;
        IssueNextPreset();
        return result;
    }

    private void IssueNextPreset()
    {
        if (_host.Frames.LastOrDefault() is not WushuangIntermissionFrame { Opening: true, Acquisition: null } frame ||
            frame.Id != _openingFrameId)
            throw new InvalidOperationException("A legacy reward lost its original completed acquisition parent.");
        var preset = _presets[_nextPreset++];
        frame = frame with { SeatCursor = preset.Seat, AiShopActions = 0 };
        _host.ReplaceFrame(frame.Id, frame);
        _host.ClearPrompt();
        var tactic = WushuangTactics.ById[preset.TacticId];
        var offer = new WushuangShopOffer($"fixture:legacy-opening:seat{preset.Seat}:{preset.TacticId}",
            tactic.Name, tactic.Description, 0, null, WushuangOfferKind.Tactic, 1)
            { TacticId = tactic.Id, Quality = tactic.Quality };
        _native.BuyWushuangOffer(preset.Seat, offer, null, opening: true);
        if (_host.TryBeginAcquisition(frame.Id, offer)) return;
        if (_nextPreset < _presets.Length) IssueNextPreset();
        else _native.AdvanceWushuangShopSeat(frame);
    }
}
