# 人物 Runtime 向 JSON 迁移

衡量真实退役的人物专用业务 C#：删除文件、删除行数、新增共享 C# 和 `src` C# 净变化。已有 JSON 技能的定义校验清理、未使用类清理、将人物分支搬到 Core，均不增加迁移技能数。共享文件只计一次；同一目录中其他技能已经是 JSON 时，不重复计算。

## 基线与统计边界

基线为 `236f9eabc`，即主构造与安全转发清理完成之后。按 Git 中的源文件统计，不含 `bin/obj`。

| 指标 | 数量 |
| --- | ---: |
| `src` C# | 563,971 行 |
| 人物 Runtime | 515 个版本目录 / 4,309 文件 / 243,503 行 |
| 规则中的唯一技能 ID | 1,876 |
| 所在版本含 Runtime 的技能 ID | 1,205 |
| 所在版本均无 Runtime 的技能 ID | 671 |

最后两项只描述目录占用，不能当成真实业务迁移率。例如伏枥已是 JSON，廖化目录仍因当先保留 Runtime；移动龙魂剩下的 C# 是定义约束。暂未逐一审查的技能保持未分类，不凭文件名归入“难题”。

## 执行办法

- 先复用现有操作、条件和数量表达式，优先能删除完整执行链的候选。
- 本轮按方法级实际退役计量；20 处只作收益参考。按完整能力簇交付，不为凑数合并语义不同的规则，也不因未满数量长期停在扫描。
- 每批 `src` C# 净变化必须为负；单独检查每个能力簇，不能用其他批的删除掩盖新增膨胀。
- 资格条件在原资格阶段运行，不能挪到效果阶段，避免多出询问、使用次数或支付。
- 难题登记一行缺口并继续。复用既有检查，只为新增共享行为补检查，不剪测试，不新增每人物定义快照。

## 已核实的候选

这是经过执行路径核对的短清单，不是对 1,876 个技能的完整人工分类。行数是基线中的专用文件；目录总量不充当可删除量。

| 优先级 | 技能 | 最小缺口 / 复用方式 | 可退役专用 C# | 状态 |
| --- | --- | --- | ---: | --- |
| 1 | `mobile:tuntian`、`boundary:tuntian-mobile`、`anniversary:qinguo` | 共用“本移动批有牌离开拥有者指定区域集合”资格；屯田为手牌+装备，勤国对装备取否 | 6 文件 / 64 行 | 已迁移；3 技能 ID、2 种规则 |
| 2 | `mobile:bingyi` | 现有同色绑定条件同时控制选目标与摸牌；解析及空目标摸牌协议接受相同条件配对 | 3 文件 / 66 行；另删死副本 1 文件 / 15 行 | 已迁移，专属枚举和工厂已删 |
| D | `happy:wushen` | 旧 hook 允许死亡用牌者尚未结算的杀加伤，共享修正要求来源存活 | 同文件中的加伤 hook；完整文件删除为 0 | 存在行为差异，跳过 |
| B | `boundary:dangxian-mobile` | 公共弃牌堆按牌类选择、绑定后获得，等待移牌子结算 | 4 文件 / 165 行 | 仅确认 1 个完整消费者，暂缓 |
| B | `ol:fenglve`、`boundary:lieren-mobile` | 拼点实体输出之外，还缺条件移牌、等待边界及已被领取牌的跳过 | 6 文件 / 168 行 | 单补绑定不能完整迁移，暂缓 |
| B | `mobile:jingce` | 读取当前真实出牌阶段的使用牌数 | 4 文件 / 56 行 | 仅确认 1 个消费者，暂缓 |

## 缺失能力按实际解锁量排序

| 能力 | 已确认可完整迁移的技能数 | 已确认专用 C# | 决策 |
| --- | ---: | ---: | --- |
| 移动批区域集合损失资格 | 3 | 64 行 | 先做；新增生产代码预算低于删除量 |
| 拼点实体输出绑定 | 0（2 个相邻技能尚有其他缺口） | 168 行潜在范围 | 不把输出绑定成本当成完整迁移报价 |
| 公共弃牌堆筛选并选择 | 1 | 165 行 | 其他相邻技能还有不同机制，不能算作一并解锁 |
| 有条件的目标集合选择 | 1 | 66 行业务链，另 15 行死副本 | 复用执行器条件；让空目标摸牌识别相同条件配对 |
| 真实出牌阶段使用数读取 | 1 | 56 行 | 等更多独立消费者 |

