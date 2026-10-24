namespace CardGame.Core;

internal sealed partial class MatchModeRuntime
{
    private NativePipelinePorts? _nativePipeline;
    private NativePipelinePorts NativePipeline => _nativePipeline ??= new(_features.Values);
    internal ModeNativeFrameRuntime NativeFrames => NativePipeline.Frames;
    internal IReadOnlyList<ModePromptAnswerRegistration> PromptAnswers => NativePipeline.Answers;

    // Called at composition, before any handler can observe or modify a match.
    internal void ValidateNativePorts() { _ = NativePipeline; ValidateDamagePorts(); }

    private sealed class NativePipelinePorts
    {
        internal NativePipelinePorts(IEnumerable<object> features)
        {
            var installed = features.ToArray();
            var routes = installed.OfType<IMatchModeNativeFrameProvider>().SelectMany(provider => provider.FrameRoutes ??
                throw new InvalidOperationException("A mode native frame provider returned no registrations.")).ToArray();
            Frames = new(NativeFrameRouteCatalog.Default, NativeFrameAutomaticStepCatalog.Default, routes);
            Answers = ProgramInteractionCatalog.Default.FreezeModeAnswers(installed.OfType<IMatchModePromptAnswerProvider>()
                .SelectMany(provider => provider.PromptAnswers ??
                    throw new InvalidOperationException("A mode prompt answer provider returned no registrations.")));
        }
        internal ModeNativeFrameRuntime Frames { get; }
        internal IReadOnlyList<ModePromptAnswerRegistration> Answers { get; }
    }
}
