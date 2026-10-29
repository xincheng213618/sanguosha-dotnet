# 48 项失败的归因与结算协议试点

审计日期：2026-10-30。对照基线提交 `28350e4e5ce915ecbaddaad9430c12f56aee4329`。基线同一组名称过滤共运行 Core 428 项、WPF 77 项，分别失败 36 项和 12 项；这是定向范围，不是全量回归。

## 归因标准与修复边界

48 是失败用例数，不是独立缺陷数。按每项的主因，18 项涉及实际回归，30 项为测试夹具或断言过时；部分用例同时存在夹具缺失。不能仅凭这次复现把所有缺陷归因于基线提交本身。

实际回归收敛为三组：13 项来自旧配方不应受到新资源契约影响，4 项暴露存档指纹校验失效，1 项暴露开局专用财富错误进入战后奖池。修复保留新配方的装备直接安装、装备不入抽牌堆和用后销毁；旧配方仍按既有标志恢复。存档恢复重新计算实际定义指纹，不复活已退休目录，不改写不能加载的源存档。未修改存档格式、规则 epoch、内容包版本或技能 JSON schema。

测试更新依据 [山河规则](SHANHE_CAMPAIGN.md) 与实际已实现契约，包括新资源标志、当前目录恢复、平衡奖励和安全边界升级。没有删除失败用例，没有把支付、选择、冷恢复、隐私或牌实体断言替换成仅检查“不抛异常”。无双固定种子先用目录与确定性随机顺序核对，再在原生 UI 路径验证；付费战法通过实际回合收入购买。

## 逐项记录