## 暂缓项目

| 项目 | 一行原因 |
| --- | --- |
| 鲍信穆讨 / 义谋 | 随机取牌、分支选择、逐人结算未被现有顺序动作完整覆盖。 |
| `ol:leiluan` | 需要当前实际回合移牌历史过滤，单补弃牌选择不能退役。 |
| 势邓艾凿险奖励段 | 仍有奖励循环与消耗状态，单补公共选择不能退役。 |
| `mou-mobile:qicai` | 包含装备区装入、替换及原生装备结算。 |
| `mou-mobile:huoshou` | 随机获得南蛮之外，仍有蓄力和伤害归属。 |
| 陆绩 mobile / 吕蒙 boundary-mobile | 分别缺其他技能拥有关系、固定初始人数的资格读取。 |
| 何太后 mobile / 周泰 boundary-mobile | 分别依赖冻结的实际回合击杀数、回合结束目标手牌的实时读取，现有相似读取时点不同。 |
| 界顾雍 / 移动诸葛瑾空目标摸牌 | 分别使用具名牌数选择、限定他人选择及原生摸牌游标，当前通用选择未覆盖。 |

补查排除纯定义校验后的 20 个最小 Runtime 版本，未再确认可直接退役的 A 类；不能把相似字段或效果数量当成资格语义相同。

另批清理了 967 个仅被各自未用工厂引用的旧 NativeOwners 类（秉壹副本已在前批删除）：类文件 10,029 行、完整工厂方法 999 行，净减 11,028 行。其中人物文件 526 个 / 5,887 行。这是死代码删除，不计新技能 JSON 覆盖，也不混入业务迁移收益。

## 批次结果

| 批次 / 提交 | 技能专用 C# 删除 | 共享 C# 新增 | `src` C# 净变化 |
| --- | ---: | ---: | ---: |
| 首轮：屯田两版、勤国 | 64 行 / 6 文件 | 净 +27 行 | −37 行 |
| 首轮：移动秉壹 | 66 行 / 3 文件 | 净 +6 行 | −77 行 |
| 独立旧 NativeOwners 清理 | 不计技能退役 | 0 行 | −11,028 行 |

秉壹另删除旧死副本 15 行和枚举/工厂 2 行，不计业务迁移量。合计真实迁移 4 技能、业务执行链 9 文件 / 130 行，实际人物 C# 删除 10 文件 / 145 行，`src` C# 净减 114 行至 563,857 行。16 项相关 Core、3 项 WPF 和常规 4 项检查通过；无全量验证。

每批附报 `bao-xin`、`liao-hua`、`xun-yu` 的文件数、行数与字节数。基线分别为：14 / 662 / 56,188；4 / 165 / 14,226；21 / 1,196 / 135,576。

旧副本清理后分别为：13 / 656 / 55,859；3 / 159 / 13,767；19 / 1,133 / 127,949。`src` C# 为 552,829 行，比拆分前的 491,309 行仍多 61,520 行；不把架构样板缩减当成人物已完成迁移。

## 模式排名（本轮）

Roslyn 基线 `e27ab1a64`：20,564 方法、159,052 方法体行、2,626 个含方法文件。共享调用按实际 callee 绑定，变量/人物前缀归一；先保留控制骨架与精确变体，再按动作/查询族及注册边界合并。定义校验、来源元数据、桥接与 AI 策略另列，不当业务动作。
下列是按方法数排序的非等价候选范围，行数为方法体 span 合计，不能视作可删除量。A 表示已有基础操作，不承诺整个族自动迁移；例子为基线文件行号。工具：`tools/SkillMigrationAnalysis/`。

