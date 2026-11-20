# Card development guidance

Card is a .NET rules engine with a WPF desktop client. Use the SDK selected by
[global.json](global.json); setup and project entry points are in
[docs/GETTING_STARTED.md](docs/GETTING_STARTED.md).

## Working boundaries

- Use PowerShell. Before any recursive delete or move, resolve and verify the absolute target, then use PowerShell cmdlets with `-LiteralPath`.
- Inspect Git status before editing or committing. Preserve concurrent changes and stage only the task's intended files or hunks.
- Routine design decisions and format migrations within the requested task are authorized; document material rule and compatibility decisions.
- Publishing, installers and distribution packages require an explicit publishing or packaging request.
- Reuse compatible temporary outputs and clean up this task's disposable files after inspection. Respect ownership, links and locks; never bypass a policy rejection or clear another task's outputs. Follow the exact cleanup procedure in [development gates](docs/DEVELOPMENT_GATES.md#temporary-files-and-cleanup) when creating or cleaning artifacts.

## Architecture essentials

- Iterate in the existing engine. Keep current code and tests as the baseline, and change one bounded part of the rules at a time.
- Use JSON for straightforward combinations of existing shared operations and conditions. Use typed C# directly for complex rules. Keep each skill's behavior in one implementation, and avoid a skill-specific JSON instruction backed by a dedicated runtime and forwarding layer.
- Keep shared rule concepts in their owning module. Shared capabilities must be independent of characters, and characters must be independent of each other. Extract a shared capability when the current change demonstrates a reusable rule. Migrate the consumers in scope and leave other equivalent consumers for later batches.
- Validate at command, loader, restore and executor boundaries. Keep pending state, accepted facts, payments and typed child returns on the original owning frame; do not rebuild them with character proof events, history scans or sidecars.
- Preserve input validation, seat-specific privacy, card movement, checkpoint, replay and content fingerprints. Keep source loss, owner death, ongoing eligibility and already-issued permission distinct.
- Do not add a global gameplay rules epoch or checkpoint `RulesVersion`, or bump formats/package versions per character. If loading or replay fails, preserve the source save while starting a fresh game. Format changes follow [development gates](docs/DEVELOPMENT_GATES.md#formats-and-content).

## Verification

- The sole routine gameplay acceptance is a fixed-seed, all-AI match through the engine's public command entry point. It must reach a normal terminal state without exceptions. Replay the same accepted command record from the same initial setup and compare the winner and observable final state. A stall, exception or mismatch fails acceptance.
- Run routine acceptance once when a gameplay batch is ready. Retain the existing test suite for targeted diagnosis when needed. For documentation changes, review content and links and run `git diff --check`.
- **Full verification is manual only:** `-Full` and unfiltered test assemblies require an explicit user request for a manual full run. A routine failure, milestone or follow-up cycle never lifts this restriction.
- The runner's historical default scope and architecture checks remain available for diagnosis. This file governs routine acceptance, skill implementation choices and migration scope when linked guides differ.

## Read when relevant

Start with the relevant code or configuration and the matching section below. Use source and evidence files for the exact edition when a rule, asset or attribution needs verification.

| Task | Guidance |
| --- | --- |
| Add a general or edit skill JSON | [Character registration](docs/CHARACTER_MODULES.md#内容声明), [rule syntax](docs/content/skill-composition/RUNTIME.md) |
| Understand an existing capability | [Capability map](docs/content/skill-composition/CAPABILITY_MAP.md) and the affected contract |
| Change a rule mechanism or any consumer of it | [Contract index](docs/content/skill-composition/contracts/README.md): select the affected topics and follow their owning-frame and lifetime contracts |
| Change formats or manage temporary outputs | [Development gates](docs/DEVELOPMENT_GATES.md) |
| Change desktop saves or recovery | [Save and resume](docs/SAVE_AND_RESUME.md) |
| Verify a rule edition or asset source | [Content and source locations](docs/content/CONTENT_MANIFEST.md#来源与素材) |
| Find UI, mode, content or architecture guidance | [Documentation index](docs/README.md) |

## Maintaining guidance

- Keep this root file short: repository-wide constraints, useful commands and task-based links.
- Record a mechanism's ownership, semantics, exceptions and remaining gaps in its [contract topic](docs/content/skill-composition/contracts/README.md). Update the capability map when its method or navigation changes.
- Use a nested `AGENTS.md` only for instructions unique to that directory. Cross-cutting mechanisms belong in linked documents so Core, characters, activities and tests share one contract.
- Update the owning document when its contract, syntax or guidance changes. Implementation changes and reuse of existing capabilities can leave documentation as-is. Put batch timings, size statistics and detailed verification results in commit messages.