| 序号 | 原失败检查 | 主因 | 处理 |
| --- | --- | --- | --- |
| 1 | core · `Shanhe starting wealth: opening only inventory and rejected late grants` | 实际回归 | 开局专用财富奖励错误进入战后奖励池；从普通和第三章精锐候选移除，夹具同时补齐流派元数据。 |
| 2 | core · `Shanhe bonus rewards: elite ownership primary claim and no duplication` | 测试过时 | 复制正式奖励时漏复制 Schools；补齐所属流派。 |
| 3 | core · `Shanhe boss opening: typed battle kinds and actual initial entities` | 测试过时 | 同上；注册表预期还需包含正式派生流派技能。 |
| 4 | core · `Shanhe boss opening: native second phase preserves dealt hand` | 测试过时 | 复制正式奖励时漏复制 Schools。 |
| 5 | core · `Shanhe recast tactics: purchased tiers and outside-turn response` | 测试过时 | 复制正式奖励时漏复制 Schools。 |
| 6 | core · `Shanhe cycle tactics: exact completed uses outside play and actual turn reset` | 实际回归 | 无标志旧配方被新契约的无装备牌堆与生成装备逻辑覆盖；三个入口统一按 StartingResourcesExpireOnUse 选择既有契约。 |
| 7 | core · `Shanhe chapter progression: White Horse screenshot boss preview and native stats` | 测试过时 | 裁剪地图夹具遗漏 Schools。 |
| 8 | core · `Shanhe chapter progression: Hebei screenshot boss preview and native stats` | 测试过时 | 裁剪地图夹具遗漏 Schools。 |
| 9 | core · `Shanhe elite Handang: real empty-slot equipment and own dodged Slash cold` | 实际回归 | 无标志旧配方被新契约的无装备牌堆与生成装备逻辑覆盖；三个入口统一按 StartingResourcesExpireOnUse 选择既有契约。 |
| 10 | core · `Shanhe third-chapter elite: complete roster loot and onward routes cold` | 测试过时 | 精锐新增每日追击分支、自动奖励类别与品质筛选；裁剪图仅保留本用例节点，使用史诗候选，保留三 Boss 路线与冷恢复断言。 |
| 11 | core · `Shanhe Niepan skill stock: bought self-dying rescue resumes lethal parent` | 实际回归 | 无标志旧配方被新契约的无装备牌堆与生成装备逻辑覆盖；三个入口统一按 StartingResourcesExpireOnUse 选择既有契约。 |
| 12 | core · `Shanhe Beige skill source: bought equipment cost Club judgment source discard` | 实际回归 | 无标志旧配方被新契约的无装备牌堆与生成装备逻辑覆盖；三个入口统一按 StartingResourcesExpireOnUse 选择既有契约。 |
| 13 | core · `Shanhe build tactics: purchased armor tiers actual opening and physique` | 测试过时 | 复制正式奖励时漏复制 Schools。 |
| 14 | core · `Shanhe shop stock: purchases native equipment and cards` | 测试过时 | 当前新契约的开局装备直接入装备区；检验一次真实进入与起源，不再把已装备实体从手牌重复使用。 |
| 15 | core · `Shanhe inheritance: verified rare pool and old definition fingerprints` | 测试过时 | 当前目录不重建已退休的继承前定义；目录拒绝旧指纹且不改源存档，显式提供原定义的诊断恢复仍验证原状态。 |
| 16 | core · `Shanhe companion: dual opening fixed faces remaining seats and cold replay` | 测试过时 | 双将开局夹具遗漏 Schools。 |
| 17 | core · `Shanhe guide integration: stock inheritance terrain Mulu and cold` | 测试过时 | 未经确认的队伍战法没有商店稀有度元数据；只对已确认条目断言来源稀有度，保留队伍战法身份与实际规则断言。 |
| 18 | core · `Shanhe source Warrior: post-reveal gain and source damage expiry` | 实际回归 | 无标志旧配方被新契约的无装备牌堆与生成装备逻辑覆盖；三个入口统一按 StartingResourcesExpireOnUse 选择既有契约。 |
| 19 | core · `Shanhe source phases: Jushou after discard and native equipment use` | 实际回归 | 无标志旧配方被新契约的无装备牌堆与生成装备逻辑覆盖；三个入口统一按 StartingResourcesExpireOnUse 选择既有契约。 |
| 20 | core · `Shanhe source garrisons: native preview move and physical opening` | 测试过时 | 正式节点已是入口，裁剪夹具又追加一次；对入口去重。 |
| 21 | core · `Shanhe Sanshou market: source price paid hand card and native equip during play` | 测试过时 | 只保留商店的夹具仍带完整章节路线条件；清除无关路线角色与进度。 |
| 22 | core · `Shanhe codex content: editions checkpoint pool and native equipment` | 实际回归 | 恢复时信任存档自报哈希，未比对实际定义；恢复与目录选择都重新计算并严格比对内容指纹。 |
| 23 | core · `Shanhe god faction: null input hash session propagation and tampered battle rejection` | 实际回归 | 恢复时信任存档自报哈希，未比对实际定义；恢复与目录选择都重新计算并严格比对内容指纹。 |
| 24 | core · `Shanhe Dong Min: fixed cap before Armor public prediction and replacements cold` | 测试过时 | 多个章节共享董旻武将；按实际 encounter ID 选定原配方。 |
| 25 | core · `Shanhe Yujin: legacy ending timing and isolated actual recipes cold` | 测试过时 | 普通胜利的正式金币现为 200；不再断言旧值 80。 |
| 26 | core · `Shanhe Liru: actual single enemy recipe conditional registry and recovery` | 测试过时 | 替换首形态的普通配方仍保留黎儒二形态；真正无 Boss 的对照同时移除 BossTransition。 |
| 27 | core · `Shanhe S5 Ma siblings: whole-color Tieqi actual judgment HE payment and native Dodge cold` | 实际回归 | 无标志旧配方被新契约的无装备牌堆与生成装备逻辑覆盖；三个入口统一按 StartingResourcesExpireOnUse 选择既有契约。 |
| 28 | core · `Shanhe Gongsun: native range three area discard before Dodge paid cold` | 实际回归 | 无标志旧配方被新契约的无装备牌堆与生成装备逻辑覆盖；三个入口统一按 StartingResourcesExpireOnUse 选择既有契约。 |
| 29 | core · `Shanhe Zhuling: three conditions native Slash frozen draw and original target penalty cold` | 实际回归 | 无标志旧配方被新契约的无装备牌堆与生成装备逻辑覆盖；三个入口统一按 StartingResourcesExpireOnUse 选择既有契约。 |
| 30 | core · `Shanhe Xuchu: frozen HP reveal native Slash Duel and owner expiry cold` | 实际回归 | 无标志旧配方被新契约的无装备牌堆与生成装备逻辑覆盖；三个入口统一按 StartingResourcesExpireOnUse 选择既有契约。 |
| 31 | core · `Shanhe Xuchu: revealed cards decline substitution and normal draw cold` | 实际回归 | 无标志旧配方被新契约的无装备牌堆与生成装备逻辑覆盖；三个入口统一按 StartingResourcesExpireOnUse 选择既有契约。 |
| 32 | core · `Shanhe Xuchu: NPC revealed options compare actual cards and normal draw cold` | 实际回归 | 无标志旧配方被新契约的无装备牌堆与生成装备逻辑覆盖；三个入口统一按 StartingResourcesExpireOnUse 选择既有契约。 |
| 33 | core · `Shanhe rank: current printed health fifth rank and explicit growth cold` | 实际回归 | 恢复时信任存档自报哈希，未比对实际定义；恢复与目录选择都重新计算并严格比对内容指纹。 |
| 34 | core · `Shanhe companion: fixed opening frozen faces native Fudao cold` | 实际回归 | 恢复时信任存档自报哈希，未比对实际定义；恢复与目录选择都重新计算并严格比对内容指纹。 |
| 35 | core · `Shanhe trick ordinal: converted repeated AOE original receipt cold` | 测试过时 | 群体锦囊逐目标询问无懈；验证目标一被取消、其余两次原效果与三次额外效果，保留用牌/付款各一次和冷恢复。 |
| 36 | core · `Shanhe Guanping companion: mandatory Moyan paid Longyin native return cold` | 实际回归 | 无标志旧配方被新契约的无装备牌堆与生成装备逻辑覆盖；三个入口统一按 StartingResourcesExpireOnUse 选择既有契约。 |
| 37 | wpf · `Wushuang UI: mixed shop equipment shows its native purchase` | 测试过时 | 固定种子的旧目录前提失效；验证真实混合页与 Core 公开报价一致，允许多技能与原生装备并存并实际购买。 |
| 38 | wpf · `Wushuang UI: active tactic keeps an executable entry` | 测试过时 | 机制战法不再免费开局提供；从实际付费页锁定，走正常回合收入购买后验证主动入口与原生选牌。 |
| 39 | wpf · `Shanhe chapter route UI: stages extra battles and progress` | 测试过时 | 当前平衡奖励排除白色候选；夹具采用真实可选的稀有奖励。 |
| 40 | wpf · `Shanhe third chapter elite: complete roster preview and onward rewards` | 测试过时 | 裁剪图遗漏新增追击节点引用，旧文本又预期稀有；截断无关出边并按史诗预览验证。 |
| 41 | wpf · `Shanhe codex UI: unlock equip persist and market` | 测试过时 | 继续存档在安全边界追加一次平衡奖励命令；验证原日志前缀及已冻结权益逐字不变，升级后快照持久化且不写账户。 |
| 42 | wpf · `Shanhe market UI: free refresh clicks paid allowance and zero wallet` | 测试过时 | 旧白色增长商品在规则升级时被移除；夹具显式采用当前平衡规则与稀有商品，保留真实扣费和免费刷新断言。 |
| 43 | wpf · `Shanhe market UI: restock purchase same slot prices and refresh allowances` | 测试过时 | 同上，保留同槽补货、递增价格与刷新次数断言。 |
| 44 | wpf · `Shanhe Dong Ji UI: dynamic exact card confirmation and locked gift` | 测试过时 | 共用 owned-card 选择器提示文案已变化；检查精确张数，原支付、隐私与锁牌行为继续验证。 |
| 45 | wpf · `Shanhe reward UI: real victory cards confirmation skip skill and replacement` | 测试过时 | 确认奖励后会在安全边界追加一次规则升级；保留一次补偿、一次替换、零重复支付，修正 revision 增量。 |
| 46 | wpf · `Shanhe difficulty UI: published preview and actual challenge stats` | 测试过时 | 单节点难度夹具携带悬空 EliteUpgradeNodeId；只清除不属于本用例的升级引用。 |
| 47 | wpf · `Shanhe reward UI: boss bundles advance through three chapters` | 测试过时 | 第一章结束后追加一次安全升级；后两章不重复升级，原奖励领取与章节推进断言保留。 |
| 48 | wpf · `Shanhe reward UI: persistent market skill replacement confirms before payment` | 测试过时 | 同 45，购买命令和一次规则升级分别占一个 revision；保留确认前零支付、确认后一次支付与替换。 |