| # | 一句话模式 | 方法 / 行 | 现有能力或最小缺口 | 3 个例子（相对 src/Characters） |
| --- | --- | ---: | --- | --- |
| 1 | 资格：聚合已发生历史 | 583 / 670 | B：冻结历史范围/过滤读取 | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoRuntime.OlMoRumo.cs:24`；`bai-ling-yun/anniversary/Runtime/CharacterBaiLingYunAnniversaryRuntime.AnniversaryBaiLingYunNative.cs:69`；`bu-lian-shi/mobile/Runtime/CharacterBuLianShiMobileRuntime.MobileBuLianShi.cs:15` |
| 2 | 资格：带守卫聚合历史 | 198 / 1590 | B：历史读取及原资格阶段 | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoObserversRuntime.OlMoRumoObservers.cs:6`；`a-hui-nan/ol/Runtime/CharacterAHuiNanOlRuntime.AHuiNan.cs:69`；`bu-lian-shi/boundary-mobile/Runtime/CharacterBuLianShiBoundaryMobileRuntime.BoundaryBuLianShi.cs:47` |
| 3 | 等待原生子窗口返回 | 164 / 164 | B：接回同帧续接，保留已付状态 | `bai-ling-yun/anniversary/Runtime/CharacterBaiLingYunAnniversaryRuntime.AnniversaryBaiLingYun.cs:93`；`bao-san-niang/ol/Runtime/CharacterBaoSanNiangOlRuntime.Bao2025.cs:240`；`bao-xin/ol/Runtime/CharacterBaoXinOlRuntime.BaoXin.cs:118` |
| 4 | 带条件读取执行历史 | 149 / 662 | B：共享历史范围/过滤 | `cai-yang/anniversary/Runtime/CharacterCaiYangAnniversaryRuntime.AnniversaryCaiYangProofs.cs:14`；`cai-yang/anniversary/Runtime/CharacterCaiYangAnniversaryRuntime.AnniversaryCaiYangSource.cs:92`；`cai-yong/le/Runtime/CharacterCaiYongLeRuntime.LeCaiYongJiaowei.cs:12` |
| 5 | 移牌/伤害/摸牌混合查询 | 115 / 131 | B：混合族需拆分，不能当单动作 | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoObserversRuntime.OlMoRumoObservers.cs:22`；`_shared/Runtime/CharacterSharedGameEngineOlMoRumoObserversRuntime.OlMoRumoObservers.cs:23`；`cai-yong/le/Runtime/CharacterCaiYongLeRuntime.LeCaiYongFeibai.cs:369` |
| 6 | 筛选候选后选择目标/牌 | 109 / 665 | A：基础选择已有，复杂候选保留 | `bai-ling-yun/anniversary/Runtime/CharacterBaiLingYunAnniversaryRuntime.AnniversaryBaiLingYun.cs:47`；`bu-lian-shi/boundary-mobile/Runtime/BoundaryBuLianShiPrograms.cs:48`；`cao-ang/mou/Runtime/MouCaoAngRuntime.cs:40` |
| 7 | 历史限定的选择资格 | 99 / 129 | B：历史限定候选读取 | `bai-ling-yun/anniversary/Runtime/CharacterBaiLingYunAnniversaryRuntime.AnniversaryBaiLingYunNative.cs:71`；`bai-ling-yun/anniversary/Runtime/CharacterBaiLingYunAnniversaryRuntime.AnniversaryBaiLingYunProofs.cs:33`；`bao-xin/ol/Runtime/CharacterBaoXinOlRuntime.MutaoQualifications.cs:6` |
| 8 | 写帧并签发带历史的事实 | 95 / 1079 | B：原返回时点的共享事实接点 | `bu-lian-shi/boundary-mobile/Runtime/CharacterBuLianShiBoundaryMobileRuntime.BoundaryBuLianShi.cs:29`；`cao-pi/anniversary-god/Runtime/CharacterCaoPiAnniversaryGodRuntime.ShenCaoPi.cs:167`；`cao-zhi/le/Runtime/CharacterCaoZhiLeRuntime.LeCaoZhiWenlanLegacyDuel.cs:93` |
| 9 | 签发结束事实并完成技能 | 95 / 509 | B：专属结束事实/清理语义 | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoRuntime.OlMoRumo.cs:97`；`a-hui-nan/ol/Runtime/CharacterAHuiNanOlRuntime.AHuiNan.cs:170`；`bao-san-niang/ol/Runtime/CharacterBaoSanNiangOlRuntime.Bao2025.cs:400` |
| 10 | 带守卫筛选历史候选 | 93 / 750 | B：历史范围与候选读取 | `bai-ling-yun/anniversary/Runtime/CharacterBaiLingYunAnniversaryRuntime.AnniversaryBaiLingYunProofs.cs:6`；`cai-wen-ji/le/Runtime/CharacterCaiWenJiLeRuntime.LeCaiWenJiBeifen.cs:28`；`cai-wen-ji/le/Runtime/CharacterCaiWenJiLeRuntime.LeCaiWenJiBeifen.cs:123` |
| 11 | 注册钩子中的历史资格 | 85 / 88 | B：保持钩子阶段的事实读取 | `bu-lian-shi/boundary-mobile/Runtime/CharacterBuLianShiBoundaryMobileRuntime.BoundaryBuLianShi.cs:58`；`bu-lian-shi/mobile/Runtime/CharacterBuLianShiMobileRuntime.MobileBuLianShi.cs:16`；`cao-ang/mou/Runtime/MouCaoAngRuntime.cs:82` |
| 12 | 目标或牌选择资格 | 83 / 96 | A：已有目标类型；缺口逐实例核对 | `bu-zhi/ol/Runtime/CharacterBuZhiOlRuntime.Dingpan.cs:8`；`cao-cao/ol-magic/Runtime/CharacterCaoCaoOlMagicRuntime.MoCaoCaoEquipment.cs:41`；`cao-cao/ol-magic/Runtime/CharacterCaoCaoOlMagicRuntime.MoCaoCaoRuntimeServices.cs:109` |
| 13 | 移牌/健康后等待子窗口 | 77 / 79 | A：已有动作；自定义履约保留 | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoRuntime.OlMoRumo.cs:71`；`cao-ang/mou/Runtime/CharacterCaoAngMouRuntime.MouCaoAngServices.cs:31`；`cao-cao/ol-magic/Runtime/CharacterCaoCaoOlMagicRuntime.MoCaoCaoRuntimeServices.cs:115` |
| 14 | 历史查询与异常守卫 | 67 / 370 | B：共享冻结事实读取 | `cao-ang/mou/Runtime/MouCaoAngRuntime.cs:67`；`cao-ang/mou/Runtime/MouCaoAngRuntime.cs:74`；`cao-pi/ol-god/Runtime/OlShenCaoPiRuntime.cs:42` |
| 15 | 按条件构造选择项 | 64 / 115 | A：chooseOption；部分选项条件缺表达式 | `bao-xin/ol/Runtime/CharacterBaoXinOlRuntime.Yimou.cs:6`；`cao-cao/ol-magic/Runtime/MoCaoCaoBachaoRuntime.cs:45`；`cao-hong/mou/Runtime/MouCaoHongRuntime.cs:39` |
| 16 | 清理原帧并完成技能 | 63 / 253 | A：既有游标结束；专属状态仍需处理 | `bei-mi-hu/ol/Runtime/CharacterBeiMiHuOlRuntime.BeiMiHuPrograms.cs:209`；`bu-zhi/ol/Runtime/CharacterBuZhiOlRuntime.BuZhiPrograms.cs:241`；`cao-shuang/mobile-shi/Runtime/CharacterCaoShuangMobileShiRuntime.MobileShiCaoShuang.cs:448` |
| 17 | 推进/替换帧游标 | 61 / 252 | B：自定义阶段到原游标的连接 | `cao-pi/anniversary-god/Runtime/CharacterCaoPiAnniversaryGodRuntime.ShenCaoPi.cs:178`；`cao-zhen/mobile-shi/Runtime/CharacterCaoZhenMobileShiRuntime.MobileShiCaoZhen.cs:155`；`chen-qun/mobile-shi/Runtime/CharacterChenQunMobileShiRuntime.MobileShiChenQun.cs:117` |
| 18 | 历史筛选后选择 | 59 / 348 | B：历史候选绑定 | `cai-yang/anniversary/Runtime/CharacterCaiYangAnniversaryRuntime.AnniversaryCaiYangSource.cs:55`；`cao-xian/ol/Runtime/CaoXianRuntime.cs:47`；`cao-zhen/mobile-shi/Runtime/CharacterCaoZhenMobileShiRuntime.MobileShiCaoZhenLedger.cs:12` |
| 19 | 更新帧并签发规则事件 | 58 / 312 | B：共同事实的发布接点 | `bao-xin/ol/Runtime/CharacterBaoXinOlRuntime.BaoXin.cs:71`；`cai-yang/anniversary/Runtime/CharacterCaiYangAnniversaryRuntime.AnniversaryCaiYang.cs:116`；`cao-pi/anniversary-god/Runtime/CharacterCaoPiAnniversaryGodRuntime.KemingSkillEcho.cs:77` |
| 20 | 读取拥有区域牌作资格 | 57 / 77 | A：已有区域/种类过滤，逐处核对 | `bao-san-niang/ol/Runtime/CharacterBaoSanNiangOlRuntime.Bao2025Observers.cs:100`；`bu-zhi/ol/Runtime/ProgramBuZhiSelection.cs:14`；`cai-fu-ren/ol/Runtime/CharacterCaiFuRenOlRuntime.OrdinaryQieting.cs:10` |
| 21 | 构造固定选择项 | 50 / 61 | A：chooseOption | `bao-san-niang/ol/Runtime/CharacterBaoSanNiangOlRuntime.Bao2025.cs:52`；`cao-shuang/mobile-shi/Runtime/CharacterCaoShuangMobileShiRuntime.MobileShiCaoShuang.cs:192`；`cao-ying/ol/Runtime/CharacterCaoYingOlRuntime.CaoYingPrograms.cs:65` |
| 22 | 公开状态与历史资格 | 49 / 49 | B：事实时点读取，不可偷换live/frozen | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoRuntime.OlMoRumo.cs:44`；`cai-wen-ji/le/Runtime/CharacterCaiWenJiLeRuntime.LeCaiWenJiBeifen.cs:40`；`cao-hong/mou/Runtime/CharacterCaoHongMouRuntime.MouCaoHongServices.cs:39` |
| 23 | 带条件读取事件历史 | 45 / 223 | B：共享历史范围/过滤 | `cao-cao/ol-magic/Runtime/MoCaoCaoFuzaiRuntime.cs:39`；`cao-cao/ol-magic/Runtime/MoCaoCaoFuzaiRuntime.cs:42`；`cao-yu/ol/Runtime/CharacterCaoYuOlRuntime.XiangxuLedger.cs:163` |
| 24 | 拥有技能关系资格 | 44 / 44 | B：同资格阶段的技能关系读取 | `cai-wen-ji/le/Runtime/CharacterCaiWenJiLeRuntime.LeCaiWenJiShuangjia.cs:11`；`cai-wen-ji/le/Runtime/ShuangjiaMarkRuntime.cs:20`；`cao-pi/mou/Runtime/CharacterCaoPiMouRuntime.MouXingshang.cs:6` |
| 25 | 移牌/摸牌/健康混合流程 | 43 / 530 | B：拆出明确动作链，不建大一统节点 | `cao-ang/mou/Runtime/CharacterCaoAngMouRuntime.MouCaoAngChildren.cs:7`；`cao-ang/mou/Runtime/CharacterCaoAngMouRuntime.MouCaoAngChildren.cs:33`；`cao-pi/mou/Runtime/CharacterCaoPiMouRuntime.MouCaoPiObservers.cs:9` |

