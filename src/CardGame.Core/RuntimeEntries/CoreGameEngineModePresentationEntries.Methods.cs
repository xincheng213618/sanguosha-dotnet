#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineModePresentationEntries
{
    internal ModePresentationContext CaptureModePresentationContext() => _resolve().CaptureModePresentationContext();
    internal ModeWinnerPresentationContext CaptureModeWinnerPresentationContext() => _resolve().CaptureModeWinnerPresentationContext();
}