## 扩展定向范围发现的冲突

修复指纹校验后，同范围另有 5 项原先通过的检查依赖被修复的宽松恢复行为：武将池在旧哈希下重排或缺省、叙事文本变更、重试战斗后的目录文本变更、跳过补偿定义变更、WPF 篡改指纹后仍继续。将其改为内容不匹配则拒绝、匹配原定义则冷恢复；WPF 分别验证指纹错误与命令日志错误均保留源存档。另有 1 项锻甲夹具验证新装备契约却没有启用标志，补齐标志，仍检查生成实体与不入牌堆。它们不计入原先 48 项。

## 修复验证

原先 48 项在扩大范围复跑中均通过。该次 Core 428 项为 423 通过、5 项暴露上述恢复契约冲突；WPF 77 项为 76 通过、1 项暴露指纹冲突。修正后只复跑这 6 项，Core 5/5、WPF 1/1 通过；未重复整组 505 项，也未运行全量检查。

扩大范围的构建为 2.97 秒、依赖校验 0.25 秒，Core 与 WPF 测试进程并行分别为 185.18 秒、350.53 秒。6 项复跑构建 6.21 秒，Core 8.42 秒、WPF 6.89 秒。例行 4 项 smoke 全部通过；构建 2.15 秒、检查进程 1.09 秒，依赖校验 0.17 秒；计时包含该 runner 的进程启动和输出开销。