本轮选“选择分支→摸牌”贯通：AST 另筛到 34 个回答后摸牌候选。先迁谋移动陈宫、快乐神司马懿、SP OL 孙尚香的公共选择/摸牌链，保留实际阶段、首次接受、已付代价和帮助事实等规则；卫兹公开提示不同，暂不混入。
共享契约：在原帧中签发选择结果并继续普通 Draw；完成事实发生于摸牌/替换返回之后、移牌子窗口之前，零实际牌同样完成。暂不引入循环或第二套 pending。

本批结果：19 个执行方法（原方法段 226 行）由 JSON/既有执行器替代：陈宫 5、司马懿 7、孙尚香 7；另删 12 个配套续接/校验方法与 1 个桥接。首用资格捕获改名保留，不计迁移；三人物均仍有规则 C#。
人物 C# 删 482 / 增 113，净减 369；共享 C# 增 110 / 删 51，净增 59；`src` C# 净减 310 至 552,519 行，含 JSON 的 `src` 净减 279 行。26 项相关 Core、3 项 WPF、例行 4 项通过，无全量；覆盖原付费失技、原生 HP/移牌子窗、选择与子窗冷恢复、真实禅润替换摸零张的帮助事实。
样本（C# 文件 / 行 / 字节）：`bao-xin` 13 / 656 / 55,859；`liao-hua` 3 / 159 / 13,767；`xun-yu` 19 / 1,133 / 127,949。跳过：卫兹需公开提示；历史资格族保留原冻结/资格阶段，不能按同族计作等价迁移。