## 对公共结算协议建议的判断

当前 `SkillProgramEffectOp` 按源码成员声明及已编译枚举的公共静态字段两种独立口径核对，均为 **1862** 个；没有把数值最大值当作数量。此数与外部分析的 1460 不同，未取得其同一时点源码，不能直接比较旧统计。

试点迁移前，仅扫描人物目录中表达式形式的私有布尔方法，命中包含五类子窗口调用的方法 225 个，按调用序列和出现的 `PostEventContinuation` 分组有 11 类；该正则口径不覆盖语句块方法，也不能证明组内全部语义相同。其中原顺序且两个续接参数均为 `AwaitedProgramMovement` 的组有 123 个，两个均为 `Program` 的组有 47 个；另有先技能变更、后移动窗口等顺序变体。

“重复的结算协议值得集中”有代码依据，但不能从正则命中占比直接推出可删除三分之一武将代码。相同函数集合也可能有不同顺序、返回种类和父帧资格；这些差异关系到规则次序、已付费用与回放。专用事件、阶段和不变量也不全是重复，有些就是规则语义及恢复凭据。

先抽取调用顺序与参数都已确认相同的窄协议，再测量实际差异、保存恢复和测试成本。当前 DSL 在 [SkillPrograms.cs](../src/CardGame.Core/SkillPrograms.cs) 已有条件、表达式和绑定能力，后续应找出具体缺少的表达能力或宿主入口；不能仅从指令名称断言这三项完全缺失。保留复杂技能的原生实现，避免为降低 C# 字节数引入更难诊断的解释器。

有效指标包括新增武将所需修改文件/字节、读入上下文、实现及定向验证耗时、回放与支付回归数。纯 JSON 占比仅作辅助：把复杂度搬进解释器或巨型 JSON 并不自动降低维护成本。

## 已完成的小范围试点

在 `GameEngine.ProgramContinuations.cs` 增加 `TryBeginProgramChildWindows`，只集中已有的回复替换、武将状态、体力变化、牌移动、技能变更短路链。迁移鲍信、普通蔡夫人、杨奉的原包装方法；通用指令宿主也复用同一入口，公共手牌展示仍在前、后续伤害仍在后。

三个原生调用者仍传 `AwaitedProgramMovement`，指令宿主仍传 `Program`。人物的动态资格、不变量、已支付状态、原帧替换和子返回均未移动；未新增反射注册、状态记录、事件、指令或存档字段。其他顺序与参数组合保持原实现。

以 LF 统一换行计算，三个武将源文件共减少 743 个 UTF-8 字节，含共享入口与宿主的五个源码文件净减少 462 字节，源码 diff 为 14 行新增、8 行删除。收益首先是四处相同链共用一个维护点；这不是完整步骤运行器，也不能据此推算整个项目能减少 32% 代码。

复用现存 21 项定向检查全部通过，包含三名武将的真实行为和冷恢复，以及程序插入、续接、窗口返回、原生返回、牌移动、已付子树、末区转换和帧内选择。未增加重复人物快照测试。构建 42.68 秒、依赖校验 1.29 秒、检查进程 14.86 秒，包含构建与进程开销的总时长另由 runner 统计。

试点后的例行 4 项 smoke 也全部通过：构建 2.09 秒、检查进程 1.02 秒、依赖校验 0.16 秒。没有运行全量测试或发布。后续迁移应逐个核对顺序、两个续接参数和帧资格，先沿已有行为覆盖扩展同类调用者；复杂状态机和异序链另作机制设计。
